using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Business.Abstract;
using Castle.DynamicProxy;
using Core.Logging;
using Entity.Concrete;

namespace Business.Interceptors;

public class LogInterceptor(ILogService logService, IIstekBaglami istekBaglami) : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        var kronometre = Stopwatch.StartNew();

        var log = new Log
        {
            OrganizasyonId = istekBaglami.OrganizasyonId,
            MethodName = invocation.Method.Name,
            ClassName = invocation.Method.DeclaringType?.Name ?? "",
            Parameters = SerializeObject(invocation.Arguments),
            ExecutingTime = DateTime.Now,
            Username = istekBaglami.Kullanici,
            IpAdresi = istekBaglami.IpAdresi,
            Yol = istekBaglami.Yol
        };

        try
        {
            invocation.Proceed();
        }
        catch (Exception ex)
        {
            HataIsle(log, ex, kronometre);
            logService.Add(log);
            throw;
        }

        if (IsAsyncMethod(invocation.Method))
        {
            if (invocation.Method.ReturnType == typeof(Task))
            {
                invocation.ReturnValue = InterceptAsync((Task)(invocation.ReturnValue ?? Task.CompletedTask), log, kronometre);
            }
            else // Task<T>
            {
                var resultType = invocation.Method.ReturnType.GetGenericArguments()[0];
                var method = typeof(LogInterceptor).GetMethod(nameof(InterceptAsyncGeneric), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var genericMethod = method!.MakeGenericMethod(resultType);
                invocation.ReturnValue = genericMethod.Invoke(this, new object[] { invocation.ReturnValue!, log, kronometre });
            }
        }
        else
        {
            log.ReturnValue = GetReturnValue(invocation);
            log.SureMs = (int)kronometre.ElapsedMilliseconds;
            logService.Add(log);
        }
    }

    /// <summary>
    /// Başarısız çağrıyı işaretler ve isteğin hata koduyla eşler. Aynı kod,
    /// istisna kullanıcıya ulaştığında hata kaydına da yazılır; iki log
    /// böylece birbirine bağlanır.
    /// </summary>
    private void HataIsle(Log log, Exception ex, Stopwatch kronometre)
    {
        log.Error = ex.Message;
        log.Basarili = false;
        log.HataKodu = istekBaglami.HataKodu();
        log.SureMs = (int)kronometre.ElapsedMilliseconds;
    }

    private static bool IsAsyncMethod(System.Reflection.MethodInfo method)
    {
        return (
            method.ReturnType == typeof(Task) ||
            (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
        );
    }

    private async Task InterceptAsync(Task task, Log log, Stopwatch kronometre)
    {
        try
        {
            await task;
            log.ReturnValue = "Task completed successfully";
            log.SureMs = (int)kronometre.ElapsedMilliseconds;
        }
        catch (Exception ex)
        {
            HataIsle(log, ex, kronometre);
            throw;
        }
        finally
        {
            logService.Add(log);
        }
    }

    private async Task<T> InterceptAsyncGeneric<T>(Task<T> task, Log log, Stopwatch kronometre)
    {
        T result;
        try
        {
            result = await task;
            log.ReturnValue = SerializeObject(result!);
            log.SureMs = (int)kronometre.ElapsedMilliseconds;
        }
        catch (Exception ex)
        {
            HataIsle(log, ex, kronometre);
            throw;
        }
        finally
        {
            logService.Add(log);
        }
        return result;
    }

    private static string GetReturnValue(IInvocation invocation)
    {
        if (invocation.ReturnValue != null)
        {
            try
            {
                 return SerializeObject(invocation.ReturnValue);
            }
            catch (Exception ex)
            {
                return $"Could not serialize return value: {ex.Message}";
            }
        }
        return "null";
    }

    private static string SerializeObject(object obj)
    {
        try
        {
            // IFormFile içeren nesneleri temizle
            var sanitizedObj = SanitizeObject(obj);

            return JsonSerializer.Serialize(sanitizedObj, new JsonSerializerOptions
            {
                WriteIndented = false,
                ReferenceHandler = ReferenceHandler.IgnoreCycles, // Döngüleri engelle
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                MaxDepth = 3
            });
        }
        catch (Exception ex)
        {
            return $"Serialization error: {ex.Message}";
        }
    }

    private static object SanitizeObject(object obj)
    {
        if (obj == null) return null!;

        var type = obj.GetType();

        // IFormFile kontrolü (Microsoft.AspNetCore.Http.IFormFile)
        if (type.GetInterfaces().Any(i => i.Name == "IFormFile") || type.Name == "IFormFile")
        {
             var fileNameProp = type.GetProperty("FileName");
             var lengthProp = type.GetProperty("Length");
             var fileName = fileNameProp?.GetValue(obj)?.ToString() ?? "Unknown";
             var length = lengthProp?.GetValue(obj)?.ToString() ?? "0";
             return $"[File: {fileName}, Size: {length} bytes]";
        }

        // Primitive tipler
        if (type.IsPrimitive || type == typeof(string) || type == typeof(decimal) || type == typeof(DateTime))
        {
            return obj;
        }

        // Koleksiyonlar (List, Array vb.)
        if (obj is System.Collections.IEnumerable enumerable)
        {
            var list = new List<object>();
            foreach (var item in enumerable)
            {
                list.Add(SanitizeObject(item));
            }
            return list;
        }

        // Karmaşık nesneler (DTO'lar vb.) - Reflection ile özellikleri dolaş
        // Dikkat: Bu kısım performans maliyeti oluşturabilir, ancak loglama için kabul edilebilir.
        // Stack overflow riskine karşı derinlik kontrolü eklenebilir ama şimdilik basit tutalım.
        // Sadece DTO namespace altındakileri veya belirli tipleri filteleyebiliriz.
        // Ancak en garantisi yeni bir Dictionary'e maplemek.

        try
        {
             var dict = new Dictionary<string, object>();
             foreach (var prop in type.GetProperties())
             {
                 if (prop.CanRead)
                 {
                     var val = prop.GetValue(obj);
                     dict[prop.Name] = SanitizeObject(val!);
                 }
             }
             return dict;
        }
        catch
        {
            return obj.ToString() ?? "ComplexObject";
        }
    }
}

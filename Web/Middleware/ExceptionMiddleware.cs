using Core.Exceptions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Web.Extensions;

namespace Web.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);

            if (context.Response is { StatusCode: 404, HasStarted: false })
            {
                context.Response.Redirect($"/Home/Error?message=NotFound&statusCode=404");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Bir hata oluştu: {Path}", context.Request.Path);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Yanıt başlatılmışsa işlem yapamayız
        if (context.Response.HasStarted)
        {
            _logger.LogWarning("Yanıt zaten başlatılmış, hata işlenemedi");
            return;
        }

        var statusCode = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            ValidationException => StatusCodes.Status400BadRequest,
            UniqueConstraintException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        var errorMessage = exception.Message;

        // Production ortamında kullanıcıya teknik hata detayını gösterme
        // Bilinen/beklenen hatalar (UniqueConstraint, Validation, NotFound) mesajlarını her ortamda göster
        var environment = context.RequestServices.GetRequiredService<IWebHostEnvironment>();
        if (!environment.IsDevelopment() && exception is not (UniqueConstraintException or ValidationException or NotFoundException))
        {
            errorMessage = "İşleminiz sırasında beklenmedik bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";
        }

        // AJAX isteği ise JSON dön
        if (context.Request.IsAjaxRequest())
        {
            context.Response.StatusCode = statusCode; // 200 OK dönmek yerine hata kodunu dönmek daha doğru olabilir, ancak DataTables bazen 200 bekleyebilir.
            // Genelde jQuery ajax fail bloğu için status code hata olmalı.
            // Ancak DataTables error handling için JSON formatı önemli.
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new 
            { 
                success = false, 
                message = errorMessage,
                error = errorMessage 
            });
            return;
        }

        // Login sayfasında mıyız kontrol et
        bool isLoginPage = context.Request.Path.Value?.ToLower().Contains("/account/login") ?? false;

        try
        {
            // Login sayfasında özel işlem yapalım
            if (isLoginPage)
            {
                context.Response.Redirect($"/Account/Login?error={Uri.EscapeDataString(errorMessage)}");
                return;
            }

            // TempData'ya erişmeyi deneyelim
            var tempDataFactory = context.RequestServices.GetService<ITempDataDictionaryFactory>();

            if (tempDataFactory != null)
            {
                try
                {
                    var tempData = tempDataFactory.GetTempData(context);
                    tempData["ErrorMessage"] = errorMessage;
                    tempData["ErrorStatusCode"] = statusCode;
                    
                    // TempData'yı kaydet - Bu çok önemli!
                    tempData.Save();

                    // Referer header'ı kontrol edelim
                    if (context.Request.Headers.ContainsKey("Referer") &&
                        !string.IsNullOrEmpty(context.Request.Headers["Referer"].ToString()))
                    {
                        var referer = context.Request.Headers["Referer"].ToString();

                        // Referer'ın geçerli olduğundan emin olalım
                        if (Uri.TryCreate(referer, UriKind.Absolute, out var uri) && 
                            IsLocalUrl(referer, context))
                        {
                            context.Response.Redirect(referer);
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "TempData işlemi sırasında hata oluştu");
                }
            }

            // Query string ile Error sayfasına yönlendir (fallback)
            context.Response.Redirect(
                $"/Home/Error?message={Uri.EscapeDataString(errorMessage)}&statusCode={statusCode}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hata yönlendirme işlemi sırasında ikincil bir hata oluştu");
            
            // Son çare olarak basit bir hata mesajı göster
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "text/html; charset=utf-8";
            await context.Response.WriteAsync($@"
                <html>
                    <head><title>Hata</title></head>
                    <body>
                        <h1>Hata: {statusCode}</h1>
                        <p>{errorMessage}</p>
                        <a href='/'>Ana Sayfaya Dön</a>
                    </body>
                </html>");
        }
    }

    private bool IsLocalUrl(string url, HttpContext context)
    {
        if (string.IsNullOrEmpty(url))
            return false;

        if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            return string.Equals(uri.Host, context.Request.Host.Host, StringComparison.OrdinalIgnoreCase);
        }

        return false;
    }
}

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
}
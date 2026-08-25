using Castle.DynamicProxy;

namespace Business.Interceptors;

public class ExceptionInterceptor : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        var methodName = invocation.Method.Name;
        var className = invocation.Method.DeclaringType?.Name;

        try
        {
            invocation.Proceed();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[EXCEPTION] - {DateTime.Now} - Exception occurred in {className}.{methodName}");
            Console.WriteLine($"[EXCEPTION] - Exception Type: {ex.GetType().Name}");
            Console.WriteLine($"[EXCEPTION] - Message: {ex.Message}");
            Console.WriteLine($"[EXCEPTION] - Stack Trace: {ex.StackTrace}");
            Console.ResetColor();
            
            throw; // Rethrow the exception after logging
        }
    }
}
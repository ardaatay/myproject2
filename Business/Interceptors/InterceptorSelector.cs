using System.Reflection;
using Castle.DynamicProxy;
using Core.Aspects;

namespace Business.Interceptors;

public class InterceptorSelector(Type? implementationType = null) : IInterceptorSelector
{
    public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
    {
        var targetType = implementationType ?? type;
        
        var classAttributes = targetType.GetCustomAttributes(true).OfType<Attribute>().ToList();

        var methodInfo = targetType.GetMethod(method.Name, method.GetParameters().Select(p => p.ParameterType).ToArray());
        var methodAttributes = methodInfo?.GetCustomAttributes(true).OfType<Attribute>().ToList() ?? new List<Attribute>();

        classAttributes.AddRange(methodAttributes);

        var result = interceptors.Where(x =>
            (x is LogInterceptor && classAttributes.Any(a => a is LogAspect)) ||
            (x is ExceptionInterceptor && classAttributes.Any(a => a is ExceptionAspect))
        ).ToArray();

        return result;
    }
}
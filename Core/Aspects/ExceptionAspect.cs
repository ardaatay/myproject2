namespace Core.Aspects;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class ExceptionAspect : Attribute
{
}
namespace Openbook.AzureFunctions.Authorization;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class FunctionAuthorizeAttribute : Attribute;
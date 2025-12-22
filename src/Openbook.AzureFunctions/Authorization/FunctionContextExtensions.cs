using System.Reflection;
using Microsoft.Azure.Functions.Worker;

namespace Openbook.AzureFunctions.Authorization;

public static class FunctionContextExtensions
{
    public static MethodInfo GetTargetFunctionMethod(this FunctionContext context)
    {
        var entryPoint = context.FunctionDefinition.EntryPoint;
        var lastDot = entryPoint.LastIndexOf('.');
        var typeName = entryPoint[..lastDot];
        var methodName = entryPoint[(lastDot + 1)..];

        var type = Type.GetType(typeName);
        if (type is null)
            throw new InvalidOperationException($"Cannot load type {typeName}");

        return type.GetMethod(
            methodName,
            BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
        )!;
    }
}
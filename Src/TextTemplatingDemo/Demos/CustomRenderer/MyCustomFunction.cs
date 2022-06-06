using System;
using System.Linq;
using System.Threading.Tasks;
using Scriban;
using Scriban.Runtime;
using Scriban.Syntax;

namespace TextTemplatingDemo.Demos.CustomRenderer;

public class MyCustomFunction : IScriptCustomFunction
{
    public object Invoke(TemplateContext context, ScriptNode callerContext, ScriptArray arguments,
        ScriptBlockStatement blockStatement)
    {
        return GetString(arguments);
    }

    public ValueTask<object> InvokeAsync(TemplateContext context, ScriptNode callerContext, ScriptArray arguments,
        ScriptBlockStatement blockStatement)
    {
        return new ValueTask<object>(GetString(arguments));
    }

    private static string GetString(ScriptArray arguments)
    {
        if (arguments.Count == 0)
        {
            return "*Nothing to calculate*";
        }

        var name = arguments[0];
        if (name == null || string.IsNullOrWhiteSpace(name.ToString()))
        {
            return string.Empty;
        }

        var args = arguments.Where(x => x != null && !string.IsNullOrWhiteSpace(x.ToString())).Select(x => int.Parse(x.ToString() ?? "0")).ToArray();

        return args.Any() ? string.Join(" + ", args) + " = " + args.Sum() : "*Nothing to calculate*";
    }

    public ScriptParameterInfo GetParameterInfo(int index)
    {
        return new ScriptParameterInfo(typeof(int), "arg" + index);
    }

    public int RequiredParameterCount => 0;
    public int ParameterCount => 0;
    public ScriptVarParamKind VarParamKind => ScriptVarParamKind.Direct;
    public Type ReturnType => typeof(string);
}
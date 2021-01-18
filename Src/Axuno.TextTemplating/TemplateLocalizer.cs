using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Scriban;
using Scriban.Runtime;
using Scriban.Syntax;

namespace Axuno.TextTemplating
{
    internal class TemplateLocalizer : IScriptCustomFunction
    {
        private readonly IStringLocalizer _localizer;

        public TemplateLocalizer(IStringLocalizer localizer)
        {
            _localizer = localizer;
        }

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

        private string GetString(ScriptArray? arguments)
        {
            if (arguments is null || arguments.Count == 0)
            {
                return string.Empty;
            }

            var name = arguments[0];
            if (name == null || string.IsNullOrWhiteSpace(name.ToString()))
            {
                return string.Empty;
            }

            var args = arguments.Skip(1).Where(x => x != null && !string.IsNullOrWhiteSpace(x.ToString())).ToArray();
            return (args.Any() ? _localizer[name.ToString(), args] : _localizer[name.ToString()])!;
        }

        public int RequiredParameterCount => 0;
        
        public int ParameterCount => 0;
        
        public ScriptVarParamKind VarParamKind => ScriptVarParamKind.Direct;

        public Type ReturnType => typeof(string);
        
        public ScriptParameterInfo GetParameterInfo(int index)
        {
            return index == 0
                ? new ScriptParameterInfo(typeof(string), "template_name")
                : new ScriptParameterInfo(typeof(object), "value");
        }
    }
}

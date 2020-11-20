using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Scriban;
using Scriban.Runtime;
using Scriban.Syntax;

namespace Axuno.TextTemplating
{
    public class TemplateLocalizer : IScriptCustomFunction
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
            return args.Any() ? _localizer[name.ToString(), args] : _localizer[name.ToString()];
        }

        public ScriptParameterInfo GetParameterInfo(int index)
        {
            switch (index)
            {
                case 0: return new ScriptParameterInfo(typeof(string), string.Empty);
                default: return new ScriptParameterInfo(typeof(object), string.Empty);
            }
        }

        public int RequiredParameterCount => 0;
        public int ParameterCount => 0;
        public ScriptVarParamKind VarParamKind => ScriptVarParamKind.Direct;

        public Type ReturnType => typeof(string);
    }
}

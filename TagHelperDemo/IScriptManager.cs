using System.Collections.Generic;

namespace TagHelperDemo
{
    public interface IScriptManager
    {
        void AddScript(ScriptReference script);

        List<ScriptReference> Scripts { get; }
        List<string> ScriptTexts { get; }
        void AddScriptText(string scriptTextExecute);
    }
}

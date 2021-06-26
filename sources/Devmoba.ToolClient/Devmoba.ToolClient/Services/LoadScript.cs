using Devmoba.ToolClient.Models;
using Microsoft.ClearScript;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Devmoba.ToolClient.Services
{
    public class LoadScript
    {
        private static LoadScript _instance;
        private List<Script> _scripts = new List<Script>();
        private List<string> _libraries = new List<string>();

        private LoadScript() 
        {
            AddToLibraries();
        }

        public static LoadScript GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LoadScript();
            }
            return _instance;
        }

        public void AddToLibraries()
        {
            var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

            var files = Directory.GetFiles($"{projectDirectory}\\Libraries\\js", "*.js");
            foreach (var file in files)
            {
                _libraries.Add(File.ReadAllText(file));
            }
        }

        public void ExecuteLibraries(ScriptEngine engine)
        {
            foreach (var lib in _libraries)
            {
                engine.Execute(lib);
            }
        }

        public string CreateFuncIncluded(List<Script> scripts, ScriptEngine engine)
        {
            var stringBuilder = new StringBuilder();
            
            foreach (var script in scripts)
            {
                var splitScripts = script.Content.Split(new String[] { "\n" }, StringSplitOptions.None);
                var jsonResult = engine.Script.getFunctionInfo(script.Content);
                var functionDeclarations = JsonConvert.DeserializeObject<List<FunctionDeclaration>>(jsonResult);

                stringBuilder.Append($"var S{script.Id} = " + "{");
                AppendLines(stringBuilder, functionDeclarations, splitScripts);
                stringBuilder.Append("}\r\n");
            }
            return stringBuilder.ToString();
        }

        private void AppendLines(
           StringBuilder stringBuilder,
           List<FunctionDeclaration> functionDeclarations,
           string[] splitScripts)
        {
            foreach (var func in functionDeclarations)
            {
                stringBuilder.Append($"{func.Name}: ");
                stringBuilder.Append(GetFunction(func, splitScripts));
                stringBuilder.Append($",");
            }
        }

        private string GetFunction(FunctionDeclaration func, string[] splitScripts)
        {
            var stringBuilder = new StringBuilder();
            if (func.LineStart == func.LineEnd)
            {
                stringBuilder
                    .Append($"{splitScripts[func.LineStart - 1].Substring(func.ColumnStart, (func.ColumnEnd - func.ColumnStart))}\r\n");
            }
            else
            {
                for (int i = func.LineStart - 1; i < func.LineEnd; i++)
                {
                    if (func.LineStart - 1 == i)
                    {
                        stringBuilder
                            .Append($"{splitScripts[i].Substring(func.ColumnStart, splitScripts[i].Length - func.ColumnStart)}\r\n");
                    }
                    else if (func.LineEnd - 1 == i)
                        stringBuilder.Append($"{splitScripts[i].Substring(0, func.ColumnEnd)}\r\n");
                    else
                        stringBuilder.Append($"{splitScripts[i]}\r\n");
                }
            }
            return stringBuilder.ToString();
        }

        public string ExcludeStatement(string content, List<FunctionDeclaration> functionDeclarations)
        {
            var splitScripts = content.Split(new String[] { "\n" }, StringSplitOptions.None);
            var stringBuilder = new StringBuilder();
            foreach (var func in functionDeclarations)
            {
                stringBuilder.Append(GetFunction(func, splitScripts));
            }
            return stringBuilder.ToString();
        }
    }
}

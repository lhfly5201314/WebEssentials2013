﻿using System;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Utilities;

namespace MadsKristensen.EditorExtensions.Compilers.Sweet.js
{
    [Export(typeof(NodeExecutorBase))]
    [ContentType("SweetJs")]
    public class SweetjsCompiler : NodeExecutorBase
    {
        private static readonly string _compilerPath = Path.Combine(WebEssentialsResourceDirectory, @"nodejs\tools\node_modules\sweet.js\bin\sjs");
        //private static readonly Regex _errorParsingPattern = new Regex(@"(?<fileName>.*):(?<line>.\d*):(?<column>.\d*): error: (?<message>.*\n.*)", RegexOptions.Multiline);
        //private static readonly Regex _sourceMapInJs = new Regex(@"\/\*\n.*=(.*)\n\*\/", RegexOptions.Multiline);

        public override string TargetExtension { get { return ".js"; } }
        public override bool GenerateSourceMap { get { return true; } }
        public override string ServiceName { get { return "Sweetjs"; } }
        protected override string CompilerPath { get { return _compilerPath; } }
        public override bool RequireMatchingFileName { get { return true; } }
        protected override Regex ErrorParsingPattern { get { throw new NotImplementedException(); } }

        protected override string GetArguments(string sourceFileName, string targetFileName)
        {
            var args = new StringBuilder();

            if (GenerateSourceMap)
                args.Append("--sourcemap ");

            args.AppendFormat(CultureInfo.CurrentCulture, "--output \"{0}\" \"{1}\"", Path.GetDirectoryName(targetFileName), sourceFileName);
            return args.ToString();
        }

        protected override string PostProcessResult(string resultSource, string sourceFileName, string targetFileName)
        {
            Logger.Log(ServiceName + ": " + Path.GetFileName(sourceFileName) + " compiled.");

            return resultSource;
        }
    }
}

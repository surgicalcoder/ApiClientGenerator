using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace GoLive.Generator.ApiClientGenerator
{
    public class SourceStringBuilder
    {
        private readonly string SingleIndent = new string(' ', 4);

        public int IndentLevel = 0;
        private readonly StringBuilder _stringBuilder;

        public SourceStringBuilder()
        {
            _stringBuilder = new StringBuilder();
        }

        public void IncreaseIndent()
        {
            IndentLevel++;
        }

        public void DecreaseIndent()
        {
            IndentLevel--;
        }

        public void AppendOpenCurlyBracketLine()
        {
            AppendLine("{");
            IncreaseIndent();
        }

        public void AppendCloseCurlyBracketLine()
        {
            DecreaseIndent();
            AppendLine("}");
        }

        public void Append(string text, bool indent = true)
        {
            if (indent)
            {
                AppendIndent();
            }

            _stringBuilder.Append(text);
        }

        public void AppendIndent()
        {
            for (int i = 0; i < IndentLevel; i++)
            {
                _stringBuilder.Append(SingleIndent);
            }
        }

        public void AppendLine()
        {
            _stringBuilder.Append(Environment.NewLine);
        }

        public void AppendLine(string text)
        {
            Append(text);
            AppendLine();
        }

        public override string ToString()
        {
            var text = _stringBuilder.ToString();
            return string.IsNullOrWhiteSpace(text)
                ? string.Empty
                : CSharpSyntaxTree.ParseText(text).GetRoot().NormalizeWhitespace().SyntaxTree.GetText().ToString();
        }
    }
}
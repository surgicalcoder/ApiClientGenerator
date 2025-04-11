using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace GoLive.Generator.ApiClientGenerator;

public class SourceStringBuilderBracket : IDisposable
{
    private SourceStringBuilder builder;
    public SourceStringBuilderBracket(SourceStringBuilder source)
    {
        builder = source;
        builder.AppendOpenCurlyBracketLine();
    }
    public void Dispose()
    {
        builder.AppendCloseCurlyBracketLine();
    }
}

public class SourceStringBuilderParentheses : IDisposable
{
    private SourceStringBuilder builder;
    public SourceStringBuilderParentheses(SourceStringBuilder source)
    {
        builder = source;
        builder.Append("(");
        builder.IncreaseIndent();
    }
    public void Dispose()
    {
        builder.Append(")");
        builder.DecreaseIndent();
    }
}


public class SourceStringBuilder
{
    private readonly string SingleIndent = new string(' ', 4);

    public int IndentLevel = 0;
    private readonly StringBuilder _stringBuilder = new();

    public SourceStringBuilderParentheses CreateParentheses(string input = null)
    {
        if (!string.IsNullOrEmpty(input))
        {
            Append(input);
        }
        return new SourceStringBuilderParentheses(this);
    }

    public SourceStringBuilderBracket CreateBracket()
    {
        return new SourceStringBuilderBracket(this);
    }

    internal void IncreaseIndent()
    {
        IndentLevel++;
    }

    internal void DecreaseIndent()
    {
        IndentLevel--;
    }

    internal void AppendOpenCurlyBracketLine()
    {
        AppendLine("{");
        IncreaseIndent();
    }

    internal void AppendCloseCurlyBracketLine()
    {
        DecreaseIndent();
        AppendLine("}");
    }

    public void AppendMultipleLines(string text)
    {
        var lines = text.Split([Environment.NewLine], StringSplitOptions.None);
        foreach (var line in lines)
        {
            AppendLine(line);
        }
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

    public void AppendLine(int count = 1)
    {
        for (var i = 0; i < count; i++)
        {
            _stringBuilder.Append("\n");
        }
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
            : PrettyFormatCode(text);
    }

    public static string PrettyFormatCode(string text)
    {
        return CSharpSyntaxTree.ParseText(text).GetRoot().NormalizeWhitespace().SyntaxTree.GetText().ToString();
    }
}
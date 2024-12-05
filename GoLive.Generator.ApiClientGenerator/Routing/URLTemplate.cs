using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoLive.Generator.ApiClientGenerator.Routing;

public class URLTemplate
{
    public string Raw { get; set; }
    public List<URLTemplateSegment> Segments { get; set; } = new();
    
    
    public string Render(CaseInSensitiveDictionary values)
    {
        StringBuilder sb = new();
        sb.Append("/");
        CaseInsensitiveList usedValues = new();

        foreach (URLTemplateSegment segment in Segments)
        {
            if (segment.BuiltInReplaceable.HasValue)
            {
                if (values.ContainsKey(segment.BuiltInReplaceable.ToString().ToLower()))
                {
                    sb.Append(values[segment.BuiltInReplaceable.ToString().ToLower()]);
                    usedValues.Add(segment.BuiltInReplaceable.ToString());
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(segment.DefaultValue))
                    {
                        sb.Append(segment.DefaultValue);
                    }
                    else
                    {
                        if (segment.BuiltInReplaceable != null)
                        {
                            if (!string.Equals(segment.Restriction, URLTemplateSegmentKnownRestrictions.Exists, StringComparison.InvariantCultureIgnoreCase))
                            {
                                sb.Append(segment.BuiltInReplaceable.ToString());
                            }
                        }
                        else
                        {
                            sb.Append(segment.Raw);
                        }
                    }
                    
                }
            }
            else if (!string.IsNullOrWhiteSpace(segment.Parameter))
            {
                if (values.TryGetValue(segment.Parameter.ToLower(), out var value))
                {
                    sb.Append(value);
                    usedValues.Add(segment.Parameter);
                }
            }
            else
            {
                if (segment.BuiltInReplaceable != null)
                {
                    sb.Append(segment.BuiltInReplaceable.ToString());
                }
                else
                {
                    sb.Append(segment.Raw);
                }
            }
            
            sb.Append("/");
        }
        
        if (values.ContainsKey("area") && !string.IsNullOrWhiteSpace(values["area"]) && !usedValues.Contains("area"))
        {
            return default;
        }
        
        var unusedValues = values.Where(kvp => !usedValues.Contains(kvp.Key)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        
        unusedValues = unusedValues.Where(kvp => !Enum.GetNames(typeof(URLTemplateReplaceableElement)).Any(e => string.Equals(e.ToLower(), kvp.Key, StringComparison.InvariantCultureIgnoreCase))).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        
        QueryString qs = QueryString.Empty;
        foreach (var kvp in unusedValues)
        {
            qs = qs.Add(kvp.Key, kvp.Value, false);
        }
        
        string result = sb.ToString().Replace("//", "/");
        
        if (result.Length > 0 && result[^1] == '/')
        {
            result = result[..^1]; 
        }
        
        return $"{result}{qs}";
    }

    public static URLTemplate Parse(string template)
    {
        URLTemplate result = new()
        {
            Raw = template
        };

        string[] parts = template.Split('/');
        parts = parts.Where(p => !string.IsNullOrWhiteSpace(p)).ToArray();
        foreach (string part in parts)
        {
            URLTemplateSegment segment = new()
            {
                Raw = part
            };

            if (part.StartsWith("{") && part.EndsWith("}"))
            {
                if (part.Contains("="))
                {
                    string[] parts2 = part.Split('=');
                    segment.BuiltInReplaceable = (URLTemplateReplaceableElement)Enum.Parse(typeof(URLTemplateReplaceableElement), parts2[0][1..], true);
                    segment.Parameter = parts2[0][1..];
                    segment.DefaultValue = parts2[1][..^1];
                }
                else if (part.Contains(":"))
                {
                    string[] parts2 = part.Split(':');
                    if (Enum.TryParse<URLTemplateReplaceableElement>(parts2[0][1..], true, out var replaceable))
                    {
                        segment.BuiltInReplaceable = replaceable;
                    }
                    segment.Parameter = parts2[0][1..];
                    segment.Restriction = parts2[1][..^1];
                }
                else if (part.EndsWith("?}"))
                {
                    segment.Parameter = part[1..^2];
                    segment.Restriction = URLTemplateSegmentKnownRestrictions.Optional;
                }
                else if (part.StartsWith("{*"))
                {
                    segment.BuiltInReplaceable = (URLTemplateReplaceableElement)Enum.Parse(typeof(URLTemplateReplaceableElement), part[2..^1], true);
                    segment.Parameter = part[2..^1];
                    segment.IsCatchall = true;
                }
                else
                {
                    if (Enum.TryParse<URLTemplateReplaceableElement>(part[1..^1], true, out URLTemplateReplaceableElement replaceable))
                    {
                        segment.BuiltInReplaceable = replaceable;
                        segment.Parameter = part[1..^1];
                    }
                    else
                    {
                        segment.Parameter = part[1..^1];
                    }
                }
            }
            
            result.Segments.Add(segment);

            if (segment.IsCatchall)
            {
                break;
            }
        }

        return result;
    }
}
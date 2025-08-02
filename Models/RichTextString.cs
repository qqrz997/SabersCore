using System;
using System.Collections;
using System.Collections.Generic;
using SabersCore.Utilities.Common;

namespace SabersCore.Models;

public record RichTextString : IEnumerable<char>, IComparable, IComparable<RichTextString>
{
    public string FullName { get; }
    private string Value { get; }
    
    private RichTextString(string fullName, string comparableValue)
    {
        FullName = fullName;
        Value = comparableValue;
    }

    public static RichTextString Unknown { get; } = new(string.Empty, string.Empty);

    public static RichTextString Create(string? fullText)
    {
        if (fullText is null) return Unknown;

        string trimmed = fullText.Trim();
        if (trimmed is []) return Unknown;

        string replaced = RegularExpressions.RichTextRegex.Replace(trimmed, string.Empty);
        return replaced is [] ? Unknown : new(fullText, replaced);
    }

    public int CompareTo(RichTextString other) => 
        string.Compare(Value, other.Value, StringComparison.Ordinal);

    public int CompareTo(object value) => 
        value is not RichTextString stringB ? 1 : CompareTo(stringB);
    
    public bool Contains(string value, StringComparison comparison = StringComparison.CurrentCulture) => 
        Value.Contains(value, comparison);

    public IEnumerator<char> GetEnumerator() => 
        FullName.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => 
        GetEnumerator();
}
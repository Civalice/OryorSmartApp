using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Globalization;

public class StringUtil {
	public static string ParseUnicodeEscapes(string escapedString)
	{
		if ((escapedString == null) || (escapedString.Length <= 0))
			return "";
		const string literalBackslashPlaceholder = "\uf00b";
		const string unicodeEscapeRegexString = @"(?:\\u([0-9a-fA-F]{4}))|(?:\\U([0-9a-fA-F]{8}))";
		// Replace escaped backslashes with something else so we don't
		// accidentally expand escaped unicode escapes.
		string workingString = escapedString.Replace("\\\\", literalBackslashPlaceholder);
		
		// Replace unicode escapes with actual unicode characters.
		workingString = new Regex(unicodeEscapeRegexString).Replace(workingString,
		                                                            match => ((char) int.Parse(match.Value.Substring(2), NumberStyles.HexNumber))
		                                                            .ToString(CultureInfo.InvariantCulture));
		
		// Replace the escaped backslash placeholders with non-escaped literal backslashes.
		workingString = workingString.Replace(literalBackslashPlaceholder, "\\");
		return workingString;
	}
}

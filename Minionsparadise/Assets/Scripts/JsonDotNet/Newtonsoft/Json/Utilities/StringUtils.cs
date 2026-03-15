namespace Newtonsoft.Json.Utilities
{
	internal static class StringUtils
	{
		private delegate void ActionLine(global::System.IO.TextWriter textWriter, string line);

		public const string CarriageReturnLineFeed = "\r\n";

		public const string Empty = "";

		public const char CarriageReturn = '\r';

		public const char LineFeed = '\n';

		public const char Tab = '\t';

		public static string FormatWith(this string format, global::System.IFormatProvider provider, params object[] args)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(format, "format");
			return string.Format(provider, format, args);
		}

		public static bool ContainsWhiteSpace(string s)
		{
			if (s == null)
			{
				throw new global::System.ArgumentNullException("s");
			}
			for (int i = 0; i < s.Length; i++)
			{
				if (char.IsWhiteSpace(s[i]))
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsWhiteSpace(string s)
		{
			if (s == null)
			{
				throw new global::System.ArgumentNullException("s");
			}
			if (s.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < s.Length; i++)
			{
				if (!char.IsWhiteSpace(s[i]))
				{
					return false;
				}
			}
			return true;
		}

		public static string EnsureEndsWith(string target, string value)
		{
			if (target == null)
			{
				throw new global::System.ArgumentNullException("target");
			}
			if (value == null)
			{
				throw new global::System.ArgumentNullException("value");
			}
			if (target.Length >= value.Length)
			{
				if (string.Compare(target, target.Length - value.Length, value, 0, value.Length, global::System.StringComparison.OrdinalIgnoreCase) == 0)
				{
					return target;
				}
				string text = target.TrimEnd(null);
				if (string.Compare(text, text.Length - value.Length, value, 0, value.Length, global::System.StringComparison.OrdinalIgnoreCase) == 0)
				{
					return target;
				}
			}
			return target + value;
		}

		public static bool IsNullOrEmptyOrWhiteSpace(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return true;
			}
			if (IsWhiteSpace(s))
			{
				return true;
			}
			return false;
		}

		public static void IfNotNullOrEmpty(string value, global::System.Action<string> action)
		{
			IfNotNullOrEmpty(value, action, null);
		}

		private static void IfNotNullOrEmpty(string value, global::System.Action<string> trueAction, global::System.Action<string> falseAction)
		{
			if (!string.IsNullOrEmpty(value))
			{
				if (trueAction != null)
				{
					trueAction(value);
				}
			}
			else if (falseAction != null)
			{
				falseAction(value);
			}
		}

		public static string Indent(string s, int indentation)
		{
			return Indent(s, indentation, ' ');
		}

		public static string Indent(string s, int indentation, char indentChar)
		{
			if (s == null)
			{
				throw new global::System.ArgumentNullException("s");
			}
			if (indentation <= 0)
			{
				throw new global::System.ArgumentException("Must be greater than zero.", "indentation");
			}
			global::System.IO.StringReader textReader = new global::System.IO.StringReader(s);
			global::System.IO.StringWriter stringWriter = new global::System.IO.StringWriter(global::System.Globalization.CultureInfo.InvariantCulture);
			ActionTextReaderLine(textReader, stringWriter, delegate(global::System.IO.TextWriter tw, string line)
			{
				tw.Write(new string(indentChar, indentation));
				tw.Write(line);
			});
			return stringWriter.ToString();
		}

		private static void ActionTextReaderLine(global::System.IO.TextReader textReader, global::System.IO.TextWriter textWriter, global::Newtonsoft.Json.Utilities.StringUtils.ActionLine lineAction)
		{
			bool flag = true;
			string line;
			while ((line = textReader.ReadLine()) != null)
			{
				if (!flag)
				{
					textWriter.WriteLine();
				}
				else
				{
					flag = false;
				}
				lineAction(textWriter, line);
			}
		}

		public static string NumberLines(string s)
		{
			if (s == null)
			{
				throw new global::System.ArgumentNullException("s");
			}
			global::System.IO.StringReader textReader = new global::System.IO.StringReader(s);
			global::System.IO.StringWriter stringWriter = new global::System.IO.StringWriter(global::System.Globalization.CultureInfo.InvariantCulture);
			int lineNumber = 1;
			ActionTextReaderLine(textReader, stringWriter, delegate(global::System.IO.TextWriter tw, string line)
			{
				tw.Write(lineNumber.ToString(global::System.Globalization.CultureInfo.InvariantCulture).PadLeft(4));
				tw.Write(". ");
				tw.Write(line);
				lineNumber++;
			});
			return stringWriter.ToString();
		}

		public static string NullEmptyString(string s)
		{
			if (!string.IsNullOrEmpty(s))
			{
				return s;
			}
			return null;
		}

		public static string ReplaceNewLines(string s, string replacement)
		{
			global::System.IO.StringReader stringReader = new global::System.IO.StringReader(s);
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
			bool flag = true;
			string value;
			while ((value = stringReader.ReadLine()) != null)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append(replacement);
				}
				stringBuilder.Append(value);
			}
			return stringBuilder.ToString();
		}

		public static string Truncate(string s, int maximumLength)
		{
			return Truncate(s, maximumLength, "...");
		}

		public static string Truncate(string s, int maximumLength, string suffix)
		{
			if (suffix == null)
			{
				throw new global::System.ArgumentNullException("suffix");
			}
			if (maximumLength <= 0)
			{
				throw new global::System.ArgumentException("Maximum length must be greater than zero.", "maximumLength");
			}
			int num = maximumLength - suffix.Length;
			if (num <= 0)
			{
				throw new global::System.ArgumentException("Length of suffix string is greater or equal to maximumLength");
			}
			if (s != null && s.Length > maximumLength)
			{
				string text = s.Substring(0, num);
				text = text.Trim();
				return text + suffix;
			}
			return s;
		}

		public static global::System.IO.StringWriter CreateStringWriter(int capacity)
		{
			global::System.Text.StringBuilder sb = new global::System.Text.StringBuilder(capacity);
			return new global::System.IO.StringWriter(sb, global::System.Globalization.CultureInfo.InvariantCulture);
		}

		public static int? GetLength(string value)
		{
			if (value == null)
			{
				return null;
			}
			return value.Length;
		}

		public static string ToCharAsUnicode(char c)
		{
			char c2 = global::Newtonsoft.Json.Utilities.MathUtils.IntToHex(((int)c >> 12) & 0xF);
			char c3 = global::Newtonsoft.Json.Utilities.MathUtils.IntToHex(((int)c >> 8) & 0xF);
			char c4 = global::Newtonsoft.Json.Utilities.MathUtils.IntToHex(((int)c >> 4) & 0xF);
			char c5 = global::Newtonsoft.Json.Utilities.MathUtils.IntToHex(c & 0xF);
			return new string(new char[6] { '\\', 'u', c2, c3, c4, c5 });
		}

		public static void WriteCharAsUnicode(global::System.IO.TextWriter writer, char c)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(writer, "writer");
			char value = global::Newtonsoft.Json.Utilities.MathUtils.IntToHex(((int)c >> 12) & 0xF);
			char value2 = global::Newtonsoft.Json.Utilities.MathUtils.IntToHex(((int)c >> 8) & 0xF);
			char value3 = global::Newtonsoft.Json.Utilities.MathUtils.IntToHex(((int)c >> 4) & 0xF);
			char value4 = global::Newtonsoft.Json.Utilities.MathUtils.IntToHex(c & 0xF);
			writer.Write('\\');
			writer.Write('u');
			writer.Write(value);
			writer.Write(value2);
			writer.Write(value3);
			writer.Write(value4);
		}

		public static TSource ForgivingCaseSensitiveFind<TSource>(this global::System.Collections.Generic.IEnumerable<TSource> source, global::System.Func<TSource, string> valueSelector, string testValue)
		{
			if (source == null)
			{
				throw new global::System.ArgumentNullException("source");
			}
			if (valueSelector == null)
			{
				throw new global::System.ArgumentNullException("valueSelector");
			}
			global::System.Collections.Generic.List<TSource> caseInsensitiveResults = new global::System.Collections.Generic.List<TSource>();
			global::Newtonsoft.Json.Aot.EnumerationExtension.ForEach(source, delegate(TSource itm)
			{
				if (string.Compare(valueSelector(itm), testValue, global::System.StringComparison.OrdinalIgnoreCase) == 0)
				{
					caseInsensitiveResults.Add(itm);
				}
			});
			int count = caseInsensitiveResults.Count;
			if (count <= 1)
			{
				if (count != 1)
				{
					return default(TSource);
				}
				return caseInsensitiveResults[0];
			}
			global::System.Collections.Generic.List<TSource> caseSensitiveResults = new global::System.Collections.Generic.List<TSource>();
			global::Newtonsoft.Json.Aot.EnumerationExtension.ForEach(source, delegate(TSource itm)
			{
				if (string.Compare(valueSelector(itm), testValue, global::System.StringComparison.Ordinal) == 0)
				{
					caseSensitiveResults.Add(itm);
				}
			});
			if (caseSensitiveResults.Count <= 0)
			{
				return default(TSource);
			}
			return caseSensitiveResults[0];
		}

		public static string ToCamelCase(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return s;
			}
			if (!char.IsUpper(s[0]))
			{
				return s;
			}
			string text = char.ToLower(s[0], global::System.Globalization.CultureInfo.InvariantCulture).ToString(global::System.Globalization.CultureInfo.InvariantCulture);
			if (s.Length > 1)
			{
				text += s.Substring(1);
			}
			return text;
		}
	}
}

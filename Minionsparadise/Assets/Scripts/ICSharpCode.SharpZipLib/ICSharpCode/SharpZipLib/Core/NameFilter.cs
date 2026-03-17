namespace ICSharpCode.SharpZipLib.Core
{
	public class NameFilter : global::ICSharpCode.SharpZipLib.Core.IScanFilter
	{
		private string filter_;

		private global::System.Collections.ArrayList inclusions_;

		private global::System.Collections.ArrayList exclusions_;

		public NameFilter(string filter)
		{
			filter_ = filter;
			inclusions_ = new global::System.Collections.ArrayList();
			exclusions_ = new global::System.Collections.ArrayList();
			Compile();
		}

		public static bool IsValidExpression(string expression)
		{
			bool result = true;
			try
			{
				new global::System.Text.RegularExpressions.Regex(expression, global::System.Text.RegularExpressions.RegexOptions.IgnoreCase | global::System.Text.RegularExpressions.RegexOptions.Singleline);
			}
			catch (global::System.ArgumentException)
			{
				result = false;
			}
			return result;
		}

		public static bool IsValidFilterExpression(string toTest)
		{
			bool result = true;
			try
			{
				if (toTest != null)
				{
					string[] array = SplitQuoted(toTest);
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] != null && array[i].Length > 0)
						{
							string pattern = ((array[i][0] == '+') ? array[i].Substring(1, array[i].Length - 1) : ((array[i][0] != '-') ? array[i] : array[i].Substring(1, array[i].Length - 1)));
							new global::System.Text.RegularExpressions.Regex(pattern, global::System.Text.RegularExpressions.RegexOptions.IgnoreCase | global::System.Text.RegularExpressions.RegexOptions.Singleline);
						}
					}
				}
			}
			catch (global::System.ArgumentException)
			{
				result = false;
			}
			return result;
		}

		public static string[] SplitQuoted(string original)
		{
			char c = '\\';
			char[] array = new char[1] { ';' };
			global::System.Collections.ArrayList arrayList = new global::System.Collections.ArrayList();
			if (original != null && original.Length > 0)
			{
				int num = -1;
				global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
				while (num < original.Length)
				{
					num++;
					if (num >= original.Length)
					{
						arrayList.Add(stringBuilder.ToString());
					}
					else if (original[num] == c)
					{
						num++;
						if (num >= original.Length)
						{
							throw new global::System.ArgumentException("Missing terminating escape character", "original");
						}
						if (global::System.Array.IndexOf(array, original[num]) < 0)
						{
							stringBuilder.Append(c);
						}
						stringBuilder.Append(original[num]);
					}
					else if (global::System.Array.IndexOf(array, original[num]) >= 0)
					{
						arrayList.Add(stringBuilder.ToString());
						stringBuilder.Length = 0;
					}
					else
					{
						stringBuilder.Append(original[num]);
					}
				}
			}
			return (string[])arrayList.ToArray(typeof(string));
		}

		public override string ToString()
		{
			return filter_;
		}

		public bool IsIncluded(string name)
		{
			bool result = false;
			if (inclusions_.Count == 0)
			{
				result = true;
			}
			else
			{
				foreach (global::System.Text.RegularExpressions.Regex item in inclusions_)
				{
					if (item.IsMatch(name))
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		public bool IsExcluded(string name)
		{
			bool result = false;
			foreach (global::System.Text.RegularExpressions.Regex item in exclusions_)
			{
				if (item.IsMatch(name))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		public bool IsMatch(string name)
		{
			if (IsIncluded(name))
			{
				return !IsExcluded(name);
			}
			return false;
		}

		private void Compile()
		{
			if (filter_ == null)
			{
				return;
			}
			string[] array = SplitQuoted(filter_);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null && array[i].Length > 0)
				{
					bool flag = array[i][0] != '-';
					string pattern = ((array[i][0] == '+') ? array[i].Substring(1, array[i].Length - 1) : ((array[i][0] != '-') ? array[i] : array[i].Substring(1, array[i].Length - 1)));
					if (flag)
					{
						inclusions_.Add(new global::System.Text.RegularExpressions.Regex(pattern, (global::System.Text.RegularExpressions.RegexOptions)25));
					}
					else
					{
						exclusions_.Add(new global::System.Text.RegularExpressions.Regex(pattern, (global::System.Text.RegularExpressions.RegexOptions)25));
					}
				}
			}
		}
	}
}

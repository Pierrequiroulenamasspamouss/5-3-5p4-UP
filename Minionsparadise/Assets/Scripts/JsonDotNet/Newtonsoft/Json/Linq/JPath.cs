namespace Newtonsoft.Json.Linq
{
	internal class JPath
	{
		private readonly string _expression;

		private int _currentIndex;

		public global::System.Collections.Generic.List<object> Parts { get; private set; }

		public JPath(string expression)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(expression, "expression");
			_expression = expression;
			Parts = new global::System.Collections.Generic.List<object>();
			ParseMain();
		}

		private void ParseMain()
		{
			int num = _currentIndex;
			bool flag = false;
			while (_currentIndex < _expression.Length)
			{
				char c = _expression[_currentIndex];
				switch (c)
				{
				case '(':
				case '[':
					if (_currentIndex > num)
					{
						string item2 = _expression.Substring(num, _currentIndex - num);
						Parts.Add(item2);
					}
					ParseIndexer(c);
					num = _currentIndex + 1;
					flag = true;
					break;
				case ')':
				case ']':
					throw new global::System.Exception("Unexpected character while parsing path: " + c);
				case '.':
					if (_currentIndex > num)
					{
						string item = _expression.Substring(num, _currentIndex - num);
						Parts.Add(item);
					}
					num = _currentIndex + 1;
					flag = false;
					break;
				default:
					if (flag)
					{
						throw new global::System.Exception("Unexpected character following indexer: " + c);
					}
					break;
				}
				_currentIndex++;
			}
			if (_currentIndex > num)
			{
				string item3 = _expression.Substring(num, _currentIndex - num);
				Parts.Add(item3);
			}
		}

		private void ParseIndexer(char indexerOpenChar)
		{
			_currentIndex++;
			char c = ((indexerOpenChar == '[') ? ']' : ')');
			int currentIndex = _currentIndex;
			int num = 0;
			bool flag = false;
			while (_currentIndex < _expression.Length)
			{
				char c2 = _expression[_currentIndex];
				if (char.IsDigit(c2))
				{
					num++;
					_currentIndex++;
					continue;
				}
				if (c2 == c)
				{
					flag = true;
					break;
				}
				throw new global::System.Exception("Unexpected character while parsing path indexer: " + c2);
			}
			if (!flag)
			{
				throw new global::System.Exception("Path ended with open indexer. Expected " + c);
			}
			if (num == 0)
			{
				throw new global::System.Exception("Empty path indexer.");
			}
			string value = _expression.Substring(currentIndex, num);
			Parts.Add(global::System.Convert.ToInt32(value, global::System.Globalization.CultureInfo.InvariantCulture));
		}

		internal global::Newtonsoft.Json.Linq.JToken Evaluate(global::Newtonsoft.Json.Linq.JToken root, bool errorWhenNoMatch)
		{
			global::Newtonsoft.Json.Linq.JToken jToken = root;
			foreach (object part in Parts)
			{
				string text = part as string;
				if (text != null)
				{
					global::Newtonsoft.Json.Linq.JObject jObject = jToken as global::Newtonsoft.Json.Linq.JObject;
					if (jObject != null)
					{
						jToken = jObject[text];
						if (jToken == null && errorWhenNoMatch)
						{
							throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Property '{0}' does not exist on JObject.", global::System.Globalization.CultureInfo.InvariantCulture, text));
						}
						continue;
					}
					if (errorWhenNoMatch)
					{
						throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Property '{0}' not valid on {1}.", global::System.Globalization.CultureInfo.InvariantCulture, text, jToken.GetType().Name));
					}
					return null;
				}
				int num = (int)part;
				global::Newtonsoft.Json.Linq.JArray jArray = jToken as global::Newtonsoft.Json.Linq.JArray;
				if (jArray != null)
				{
					if (jArray.Count <= num)
					{
						if (errorWhenNoMatch)
						{
							throw new global::System.IndexOutOfRangeException(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Index {0} outside the bounds of JArray.", global::System.Globalization.CultureInfo.InvariantCulture, num));
						}
						return null;
					}
					jToken = jArray[num];
					continue;
				}
				if (errorWhenNoMatch)
				{
					throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Index {0} not valid on {1}.", global::System.Globalization.CultureInfo.InvariantCulture, num, jToken.GetType().Name));
				}
				return null;
			}
			return jToken;
		}
	}
}

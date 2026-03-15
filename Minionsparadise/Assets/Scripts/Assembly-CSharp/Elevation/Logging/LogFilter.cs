namespace Elevation.Logging
{
	public class LogFilter
	{
		private global::System.Text.RegularExpressions.Regex _regex;

		public string Pattern { get; private set; }

		public global::Elevation.Logging.FilterType Type { get; private set; }

		public bool Inclusive { get; private set; }

		public LogFilter(string pattern, global::Elevation.Logging.FilterType type, bool inclusive)
		{
			Pattern = pattern;
			Type = type;
			Inclusive = inclusive;
			if (!string.IsNullOrEmpty(pattern))
			{
				_regex = new global::System.Text.RegularExpressions.Regex(pattern, global::System.Text.RegularExpressions.RegexOptions.Singleline);
			}
		}

		public bool IsMatch(global::Elevation.Logging.LogEvent logEvent)
		{
			if (_regex == null)
			{
				return true;
			}
			string text;
			switch (Type)
			{
			default:
				text = logEvent.FormattedMessage;
				break;
			case global::Elevation.Logging.FilterType.Scope:
				text = logEvent.Scope.ToString();
				break;
			case global::Elevation.Logging.FilterType.ClassName:
				text = logEvent.ClassName;
				break;
			case global::Elevation.Logging.FilterType.MethodName:
				text = logEvent.MethodName;
				break;
			case global::Elevation.Logging.FilterType.ClassAndMethodNames:
				text = logEvent.ClassAndMethodName;
				break;
			}
			if (text == null)
			{
				return false;
			}
			return _regex.IsMatch(text);
		}
	}
}

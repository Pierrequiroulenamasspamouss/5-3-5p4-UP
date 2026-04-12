namespace Kampai.Main
{
	public class LocalQuantityString : global::Kampai.Main.ILocalString
	{
		private const char QUANTITY_DELIM = '#';

		private const string INVALID_ARGS = "INVALID ARGS";

		private string single;

		private string multiple;

		public LocalQuantityString(string single, string multiple)
		{
			this.single = single;
			this.multiple = multiple;
		}

		public string GetStringFormat(params object[] args)
		{
			if (args.Length == 0)
			{
				return "INVALID ARGS";
			}
			if (args.Length == 1 && global::System.Convert.ToInt32(args[0]) == 1)
			{
				return single;
			}
			int num = 0;
			int num2 = multiple.IndexOf('#');
			string text = multiple;
			while (num2 != -1)
			{
				if (num > args.Length)
				{
					return "INVALID ARGS";
				}
				string arg = text.Substring(0, num2);
				string arg2 = text.Substring(num2 + 1);
				text = string.Format("{0}{1}{2}", arg, global::System.Convert.ToInt32(args[num++]), arg2);
				num2 = text.IndexOf('#');
			}
			if (num < args.Length)
			{
				return "INVALID ARGS";
			}
			return text;
		}
	}
}

namespace Kampai.Util
{
	public class FatalException : global::System.Exception
	{
		public global::Kampai.Util.FatalCode FatalCode;

		public int ReferencedId;

		public FatalException(global::Kampai.Util.FatalCode fatalCode, int referenceId, global::System.Exception inner, string format, params object[] args)
			: base((format == null) ? string.Empty : string.Format(format, args), inner)
		{
			FatalCode = fatalCode;
			ReferencedId = referenceId;
		}

		public FatalException(global::Kampai.Util.FatalCode fatalCode, string format, params object[] args)
			: this(fatalCode, 0, null, format, args)
		{
		}

		public FatalException(global::Kampai.Util.FatalCode fatalCode, global::System.Exception inner, string format, params object[] args)
			: this(fatalCode, 0, inner, format, args)
		{
		}

		public FatalException(global::Kampai.Util.FatalCode fatalCode, int referenceId, string format, params object[] args)
			: this(fatalCode, referenceId, null, format, args)
		{
		}

		public FatalException(global::Kampai.Util.FatalCode fatalCode, int referenceId)
			: this(fatalCode, referenceId, null, string.Empty)
		{
		}

		public override string ToString()
		{
			return string.Format("FatalCode = {0}, ReferencedId = {1}, {2}", FatalCode, ReferencedId, base.ToString());
		}
	}
}

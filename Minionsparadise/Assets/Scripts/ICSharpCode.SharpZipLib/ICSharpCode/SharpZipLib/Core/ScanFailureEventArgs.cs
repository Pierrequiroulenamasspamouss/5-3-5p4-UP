namespace ICSharpCode.SharpZipLib.Core
{
	public class ScanFailureEventArgs : global::System.EventArgs
	{
		private string name_;

		private global::System.Exception exception_;

		private bool continueRunning_;

		public string Name
		{
			get
			{
				return name_;
			}
		}

		public global::System.Exception Exception
		{
			get
			{
				return exception_;
			}
		}

		public bool ContinueRunning
		{
			get
			{
				return continueRunning_;
			}
			set
			{
				continueRunning_ = value;
			}
		}

		public ScanFailureEventArgs(string name, global::System.Exception e)
		{
			name_ = name;
			exception_ = e;
			continueRunning_ = true;
		}
	}
}

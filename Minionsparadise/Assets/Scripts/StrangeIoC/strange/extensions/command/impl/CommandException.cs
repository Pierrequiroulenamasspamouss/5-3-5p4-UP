namespace strange.extensions.command.impl
{
	public class CommandException : global::System.Exception
	{
		public global::strange.extensions.command.api.CommandExceptionType type { get; set; }

		public CommandException()
		{
		}

		public CommandException(string message, global::strange.extensions.command.api.CommandExceptionType exceptionType)
			: base(message)
		{
			type = exceptionType;
		}
	}
}

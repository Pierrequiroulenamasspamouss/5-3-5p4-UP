namespace strange.extensions.command.api
{
	public interface ICommand
	{
		bool IsClean { get; set; }

		bool retain { get; }

		bool cancelled { get; set; }

		object data { get; set; }

		int sequenceId { get; set; }

		void Execute();

		void Retain();

		void Release();

		void Fail();

		void Cancel();
	}
}

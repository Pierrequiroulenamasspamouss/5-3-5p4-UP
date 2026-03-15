namespace strange.extensions.command.impl
{
	public class Command : global::strange.extensions.command.api.ICommand, global::strange.extensions.pool.api.IPoolable
	{
		[Inject]
		public global::strange.extensions.command.api.ICommandBinder commandBinder { get; set; }

		[Inject]
		public global::strange.extensions.injector.api.IInjectionBinder injectionBinder { get; set; }

		public object data { get; set; }

		public bool cancelled { get; set; }

		public bool IsClean { get; set; }

		public int sequenceId { get; set; }

		public bool retain { get; set; }

		public Command()
		{
			IsClean = false;
		}

		public virtual void Execute()
		{
			throw new global::strange.extensions.command.impl.CommandException("You must override the Execute method in every Command", global::strange.extensions.command.api.CommandExceptionType.EXECUTE_OVERRIDE);
		}

		public virtual void Retain()
		{
			retain = true;
		}

		public virtual void Release()
		{
			retain = false;
			if (commandBinder != null)
			{
				commandBinder.ReleaseCommand(this);
			}
		}

		public virtual void Restore()
		{
			injectionBinder.injector.Uninject(this);
			IsClean = true;
		}

		public virtual void Fail()
		{
			if (commandBinder != null)
			{
				commandBinder.Stop(this);
			}
		}

		public void Cancel()
		{
			cancelled = true;
		}
	}
}

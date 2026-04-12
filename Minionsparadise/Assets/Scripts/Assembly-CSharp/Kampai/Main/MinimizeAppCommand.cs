namespace Kampai.Main
{
	public class MinimizeAppCommand : global::strange.extensions.command.impl.Command
	{
		public override void Execute()
		{
			global::Kampai.Util.Native.Exit();
		}
	}
}

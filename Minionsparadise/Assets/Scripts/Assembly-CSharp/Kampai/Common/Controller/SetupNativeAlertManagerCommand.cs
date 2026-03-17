namespace Kampai.Common.Controller
{
	public class SetupNativeAlertManagerCommand : global::strange.extensions.command.impl.Command
	{
		public override void Execute()
		{
			NativeAlertManager.Init();
		}
	}
}

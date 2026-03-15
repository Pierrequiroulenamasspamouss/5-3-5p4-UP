namespace Kampai.Common
{
	public class OpenRateAppPageCommand : global::strange.extensions.command.impl.Command
	{
		public override void Execute()
		{
			global::UnityEngine.Application.OpenURL("market://details?id=com.ea.gp.minions");
		}
	}
}

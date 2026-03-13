namespace Kampai.UI.Controller
{
	public class ShowForcedClientUpdateScreenCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Splash.IDownloadService downloadService { get; set; }

		public override void Execute()
		{
			if (downloadService != null)
			{
				downloadService.Shutdown();
			}
			global::UnityEngine.SceneManagement.SceneManager.LoadScene("ForcedUpgrade");
		}
	}
}

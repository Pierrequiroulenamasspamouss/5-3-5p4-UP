namespace Kampai.Main
{
	public class SetupDeepLinkCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SetupDeepLinkCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject(global::Kampai.Main.MainElement.MANAGER_PARENT)]
		public global::UnityEngine.GameObject managers { get; set; }

		[Inject]
		public global::Kampai.UI.View.MoveBuildMenuSignal moveBuildMenuSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowMTXStoreSignal showMTXStoreSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		public override void Execute()
		{
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("DeepLink");
			gameObject.transform.parent = managers.transform;
			gameObject.SetActive(false);
			global::Kampai.UI.View.DeepLinkHandler deepLinkHandler = gameObject.AddComponent<global::Kampai.UI.View.DeepLinkHandler>();
			deepLinkHandler.logger = logger;
			deepLinkHandler.moveBuildMenuSignal = moveBuildMenuSignal;
			deepLinkHandler.showMTXStoreSignal = showMTXStoreSignal;
			deepLinkHandler.cloneUserFromEnvSignal = gameContext.injectionBinder.GetInstance<global::Kampai.Game.CloneUserFromEnvSignal>();
			gameObject.SetActive(true);
		}
	}
}

namespace Kampai.Main
{
	internal sealed class SetupEventSystemCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SetupEventSystemCommand") as global::Kampai.Util.IKampaiLogger;

		private global::UnityEngine.GameObject eventSystem;

		[Inject]
		public global::Kampai.UI.View.LoadUICompleteSignal uiLoadCompleteSignal { get; set; }

		public override void Execute()
		{
			logger.EventStart("SetupEventSystemCommand.Execute");
			eventSystem = new global::UnityEngine.GameObject("EventSystem");
			eventSystem.AddComponent<global::UnityEngine.EventSystems.EventSystem>();
			eventSystem.AddComponent<global::UnityEngine.EventSystems.StandaloneInputModule>();
			eventSystem.AddComponent<global::Kampai.Game.KampaiTouchInputModule>();
			base.injectionBinder.Bind<global::UnityEngine.GameObject>().ToValue(eventSystem).ToName(global::Kampai.Main.MainElement.UI_EVENTSYSTEM)
				.CrossContext();
			uiLoadCompleteSignal.AddListener(SetParent);
			logger.EventStop("SetupEventSystemCommand.Execute");
		}

		private void SetParent(global::UnityEngine.GameObject contextView)
		{
			if (!(eventSystem == null))
			{
				eventSystem.transform.parent = contextView.transform;
			}
		}
	}
}

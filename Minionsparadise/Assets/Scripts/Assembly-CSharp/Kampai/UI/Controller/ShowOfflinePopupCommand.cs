namespace Kampai.UI.Controller
{
	public class ShowOfflinePopupCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("ShowOfflinePopupCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public bool isShow { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetupCanvasSignal setupCanvasSignal { get; set; }

		[Inject(global::Kampai.Splash.SplashElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable splashContext { get; set; }

		public override void Execute()
		{
			global::Kampai.UI.View.IGUIService iGUIService = null;
			try
			{
				iGUIService = splashContext.injectionBinder.GetInstance<global::Kampai.UI.View.IGUIService>();
			}
			catch (global::strange.extensions.injector.impl.InjectionException ex)
			{
				logger.Debug("Caught exception when attempting to Get GUIService: {0}", ex);
			}
			if (iGUIService != null)
			{
				if (isShow)
				{
					global::Kampai.UI.View.IGUICommand command = iGUIService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "popup_Error_LostConnectivity");
					iGUIService.Execute(command);
				}
				else
				{
					iGUIService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "popup_Error_LostConnectivity");
				}
				return;
			}
			global::UnityEngine.Canvas overlayCanvas = GetOverlayCanvas();
			if (overlayCanvas == null && !isShow)
			{
				return;
			}
			global::Kampai.UI.View.OfflineView[] componentsInChildren = overlayCanvas.gameObject.GetComponentsInChildren<global::Kampai.UI.View.OfflineView>(true);
			if (componentsInChildren.Length == 0)
			{
				if (isShow)
				{
					string path = "UI/popup_Error_LostConnectivity";
					global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(global::UnityEngine.Resources.Load(path)) as global::UnityEngine.GameObject;
					gameObject.transform.SetParent(overlayCanvas.transform, false);
				}
			}
			else if (!isShow)
			{
				global::UnityEngine.Object.Destroy(componentsInChildren[0].gameObject);
			}
			else if (!componentsInChildren[0].gameObject.activeSelf)
			{
				componentsInChildren[0].gameObject.SetActive(true);
			}
			else if (isShow)
			{
				string path2 = "UI/popup_Error_LostConnectivity";
				global::UnityEngine.GameObject gameObject2 = global::UnityEngine.Object.Instantiate(global::UnityEngine.Resources.Load(path2)) as global::UnityEngine.GameObject;
				gameObject2.transform.SetParent(overlayCanvas.transform, false);
			}
		}

		private global::UnityEngine.Canvas GetOverlayCanvas()
		{
			global::UnityEngine.GameObject gameObject = null;
			global::strange.extensions.injector.api.IInjectionBinding binding = base.injectionBinder.GetBinding<global::UnityEngine.GameObject>(global::Kampai.Main.MainElement.UI_OVERLAY_CANVAS);
			if (binding == null || binding.value == null || ((global::UnityEngine.GameObject)binding.value).gameObject == null)
			{
				if (binding != null)
				{
					base.injectionBinder.Unbind(binding);
				}
				if (isShow)
				{
					setupCanvasSignal.Dispatch(global::Kampai.Util.Tuple.Create<string, global::Kampai.Main.MainElement, global::UnityEngine.Camera>("OverlayCanvas", global::Kampai.Main.MainElement.UI_OVERLAY_CANVAS, null));
					gameObject = base.injectionBinder.GetInstance<global::UnityEngine.GameObject>(global::Kampai.Main.MainElement.UI_OVERLAY_CANVAS);
				}
			}
			else
			{
				gameObject = (global::UnityEngine.GameObject)binding.value;
			}
			return (!(gameObject != null)) ? null : gameObject.GetComponent<global::UnityEngine.Canvas>();
		}
	}
}

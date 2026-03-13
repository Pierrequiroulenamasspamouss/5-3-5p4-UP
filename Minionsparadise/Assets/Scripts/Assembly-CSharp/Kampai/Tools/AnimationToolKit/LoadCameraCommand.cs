namespace Kampai.Tools.AnimationToolKit
{
	public class LoadCameraCommand : global::strange.extensions.command.impl.Command
	{
		public override void Execute()
		{
			global::UnityEngine.Camera main = global::UnityEngine.Camera.main;
			base.injectionBinder.Bind<global::UnityEngine.Camera>().ToValue(main);
			global::Kampai.Game.View.CameraUtils o = main.gameObject.AddComponent<global::Kampai.Game.View.CameraUtils>();
			base.injectionBinder.Bind<global::Kampai.Game.View.CameraUtils>().ToValue(o).ToSingleton();
			main.gameObject.AddComponent<global::Kampai.Game.View.TouchPanView>();
			main.gameObject.AddComponent<global::Kampai.Game.View.TouchZoomView>();
			main.gameObject.AddComponent<global::Kampai.Game.View.TouchDragPanView>();
		}
	}
}

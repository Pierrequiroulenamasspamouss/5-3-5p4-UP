namespace Kampai.Tools.AnimationToolKit
{
	public class AnimationToolKitButtonView : global::Kampai.Util.KampaiView
	{
		public global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType ButtonType;

		public global::strange.extensions.signal.impl.Signal<global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType> ButtonPressSignal = new global::strange.extensions.signal.impl.Signal<global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType>();

		public void OnButtonPress()
		{
			ButtonPressSignal.Dispatch(ButtonType);
		}
	}
}

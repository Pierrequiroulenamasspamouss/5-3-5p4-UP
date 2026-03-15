namespace Kampai.UI.View
{
	public class ToggleButtonView : global::Kampai.Util.KampaiView
	{
		public global::strange.extensions.signal.impl.Signal<bool> OnValueChangedSignal = new global::strange.extensions.signal.impl.Signal<bool>();

		public bool IsOn { get; private set; }

		public virtual void OnValueChanged(bool isOff)
		{
			OnValueChangedSignal.Dispatch(isOff);
			IsOn = !isOff;
		}
	}
}

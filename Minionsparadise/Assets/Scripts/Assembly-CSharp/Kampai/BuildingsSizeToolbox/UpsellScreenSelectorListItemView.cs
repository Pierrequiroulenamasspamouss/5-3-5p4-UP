namespace Kampai.BuildingsSizeToolbox
{
	public class UpsellScreenSelectorListItemView : global::UnityEngine.MonoBehaviour
	{
		public global::UnityEngine.UI.Text ScreenName;

		public global::strange.extensions.signal.impl.Signal<string> ClickedSignal = new global::strange.extensions.signal.impl.Signal<string>();

		public void OnClick()
		{
			ClickedSignal.Dispatch(ScreenName.text);
		}
	}
}

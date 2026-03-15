namespace Kampai.Splash
{
	public class LoadInTipView : global::strange.extensions.mediation.impl.View
	{
		internal void SetTip(string tip)
		{
			global::UnityEngine.GameObject gameObject = base.gameObject.FindChild("txt_ToolTip");
			gameObject.GetComponent<global::UnityEngine.UI.Text>().text = tip;
		}
	}
}

namespace Kampai.Tools.AnimationToolKit
{
	public class GachaButtonView : global::Kampai.Util.KampaiView
	{
		private global::Kampai.Game.AnimationDefinition def;

		public global::strange.extensions.signal.impl.Signal<global::Kampai.Game.AnimationDefinition> FireGachaSignal = new global::strange.extensions.signal.impl.Signal<global::Kampai.Game.AnimationDefinition>();

		public void OnButtonPress()
		{
			FireGachaSignal.Dispatch(def);
		}

		public void SetGachaDefinition(global::Kampai.Game.AnimationDefinition def)
		{
			this.def = def;
		}
	}
}

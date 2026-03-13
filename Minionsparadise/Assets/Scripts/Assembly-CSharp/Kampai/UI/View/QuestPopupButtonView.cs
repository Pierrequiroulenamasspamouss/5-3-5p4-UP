namespace Kampai.UI.View
{
	public class QuestPopupButtonView : global::Kampai.UI.View.ButtonView
	{
		public new global::strange.extensions.signal.impl.Signal<global::System.Collections.Generic.List<global::Kampai.Game.DisplayableDefinition>> ClickedSignal = new global::strange.extensions.signal.impl.Signal<global::System.Collections.Generic.List<global::Kampai.Game.DisplayableDefinition>>();

		public global::System.Collections.Generic.List<global::Kampai.Game.DisplayableDefinition> questRewards = new global::System.Collections.Generic.List<global::Kampai.Game.DisplayableDefinition>();

		public override void OnClickEvent()
		{
			if (PlaySoundOnClick)
			{
				base.playSFXSignal.Dispatch("Play_button_click_01");
			}
			ClickedSignal.Dispatch(questRewards);
		}
	}
}

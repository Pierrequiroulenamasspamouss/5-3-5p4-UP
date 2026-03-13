namespace Kampai.UI.View
{
	public class MasterPlanOnboardingView : global::Kampai.UI.View.PopupMenuView
	{
		public global::Kampai.UI.View.LocalizeView titleText;

		public global::Kampai.UI.View.ButtonView actionButtonView;

		public global::Kampai.UI.View.LocalizeView actionButtonText;

		internal global::Kampai.Game.MasterPlanOnboardDefinition definition;

		internal global::Kampai.Game.IDefinitionService definitionService;

		internal bool IsLast
		{
			get
			{
				return !definitionService.Has<global::Kampai.Game.MasterPlanOnboardDefinition>(definition.nextOnboardDefinitionId);
			}
		}

		internal void Initialize()
		{
			if (definition != null)
			{
				titleText.LocKey = definition.LocalizedKey;
				actionButtonText.LocKey = ((!IsLast) ? "Next" : "PlayerTrainingGotIt");
			}
		}
	}
}

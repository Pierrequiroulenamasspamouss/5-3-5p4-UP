namespace Kampai.UI.View
{
	public class HideHUDAndIconsCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public bool isVisible { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetBuildMenuEnabledSignal setBuildMenuEnabledSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetHUDButtonsVisibleSignal setHUDButtonsVisibleSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowAllWayFindersSignal showAllWayFindersSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideAllWayFindersSignal hideAllWayFindersSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowAllResourceIconsSignal showAllResourceIconsSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideAllResourceIconsSignal hideAllResourceIconsSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ToggleAllFloatingTextSignal toggleAllFloatingTextSignal { get; set; }

		public override void Execute()
		{
			setBuildMenuEnabledSignal.Dispatch(isVisible);
			setHUDButtonsVisibleSignal.Dispatch(isVisible);
			toggleAllFloatingTextSignal.Dispatch(isVisible);
			if (isVisible)
			{
				showAllWayFindersSignal.Dispatch();
				showAllResourceIconsSignal.Dispatch();
			}
			else
			{
				hideAllWayFindersSignal.Dispatch();
				hideAllResourceIconsSignal.Dispatch();
			}
		}
	}
}

namespace Kampai.UI
{
	public interface IGhostComponentService
	{
		global::Kampai.Game.View.BuildingObject DisplayGhostBuilding(int componentDefID, global::Kampai.UI.GhostBuildingDisplayType displayType);

		void DisplayAllSelectablePlanComponents();

		void DisplayComponentMarkedAsInProgress(global::Kampai.Game.MasterPlanComponent component);

		bool DisplayAutoCloseGhostComponent(int componentBuildingDefID, float fadeTime, float openDuration);

		void DisplayZoomedInComponent(int componentID, bool isRegularBuilding);

		void ClearGhostComponentBuildings(bool alsoClearComponentsInProgress = false, bool immediate = false);

		void GhostBuildingAutoRemoved(int id, GhostComponentFadeHelperObject helper);

		void RunBeginGhostComponentFunctionFromDefinition(global::Kampai.UI.GhostComponentFunctionType functionType, int defID = 0);

		void RunEndGhostComponentFunctionFromDefinition(global::Kampai.UI.GhostFunctionCloseType closeType);
	}
}

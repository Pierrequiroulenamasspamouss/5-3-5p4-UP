namespace Kampai.UI.View
{
	public class MoveBuildingWayFinderView : global::Kampai.UI.View.AbstractWayFinderView
	{
		protected override string UIName
		{
			get
			{
				return "MoveBuildingWayFinder";
			}
		}

		protected override string WayFinderDefaultIcon
		{
			get
			{
				return wayFinderDefinition.ConnectableIcon;
			}
		}

		protected override bool OnCanUpdate()
		{
			return true;
		}

		protected override void OnInvisible(global::UnityEngine.Vector3 direction)
		{
			Snappable = true;
			AvoidsHUD = false;
			isForceHideEnabled = false;
			base.OnInvisible(direction);
		}

		protected override void OnVisible()
		{
			base.OnVisible();
			isForceHideEnabled = true;
		}

		protected override void LoadModalData(global::Kampai.UI.View.WorldToGlassUIModal modal)
		{
			base.LoadModalData(modal);
			if (targetObject == null)
			{
				global::Kampai.Game.View.BuildingManagerMediator component = gameContext.injectionBinder.GetInstance<global::UnityEngine.GameObject>(global::Kampai.Game.GameElement.BUILDING_MANAGER).GetComponent<global::Kampai.Game.View.BuildingManagerMediator>();
				targetObject = component.GetCurrentDummyBuilding();
			}
		}

		public override void SetForceHide(bool forceHide)
		{
		}

		public override global::UnityEngine.Vector3 GetIndicatorPosition()
		{
			global::Kampai.Game.View.BuildingDefinitionObject buildingDefinitionObject = targetObject as global::Kampai.Game.View.BuildingDefinitionObject;
			if (buildingDefinitionObject != null)
			{
				return buildingDefinitionObject.ResourceIconPosition;
			}
			return global::UnityEngine.Vector3.zero;
		}

		public global::Kampai.Game.View.BuildingDefinitionObject GetTargetObject()
		{
			return targetObject as global::Kampai.Game.View.BuildingDefinitionObject;
		}
	}
}

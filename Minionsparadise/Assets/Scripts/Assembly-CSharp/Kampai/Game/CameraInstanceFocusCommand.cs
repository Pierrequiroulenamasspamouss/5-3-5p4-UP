namespace Kampai.Game
{
	public class CameraInstanceFocusCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int buildingId { get; set; }

		[Inject]
		public global::UnityEngine.Vector3 focusPosition { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.Game.ShowHiddenBuildingsSignal showHiddenBuildingsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ToggleMinionRendererSignal toggleMinionSignal { get; set; }

		public override void Execute()
		{
			showHiddenBuildingsSignal.Dispatch();
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Util.FadeMaterialBlock fadeMaterialBlock = component.gameObject.GetComponent<global::Kampai.Util.FadeMaterialBlock>();
			if (fadeMaterialBlock == null)
			{
				fadeMaterialBlock = component.gameObject.AddComponent<global::Kampai.Util.FadeMaterialBlock>();
			}
			global::System.Collections.Generic.LinkedList<global::Kampai.Game.View.ActionableObject> linkedList = new global::System.Collections.Generic.LinkedList<global::Kampai.Game.View.ActionableObject>();
			global::System.Collections.Generic.LinkedList<global::Kampai.Game.View.ActionableObject> linkedList2 = new global::System.Collections.Generic.LinkedList<global::Kampai.Game.View.ActionableObject>();
			component.GetOccludingObjects(focusPosition, buildingId, linkedList, linkedList2);
			global::Kampai.Game.View.TaskableBuildingObject taskableBuildingObject = null;
			global::Kampai.Game.View.LeisureBuildingObjectView leisureBuildingObjectView = null;
			global::System.Collections.Generic.List<global::UnityEngine.Renderer> list = new global::System.Collections.Generic.List<global::UnityEngine.Renderer>();
			foreach (global::Kampai.Game.View.ActionableObject item in linkedList)
			{
				if (item.ID != buildingId && item.CanFadeGFX() && item.CanFadeSFX())
				{
					list.AddRange(item.gameObject.GetComponentsInChildren<global::UnityEngine.Renderer>());
					item.gfxFaded = true;
					taskableBuildingObject = item as global::Kampai.Game.View.TaskableBuildingObject;
					leisureBuildingObjectView = item as global::Kampai.Game.View.LeisureBuildingObjectView;
					if (taskableBuildingObject != null)
					{
						taskableBuildingObject.FadeMinions(toggleMinionSignal, false);
					}
					else if (leisureBuildingObjectView != null)
					{
						leisureBuildingObjectView.FadeMinions(toggleMinionSignal, false);
					}
					item.FadeSFX(0.5f, false);
				}
			}
			if (fadeMaterialBlock != null && list.Count > 0)
			{
				fadeMaterialBlock.StartFade(false, 0.5f, list);
			}
			foreach (global::Kampai.Game.View.ActionableObject item2 in linkedList2)
			{
				if (item2.ID != buildingId)
				{
					item2.FadeSFX(0.5f, false);
				}
			}
		}
	}
}

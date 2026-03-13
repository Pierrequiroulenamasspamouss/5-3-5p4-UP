namespace Kampai.Game
{
	public class ShowHiddenBuildingCommand : global::strange.extensions.command.impl.Command
	{
		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.Game.ToggleMinionRendererSignal toggleMinionSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::System.Collections.Generic.ICollection<global::Kampai.Game.View.ActionableObject> fadedObjects = component.GetFadedObjects();
			global::Kampai.Util.FadeMaterialBlock component2 = component.gameObject.GetComponent<global::Kampai.Util.FadeMaterialBlock>();
			global::Kampai.Game.View.TaskableBuildingObject taskableBuildingObject = null;
			global::Kampai.Game.View.LeisureBuildingObjectView leisureBuildingObjectView = null;
			global::System.Collections.Generic.List<global::UnityEngine.Renderer> list = new global::System.Collections.Generic.List<global::UnityEngine.Renderer>();
			foreach (global::Kampai.Game.View.ActionableObject item in fadedObjects)
			{
				if (item.gfxFaded && item.CanFadeGFX())
				{
					list.AddRange(item.gameObject.GetComponentsInChildren<global::UnityEngine.Renderer>());
					item.gfxFaded = false;
					taskableBuildingObject = item as global::Kampai.Game.View.TaskableBuildingObject;
					leisureBuildingObjectView = item as global::Kampai.Game.View.LeisureBuildingObjectView;
					if (taskableBuildingObject != null)
					{
						taskableBuildingObject.FadeMinions(toggleMinionSignal, true);
					}
					else if (leisureBuildingObjectView != null)
					{
						leisureBuildingObjectView.FadeMinions(toggleMinionSignal, true);
					}
				}
				if (item.CanFadeSFX())
				{
					item.FadeSFX(0.5f, true);
				}
			}
			if (component2 != null && list.Count > 0)
			{
				component2.StartFade(true, 0.5f, list);
			}
		}
	}
}

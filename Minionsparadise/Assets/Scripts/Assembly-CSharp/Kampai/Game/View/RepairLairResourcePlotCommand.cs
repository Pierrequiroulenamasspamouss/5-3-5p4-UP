namespace Kampai.Game.View
{
	public class RepairLairResourcePlotCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.VillainLairResourcePlot resourcePlot { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.Game.ParentLairResourcePlotSignal parentLairResourcePlotSignal { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal buildingChangeStateSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.VillainLairResourcePlotObjectView villainLairResourcePlotObjectView = component.GetBuildingObject(resourcePlot.ID) as global::Kampai.Game.View.VillainLairResourcePlotObjectView;
			global::UnityEngine.GameObject gameObject = villainLairResourcePlotObjectView.gameObject;
			global::UnityEngine.GameObject gameObject2 = gameObject.FindChild(string.Format("{0}(Clone)", resourcePlot.Definition.brokenPrefab_loaded));
			if (gameObject2 != null)
			{
				global::UnityEngine.Object.DestroyImmediate(gameObject2);
			}
			global::UnityEngine.GameObject original = global::Kampai.Util.KampaiResources.Load(resourcePlot.Definition.prefab_loaded) as global::UnityEngine.GameObject;
			global::UnityEngine.GameObject type = global::UnityEngine.Object.Instantiate(original, (global::UnityEngine.Vector3)resourcePlot.Location, global::UnityEngine.Quaternion.Euler(0f, resourcePlot.rotation, 0f)) as global::UnityEngine.GameObject;
			buildingChangeStateSignal.Dispatch(resourcePlot.ID, global::Kampai.Game.BuildingState.Idle);
			parentLairResourcePlotSignal.Dispatch(resourcePlot, type);
			villainLairResourcePlotObjectView.InitializeAnimators();
		}
	}
}

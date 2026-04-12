namespace Kampai.Game.View
{
	public class ParentLairResourcePlotCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.VillainLairResourcePlot resourcePlot { get; set; }

		[Inject]
		public global::UnityEngine.GameObject childInstance { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject(global::Kampai.Game.GameElement.MINION_MANAGER)]
		public global::UnityEngine.GameObject minionManager { get; set; }

		public override void Execute()
		{
			int iD = resourcePlot.ID;
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.VillainLairResourcePlotObjectView villainLairResourcePlotObjectView = component.GetBuildingObject(iD) as global::Kampai.Game.View.VillainLairResourcePlotObjectView;
			if (villainLairResourcePlotObjectView != null)
			{
				global::UnityEngine.GameObject gameObject = villainLairResourcePlotObjectView.gameObject;
				childInstance.transform.SetParent(gameObject.transform);
				global::Kampai.Game.View.MinionObject mo = null;
				if (resourcePlot.MinionIsTaskedToBuilding())
				{
					global::Kampai.Game.View.MinionManagerView component2 = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerView>();
					mo = component2.Get(resourcePlot.MinionIDInBuilding);
				}
				villainLairResourcePlotObjectView.UpdateRoutes(childInstance, mo);
				villainLairResourcePlotObjectView.UpdateRenderers();
			}
		}
	}
}

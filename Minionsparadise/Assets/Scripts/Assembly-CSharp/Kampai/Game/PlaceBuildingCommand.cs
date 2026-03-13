namespace Kampai.Game
{
	internal sealed class PlaceBuildingCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int buildingId { get; set; }

		[Inject]
		public global::Kampai.Game.Location location { get; set; }

		[Inject]
		public global::Kampai.Game.RerouteMinionPathsSignal rerouteSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ShowBuildingFootprintSignal showFootprintSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RemoveBuildingSignal removeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.AddFootprintSignal addFootprintSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IBuildingUtilities buildingUtil { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.DebugUpdateGridSignal debugUpdateGridSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.DecoGridModel decoGridModel { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateConnectablesSignal updateConnectablesSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Building byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Building>(buildingId);
			if (buildingUtil.ValidateLocation(byInstanceId, this.location))
			{
				global::Kampai.Game.ConnectableBuilding connectableBuilding = byInstanceId as global::Kampai.Game.ConnectableBuilding;
				global::Kampai.Game.ConnectableBuildingDefinition connectableBuildingDefinition = null;
				if (connectableBuilding != null)
				{
					connectableBuildingDefinition = connectableBuilding.Definition;
					global::Kampai.Game.Location location = byInstanceId.Location;
					decoGridModel.RemoveDeco(location.x, location.y);
					updateConnectablesSignal.Dispatch(location, connectableBuildingDefinition.connectableType);
				}
				showFootprintSignal.Dispatch(null, null, global::Kampai.Util.Tuple.Create(1, 1), false);
				string buildingFootprint = definitionService.GetBuildingFootprint(byInstanceId.Definition.FootprintID);
				removeSignal.Dispatch(byInstanceId.Location, buildingFootprint);
				if (byInstanceId.IsFootprintable)
				{
					addFootprintSignal.Dispatch(byInstanceId, this.location);
				}
				byInstanceId.Location = this.location;
				if (connectableBuilding != null && connectableBuildingDefinition != null)
				{
					decoGridModel.AddDeco(this.location.x, this.location.y, connectableBuildingDefinition.connectableType);
					updateConnectablesSignal.Dispatch(this.location, connectableBuildingDefinition.connectableType);
				}
				global::Kampai.Util.Point ll = default(global::Kampai.Util.Point);
				global::Kampai.Util.Point ur = default(global::Kampai.Util.Point);
				GetBounds(this.location, buildingFootprint, out ll, out ur);
				rerouteSignal.Dispatch(new global::Kampai.Util.Tuple<global::Kampai.Util.Point, global::Kampai.Util.Point>(ll, ur));
				debugUpdateGridSignal.Dispatch();
			}
		}

		private void GetBounds(global::Kampai.Game.Location loc, string footprint, out global::Kampai.Util.Point ll, out global::Kampai.Util.Point ur)
		{
			int num = loc.x;
			int num2 = loc.y;
			ll.x = (ll.y = int.MaxValue);
			ur.x = (ur.y = int.MinValue);
			for (int i = 0; i < footprint.Length; i++)
			{
				switch (footprint[i])
				{
				case '|':
					num = loc.x;
					num2--;
					continue;
				case '.':
					num++;
					continue;
				}
				if (num < ll.x)
				{
					ll.x = num;
				}
				if (num2 < ll.y)
				{
					ll.y = num2;
				}
				if (num > ur.x)
				{
					ur.x = num;
				}
				if (num2 > ur.y)
				{
					ur.y = num2;
				}
				num++;
			}
		}
	}
}

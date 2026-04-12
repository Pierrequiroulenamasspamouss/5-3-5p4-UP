namespace Kampai.Game
{
	public class PopulateBuildingCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.PlaceBuildingSignal placeBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DebugUpdateGridSignal DebugUpdateGridSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.IPlayerService instance = base.injectionBinder.GetInstance<global::Kampai.Game.IPlayerService>();
			foreach (global::Kampai.Game.Building item in instance.GetInstancesByType<global::Kampai.Game.Building>())
			{
				if (item.State != global::Kampai.Game.BuildingState.Inventory)
				{
					if (item.Location == null)
					{
						TryPatchBuildingLocation(item);
					}
					placeBuildingSignal.Dispatch(item.ID, item.Location);
				}
			}
			DebugUpdateGridSignal.Dispatch();
		}

		private void TryPatchBuildingLocation(global::Kampai.Game.Building building)
		{
			if (building == null)
			{
				return;
			}
			global::Kampai.Game.Definition definition = building.Definition;
			if (definition == null || building.Definition.Movable)
			{
				return;
			}
			string initialPlayer = definitionService.GetInitialPlayer();
			if (string.IsNullOrEmpty(initialPlayer))
			{
				return;
			}
			try
			{
				global::Kampai.Game.Player player = playerService.LoadPlayerData(initialPlayer);
				global::System.Collections.Generic.List<global::Kampai.Game.Instance> instancesByDefinitionID = player.GetInstancesByDefinitionID(definition.ID);
				if (instancesByDefinitionID.Count > 0 && instancesByDefinitionID[0] != null)
				{
					global::Kampai.Game.Locatable locatable = instancesByDefinitionID[0] as global::Kampai.Game.Locatable;
					if (locatable != null)
					{
						building.Location = locatable.Location;
					}
				}
			}
			catch (global::System.Exception)
			{
			}
		}
	}
}

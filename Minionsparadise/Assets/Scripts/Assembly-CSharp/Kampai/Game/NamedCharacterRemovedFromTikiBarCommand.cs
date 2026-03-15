namespace Kampai.Game
{
	public class NamedCharacterRemovedFromTikiBarCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.View.NamedCharacterObject namedCharacterObject { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.AddStuartToStageSignal addStuartToStageSignal { get; set; }

		[Inject]
		public global::Kampai.Game.FrolicSignal frolicSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PointBobLandExpansionSignal pointBobLandExpansionSignal { get; set; }

		[Inject]
		public global::Kampai.Game.StuartTunesGuitarSignal stuartTunesGuitarSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.NamedCharacter byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.NamedCharacter>(namedCharacterObject.ID);
			global::Kampai.Game.NamedCharacterDefinition definition = byInstanceId.Definition;
			if (byInstanceId is global::Kampai.Game.StuartCharacter)
			{
				global::Kampai.Game.StageBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.StageBuilding>(3054);
				if (firstInstanceByDefinitionId != null && firstInstanceByDefinitionId.IsBuildingRepaired())
				{
					global::Kampai.Game.Prestige firstInstanceByDefinitionId2 = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Prestige>(40003);
					if (firstInstanceByDefinitionId2.CurrentPrestigeLevel == 1)
					{
						base.injectionBinder.GetInstance<global::Kampai.Game.BuildingZoomSignal>().Dispatch(new global::Kampai.Game.BuildingZoomSettings(global::Kampai.Game.ZoomType.OUT, global::Kampai.Game.BuildingZoomType.TIKIBAR, null, false));
						addStuartToStageSignal.Dispatch(global::Kampai.Game.StuartStageAnimationType.IDLEOFFSTAGE);
						stuartTunesGuitarSignal.Dispatch();
					}
					else
					{
						frolicSignal.Dispatch(byInstanceId.ID);
					}
				}
				else
				{
					frolicSignal.Dispatch(byInstanceId.ID);
				}
			}
			else if (byInstanceId is global::Kampai.Game.BobCharacter)
			{
				pointBobLandExpansionSignal.Dispatch();
			}
			else if (definition.Location != null)
			{
				global::Kampai.Game.Location location = definition.Location;
				namedCharacterObject.transform.position = new global::UnityEngine.Vector3(location.x, 0f, location.y);
			}
		}
	}
}

namespace Kampai.Game
{
	public class GenerateTemporaryMinionsStageCommand : global::strange.extensions.command.impl.Command
	{
		public const float RANDOM_MINION_START_OFFSET_X = 4.8f;

		public const float RANDOM_MINION_START_OFFSET_Y = 0f;

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("GenerateTemporaryMinionsStageCommand") as global::Kampai.Util.IKampaiLogger;

		public int count;

		[Inject]
		public global::Kampai.Game.GenerateTemporaryMinionSignal generateTemporaryMinionSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.MoveMinionFinishedSignal moveMinionFinishedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.TemporaryMinionsService temporaryMinionsService { get; set; }

		public override void Execute()
		{
			count = 0;
			moveMinionFinishedSignal.AddListener(MinionMoved);
			global::Kampai.Game.StageBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.StageBuilding>(3054);
			int temporaryMinionNum = firstInstanceByDefinitionId.Definition.temporaryMinionNum;
			float temporaryMinionsOffset = firstInstanceByDefinitionId.Definition.temporaryMinionsOffset;
			float num = (0f - (float)temporaryMinionNum * 0.5f) * temporaryMinionsOffset - temporaryMinionsOffset * 0.5f;
			for (int i = 0; i < temporaryMinionNum; i++)
			{
				global::Kampai.Game.GenerateTemporaryMinionCommand.TemporaryMinionProperties type = new global::Kampai.Game.GenerateTemporaryMinionCommand.TemporaryMinionProperties
				{
					TempID = ((i != 0) ? (-i - 1) : (-1)),
					startX = (float)firstInstanceByDefinitionId.Location.x + 4.8f,
					startY = (float)firstInstanceByDefinitionId.Location.y + num,
					finishX = firstInstanceByDefinitionId.Location.x,
					finishY = (float)firstInstanceByDefinitionId.Location.y + num
				};
				generateTemporaryMinionSignal.Dispatch(type);
				num += temporaryMinionsOffset;
			}
		}

		public void MinionMoved(int id)
		{
			if (id <= 0)
			{
				global::System.Collections.Generic.IDictionary<int, global::Kampai.Game.View.MinionObject> temporaryMinions = temporaryMinionsService.getTemporaryMinions();
				if (temporaryMinions.Count > count)
				{
					global::Kampai.Game.StageBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.StageBuilding>(3054);
					global::Kampai.Game.View.MinionObject minionObject = temporaryMinions[id];
					minionObject.ClearActionQueue();
					global::UnityEngine.RuntimeAnimatorController controller = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(firstInstanceByDefinitionId.Definition.temporaryMinionASM);
					minionObject.EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(minionObject, controller, logger));
				}
				count++;
			}
		}
	}
}

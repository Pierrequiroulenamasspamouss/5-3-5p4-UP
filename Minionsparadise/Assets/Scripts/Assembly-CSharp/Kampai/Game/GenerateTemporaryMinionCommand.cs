namespace Kampai.Game
{
	public class GenerateTemporaryMinionCommand : global::strange.extensions.command.impl.Command
	{
		public struct TemporaryMinionProperties
		{
			public int TempID;

			public float startX;

			public float startY;

			public float finishX;

			public float finishY;
		}

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("GenerateTemporaryMinionCommand") as global::Kampai.Util.IKampaiLogger;

		private int[] CostumeIDs = new int[1] { 99 };

		[Inject]
		public global::Kampai.Game.GenerateTemporaryMinionCommand.TemporaryMinionProperties randomMinionProperties { get; set; }

		[Inject(global::strange.extensions.context.api.ContextKeys.CONTEXT_VIEW)]
		public global::UnityEngine.GameObject ContextView { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Util.IMinionBuilder minionBuilder { get; set; }

		[Inject(global::Kampai.Game.GameElement.MINION_MANAGER)]
		public global::UnityEngine.GameObject minionManager { get; set; }

		[Inject]
		public global::Kampai.Util.PathFinder pathFinder { get; set; }

		[Inject]
		public global::Kampai.Game.MoveMinionFinishedSignal moveMinionFinishedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.TemporaryMinionsService temporaryMinionsService { get; set; }

		public override void Execute()
		{
			int id = global::Kampai.Util.GameConstants.MINION_DEFINITION_IDS[global::UnityEngine.Random.Range(0, global::Kampai.Util.GameConstants.MINION_DEFINITION_IDS.Length)];
			global::Kampai.Game.MinionDefinition def = definitionService.Get<global::Kampai.Game.MinionDefinition>(id);
			global::Kampai.Game.Minion minion = new global::Kampai.Game.Minion(def);
			minion.ID = randomMinionProperties.TempID;
			int id2 = CostumeIDs[global::UnityEngine.Random.Range(0, CostumeIDs.Length)];
			global::Kampai.Game.CostumeItemDefinition costume = definitionService.Get<global::Kampai.Game.CostumeItemDefinition>(id2);
			global::Kampai.Game.View.MinionObject minionObject = minionBuilder.BuildMinion(costume, "asm_minion_movement", ContextView);
			minion.Name = minionObject.name;
			minionObject.Init(minion, logger);
			global::Kampai.Util.AI.Agent component = minionObject.GetComponent<global::Kampai.Util.AI.Agent>();
			component.enabled = false;
			minionObject.transform.GetComponentInChildren<global::UnityEngine.BoxCollider>().enabled = false;
			global::UnityEngine.RuntimeAnimatorController runtimeAnimatorController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>("asm_minion_movement");
			minionObject.SetDefaultAnimController(runtimeAnimatorController);
			minionObject.SetAnimController(runtimeAnimatorController);
			minionObject.transform.parent = minionManager.transform;
			float num = global::UnityEngine.Random.Range(0f, 1f);
			minionObject.transform.position = new global::UnityEngine.Vector3(randomMinionProperties.startX + num, 0f, randomMinionProperties.startY);
			global::UnityEngine.Vector3 position = minionObject.transform.position;
			global::UnityEngine.Vector3 goalPos = new global::UnityEngine.Vector3(randomMinionProperties.finishX, position.y, randomMinionProperties.finishY);
			bool muteStatus = true;
			global::System.Collections.Generic.IList<global::UnityEngine.Vector3> path = pathFinder.FindPath(position, goalPos, 4, true);
			global::Kampai.Game.View.MinionManagerView component2 = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerView>();
			component2.StartPathing(minionObject, path, 4.5f, muteStatus, moveMinionFinishedSignal, 0f);
			temporaryMinionsService.addTemporaryMinion(minionObject);
		}
	}
}

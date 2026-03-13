namespace Kampai.Tools.AnimationToolKit
{
	public class GenerateMinionCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("GenerateMinionCommand") as global::Kampai.Util.IKampaiLogger;

		private int[] CostumeIDs = new int[3] { 99, 104, 105 };

		[Inject(global::strange.extensions.context.api.ContextKeys.CONTEXT_VIEW)]
		public global::UnityEngine.GameObject ContextView { get; set; }

		[Inject(global::Kampai.Tools.AnimationToolKit.AnimationToolKitElement.MINIONS)]
		public global::UnityEngine.GameObject MinionGroup { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Util.IMinionBuilder minionBuilder { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Tools.AnimationToolKit.MinionCreatedSignal minionCreatedSignal { get; set; }

		public override void Execute()
		{
			int id = global::Kampai.Util.GameConstants.MINION_DEFINITION_IDS[global::UnityEngine.Random.Range(0, global::Kampai.Util.GameConstants.MINION_DEFINITION_IDS.Length)];
			global::Kampai.Game.MinionDefinition def = definitionService.Get<global::Kampai.Game.MinionDefinition>(id);
			global::Kampai.Game.Minion minion = new global::Kampai.Game.Minion(def);
			int id2 = CostumeIDs[global::UnityEngine.Random.Range(0, CostumeIDs.Length)];
			global::Kampai.Game.CostumeItemDefinition costume = definitionService.Get<global::Kampai.Game.CostumeItemDefinition>(id2);
			minionBuilder.SetLOD(global::Kampai.Util.TargetPerformance.HIGH);
			global::Kampai.Game.View.MinionObject minionObject = minionBuilder.BuildMinion(costume, "asm_minion_movement", ContextView);
			minionObject.transform.parent = MinionGroup.transform;
			minion.Name = minionObject.name;
			playerService.Add(minion);
			minionObject.Init(minion, logger);
			global::Kampai.Util.AI.Agent component = minionObject.GetComponent<global::Kampai.Util.AI.Agent>();
			component.enabled = false;
			global::UnityEngine.RuntimeAnimatorController runtimeAnimatorController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>("asm_minion_movement");
			minionObject.SetDefaultAnimController(runtimeAnimatorController);
			minionObject.SetAnimController(runtimeAnimatorController);
			base.injectionBinder.injector.Inject(minionObject, false);
			minionCreatedSignal.Dispatch(minionObject);
		}
	}
}

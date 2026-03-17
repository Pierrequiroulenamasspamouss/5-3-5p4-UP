namespace Kampai.Tools.AnimationToolKit
{
	public class GenerateVillainCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("GenerateVillainCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Util.INamedCharacterBuilder villainBuilder { get; set; }

		[Inject(global::Kampai.Tools.AnimationToolKit.AnimationToolKitElement.VILLAINS)]
		public global::UnityEngine.GameObject VillainGroup { get; set; }

		[Inject]
		public global::Kampai.Tools.AnimationToolKit.VillainCreatedSignal villainCreatedSignal { get; set; }

		[Inject]
		public int VillainId { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.VillainDefinition villainDefinition = definitionService.Get<global::Kampai.Game.VillainDefinition>(VillainId);
			global::Kampai.Game.Villain villain = new global::Kampai.Game.Villain(villainDefinition);
			playerService.Add(villain);
			global::Kampai.Game.View.NamedCharacterObject namedCharacterObject = villainBuilder.Build(villain, VillainGroup);
			namedCharacterObject.transform.parent = VillainGroup.transform;
			namedCharacterObject.Init(villain, logger);
			global::UnityEngine.RuntimeAnimatorController runtimeAnimatorController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(villainDefinition.CharacterAnimations.StateMachine);
			namedCharacterObject.SetDefaultAnimController(runtimeAnimatorController);
			namedCharacterObject.SetAnimController(runtimeAnimatorController);
			global::Kampai.Game.View.VillainView component = namedCharacterObject.GetComponent<global::Kampai.Game.View.VillainView>();
			if (component != null)
			{
				villainCreatedSignal.Dispatch(component);
			}
		}
	}
}

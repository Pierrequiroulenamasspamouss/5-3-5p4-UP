namespace Kampai.Tools.AnimationToolKit
{
	public class GenerateCharacterCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("GenerateCharacterCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject(global::Kampai.Tools.AnimationToolKit.AnimationToolKitElement.CHARACTERS)]
		public global::UnityEngine.GameObject characterGroup { get; set; }

		[Inject]
		public global::Kampai.Util.INamedCharacterBuilder builder { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Tools.AnimationToolKit.CharacterCreatedSignal characterCreatedSignal { get; set; }

		[Inject]
		public int namedCharacterId { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.NamedCharacterDefinition namedCharacterDefinition = definitionService.Get<global::Kampai.Game.NamedCharacterDefinition>(namedCharacterId);
			global::Kampai.Game.NamedCharacter namedCharacter = CreateNamedCharacter(namedCharacterDefinition);
			playerService.Add(namedCharacter);
			global::Kampai.Game.View.NamedCharacterObject namedCharacterObject = builder.Build(namedCharacter);
			namedCharacterObject.Init(namedCharacter, logger);
			namedCharacterObject.transform.parent = characterGroup.transform;
			namedCharacterObject.transform.localEulerAngles = new global::UnityEngine.Vector3(0f, 135f, 0f);
			global::UnityEngine.RuntimeAnimatorController runtimeAnimatorController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(namedCharacterDefinition.CharacterAnimations.StateMachine);
			namedCharacterObject.SetDefaultAnimController(runtimeAnimatorController);
			namedCharacterObject.SetAnimController(runtimeAnimatorController);
			if (namedCharacterDefinition.Location != null)
			{
				global::Kampai.Game.Location location = namedCharacterDefinition.Location;
				namedCharacterObject.transform.position = new global::UnityEngine.Vector3(location.x, 0f, location.y);
			}
			global::Kampai.Util.AI.Agent component = namedCharacterObject.GetComponent<global::Kampai.Util.AI.Agent>();
			if (component != null)
			{
				component.enabled = false;
			}
			base.injectionBinder.injector.Inject(namedCharacterObject, false);
			characterCreatedSignal.Dispatch(namedCharacterObject);
		}

		private global::Kampai.Game.NamedCharacter CreateNamedCharacter(global::Kampai.Game.NamedCharacterDefinition namedCharacterDefinition)
		{
			global::Kampai.Game.NamedCharacter result = null;
			global::Kampai.Game.PhilCharacterDefinition philCharacterDefinition = namedCharacterDefinition as global::Kampai.Game.PhilCharacterDefinition;
			if (philCharacterDefinition != null)
			{
				result = new global::Kampai.Game.PhilCharacter(philCharacterDefinition);
			}
			global::Kampai.Game.BobCharacterDefinition bobCharacterDefinition = namedCharacterDefinition as global::Kampai.Game.BobCharacterDefinition;
			if (bobCharacterDefinition != null)
			{
				result = new global::Kampai.Game.BobCharacter(bobCharacterDefinition);
			}
			global::Kampai.Game.KevinCharacterDefinition kevinCharacterDefinition = namedCharacterDefinition as global::Kampai.Game.KevinCharacterDefinition;
			if (kevinCharacterDefinition != null)
			{
				result = new global::Kampai.Game.KevinCharacter(kevinCharacterDefinition);
			}
			global::Kampai.Game.StuartCharacterDefinition stuartCharacterDefinition = namedCharacterDefinition as global::Kampai.Game.StuartCharacterDefinition;
			if (stuartCharacterDefinition != null)
			{
				result = new global::Kampai.Game.StuartCharacter(stuartCharacterDefinition);
			}
			global::Kampai.Game.SpecialEventCharacterDefinition specialEventCharacterDefinition = namedCharacterDefinition as global::Kampai.Game.SpecialEventCharacterDefinition;
			if (specialEventCharacterDefinition != null)
			{
				result = new global::Kampai.Game.SpecialEventCharacter(specialEventCharacterDefinition);
			}
			return result;
		}
	}
}

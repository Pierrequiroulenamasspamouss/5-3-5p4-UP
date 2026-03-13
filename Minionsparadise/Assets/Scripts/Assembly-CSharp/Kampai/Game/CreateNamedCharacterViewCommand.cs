namespace Kampai.Game
{
	public class CreateNamedCharacterViewCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CreateNamedCharacterViewCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.NamedCharacter namedCharacter { get; set; }

		[Inject]
		public global::Kampai.Util.INamedCharacterBuilder builder { get; set; }

		[Inject]
		public global::Kampai.Game.InitCharacterObjectSignal initSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.NAMED_CHARACTER_MANAGER)]
		public global::UnityEngine.GameObject namedCharacterManager { get; set; }

		[Inject]
		public global::Kampai.Game.AddNamedCharacterSignal addCharacterSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MapAnimationEventSignal mapAnimationEventSignal { get; set; }

		public override void Execute()
		{
			if (namedCharacter == null || namedCharacter.Definition == null)
			{
				logger.Error("Unable to load named character");
				return;
			}
			namedCharacter.Created = true;
			global::Kampai.Game.NamedCharacterDefinition definition = namedCharacter.Definition;
			global::Kampai.Game.View.NamedCharacterObject namedCharacterObject = builder.Build(namedCharacter);
			namedCharacterObject.transform.parent = namedCharacterManager.transform;
			initSignal.Dispatch(namedCharacterObject, namedCharacter);
			addCharacterSignal.Dispatch(namedCharacterObject);
			int boundBuildingId = definition.VFXBuildingID;
			if (boundBuildingId <= 0)
			{
				return;
			}
			global::Kampai.Game.View.AnimEventHandler component = namedCharacterObject.GetComponent<global::Kampai.Game.View.AnimEventHandler>();
			if (component != null)
			{
				global::System.Action<global::Kampai.Game.View.AnimEventHandler> vFXScriptBinder = delegate(global::Kampai.Game.View.AnimEventHandler animEventHandler)
				{
					mapAnimationEventSignal.Dispatch(animEventHandler, boundBuildingId);
				};
				component.SetVFXScriptBinder(vFXScriptBinder);
			}
			else
			{
				logger.Error("Unable to map VFXBuildingID for {0} because there is no AnimEventHandler", definition.ID);
			}
		}
	}
}

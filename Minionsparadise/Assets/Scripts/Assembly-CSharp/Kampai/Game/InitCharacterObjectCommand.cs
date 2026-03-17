namespace Kampai.Game
{
	internal sealed class InitCharacterObjectCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("InitCharacterObjectCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.View.CharacterObject characterObject { get; set; }

		[Inject]
		public global::Kampai.Game.Character character { get; set; }

		public override void Execute()
		{
			global::UnityEngine.MonoBehaviour[] components = characterObject.GetComponents<global::UnityEngine.MonoBehaviour>();
			global::UnityEngine.MonoBehaviour[] array = components;
			foreach (global::UnityEngine.MonoBehaviour target in array)
			{
				base.injectionBinder.injector.Inject(target, false);
			}
			characterObject.Init(character, logger);
		}
	}
}

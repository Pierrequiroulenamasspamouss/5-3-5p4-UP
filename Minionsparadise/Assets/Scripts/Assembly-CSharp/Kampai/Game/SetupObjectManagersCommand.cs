namespace Kampai.Game
{
	public class SetupObjectManagersCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SetupObjectManagersCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject(global::strange.extensions.context.api.ContextKeys.CONTEXT_VIEW)]
		public global::UnityEngine.GameObject contextView { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.View.MinionManagerView o = CreateManager<global::Kampai.Game.View.MinionManagerView>("Minions", global::Kampai.Game.GameElement.MINION_MANAGER);
			base.injectionBinder.Bind<global::Kampai.Game.View.MinionIdleNotifier>().ToValue(o);
			logger.Debug("SetupObjectManagersCommand: Created Minions Manager");
			CreateManager<global::Kampai.Game.View.NamedCharacterManagerView>("NamedCharacters", global::Kampai.Game.GameElement.NAMED_CHARACTER_MANAGER);
			logger.Debug("SetupObjectManagersCommand: Created Named Characters Manager");
			CreateManager<global::Kampai.Game.View.VillainManagerView>("Villains", global::Kampai.Game.GameElement.VILLAIN_MANAGER);
			logger.Debug("SetupObjectManagersCommand: Created Villain Manager");
		}

		private T CreateManager<T>(string goName, global::Kampai.Game.GameElement bindingName) where T : global::UnityEngine.MonoBehaviour
		{
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject(goName);
			T result = gameObject.AddComponent<T>();
			base.injectionBinder.Bind<global::UnityEngine.GameObject>().ToValue(gameObject).ToName(bindingName);
			gameObject.transform.parent = contextView.transform;
			return result;
		}
	}
}

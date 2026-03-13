namespace Kampai.Game.View
{
	public class DisplayMinionSelectedIconCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("DisplayMinionSelectedIconCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public int minionId { get; set; }

		[Inject]
		public bool show { get; set; }

		[Inject(global::Kampai.Game.GameElement.NAMED_CHARACTER_MANAGER)]
		public global::UnityEngine.GameObject NamedCharacterManagerView { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.View.NamedCharacterObject namedCharacterObject = NamedCharacterManagerView.GetComponent<global::Kampai.Game.View.NamedCharacterManagerView>().Get(minionId);
			if (namedCharacterObject == null)
			{
				logger.Error("TSM minion does not exist!");
				return;
			}
			global::UnityEngine.Transform transform = namedCharacterObject.transform;
			global::Kampai.Game.View.MinionSelectedIcon componentInChildren = transform.GetComponentInChildren<global::Kampai.Game.View.MinionSelectedIcon>();
			if (show)
			{
				if (componentInChildren != null)
				{
					logger.Warning("Minion Selected Icon already exists!");
					return;
				}
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>("MinionSelectedIcon"));
				gameObject.transform.SetParent(transform);
				gameObject.transform.localPosition = global::UnityEngine.Vector3.zero;
			}
			else if (componentInChildren == null)
			{
				logger.Warning("Minion Selected Icon does not exist!");
			}
			else
			{
				global::UnityEngine.Object.Destroy(componentInChildren.gameObject);
			}
		}
	}
}

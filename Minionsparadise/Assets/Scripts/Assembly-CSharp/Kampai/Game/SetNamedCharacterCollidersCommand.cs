namespace Kampai.Game
{
	public class SetNamedCharacterCollidersCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SetNamedCharacterCollidersCommand") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.View.NamedCharacterManagerView namedCharacterManagerView;

		[Inject]
		public bool Enabled { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService PlayerService { get; set; }

		[Inject(global::Kampai.Game.GameElement.NAMED_CHARACTER_MANAGER)]
		public global::UnityEngine.GameObject NamedCharacterManager { get; set; }

		public override void Execute()
		{
			namedCharacterManagerView = NamedCharacterManager.GetComponent<global::Kampai.Game.View.NamedCharacterManagerView>();
			global::System.Collections.Generic.List<global::Kampai.Game.NamedCharacter> instancesByType = PlayerService.GetInstancesByType<global::Kampai.Game.NamedCharacter>();
			if (instancesByType == null)
			{
				logger.Warning("Unable to find any named characters!");
				return;
			}
			global::System.Collections.Generic.List<global::Kampai.Game.NamedCharacter>.Enumerator enumerator = instancesByType.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					global::Kampai.Game.NamedCharacter current = enumerator.Current;
					if (!(current is global::Kampai.Game.TSMCharacter))
					{
						global::Kampai.Game.View.NamedCharacterObject namedCharacterObject = namedCharacterManagerView.Get(current.ID);
						if (namedCharacterObject != null)
						{
							namedCharacterObject.GetComponent<global::UnityEngine.Collider>().enabled = Enabled;
						}
					}
				}
			}
			finally
			{
				enumerator.Dispose();
			}
		}
	}
}

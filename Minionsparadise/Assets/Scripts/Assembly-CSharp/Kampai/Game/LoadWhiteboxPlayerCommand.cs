namespace Kampai.Game
{
	public class LoadWhiteboxPlayerCommand : global::strange.extensions.command.impl.Command
	{
		public const string PLAYER_DATA_ENDPOINT = "/rest/gamestate/{0}";

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("LoadWhiteboxPlayerCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject("devPlayerPath")]
		public string devPlayerFilename { get; set; }

		[Inject]
		public IResourceService resourceService { get; set; }

		[Inject]
		public ILocalPersistanceService localPersistService { get; set; }

		public override void Execute()
		{
			string empty = string.Empty;
			empty = resourceService.LoadText(devPlayerFilename);
			localPersistService.PutData("LoadMode", "default");
			localPersistService.PutData("LocalFileName", string.Empty);
			localPersistService.PutData("LocalID", string.Empty);
			if (empty.Length != 0)
			{
				playerService.Deserialize(empty);
			}
			else
			{
				logger.Fatal(global::Kampai.Util.FatalCode.CMD_LOAD_PLAYER);
			}
		}
	}
}

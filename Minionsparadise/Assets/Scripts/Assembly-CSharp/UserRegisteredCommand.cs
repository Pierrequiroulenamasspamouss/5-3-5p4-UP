public class UserRegisteredCommand : global::strange.extensions.command.impl.Command
{
	public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("UserRegisteredCommand") as global::Kampai.Util.IKampaiLogger;

	[Inject]
	public global::Kampai.Game.UserIdentity Identity { get; set; }

	[Inject]
	public ILocalPersistanceService LocalPersistService { get; set; }

	[Inject]
	public IEncryptionService encryptionService { get; set; }

	[Inject]
	public global::Kampai.Game.LoginUserSignal LoginUserSignal { get; set; }

	public override void Execute()
	{
		LocalPersistService.PutData("UserID", Identity.UserID);
		string text = encryptionService.Encrypt(Identity.ExternalID, "Kampai!");
		string text2 = encryptionService.Encrypt(Identity.ID, "Kampai!");
		LocalPersistService.PutData("AnonymousSecret", text);
		LocalPersistService.PutData("AnonymousID", text2);
		logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "Saved user's credentials");
		LoginUserSignal.Dispatch();
	}
}

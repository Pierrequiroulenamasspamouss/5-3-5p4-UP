namespace Kampai.Game
{
	public class GameCenterAuthManager : global::strange.extensions.mediation.impl.View
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("GameCenterAuthManager") as global::Kampai.Util.IKampaiLogger;

		private string publicKeyUrl;

		private string signature;

		private string salt;

		private string timestamp;

		[Inject]
		public global::Kampai.Game.GameCenterAuthTokenCompleteSignal authCompleteSignal { get; set; }

		public void RecievePublicKey(string publicKeyUrl)
		{
			logger.Debug("IOS RecievePublicKey called !!!!!!!1!1!!!!1!!!    {0}", publicKeyUrl);
			this.publicKeyUrl = publicKeyUrl;
		}

		public void RecieveSignature(string signature)
		{
			logger.Debug("IOS RecieveSignature called !!!!!!!1!1!!!!1!!!    {0}", signature);
			this.signature = signature;
		}

		public void RecieveSalt(string salt)
		{
			logger.Debug("IOS RecieveSalt called !!!!!!!1!1!!!!1!!!    {0}", salt);
			this.salt = salt;
		}

		public void RecieveTimestamp(string timestamp)
		{
			logger.Debug("IOS RecieveTimestamp called !!!!!!!1!1!!!!1!!!    {0}", timestamp);
			this.timestamp = timestamp;
		}

		public void RecieveClientAuthComplete(string complete)
		{
			if (complete.Equals("true"))
			{
				logger.Debug("IOS RecieveClientAuthComplete called !!!!!!!1!1!!!!1!!!    {0}", complete);
				authCompleteSignal.Dispatch(new global::Kampai.Util.Tuple<string, string, string, string>(publicKeyUrl, signature, salt, timestamp));
			}
		}
	}
}

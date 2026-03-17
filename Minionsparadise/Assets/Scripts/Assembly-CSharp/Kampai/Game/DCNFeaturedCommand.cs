namespace Kampai.Game
{
	public class DCNFeaturedCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("DCNFeaturedCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

		[Inject]
		public global::Kampai.Game.IDCNService dcnService { get; set; }

		[Inject]
		public global::Kampai.Game.ShowDCNScreenSignal showDCNScreenSignal { get; set; }

		public override void Execute()
		{
			dcnService.Perform(Request);
		}

		private global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest Request()
		{
			string uri = string.Format("{0}{1}", global::Kampai.Util.GameConstants.DCN.SERVER, "/contents/featured");
			global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> signal = new global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse>();
			signal.AddListener(Response);
			return requestFactory.Resource(uri).WithMethod("GET").WithHeaderParam("X-DCN-TOKEN", dcnService.GetToken())
				.WithResponseSignal(signal);
		}

		private void Response(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response)
		{
			if (response == null)
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "DCNFeaturedCommand response is null");
			}
			else if (!response.Success)
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Error, string.Format("DCNFeaturedCommand failed with response code: {0}", response.Code));
			}
			else
			{
				Deserialize(response.Body);
			}
		}

		private void Deserialize(string json)
		{
			global::Kampai.Game.DCNContent dCNContent = null;
			try
			{
				dCNContent = global::Newtonsoft.Json.JsonConvert.DeserializeObject<global::Kampai.Game.DCNContent>(json);
			}
			catch (global::Newtonsoft.Json.JsonSerializationException e)
			{
				HandleJsonException(e);
			}
			catch (global::Newtonsoft.Json.JsonReaderException e2)
			{
				HandleJsonException(e2);
			}
			string value;
			if (dCNContent == null)
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "Error: content is null");
			}
			else if (dCNContent.Urls.TryGetValue("html5", out value) && !string.IsNullOrEmpty(value))
			{
				if (dcnService.SetFeaturedContent(dCNContent.Id, value))
				{
					showDCNScreenSignal.Dispatch(true);
				}
			}
			else
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Warning, "HTML5 URL does not exist in the response!");
			}
		}

		private void HandleJsonException(global::System.Exception e)
		{
			logger.Error("[Error]\n{0}", e.Message);
			logger.Error("[StackTrace]\n{0}", e.StackTrace);
		}
	}
}

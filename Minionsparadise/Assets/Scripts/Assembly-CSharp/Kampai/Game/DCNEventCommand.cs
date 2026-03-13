namespace Kampai.Game
{
	public class DCNEventCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("DCNEventCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IDCNService dcnService { get; set; }

		[Inject]
		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

		public override void Execute()
		{
			dcnService.Perform(Request);
		}

		private global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest Request()
		{
			string value = string.Format("/contents/{0}/events", dcnService.GetFeaturedContentId());
			string uri = new global::System.Text.StringBuilder(global::Kampai.Util.GameConstants.DCN.SERVER).Append(value).ToString();
			string s = string.Format("{{ \"{0}\": \"{1}\" }}", "type", "display");
			byte[] bytes = global::System.Text.Encoding.UTF8.GetBytes(s);
			global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> signal = new global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse>();
			signal.AddListener(Response);
			global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest result = requestFactory.Resource(uri).WithMethod("POST").WithContentType("application/json")
				.WithHeaderParam("X-DCN-TOKEN", dcnService.GetToken())
				.WithBody(bytes)
				.WithResponseSignal(signal);
			dcnService.SetFeaturedContent(-1, null);
			return result;
		}

		private void Response(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response)
		{
			if (response == null)
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "DCNEventCommand response is null");
				return;
			}
			int code = response.Code;
			if (!response.Success)
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Error, string.Format("DCNEventCommand failed with response code: {0}", code));
			}
			else if (response.Code != 204)
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Error, string.Format("DCNEventCommand did not register event, response code: {0}", code));
			}
		}
	}
}

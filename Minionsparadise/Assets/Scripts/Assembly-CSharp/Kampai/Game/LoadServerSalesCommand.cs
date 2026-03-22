namespace Kampai.Game
{
	public class LoadServerSalesCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("LoadServerSalesCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Splash.IDownloadService downloadService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IUserSessionService userSessionService { get; set; }

		[Inject]
		public global::Kampai.Game.ReconcileSalesSignal reconcileSalesSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Common.ICoppaService coppaService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory requestFactory { get; set; }

		public override void Execute()
		{
			if (!coppaService.IsBirthdateKnown())
			{
				reconcileSalesSignal.Dispatch(0);
			}
			else
			{
				routineRunner.StartCoroutine(LoadFromServer());
			}
		}

		private global::System.Collections.IEnumerator LoadFromServer()
		{
			yield return null;
			logger.Info("ServerSales: Getting sales from server");
			global::Kampai.Game.UserSession session = userSessionService.UserSession;
			global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> responseSignal = new global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse>();
			responseSignal.AddListener(OnDownloadComplete);
			string url = string.Format("{0}/rest/sales/{1}/v2", global::Kampai.Util.GameConstants.UpSell.SALES_SERVER, session.UserID);
			downloadService.Perform(requestFactory.Resource(url).WithHeaderParam("user_id", session.UserID).WithHeaderParam("session_key", session.SessionID)
				.WithResponseSignal(responseSignal));
		}

		private void OnDownloadComplete(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse response)
		{
			if (response.Success)
			{
				try
				{
					global::System.Collections.Generic.List<global::Kampai.Game.UserSale> list = null;
					using (global::System.IO.StringReader reader = new global::System.IO.StringReader(response.Body))
					{
						using (global::Newtonsoft.Json.JsonTextReader reader2 = new global::Newtonsoft.Json.JsonTextReader(reader))
						{
							list = global::Kampai.Util.ReaderUtil.PopulateList<global::Kampai.Game.UserSale>(reader2);
						}
					}
					global::System.Collections.Generic.IList<global::Kampai.Game.SalePackDefinition> list2 = new global::System.Collections.Generic.List<global::Kampai.Game.SalePackDefinition>();
					foreach (global::Kampai.Game.UserSale item in list)
					{
						global::Kampai.Game.SalePackDefinition salePackDefinition = global::Kampai.Util.FastJSONDeserializer.Deserialize<global::Kampai.Game.SalePackDefinition>(item.SaleDefinition);
						salePackDefinition.ServerSaleId = item.SaleId.ToString();
						list2.Add(salePackDefinition);
					}
					foreach (global::Kampai.Game.SalePackDefinition item2 in list2)
					{
						global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = item2.TransactionDefinition.ToDefinition();
						
						int uTCEndDate = item2.UTCEndDate;
						int num = timeService.CurrentTime();
						
						if (definitionService.Has<global::Kampai.Game.SalePackDefinition>(item2.ID))
						{
							// UPDATE EXISTING
							global::Kampai.Game.SalePackDefinition existing = definitionService.Get<global::Kampai.Game.SalePackDefinition>(item2.ID);
							existing.UTCStartDate = item2.UTCStartDate;
							existing.UTCEndDate = item2.UTCEndDate;
							existing.CanBuyThisManyTimes = item2.CanBuyThisManyTimes;
							existing.Disabled = item2.Disabled;
							existing.Impressions = item2.Impressions;
							logger.Info("ServerSales - Updated SalePackDefinition ID = " + item2.ID + " EndDate: " + item2.UTCEndDate);
						}
						else if (uTCEndDate > num && !definitionService.Has<global::Kampai.Game.Transaction.TransactionDefinition>(transactionDefinition.ID))
						{
							// ADD NEW
							definitionService.Add(item2);
							if (item2.TransactionDefinition != null)
							{
								definitionService.Add(transactionDefinition);
							}
							logger.Info("ServerSales - Added new SalePackDefinition ID = " + item2.ID);
						}
					}
				}
				catch (global::Newtonsoft.Json.JsonSerializationException e)
				{
					HandleJsonException(e);
				}
				catch (global::Newtonsoft.Json.JsonReaderException e2)
				{
					HandleJsonException(e2);
				}
			}
			else
			{
				logger.Error("ServerSales - Error downloading sales from server: {0}", response.Body ?? "null body");
			}
			reconcileSalesSignal.Dispatch(0);
		}

		private void HandleJsonException(global::System.Exception e)
		{
			logger.Error("ServerSales - Json exception: {0}", e);
		}
	}
}

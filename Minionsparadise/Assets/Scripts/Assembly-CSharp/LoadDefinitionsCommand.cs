public class LoadDefinitionsCommand : global::strange.extensions.command.impl.Command
{
	public class LoadDefinitionsData
	{
		public string Path { get; set; }

		public string Json { get; set; }
	}

	public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("LoadDefinitionsCommand") as global::Kampai.Util.IKampaiLogger;

	[Inject]
	public bool hotSwap { get; set; }

	[Inject]
	public LoadDefinitionsCommand.LoadDefinitionsData defData { get; set; }

	[Inject]
	public global::Kampai.Game.IDefinitionService definitionService { get; set; }

	[Inject]
	public global::Kampai.Game.DefinitionsChangedSignal definitionsChangedSignal { get; set; }

	[Inject]
	public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

	[Inject]
	public global::Kampai.Game.IDLCService dlcService { get; set; }

	[Inject]
	public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

	[Inject]
	public global::Kampai.Util.IInvokerService invokerService { get; set; }

	[Inject]
	public global::Kampai.Game.IPlayerService playerService { get; set; }

	[Inject]
	public global::Kampai.Splash.SplashProgressUpdateSignal splashProgressUpdateSignal { get; set; }

	public override void Execute()
	{
		logger.EventStart("LoadDefinitionsCommand.Execute");
		string jsonString = defData.Json;
		if (jsonString != null)
		{
			logger.Debug("LoadDefinitions: Starting json deserialization from string");
			routineRunner.StartAsyncConditionTask(() => DeserializeDefinitionsFromJsonString(jsonString), OnDeserializationSuccess);
		}
		else
		{
			string jsonPath = defData.Path;
			if (string.IsNullOrEmpty(jsonPath))
			{
				throw new global::System.ArgumentException("LoadDefinitionsCommand: neither json content nor path to file is specified");
			}
			bool flag = true;
			string binaryDefinitionsPath = global::Kampai.Game.DefinitionService.GetBinaryDefinitionsPath();
#if !UNITY_WEBPLAYER
			/* if (global::System.IO.File.Exists(binaryDefinitionsPath))
			{
				logger.Debug("LoadDefinitions: Starting binary deserialization");
				if (DeserializeDefinitionsFromBinaryFile(binaryDefinitionsPath))
				{
					flag = false;
					OnDeserializationSuccess();
				}
				else
				{
					global::Kampai.Game.DefinitionService.DeleteBinarySerialization();
				}
			} */
			logger.Warning("FORCED JSON LOADING: Skipping binary definitions cache.");
#else
			if (false)
			{
			}
#endif
			if (flag)
			{
				logger.Debug("LoadDefinitions: Starting json deserialization");
				routineRunner.StartAsyncConditionTask(delegate
				{
					if (global::System.IO.File.Exists(jsonPath)) {
						long length = new global::System.IO.FileInfo(jsonPath).Length;
						logger.Warning("FORCED JSON LOADING: File {0} size: {1} bytes", jsonPath, length);
					} else {
						logger.Error("FORCED JSON LOADING: File {0} DOES NOT EXIST!", jsonPath);
					}
					bool flag2 = DeserializeDefinitionsFromJsonFile(jsonPath);
					if (!flag2)
					{
						RemoveCachedDefinitions(jsonPath);
					}
					return flag2;
				}, OnDeserializationSuccess);
			}
		}
		logger.EventStop("LoadDefinitionsCommand.Execute");
	}

	private void RemoveCachedDefinitions(string path)
	{
#if !UNITY_WEBPLAYER
		try
		{
			global::System.IO.File.Delete(path);
		}
		catch (global::System.Exception)
		{
		}
#endif
	}

	private void OnDeserializationSuccess()
	{
		this.telemetryService.Send_Telemetry_EVT_USER_GAME_LOAD_FUNNEL("80 - Loaded Definitions", playerService.SWRVEGroup, dlcService.GetDownloadQualityLevel());
		logger.Debug("LoadDefinitions: Deserialized successfully");
		global::Kampai.Common.TelemetryService telemetryService = this.telemetryService as global::Kampai.Common.TelemetryService;
		if (telemetryService != null)
		{
			telemetryService.SetDefinitionServiceReference(definitionService);
		}
		definitionsChangedSignal.Dispatch(hotSwap);
		splashProgressUpdateSignal.Dispatch(35, 10f);
	}

	private bool DeserializeDefinitionsFromBinaryFile(string path)
	{
#if !UNITY_WEBPLAYER
		try
		{
			using (global::System.IO.BinaryReader binaryReader = new global::System.IO.BinaryReader(new global::System.IO.FileStream(path, global::System.IO.FileMode.Open, global::System.IO.FileAccess.Read)))
			{
				definitionService.DeserializeBinary(binaryReader);
			}
			return true;
		}
		catch (global::System.Exception ex)
		{
			logger.Error("DeserializeDefinitionsFromBinaryFile: can't deserialize from binary file. Reason: {0}", ex);
			return false;
		}
#else
		return false;
#endif
	}

	private bool DeserializeDefinitionsFromJsonString(string jsonString)
	{
		using (global::System.IO.StringReader textReader = new global::System.IO.StringReader(jsonString))
		{
			return DeserializeDefinitionsFromJson(textReader);
		}
	}

	private bool DeserializeDefinitionsFromJsonFile(string jsonPath)
	{
#if !UNITY_WEBPLAYER
		try
		{
			using (global::System.IO.StreamReader textReader = new global::System.IO.StreamReader(jsonPath))
			{
				return DeserializeDefinitionsFromJson(textReader);
			}
		}
		catch (global::System.Exception e)
		{
			HandleDefinitionFileOpenError(e);
			return false;
		}
#else
		return false;
#endif
	}

	private bool HandleDefinitionFileOpenError(global::System.Exception e)
	{
		logger.Error("Definition file open error: {0}", e);
		int reasonCode = 0;
		if (e is global::System.IO.FileNotFoundException)
		{
			reasonCode = 1;
		}
		else if (e is global::System.IO.IOException)
		{
			reasonCode = 2;
		}
		invokerService.Add(delegate
		{
			logger.FatalNoThrow(global::Kampai.Util.FatalCode.DS_UNABLE_TO_LOAD, reasonCode, "Reason: {0}", e);
		});
		return false;
	}

	private bool DeserializeDefinitionsFromJson(global::System.IO.TextReader textReader)
	{
		try
		{
			definitionService.DeserializeJson(textReader);
			return true;
		}
		catch (global::Kampai.Util.FatalException ex)
		{
			global::Kampai.Util.FatalException ex2 = ex;
			global::Kampai.Util.FatalException e = ex2;
			global::UnityEngine.Debug.LogErrorFormat("LoadDefinitionsCommand: FatalException: {0}\n{1}", e.Message, e.StackTrace);
			logger.Error("Can't deserialize: {0}", e);
			invokerService.Add(delegate
			{
				logger.FatalNoThrow(e.FatalCode, e.ReferencedId, "Message: {0}, Reason: {1}", e.Message, e.InnerException ?? e);
			});
		}
		catch (global::System.Exception ex3)
		{
			global::System.Exception ex4 = ex3;
			global::System.Exception e2 = ex4;
			global::UnityEngine.Debug.LogErrorFormat("LoadDefinitionsCommand: System.Exception: {0}\n{1}", e2.Message, e2.StackTrace);
			logger.Error("Can't deserialize: {0}", e2);
			invokerService.Add(delegate
			{
				logger.FatalNoThrow(global::Kampai.Util.FatalCode.DS_PARSE_ERROR, 0, "Reason: {0}", e2);
			});
		}
		return false;
	}
}

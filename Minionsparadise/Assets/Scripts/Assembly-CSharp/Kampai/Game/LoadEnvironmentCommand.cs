namespace Kampai.Game
{
	public class LoadEnvironmentCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("LoadEnvironmentCommand") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.EnvironmentResourceDefinition definition;

		[Inject]
		public global::Kampai.Util.IInvokerService invokerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		public override void Execute()
		{
			global::UnityEngine.TextAsset textAsset = global::Kampai.Util.KampaiResources.Load("environment") as global::UnityEngine.TextAsset;
			if (textAsset == null)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.DS_NO_ENVIRONMENT_DEF, "Unable to read environment.json");
			}
			DeserializeEnvironmentDefinition(textAsset.text);
			DeserializeEnvironmentResources(textAsset.text);
			LoadEnvironmentResources();
		}

		private void DeserializeEnvironmentResources(string json)
		{
			using (global::System.IO.StringReader textReader = new global::System.IO.StringReader(json))
			{
				DeserializeEnvironmentResources(textReader);
			}
		}

		private void DeserializeEnvironmentResources(global::System.IO.TextReader textReader)
		{
			using (global::Newtonsoft.Json.JsonTextReader reader = new global::Newtonsoft.Json.JsonTextReader(textReader))
			{
				definition = global::Kampai.Util.FastJSONDeserializer.Deserialize<global::Kampai.Game.EnvironmentResourceDefinition>(reader);
			}
		}

		private bool DeserializeEnvironmentDefinition(string json)
		{
			using (global::System.IO.StringReader textReader = new global::System.IO.StringReader(json))
			{
				return DeserializeEnvironmentDefinition(textReader);
			}
		}

		private bool DeserializeEnvironmentDefinition(global::System.IO.TextReader textReader)
		{
			try
			{
				definitionService.DeserializeEnvironmentDefinition(textReader);
				return true;
			}
			catch (global::Kampai.Util.FatalException ex)
			{
				global::Kampai.Util.FatalException ex2 = ex;
				global::Kampai.Util.FatalException e = ex2;
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
				logger.Error("Can't deserialize: {0}", e2);
				invokerService.Add(delegate
				{
					logger.FatalNoThrow(global::Kampai.Util.FatalCode.DS_PARSE_ERROR, 0, "Reason: {0}", e2);
				});
			}
			return false;
		}

		private void LoadEnvironmentResources()
		{
			foreach (string environmentResource in definition.environmentResources)
			{
				global::UnityEngine.GameObject gameObject = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>(environmentResource);
				if (gameObject == null)
				{
					logger.Fatal(global::Kampai.Util.FatalCode.DLC_ENVIRONMENT_MISSING, "Unable to load {0}", environmentResource);
				}
				global::UnityEngine.GameObject gameObject2 = global::UnityEngine.Object.Instantiate(gameObject);
				if (gameObject2 == null)
				{
					logger.Fatal(global::Kampai.Util.FatalCode.DLC_ENVIRONMENT_MISSING, "Unable to instantiate {0}", environmentResource);
				}
			}
		}
	}
}

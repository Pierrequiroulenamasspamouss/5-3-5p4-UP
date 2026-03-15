namespace Kampai.Game
{
	public class PlotDefinitionFastConverter : global::Kampai.Util.FastJsonCreationConverter<global::Kampai.Game.PlotDefinition>
	{
		private global::Kampai.Game.PlotType plotType;

		private readonly global::Kampai.Util.IKampaiLogger logger;

		public PlotDefinitionFastConverter(global::Kampai.Util.IKampaiLogger logger)
		{
			this.logger = logger;
		}

		public override global::Kampai.Game.PlotDefinition ReadJson(global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			if (reader.TokenType == global::Newtonsoft.Json.JsonToken.Null)
			{
				return null;
			}
			global::Newtonsoft.Json.Linq.JObject jObject = global::Newtonsoft.Json.Linq.JObject.Load(reader);
			global::Newtonsoft.Json.Linq.JProperty jProperty = jObject.Property("type");
			if (jProperty != null)
			{
				string value = jProperty.Value.ToString();
				plotType = (global::Kampai.Game.PlotType)(int)global::System.Enum.Parse(typeof(global::Kampai.Game.PlotType), value);
			}
			else
			{
				plotType = global::Kampai.Game.PlotType.UNKNOWN;
			}
			reader = jObject.CreateReader();
			return base.ReadJson(reader, converters);
		}

		public override global::Kampai.Game.PlotDefinition Create()
		{
			global::Kampai.Game.PlotType plotType = this.plotType;
			if (plotType == global::Kampai.Game.PlotType.RED_CARPET)
			{
				return new global::Kampai.Game.NoOpPlotDefinition();
			}
			logger.Fatal(global::Kampai.Util.FatalCode.EX_INVALID_ENUM);
			return null;
		}
	}
}

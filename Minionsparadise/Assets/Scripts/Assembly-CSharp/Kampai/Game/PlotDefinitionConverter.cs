namespace Kampai.Game
{
	public class PlotDefinitionConverter : global::Newtonsoft.Json.Converters.CustomCreationConverter<global::Kampai.Game.PlotDefinition>
	{
		private global::Kampai.Game.PlotType plotType;

		private readonly global::Kampai.Util.IKampaiLogger logger;

		public PlotDefinitionConverter(global::Kampai.Util.IKampaiLogger logger)
		{
			this.logger = logger;
		}

		public override object ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			global::Newtonsoft.Json.Linq.JObject jObject = global::Newtonsoft.Json.Linq.JObject.Load(reader);
			if (jObject.Property("type") != null)
			{
				string value = jObject.Property("type").Value.ToString();
				plotType = (global::Kampai.Game.PlotType)(int)global::System.Enum.Parse(typeof(global::Kampai.Game.PlotType), value);
			}
			else
			{
				plotType = global::Kampai.Game.PlotType.UNKNOWN;
			}
			reader = jObject.CreateReader();
			return base.ReadJson(reader, objectType, existingValue, serializer);
		}

		public override global::Kampai.Game.PlotDefinition Create(global::System.Type objectType)
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

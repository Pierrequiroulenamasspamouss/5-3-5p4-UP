namespace Kampai.Game.Trigger
{
	public class TriggerInstanceConverter : global::Newtonsoft.Json.Converters.CustomCreationConverter<global::Kampai.Game.Trigger.TriggerInstance>
	{
		private global::Kampai.Game.IDefinitionService definitionService;

		private global::Kampai.Game.Definition def;

		public TriggerInstanceConverter(global::Kampai.Game.IDefinitionService definitionService)
		{
			this.definitionService = definitionService;
		}

		public override object ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			if (reader.TokenType == global::Newtonsoft.Json.JsonToken.Null)
			{
				return null;
			}
			global::Newtonsoft.Json.Linq.JObject jObject = global::Newtonsoft.Json.Linq.JObject.Load(reader);
			global::Newtonsoft.Json.Linq.JProperty jProperty = ((jObject.Property("def") != null) ? jObject.Property("def") : jObject.Property("Definition"));
			if (jProperty == null)
			{
				return null;
			}
			int id = global::Newtonsoft.Json.Linq.LinqExtensions.Value<int>(jProperty.Value);
			def = null;
			if (!definitionService.TryGet<global::Kampai.Game.Definition>(id, out def))
			{
				return null;
			}
			reader = jObject.CreateReader();
			return base.ReadJson(reader, objectType, existingValue, serializer);
		}

		public override global::Kampai.Game.Trigger.TriggerInstance Create(global::System.Type objectType)
		{
			if (def == null)
			{
				return null;
			}
			global::Kampai.Util.IBuilder<global::Kampai.Game.Trigger.TriggerInstance> builder = def as global::Kampai.Util.IBuilder<global::Kampai.Game.Trigger.TriggerInstance>;
			object result;
			if (builder != null)
			{
				global::Kampai.Game.Trigger.TriggerInstance triggerInstance = builder.Build();
				result = triggerInstance;
			}
			else
			{
				result = null;
			}
			return (global::Kampai.Game.Trigger.TriggerInstance)result;
		}
	}
}

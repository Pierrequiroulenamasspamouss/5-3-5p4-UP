namespace Kampai.Game
{
	public class QuestDefinitionConverter : global::Newtonsoft.Json.Converters.CustomCreationConverter<global::Kampai.Game.QuestDefinition>
	{
		private global::Kampai.Game.QuestType questType;

		public override object ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			questType = global::Kampai.Game.QuestType.Default;
			if (reader.TokenType != global::Newtonsoft.Json.JsonToken.Null)
			{
				global::Newtonsoft.Json.Linq.JObject jObject = global::Newtonsoft.Json.Linq.JObject.Load(reader);
				if (jObject.Property("type") != null)
				{
					string value = jObject.Property("type").Value.ToString();
					questType = (global::Kampai.Game.QuestType)(int)global::System.Enum.Parse(typeof(global::Kampai.Game.QuestType), value);
				}
				else if (jObject.Property("serverStartTimeUTC") != null && jObject.Property("serverStopTimeUTC") != null)
				{
					questType = global::Kampai.Game.QuestType.LimitedQuest;
				}
				reader = jObject.CreateReader();
			}
			return base.ReadJson(reader, objectType, existingValue, serializer);
		}

		public override global::Kampai.Game.QuestDefinition Create(global::System.Type objectType)
		{
			switch (questType)
			{
			case global::Kampai.Game.QuestType.Default:
				return new global::Kampai.Game.QuestDefinition();
			case global::Kampai.Game.QuestType.TimedQuest:
				return new global::Kampai.Game.TimedQuestDefinition();
			case global::Kampai.Game.QuestType.LimitedQuest:
				return new global::Kampai.Game.LimitedQuestDefinition();
			case global::Kampai.Game.QuestType.DynamicQuest:
				return new global::Kampai.Game.DynamicQuestDefinition();
			default:
				return null;
			}
		}
	}
}

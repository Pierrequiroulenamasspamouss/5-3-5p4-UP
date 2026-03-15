namespace Kampai.Game.Trigger
{
	public class QuestTriggerConditionDefinition : global::Kampai.Game.Trigger.TriggerConditionDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1169;
			}
		}

		public int questDefinitionID { get; set; }

		public int duration { get; set; }

		public override global::Kampai.Game.Trigger.TriggerConditionType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerConditionType.Identifier.Quest;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(questDefinitionID);
			writer.Write(duration);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			questDefinitionID = reader.ReadInt32();
			duration = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "DURATION":
				reader.Read();
				duration = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			case "QUESTDEFINITIONID":
				reader.Read();
				questDefinitionID = global::System.Convert.ToInt32(reader.Value);
				break;
			}
			return true;
		}

		public override bool IsTriggered(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			global::Kampai.Game.IQuestService instance = gameContext.injectionBinder.GetInstance<global::Kampai.Game.IQuestService>();
			if (instance == null)
			{
				return false;
			}
			int actualValue = ((questDefinitionID != 0) ? instance.GetIdleQuestDuration(questDefinitionID) : instance.GetLongestIdleQuestDuration());
			return TestOperator(duration, actualValue);
		}

		public override string ToString()
		{
			return string.Format("{0}, Operator: {1}, Type: {2}, questDefinitionID: {3}, duration: {4}", GetType(), base.conditionOp, type, questDefinitionID, duration);
		}

		public override global::Kampai.Game.Transaction.TransactionDefinition GetDynamicTriggerTransaction(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			global::Kampai.Game.IQuestService instance = gameContext.injectionBinder.GetInstance<global::Kampai.Game.IQuestService>();
			if (instance == null)
			{
				return null;
			}
			global::Kampai.Game.IQuestController questController;
			if (questDefinitionID == 0)
			{
				global::Kampai.Game.IQuestController longestIdleQuestController = instance.GetLongestIdleQuestController();
				questController = longestIdleQuestController;
			}
			else
			{
				questController = instance.GetQuestControllerByDefinitionID(questDefinitionID);
			}
			global::Kampai.Game.IQuestController questController2 = questController;
			if (questController2 != null)
			{
				global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> requiredQuantityItems = questController2.GetRequiredQuantityItems();
				global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = new global::Kampai.Game.Transaction.TransactionDefinition();
				transactionDefinition.Inputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
				transactionDefinition.Outputs = requiredQuantityItems;
				return transactionDefinition;
			}
			return null;
		}
	}
}

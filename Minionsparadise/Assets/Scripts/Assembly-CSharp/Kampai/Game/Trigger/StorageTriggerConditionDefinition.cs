namespace Kampai.Game.Trigger
{
	public class StorageTriggerConditionDefinition : global::Kampai.Game.Trigger.TriggerConditionDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1176;
			}
		}

		public int percent { get; set; }

		public override global::Kampai.Game.Trigger.TriggerConditionType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerConditionType.Identifier.Storage;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(percent);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			percent = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "PERCENT":
				reader.Read();
				percent = global::System.Convert.ToInt32(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override bool IsTriggered(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			global::Kampai.Game.IPlayerService instance = gameContext.injectionBinder.GetInstance<global::Kampai.Game.IPlayerService>();
			if (instance == null)
			{
				return false;
			}
			int currentStorageSpace = GetCurrentStorageSpace(instance);
			if (currentStorageSpace == -1)
			{
				return false;
			}
			return TestOperator(percent, currentStorageSpace);
		}

		public override string ToString()
		{
			return string.Format("{0}, Operator: {1}, Type: {2}, percent: {3}", GetType(), base.conditionOp, type, percent);
		}

		private int GetCurrentStorageSpace(global::Kampai.Game.IPlayerService playerService)
		{
			global::Kampai.Game.StorageBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.StorageBuilding>(314);
			if (byInstanceId == null)
			{
				return -1;
			}
			uint totalQuantity = 0u;
			playerService.GetStorableItems(out totalQuantity);
			uint storageCapacity = byInstanceId.Definition.StorageUpgrades[byInstanceId.CurrentStorageBuildingLevel].StorageCapacity;
			return (int)((float)totalQuantity / (float)storageCapacity * 100f);
		}
	}
}

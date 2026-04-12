namespace Kampai.Game
{
	public class ResourceBuilding : global::Kampai.Game.TaskableBuilding<global::Kampai.Game.ResourceBuildingDefinition>
	{
		public global::System.Collections.Generic.List<int> BonusMinionItems = new global::System.Collections.Generic.List<int>();

		public global::System.Collections.Generic.List<int> minionHarvesters = new global::System.Collections.Generic.List<int>();

		public int AvailableHarvest { get; set; }

		public ResourceBuilding(global::Kampai.Game.ResourceBuildingDefinition def)
			: base(def)
		{
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "AVAILABLEHARVEST":
				reader.Read();
				AvailableHarvest = global::System.Convert.ToInt32(reader.Value);
				break;
			case "BONUSMINIONITEMS":
				reader.Read();
				BonusMinionItems = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, BonusMinionItems);
				break;
			case "MINIONHARVESTERS":
				reader.Read();
				minionHarvesters = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, minionHarvesters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public override void Serialize(global::Newtonsoft.Json.JsonWriter writer)
		{
			writer.WriteStartObject();
			SerializeProperties(writer);
			writer.WriteEndObject();
		}

		protected override void SerializeProperties(global::Newtonsoft.Json.JsonWriter writer)
		{
			base.SerializeProperties(writer);
			writer.WritePropertyName("AvailableHarvest");
			writer.WriteValue(AvailableHarvest);
			if (BonusMinionItems != null)
			{
				writer.WritePropertyName("BonusMinionItems");
				writer.WriteStartArray();
				global::System.Collections.Generic.List<int>.Enumerator enumerator = BonusMinionItems.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						int current = enumerator.Current;
						writer.WriteValue(current);
					}
				}
				finally
				{
					enumerator.Dispose();
				}
				writer.WriteEndArray();
			}
			if (minionHarvesters == null)
			{
				return;
			}
			writer.WritePropertyName("minionHarvesters");
			writer.WriteStartArray();
			global::System.Collections.Generic.List<int>.Enumerator enumerator2 = minionHarvesters.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					int current2 = enumerator2.Current;
					writer.WriteValue(current2);
				}
			}
			finally
			{
				enumerator2.Dispose();
			}
			writer.WriteEndArray();
		}

		public int GetMaxSlotCount()
		{
			return GetProperSlotUnlock().SlotUnlockLevels.Count;
		}

		public int GetSlotCostByIndex(int i)
		{
			return GetProperSlotUnlock().SlotUnlockCosts[i];
		}

		public int GetSlotUnlockLevelByIndex(int i)
		{
			return GetProperSlotUnlock().SlotUnlockLevels[i];
		}

		private global::Kampai.Game.SlotUnlock GetProperSlotUnlock()
		{
			int count = base.Definition.SlotUnlocks.Count;
			int index = ((BuildingNumber <= count) ? (BuildingNumber - 1) : (count - 1));
			return base.Definition.SlotUnlocks[index];
		}

		public void IncrementMinionSlotsOwned()
		{
			base.MinionSlotsOwned++;
		}

		public override global::Kampai.Game.View.BuildingObject AddBuildingObject(global::UnityEngine.GameObject gameObject)
		{
			return gameObject.AddComponent<global::Kampai.Game.View.GaggableBuildingObject>();
		}

		public override int GetTransactionID(global::Kampai.Game.IDefinitionService definitionService)
		{
			int itemId = base.Definition.ItemId;
			global::Kampai.Game.IngredientsItemDefinition ingredientsItemDefinition = definitionService.Get<global::Kampai.Game.IngredientsItemDefinition>(itemId);
			return ingredientsItemDefinition.TransactionId;
		}

		public void PrepareForHarvest(int utcTime, int minionId)
		{
			ReconcileMinionStoppedTasking(utcTime);
			AvailableHarvest++;
			minionHarvesters.Add(minionId);
		}

		public int GetLastMinionToHarvest()
		{
			if (minionHarvesters.Count > 0)
			{
				return minionHarvesters[0];
			}
			return -1;
		}

		public void CompleteHarvest()
		{
			AvailableHarvest--;
			if (minionHarvesters.Count > 0)
			{
				minionHarvesters.RemoveAt(0);
			}
		}

		public override int GetAvailableHarvest()
		{
			return AvailableHarvest;
		}

		public int GetTotalHarvests()
		{
			return AvailableHarvest + BonusMinionItems.Count;
		}
	}
}

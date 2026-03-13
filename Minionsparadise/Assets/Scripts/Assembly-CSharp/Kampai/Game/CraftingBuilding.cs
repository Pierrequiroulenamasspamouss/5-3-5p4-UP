namespace Kampai.Game
{
	public class CraftingBuilding : global::Kampai.Game.Building<global::Kampai.Game.CraftingBuildingDefinition>
	{
		public int Slots { get; set; }

		public global::System.Collections.Generic.IList<int> RecipeInQueue { get; set; }

		public global::System.Collections.Generic.IList<int> CompletedCrafts { get; set; }

		public int CraftingStartTime { get; set; }

		public int PartyTimeReduction { get; set; }

		public CraftingBuilding(global::Kampai.Game.CraftingBuildingDefinition def)
			: base(def)
		{
			Slots = def.InitialSlots;
			RecipeInQueue = new global::System.Collections.Generic.List<int>();
			CompletedCrafts = new global::System.Collections.Generic.List<int>();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "SLOTS":
				reader.Read();
				Slots = global::System.Convert.ToInt32(reader.Value);
				break;
			case "RECIPEINQUEUE":
				reader.Read();
				RecipeInQueue = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, RecipeInQueue);
				break;
			case "COMPLETEDCRAFTS":
				reader.Read();
				CompletedCrafts = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, CompletedCrafts);
				break;
			case "CRAFTINGSTARTTIME":
				reader.Read();
				CraftingStartTime = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PARTYTIMEREDUCTION":
				reader.Read();
				PartyTimeReduction = global::System.Convert.ToInt32(reader.Value);
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
			writer.WritePropertyName("Slots");
			writer.WriteValue(Slots);
			if (RecipeInQueue != null)
			{
				writer.WritePropertyName("RecipeInQueue");
				writer.WriteStartArray();
				global::System.Collections.Generic.IEnumerator<int> enumerator = RecipeInQueue.GetEnumerator();
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
			if (CompletedCrafts != null)
			{
				writer.WritePropertyName("CompletedCrafts");
				writer.WriteStartArray();
				global::System.Collections.Generic.IEnumerator<int> enumerator2 = CompletedCrafts.GetEnumerator();
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
			writer.WritePropertyName("CraftingStartTime");
			writer.WriteValue(CraftingStartTime);
			writer.WritePropertyName("PartyTimeReduction");
			writer.WriteValue(PartyTimeReduction);
		}

		public int getNextIncrementalCost()
		{
			return base.Definition.SlotCost + (Slots - base.Definition.InitialSlots) * base.Definition.SlotIncrementalCost;
		}

		public override global::Kampai.Game.View.BuildingObject AddBuildingObject(global::UnityEngine.GameObject gameObject)
		{
			return gameObject.AddComponent<global::Kampai.Game.View.CraftableBuildingObject>();
		}
	}
}

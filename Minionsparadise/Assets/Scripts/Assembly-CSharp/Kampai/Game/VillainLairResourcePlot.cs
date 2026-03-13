namespace Kampai.Game
{
	public class VillainLairResourcePlot : global::Kampai.Game.Building<global::Kampai.Game.VillainLairResourcePlotDefinition>
	{
		public global::System.Collections.Generic.List<int> BonusMinionItems = new global::System.Collections.Generic.List<int>();

		public int MinionIDInBuilding;

		public int LastMinionTasked;

		public int rotation { get; set; }

		public int unlockTransactionID { get; set; }

		public global::Kampai.Game.VillainLair parentLair { get; set; }

		public int indexInLairResourcePlots { get; set; }

		public int UTCLastTaskingTimeStarted { get; set; }

		public int harvestCount { get; set; }

		public VillainLairResourcePlot(global::Kampai.Game.VillainLairResourcePlotDefinition def)
			: base(def)
		{
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "ROTATION":
				reader.Read();
				rotation = global::System.Convert.ToInt32(reader.Value);
				break;
			case "UNLOCKTRANSACTIONID":
				reader.Read();
				unlockTransactionID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PARENTLAIR":
				reader.Read();
				parentLair = (global::Kampai.Game.VillainLair)converters.instanceConverter.ReadJson(reader, converters);
				break;
			case "INDEXINLAIRRESOURCEPLOTS":
				reader.Read();
				indexInLairResourcePlots = global::System.Convert.ToInt32(reader.Value);
				break;
			case "UTCLASTTASKINGTIMESTARTED":
				reader.Read();
				UTCLastTaskingTimeStarted = global::System.Convert.ToInt32(reader.Value);
				break;
			case "HARVESTCOUNT":
				reader.Read();
				harvestCount = global::System.Convert.ToInt32(reader.Value);
				break;
			case "BONUSMINIONITEMS":
				reader.Read();
				BonusMinionItems = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, BonusMinionItems);
				break;
			case "MINIONIDINBUILDING":
				reader.Read();
				MinionIDInBuilding = global::System.Convert.ToInt32(reader.Value);
				break;
			case "LASTMINIONTASKED":
				reader.Read();
				LastMinionTasked = global::System.Convert.ToInt32(reader.Value);
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
			writer.WritePropertyName("rotation");
			writer.WriteValue(rotation);
			writer.WritePropertyName("unlockTransactionID");
			writer.WriteValue(unlockTransactionID);
			if (parentLair != null)
			{
				writer.WritePropertyName("parentLair");
				parentLair.Serialize(writer);
			}
			writer.WritePropertyName("indexInLairResourcePlots");
			writer.WriteValue(indexInLairResourcePlots);
			writer.WritePropertyName("UTCLastTaskingTimeStarted");
			writer.WriteValue(UTCLastTaskingTimeStarted);
			writer.WritePropertyName("harvestCount");
			writer.WriteValue(harvestCount);
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
			writer.WritePropertyName("MinionIDInBuilding");
			writer.WriteValue(MinionIDInBuilding);
			writer.WritePropertyName("LastMinionTasked");
			writer.WriteValue(LastMinionTasked);
		}

		public override global::Kampai.Game.View.BuildingObject AddBuildingObject(global::UnityEngine.GameObject gameObject)
		{
			return gameObject.AddComponent<global::Kampai.Game.View.VillainLairResourcePlotObjectView>();
		}

		public void AddMinion(int minionID, int utcTime)
		{
			if (!MinionIsTaskedToBuilding())
			{
				MinionIDInBuilding = minionID;
				LastMinionTasked = minionID;
				if (UTCLastTaskingTimeStarted == 0)
				{
					UTCLastTaskingTimeStarted = utcTime;
				}
			}
		}

		public bool MinionIsTaskedToBuilding()
		{
			if (MinionIDInBuilding != 0)
			{
				return true;
			}
			return false;
		}

		public void ClearMinionInBuilding()
		{
			MinionIDInBuilding = 0;
		}
	}
}

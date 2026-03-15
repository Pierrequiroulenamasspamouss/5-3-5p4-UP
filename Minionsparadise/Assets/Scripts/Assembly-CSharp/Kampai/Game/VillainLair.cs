namespace Kampai.Game
{
	public class VillainLair : global::Kampai.Game.Instance<global::Kampai.Game.VillainLairDefinition>
	{
		public bool hasVisited { get; set; }

		public global::System.Collections.Generic.List<int> resourcePlotInstanceIDs { get; set; }

		public int portalInstanceID { get; set; }

		public VillainLair(global::Kampai.Game.VillainLairDefinition def)
			: base(def)
		{
			resourcePlotInstanceIDs = new global::System.Collections.Generic.List<int>();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "HASVISITED":
				reader.Read();
				hasVisited = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "RESOURCEPLOTINSTANCEIDS":
				reader.Read();
				resourcePlotInstanceIDs = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, resourcePlotInstanceIDs);
				break;
			case "PORTALINSTANCEID":
				reader.Read();
				portalInstanceID = global::System.Convert.ToInt32(reader.Value);
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
			writer.WritePropertyName("hasVisited");
			writer.WriteValue(hasVisited);
			if (resourcePlotInstanceIDs != null)
			{
				writer.WritePropertyName("resourcePlotInstanceIDs");
				writer.WriteStartArray();
				global::System.Collections.Generic.List<int>.Enumerator enumerator = resourcePlotInstanceIDs.GetEnumerator();
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
			writer.WritePropertyName("portalInstanceID");
			writer.WriteValue(portalInstanceID);
		}

		public global::System.Collections.Generic.List<int> GetAllPlotBonusItems(global::Kampai.Game.IPlayerService playerService)
		{
			global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
			foreach (int resourcePlotInstanceID in resourcePlotInstanceIDs)
			{
				global::Kampai.Game.VillainLairResourcePlot byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.VillainLairResourcePlot>(resourcePlotInstanceID);
				if (byInstanceId.BonusMinionItems.Count > 0)
				{
					list.AddRange(byInstanceId.BonusMinionItems);
				}
			}
			return list;
		}

		public int GetFirstBuildingNumberOfHarvestableResourcePlot(global::Kampai.Game.IPlayerService playerService, bool isBonus)
		{
			foreach (int resourcePlotInstanceID in resourcePlotInstanceIDs)
			{
				global::Kampai.Game.VillainLairResourcePlot byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.VillainLairResourcePlot>(resourcePlotInstanceID);
				if (isBonus)
				{
					if (byInstanceId.BonusMinionItems.Count > 0)
					{
						return byInstanceId.ID;
					}
				}
				else if (byInstanceId.State == global::Kampai.Game.BuildingState.Harvestable)
				{
					return byInstanceId.ID;
				}
			}
			return 0;
		}
	}
}

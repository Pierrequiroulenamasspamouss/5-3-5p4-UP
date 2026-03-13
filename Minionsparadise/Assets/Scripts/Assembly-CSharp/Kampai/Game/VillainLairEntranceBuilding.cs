namespace Kampai.Game
{
	public class VillainLairEntranceBuilding : global::Kampai.Game.RepairableBuilding<global::Kampai.Game.VillainLairEntranceBuildingDefinition>
	{
		public bool IsUnlocked { get; set; }

		public int VillainLairInstanceID { get; set; }

		public VillainLairEntranceBuilding(global::Kampai.Game.VillainLairEntranceBuildingDefinition def)
			: base(def)
		{
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			default:
			{
                        int num = 1; //FIX USE OF UNASSIGNED VARIABLE
                        if (num == 1)
				{
					reader.Read();
					VillainLairInstanceID = global::System.Convert.ToInt32(reader.Value);
					break;
				}
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			case "ISUNLOCKED":
				reader.Read();
				IsUnlocked = global::System.Convert.ToBoolean(reader.Value);
				break;
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
			writer.WritePropertyName("IsUnlocked");
			writer.WriteValue(IsUnlocked);
			writer.WritePropertyName("VillainLairInstanceID");
			writer.WriteValue(VillainLairInstanceID);
		}

		public override global::Kampai.Game.View.BuildingObject AddBuildingObject(global::UnityEngine.GameObject gameObject)
		{
			return gameObject.AddComponent<global::Kampai.Game.View.VillainLairEntranceBuildingObject>();
		}

		public void unlockVillainLair(int lairInstanceID)
		{
			IsUnlocked = true;
			VillainLairInstanceID = lairInstanceID;
		}

		public global::Kampai.Util.Tuple<int, int> GetNewHarvestAvailableForPortal(global::Kampai.Game.IPlayerService playerService)
		{
			int num = 0;
			int num2 = 0;
			global::Kampai.Game.VillainLair byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.VillainLair>(VillainLairInstanceID);
			foreach (int resourcePlotInstanceID in byInstanceId.resourcePlotInstanceIDs)
			{
				global::Kampai.Game.VillainLairResourcePlot byInstanceId2 = playerService.GetByInstanceId<global::Kampai.Game.VillainLairResourcePlot>(resourcePlotInstanceID);
				if (byInstanceId2.State == global::Kampai.Game.BuildingState.Harvestable)
				{
					num++;
				}
				if (byInstanceId2.BonusMinionItems.Count > 0)
				{
					num2 += byInstanceId2.BonusMinionItems.Count;
				}
			}
			return new global::Kampai.Util.Tuple<int, int>(num, num2);
		}
	}
}

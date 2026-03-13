namespace Kampai.Game
{
	public class Minion : global::Kampai.Game.Character<global::Kampai.Game.MinionDefinition>, global::Kampai.Util.Prestigable, global::Kampai.Util.Taskable
	{
		public int BuildingID { get; set; }

		public global::Kampai.Game.MinionState State { get; set; }

		public int TaskDuration { get; set; }

		public int UTCTaskStartTime { get; set; }

		public int PartyTimeReduction { get; set; }

		public bool AlreadyRushed { get; set; }

		public int PrestigeId { get; set; }

		public bool IsInMinionParty { get; set; }

		public int Level { get; set; }

		[global::Newtonsoft.Json.JsonIgnore]
		public bool IsDoingPartyFavorAnimation { get; set; }

		[global::Newtonsoft.Json.JsonIgnore]
		public bool Partying { get; set; }

		[global::Newtonsoft.Json.JsonIgnore]
		public bool IsInIncidental { get; set; }

		[global::Newtonsoft.Json.JsonIgnore]
		public bool HasPrestige
		{
			get
			{
				return PrestigeId > 0;
			}
		}

		public Minion(global::Kampai.Game.MinionDefinition def)
			: base(def)
		{
			base.Definition = def;
			Partying = true;
			PrestigeId = -1;
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "BUILDINGID":
				reader.Read();
				BuildingID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "STATE":
				reader.Read();
				State = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.MinionState>(reader);
				break;
			case "TASKDURATION":
				reader.Read();
				TaskDuration = global::System.Convert.ToInt32(reader.Value);
				break;
			case "UTCTASKSTARTTIME":
				reader.Read();
				UTCTaskStartTime = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PARTYTIMEREDUCTION":
				reader.Read();
				PartyTimeReduction = global::System.Convert.ToInt32(reader.Value);
				break;
			case "ALREADYRUSHED":
				reader.Read();
				AlreadyRushed = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "PRESTIGEID":
				reader.Read();
				PrestigeId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "ISINMINIONPARTY":
				reader.Read();
				IsInMinionParty = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "LEVEL":
				reader.Read();
				Level = global::System.Convert.ToInt32(reader.Value);
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
			writer.WritePropertyName("BuildingID");
			writer.WriteValue(BuildingID);
			writer.WritePropertyName("State");
			writer.WriteValue((int)State);
			writer.WritePropertyName("TaskDuration");
			writer.WriteValue(TaskDuration);
			writer.WritePropertyName("UTCTaskStartTime");
			writer.WriteValue(UTCTaskStartTime);
			writer.WritePropertyName("PartyTimeReduction");
			writer.WriteValue(PartyTimeReduction);
			writer.WritePropertyName("AlreadyRushed");
			writer.WriteValue(AlreadyRushed);
			writer.WritePropertyName("PrestigeId");
			writer.WriteValue(PrestigeId);
			writer.WritePropertyName("IsInMinionParty");
			writer.WriteValue(IsInMinionParty);
			writer.WritePropertyName("Level");
			writer.WriteValue(Level);
		}

		public int GetCostumeId(global::Kampai.Game.IPlayerService playerService, global::Kampai.Game.IDefinitionService definitionService)
		{
			int result = 99;
			if (HasPrestige)
			{
				global::Kampai.Game.Prestige byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Prestige>(PrestigeId);
				if (byInstanceId != null && byInstanceId.Definition != null && byInstanceId.Definition.CostumeDefinitionID > 0)
				{
					result = byInstanceId.Definition.CostumeDefinitionID;
				}
			}
			else
			{
				global::Kampai.Game.MinionBenefitLevelBandDefintion minionBenefitLevelBandDefintion = definitionService.Get<global::Kampai.Game.MinionBenefitLevelBandDefintion>(global::Kampai.Game.StaticItem.MINION_BENEFITS_DEF_ID);
				global::Kampai.Game.MinionBenefitLevel minionBenefit = minionBenefitLevelBandDefintion.GetMinionBenefit(Level);
				if (minionBenefit != null)
				{
					result = minionBenefit.costumeId;
				}
			}
			return result;
		}
	}
}

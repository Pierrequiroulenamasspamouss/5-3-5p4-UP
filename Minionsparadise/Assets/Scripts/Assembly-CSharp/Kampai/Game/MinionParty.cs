namespace Kampai.Game
{
	public class MinionParty : global::Kampai.Game.Instance<global::Kampai.Game.MinionPartyDefinition>
	{
		public int BuffStartTime { get; set; }

		public int NewBuffStartTime { get; set; }

		public int PartyStartTier { get; set; }

		public global::Kampai.Game.MinionPartyType PartyType { get; set; }

		public bool IsPartyHappening { get; set; }

		public bool IsBuffHappening { get; set; }

		public int CurrentPartyIndex { get; set; }

		public int TotalLevelPartiesCount { get; set; }

		public uint CurrentPartyPoints { get; set; }

		public uint CurrentPartyPointsRequired { get; set; }

		public global::System.Collections.Generic.List<int> lastGuestsOfHonorPrestigeIDs { get; set; }

		[global::Newtonsoft.Json.JsonIgnore]
		public bool CharacterUnlocking { get; set; }

		[global::Newtonsoft.Json.JsonIgnore]
		public bool PartyPreSkip { get; set; }

		public bool IsPartyReady
		{
			get
			{
				return CurrentPartyPointsRequired != 0 && CurrentPartyPoints >= CurrentPartyPointsRequired;
			}
		}

		public MinionParty(global::Kampai.Game.MinionPartyDefinition definition)
		{
			lastGuestsOfHonorPrestigeIDs = new global::System.Collections.Generic.List<int>();
			base.Definition = definition;
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "BUFFSTARTTIME":
				reader.Read();
				BuffStartTime = global::System.Convert.ToInt32(reader.Value);
				break;
			case "NEWBUFFSTARTTIME":
				reader.Read();
				NewBuffStartTime = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PARTYSTARTTIER":
				reader.Read();
				PartyStartTier = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PARTYTYPE":
				reader.Read();
				PartyType = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.MinionPartyType>(reader);
				break;
			case "ISPARTYHAPPENING":
				reader.Read();
				IsPartyHappening = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "ISBUFFHAPPENING":
				reader.Read();
				IsBuffHappening = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "CURRENTPARTYINDEX":
				reader.Read();
				CurrentPartyIndex = global::System.Convert.ToInt32(reader.Value);
				break;
			case "TOTALLEVELPARTIESCOUNT":
				reader.Read();
				TotalLevelPartiesCount = global::System.Convert.ToInt32(reader.Value);
				break;
			case "CURRENTPARTYPOINTS":
				reader.Read();
				CurrentPartyPoints = global::System.Convert.ToUInt32(reader.Value);
				break;
			case "CURRENTPARTYPOINTSREQUIRED":
				reader.Read();
				CurrentPartyPointsRequired = global::System.Convert.ToUInt32(reader.Value);
				break;
			case "LASTGUESTSOFHONORPRESTIGEIDS":
				reader.Read();
				lastGuestsOfHonorPrestigeIDs = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, lastGuestsOfHonorPrestigeIDs);
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
			writer.WritePropertyName("BuffStartTime");
			writer.WriteValue(BuffStartTime);
			writer.WritePropertyName("NewBuffStartTime");
			writer.WriteValue(NewBuffStartTime);
			writer.WritePropertyName("PartyStartTier");
			writer.WriteValue(PartyStartTier);
			writer.WritePropertyName("PartyType");
			writer.WriteValue((int)PartyType);
			writer.WritePropertyName("IsPartyHappening");
			writer.WriteValue(IsPartyHappening);
			writer.WritePropertyName("IsBuffHappening");
			writer.WriteValue(IsBuffHappening);
			writer.WritePropertyName("CurrentPartyIndex");
			writer.WriteValue(CurrentPartyIndex);
			writer.WritePropertyName("TotalLevelPartiesCount");
			writer.WriteValue(TotalLevelPartiesCount);
			writer.WritePropertyName("CurrentPartyPoints");
			writer.WriteValue(CurrentPartyPoints);
			writer.WritePropertyName("CurrentPartyPointsRequired");
			writer.WriteValue(CurrentPartyPointsRequired);
			if (lastGuestsOfHonorPrestigeIDs == null)
			{
				return;
			}
			writer.WritePropertyName("lastGuestsOfHonorPrestigeIDs");
			writer.WriteStartArray();
			global::System.Collections.Generic.List<int>.Enumerator enumerator = lastGuestsOfHonorPrestigeIDs.GetEnumerator();
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

		public void ResolveBuffStartTime()
		{
			BuffStartTime = ((NewBuffStartTime <= BuffStartTime) ? BuffStartTime : NewBuffStartTime);
		}

		public int DeterminePartyTier(uint currentLevel)
		{
			int result = 0;
			for (int i = 0; i < base.Definition.partyMeterDefinition.Tiers.Count && currentLevel >= base.Definition.partyMeterDefinition.Tiers[i].Level; i++)
			{
				result = i;
			}
			return result;
		}
	}
}

namespace Kampai.Game
{
	public class Prestige : global::Kampai.Game.Instance<global::Kampai.Game.PrestigeDefinition>, global::Kampai.Game.IGameTimeTracker, global::Kampai.Game.ItemAccumulator
	{
		public int trackedInstanceId { get; set; }

		public global::Kampai.Game.PrestigeState state { get; set; }

		public int CurrentPrestigeLevel { get; set; }

		public int CurrentPrestigePoints { get; set; }

		public int CurrentOrdersCompleted { get; set; }

		public int UTCTimeUnlocked { get; set; }

		public bool onCooldown { get; set; }

		public int numPartiesInvited { get; set; }

		public int numPartiesThrown { get; set; }

		public int StartGameTime { get; set; }

		[global::Newtonsoft.Json.JsonIgnore]
		public int NeededPrestigePoints
		{
			get
			{
				return (int)GetCurrentPrestigeLevelDefinition().PointsNeeded;
			}
		}

		[global::Newtonsoft.Json.JsonIgnore]
		public string CurrentWelcomeMessage
		{
			get
			{
				return GetCurrentPrestigeLevelDefinition().WelcomePanelMessageLocalizedKey;
			}
		}

		[global::Newtonsoft.Json.JsonIgnore]
		public string CurrentFarewellMessage
		{
			get
			{
				return GetCurrentPrestigeLevelDefinition().FarewellPanelMessageLocalizedKey;
			}
		}

		public Prestige(global::Kampai.Game.PrestigeDefinition def)
			: base(def)
		{
			CurrentPrestigeLevel = -2;
			CurrentPrestigePoints = 0;
			trackedInstanceId = 0;
			state = global::Kampai.Game.PrestigeState.Locked;
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "TRACKEDINSTANCEID":
				reader.Read();
				trackedInstanceId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "STATE":
				reader.Read();
				state = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.PrestigeState>(reader);
				break;
			case "CURRENTPRESTIGELEVEL":
				reader.Read();
				CurrentPrestigeLevel = global::System.Convert.ToInt32(reader.Value);
				break;
			case "CURRENTPRESTIGEPOINTS":
				reader.Read();
				CurrentPrestigePoints = global::System.Convert.ToInt32(reader.Value);
				break;
			case "CURRENTORDERSCOMPLETED":
				reader.Read();
				CurrentOrdersCompleted = global::System.Convert.ToInt32(reader.Value);
				break;
			case "UTCTIMEUNLOCKED":
				reader.Read();
				UTCTimeUnlocked = global::System.Convert.ToInt32(reader.Value);
				break;
			case "ONCOOLDOWN":
				reader.Read();
				onCooldown = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "NUMPARTIESINVITED":
				reader.Read();
				numPartiesInvited = global::System.Convert.ToInt32(reader.Value);
				break;
			case "NUMPARTIESTHROWN":
				reader.Read();
				numPartiesThrown = global::System.Convert.ToInt32(reader.Value);
				break;
			case "STARTGAMETIME":
				reader.Read();
				StartGameTime = global::System.Convert.ToInt32(reader.Value);
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
			writer.WritePropertyName("trackedInstanceId");
			writer.WriteValue(trackedInstanceId);
			writer.WritePropertyName("state");
			writer.WriteValue((int)state);
			writer.WritePropertyName("CurrentPrestigeLevel");
			writer.WriteValue(CurrentPrestigeLevel);
			writer.WritePropertyName("CurrentPrestigePoints");
			writer.WriteValue(CurrentPrestigePoints);
			writer.WritePropertyName("CurrentOrdersCompleted");
			writer.WriteValue(CurrentOrdersCompleted);
			writer.WritePropertyName("UTCTimeUnlocked");
			writer.WriteValue(UTCTimeUnlocked);
			writer.WritePropertyName("onCooldown");
			writer.WriteValue(onCooldown);
			writer.WritePropertyName("numPartiesInvited");
			writer.WriteValue(numPartiesInvited);
			writer.WritePropertyName("numPartiesThrown");
			writer.WriteValue(numPartiesThrown);
			writer.WritePropertyName("StartGameTime");
			writer.WriteValue(StartGameTime);
		}

		public void AwardOutput(global::Kampai.Util.QuantityItem item)
		{
			if (item.ID == 2)
			{
				CurrentPrestigePoints += (int)item.Quantity;
			}
		}

		public override string ToString()
		{
			return string.Format("{0}(ID:{1}, State:{2}, Definition:{3})", typeof(global::Kampai.Game.Prestige).FullName, ID, state, base.Definition);
		}

		private global::Kampai.Game.CharacterPrestigeLevelDefinition GetCurrentPrestigeLevelDefinition()
		{
			if (base.Definition.PrestigeLevelSettings == null)
			{
				return new global::Kampai.Game.CharacterPrestigeLevelDefinition();
			}
			CurrentPrestigeLevel = ((CurrentPrestigeLevel < base.Definition.PrestigeLevelSettings.Count) ? CurrentPrestigeLevel : (base.Definition.PrestigeLevelSettings.Count - 1));
			return base.Definition.PrestigeLevelSettings[(CurrentPrestigeLevel >= 1) ? CurrentPrestigeLevel : 0];
		}
	}
}

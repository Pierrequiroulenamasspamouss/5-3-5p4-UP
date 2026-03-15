namespace Kampai.Game
{
	[global::Kampai.Util.RequiresJsonConverter]
	public abstract class FrolicCharacterDefinition : global::Kampai.Game.NamedCharacterDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1077;
			}
		}

		public string WanderStateMachine { get; set; }

		public global::Kampai.Game.Transaction.WeightedDefinition WanderWeightedDeck { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.LocationIncidentalAnimationDefinition> WanderAnimations { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, WanderStateMachine);
			global::Kampai.Util.BinarySerializationUtil.WriteObject(writer, WanderWeightedDeck);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, WanderAnimations);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			WanderStateMachine = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			WanderWeightedDeck = global::Kampai.Util.BinarySerializationUtil.ReadObject<global::Kampai.Game.Transaction.WeightedDefinition>(reader);
			WanderAnimations = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, WanderAnimations);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "WANDERSTATEMACHINE":
				reader.Read();
				WanderStateMachine = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "WANDERWEIGHTEDDECK":
				reader.Read();
				WanderWeightedDeck = global::Kampai.Util.FastJSONDeserializer.Deserialize<global::Kampai.Game.Transaction.WeightedDefinition>(reader, converters);
				break;
			case "WANDERANIMATIONS":
				reader.Read();
				WanderAnimations = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, WanderAnimations);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}

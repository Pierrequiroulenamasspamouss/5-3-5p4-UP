namespace Kampai.Game
{
	public class BobCharacterDefinition : global::Kampai.Game.FrolicCharacterDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1075;
			}
		}

		public global::Kampai.Game.MinionAnimationDefinition CelebrateAnimation { get; set; }

		public global::Kampai.Game.MinionAnimationDefinition AttentionAnimation { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteObject(writer, CelebrateAnimation);
			global::Kampai.Util.BinarySerializationUtil.WriteObject(writer, AttentionAnimation);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			CelebrateAnimation = global::Kampai.Util.BinarySerializationUtil.ReadObject<global::Kampai.Game.MinionAnimationDefinition>(reader);
			AttentionAnimation = global::Kampai.Util.BinarySerializationUtil.ReadObject<global::Kampai.Game.MinionAnimationDefinition>(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			default:
                        return base.DeserializeProperty(propertyName, reader, converters);
            case "CELEBRATEANIMATION":
				reader.Read();
				CelebrateAnimation = global::Kampai.Util.FastJSONDeserializer.Deserialize<global::Kampai.Game.MinionAnimationDefinition>(reader, converters);
				break;
			case "ATTENTIONANIMATION":
                    reader.Read();
                    AttentionAnimation = global::Kampai.Util.FastJSONDeserializer.Deserialize<global::Kampai.Game.MinionAnimationDefinition>(reader, converters);
                    break;
            }
			return true;
		}

		public override global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.BobCharacter(this);
		}
	}
}

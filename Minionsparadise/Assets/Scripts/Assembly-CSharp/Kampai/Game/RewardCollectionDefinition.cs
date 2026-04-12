namespace Kampai.Game
{
	public class RewardCollectionDefinition : global::Kampai.Game.Definition, global::Kampai.Util.IBuilder<global::Kampai.Game.Instance>
	{
		public override int TypeCode
		{
			get
			{
				return 1085;
			}
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.CollectionReward> Rewards { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteCollectionReward, Rewards);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Rewards = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadCollectionReward, Rewards);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "REWARDS":
				reader.Read();
				Rewards = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadCollectionReward, Rewards);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.RewardCollection(this);
		}
	}
}

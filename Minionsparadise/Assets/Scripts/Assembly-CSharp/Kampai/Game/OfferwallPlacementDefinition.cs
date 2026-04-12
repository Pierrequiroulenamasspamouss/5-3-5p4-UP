namespace Kampai.Game
{
	public class OfferwallPlacementDefinition : global::Kampai.Game.AdPlacementDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1029;
			}
		}

		public int RewardItemId { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(RewardItemId);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			RewardItemId = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "REWARDITEMID":
				reader.Read();
				RewardItemId = global::System.Convert.ToInt32(reader.Value);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}
	}
}

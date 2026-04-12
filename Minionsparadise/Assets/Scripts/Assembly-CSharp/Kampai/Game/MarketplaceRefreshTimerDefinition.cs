namespace Kampai.Game
{
	public class MarketplaceRefreshTimerDefinition : global::Kampai.Game.Definition, global::Kampai.Util.IBuilder<global::Kampai.Game.Instance>
	{
		public override int TypeCode
		{
			get
			{
				return 1105;
			}
		}

		public int RefreshTimeSeconds { get; set; }

		public int RushCost { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(RefreshTimeSeconds);
			writer.Write(RushCost);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			RefreshTimeSeconds = reader.ReadInt32();
			RushCost = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "RUSHCOST":
				reader.Read();
				RushCost = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			case "REFRESHTIMESECONDS":
				reader.Read();
				RefreshTimeSeconds = global::System.Convert.ToInt32(reader.Value);
				break;
			}
			return true;
		}

		public global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.MarketplaceRefreshTimer(this);
		}
	}
}

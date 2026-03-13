namespace Kampai.Game
{
	[global::Kampai.Util.RequiresJsonConverter]
	public class ItemDefinition : global::Kampai.Game.TaxonomyDefinition, global::Kampai.Util.IBuilder<global::Kampai.Game.Instance>
	{
		public override int TypeCode
		{
			get
			{
				return 1089;
			}
		}

		public float BasePremiumCost { get; set; }

		public int BaseGrindCost { get; set; }

		public float TSMRewardMultipler { get; set; }

		public int PlayerTrainingDefinitionID { get; set; }

		public bool Storable { get; set; }

		public bool SellableForced { get; set; }

		public ItemDefinition()
		{
			Storable = true;
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(BasePremiumCost);
			writer.Write(BaseGrindCost);
			writer.Write(TSMRewardMultipler);
			writer.Write(PlayerTrainingDefinitionID);
			writer.Write(Storable);
			writer.Write(SellableForced);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			BasePremiumCost = reader.ReadSingle();
			BaseGrindCost = reader.ReadInt32();
			TSMRewardMultipler = reader.ReadSingle();
			PlayerTrainingDefinitionID = reader.ReadInt32();
			Storable = reader.ReadBoolean();
			SellableForced = reader.ReadBoolean();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "BASEPREMIUMCOST":
				reader.Read();
				BasePremiumCost = global::System.Convert.ToSingle(reader.Value);
				break;
			case "BASEGRINDCOST":
				reader.Read();
				BaseGrindCost = global::System.Convert.ToInt32(reader.Value);
				break;
			case "TSMREWARDMULTIPLER":
				reader.Read();
				TSMRewardMultipler = global::System.Convert.ToSingle(reader.Value);
				break;
			case "PLAYERTRAININGDEFINITIONID":
				reader.Read();
				PlayerTrainingDefinitionID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "STORABLE":
				reader.Read();
				Storable = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "SELLABLEFORCED":
				reader.Read();
				SellableForced = global::System.Convert.ToBoolean(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public virtual global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.Item(this);
		}
	}
}

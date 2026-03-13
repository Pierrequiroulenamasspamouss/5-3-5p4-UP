namespace Kampai.Game
{
	public class MinionBenefitLevelBandDefintion : global::Kampai.Game.Definition
	{
		private static readonly global::Kampai.Game.MinionBenefitLevel defaultBenefit = new global::Kampai.Game.MinionBenefitLevel
		{
			doubleDropPercentage = 0f,
			premiumDropPercentage = 0f,
			rareDropPercentage = 0f,
			costumeId = -1
		};

		public override int TypeCode
		{
			get
			{
				return 1121;
			}
		}

		public global::System.Collections.Generic.List<global::Kampai.Game.MinionBenefit> benefitDescriptions { get; set; }

		public global::System.Collections.Generic.List<global::Kampai.Game.MinionBenefitLevel> minionBenefitLevelBands { get; set; }

		public int BonusPremiumRewardValue { get; set; }

		public int FirstBuildingId { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteMinionBenefit, benefitDescriptions);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteMinionBenefitLevel, minionBenefitLevelBands);
			writer.Write(BonusPremiumRewardValue);
			writer.Write(FirstBuildingId);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			benefitDescriptions = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadMinionBenefit, benefitDescriptions);
			minionBenefitLevelBands = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadMinionBenefitLevel, minionBenefitLevelBands);
			BonusPremiumRewardValue = reader.ReadInt32();
			FirstBuildingId = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "BENEFITDESCRIPTIONS":
				reader.Read();
				benefitDescriptions = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadMinionBenefit, benefitDescriptions);
				break;
			case "MINIONBENEFITLEVELBANDS":
				reader.Read();
				minionBenefitLevelBands = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadMinionBenefitLevel, minionBenefitLevelBands);
				break;
			case "BONUSPREMIUMREWARDVALUE":
				reader.Read();
				BonusPremiumRewardValue = global::System.Convert.ToInt32(reader.Value);
				break;
			case "FIRSTBUILDINGID":
				reader.Read();
				FirstBuildingId = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public global::Kampai.Game.MinionBenefitLevel GetMinionBenefit(int level)
		{
			if (minionBenefitLevelBands != null && level < minionBenefitLevelBands.Count)
			{
				return minionBenefitLevelBands[level];
			}
			return defaultBenefit;
		}
	}
}

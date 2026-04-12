namespace Kampai.Game
{
	public class DynamicMasterPlanDefinition : global::Kampai.Game.Definition
	{
		public global::System.Collections.Generic.IList<global::Kampai.Game.MasterPlanComponentDefinition> DynamicComponents = new global::System.Collections.Generic.List<global::Kampai.Game.MasterPlanComponentDefinition>();

		public override int TypeCode
		{
			get
			{
				return 1106;
			}
		}

		public int ItemCategoryCount { get; set; }

		public uint EarnSandDollarMin { get; set; }

		public uint EarnSandDollarMax { get; set; }

		public uint FillOrderRangeMin { get; set; }

		public uint FillOrderRangeMax { get; set; }

		public uint MinProductionCount { get; set; }

		public uint MaxProductionCount { get; set; }

		public uint MaxProductionTime { get; set; }

		public float MaxStorageCapactiy { get; set; }

		public uint PartyPointsRangeMin { get; set; }

		public uint PartyPointsRangeMax { get; set; }

		public uint PlayMiniGameRangeMin { get; set; }

		public uint PlayMiniGameRangeMax { get; set; }

		public uint MinGrindReward { get; set; }

		public uint MinPremiumReward { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.Reward> RewardTableCompleteOrders { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.Reward> RewardTablePlayMiniGame { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.Reward> RewardTableEarnPartyPoints { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.Reward> RewardTableEarnSandDollars { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.MiniGameScoreReward> RewardTableMiniGameScore { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.MiniGameScoreRange> RequirementTableMiniGameScore { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(ItemCategoryCount);
			writer.Write(EarnSandDollarMin);
			writer.Write(EarnSandDollarMax);
			writer.Write(FillOrderRangeMin);
			writer.Write(FillOrderRangeMax);
			writer.Write(MinProductionCount);
			writer.Write(MaxProductionCount);
			writer.Write(MaxProductionTime);
			writer.Write(MaxStorageCapactiy);
			writer.Write(PartyPointsRangeMin);
			writer.Write(PartyPointsRangeMax);
			writer.Write(PlayMiniGameRangeMin);
			writer.Write(PlayMiniGameRangeMax);
			writer.Write(MinGrindReward);
			writer.Write(MinPremiumReward);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteReward, RewardTableCompleteOrders);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteReward, RewardTablePlayMiniGame);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteReward, RewardTableEarnPartyPoints);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteReward, RewardTableEarnSandDollars);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteMiniGameScoreReward, RewardTableMiniGameScore);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteMiniGameScoreRange, RequirementTableMiniGameScore);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, DynamicComponents);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			ItemCategoryCount = reader.ReadInt32();
			EarnSandDollarMin = reader.ReadUInt32();
			EarnSandDollarMax = reader.ReadUInt32();
			FillOrderRangeMin = reader.ReadUInt32();
			FillOrderRangeMax = reader.ReadUInt32();
			MinProductionCount = reader.ReadUInt32();
			MaxProductionCount = reader.ReadUInt32();
			MaxProductionTime = reader.ReadUInt32();
			MaxStorageCapactiy = reader.ReadSingle();
			PartyPointsRangeMin = reader.ReadUInt32();
			PartyPointsRangeMax = reader.ReadUInt32();
			PlayMiniGameRangeMin = reader.ReadUInt32();
			PlayMiniGameRangeMax = reader.ReadUInt32();
			MinGrindReward = reader.ReadUInt32();
			MinPremiumReward = reader.ReadUInt32();
			RewardTableCompleteOrders = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadReward, RewardTableCompleteOrders);
			RewardTablePlayMiniGame = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadReward, RewardTablePlayMiniGame);
			RewardTableEarnPartyPoints = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadReward, RewardTableEarnPartyPoints);
			RewardTableEarnSandDollars = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadReward, RewardTableEarnSandDollars);
			RewardTableMiniGameScore = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadMiniGameScoreReward, RewardTableMiniGameScore);
			RequirementTableMiniGameScore = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadMiniGameScoreRange, RequirementTableMiniGameScore);
			DynamicComponents = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, DynamicComponents);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "ITEMCATEGORYCOUNT":
				reader.Read();
				ItemCategoryCount = global::System.Convert.ToInt32(reader.Value);
				break;
			case "EARNSANDDOLLARMIN":
				reader.Read();
				EarnSandDollarMin = global::System.Convert.ToUInt32(reader.Value);
				break;
			case "EARNSANDDOLLARMAX":
				reader.Read();
				EarnSandDollarMax = global::System.Convert.ToUInt32(reader.Value);
				break;
			case "FILLORDERRANGEMIN":
				reader.Read();
				FillOrderRangeMin = global::System.Convert.ToUInt32(reader.Value);
				break;
			case "FILLORDERRANGEMAX":
				reader.Read();
				FillOrderRangeMax = global::System.Convert.ToUInt32(reader.Value);
				break;
			case "MINPRODUCTIONCOUNT":
				reader.Read();
				MinProductionCount = global::System.Convert.ToUInt32(reader.Value);
				break;
			case "MAXPRODUCTIONCOUNT":
				reader.Read();
				MaxProductionCount = global::System.Convert.ToUInt32(reader.Value);
				break;
			case "MAXPRODUCTIONTIME":
				reader.Read();
				MaxProductionTime = global::System.Convert.ToUInt32(reader.Value);
				break;
			case "MAXSTORAGECAPACTIY":
				reader.Read();
				MaxStorageCapactiy = global::System.Convert.ToSingle(reader.Value);
				break;
			case "PARTYPOINTSRANGEMIN":
				reader.Read();
				PartyPointsRangeMin = global::System.Convert.ToUInt32(reader.Value);
				break;
			case "PARTYPOINTSRANGEMAX":
				reader.Read();
				PartyPointsRangeMax = global::System.Convert.ToUInt32(reader.Value);
				break;
			case "PLAYMINIGAMERANGEMIN":
				reader.Read();
				PlayMiniGameRangeMin = global::System.Convert.ToUInt32(reader.Value);
				break;
			case "PLAYMINIGAMERANGEMAX":
				reader.Read();
				PlayMiniGameRangeMax = global::System.Convert.ToUInt32(reader.Value);
				break;
			case "MINGRINDREWARD":
				reader.Read();
				MinGrindReward = global::System.Convert.ToUInt32(reader.Value);
				break;
			case "MINPREMIUMREWARD":
				reader.Read();
				MinPremiumReward = global::System.Convert.ToUInt32(reader.Value);
				break;
			case "REWARDTABLECOMPLETEORDERS":
				reader.Read();
				RewardTableCompleteOrders = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadReward, RewardTableCompleteOrders);
				break;
			case "REWARDTABLEPLAYMINIGAME":
				reader.Read();
				RewardTablePlayMiniGame = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadReward, RewardTablePlayMiniGame);
				break;
			case "REWARDTABLEEARNPARTYPOINTS":
				reader.Read();
				RewardTableEarnPartyPoints = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadReward, RewardTableEarnPartyPoints);
				break;
			case "REWARDTABLEEARNSANDDOLLARS":
				reader.Read();
				RewardTableEarnSandDollars = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadReward, RewardTableEarnSandDollars);
				break;
			case "REWARDTABLEMINIGAMESCORE":
				reader.Read();
				RewardTableMiniGameScore = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadMiniGameScoreReward, RewardTableMiniGameScore);
				break;
			case "REQUIREMENTTABLEMINIGAMESCORE":
				reader.Read();
				RequirementTableMiniGameScore = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadMiniGameScoreRange, RequirementTableMiniGameScore);
				break;
			case "DYNAMICCOMPONENTS":
				reader.Read();
				DynamicComponents = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, DynamicComponents);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}

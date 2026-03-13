namespace Kampai.Game
{
	[global::Kampai.Util.RequiresJsonConverter]
	public class QuestDefinition : global::Kampai.Game.Definition, global::Kampai.Util.IBuilder<global::Kampai.Game.Instance>
	{
		public override int TypeCode
		{
			get
			{
				return 1130;
			}
		}

		public int QuestLineID { get; set; }

		public virtual global::Kampai.Game.QuestType type { get; set; }

		public int NarrativeOrder { get; set; }

		public bool ProgressiveGoto { get; set; }

		public bool ShowRewardsPopupByDefault { get; set; }

		public global::Kampai.Game.QuestSurfaceType SurfaceType { get; set; }

		public int SurfaceID { get; set; }

		public int UnlockLevel { get; set; }

		public int UnlockQuestId { get; set; }

		public int QuestPriority { get; set; }

		public int QuestVersion { get; set; }

		public string QuestBookIcon { get; set; }

		public string QuestBookMask { get; set; }

		public int QuestCompletePlayerTrainingCategoryItemId { get; set; }

		public int QuestModalClosePlayerTrainingCategoryItemId { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.QuestStepDefinition> QuestSteps { get; set; }

		public int RewardTransaction { get; set; }

		public virtual int RewardDisplayCount { get; set; }

		public string WayFinderIcon { get; set; }

		public string QuestIntro { get; set; }

		public string QuestVoice { get; set; }

		public string QuestOutro { get; set; }

		public string QuestIntroMood { get; set; }

		public string QuestVoiceMood { get; set; }

		public string QuestOutroMood { get; set; }

		public bool ForceEnableRewardedAd2xReward { get; set; }

		public bool ForceDisableRewardedAd2xReward { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(QuestLineID);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, type);
			writer.Write(NarrativeOrder);
			writer.Write(ProgressiveGoto);
			writer.Write(ShowRewardsPopupByDefault);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, SurfaceType);
			writer.Write(SurfaceID);
			writer.Write(UnlockLevel);
			writer.Write(UnlockQuestId);
			writer.Write(QuestPriority);
			writer.Write(QuestVersion);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, QuestBookIcon);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, QuestBookMask);
			writer.Write(QuestCompletePlayerTrainingCategoryItemId);
			writer.Write(QuestModalClosePlayerTrainingCategoryItemId);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteQuestStepDefinition, QuestSteps);
			writer.Write(RewardTransaction);
			writer.Write(RewardDisplayCount);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, WayFinderIcon);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, QuestIntro);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, QuestVoice);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, QuestOutro);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, QuestIntroMood);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, QuestVoiceMood);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, QuestOutroMood);
			writer.Write(ForceEnableRewardedAd2xReward);
			writer.Write(ForceDisableRewardedAd2xReward);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			QuestLineID = reader.ReadInt32();
			type = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Game.QuestType>(reader);
			NarrativeOrder = reader.ReadInt32();
			ProgressiveGoto = reader.ReadBoolean();
			ShowRewardsPopupByDefault = reader.ReadBoolean();
			SurfaceType = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Game.QuestSurfaceType>(reader);
			SurfaceID = reader.ReadInt32();
			UnlockLevel = reader.ReadInt32();
			UnlockQuestId = reader.ReadInt32();
			QuestPriority = reader.ReadInt32();
			QuestVersion = reader.ReadInt32();
			QuestBookIcon = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			QuestBookMask = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			QuestCompletePlayerTrainingCategoryItemId = reader.ReadInt32();
			QuestModalClosePlayerTrainingCategoryItemId = reader.ReadInt32();
			QuestSteps = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadQuestStepDefinition, QuestSteps);
			RewardTransaction = reader.ReadInt32();
			RewardDisplayCount = reader.ReadInt32();
			WayFinderIcon = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			QuestIntro = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			QuestVoice = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			QuestOutro = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			QuestIntroMood = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			QuestVoiceMood = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			QuestOutroMood = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			ForceEnableRewardedAd2xReward = reader.ReadBoolean();
			ForceDisableRewardedAd2xReward = reader.ReadBoolean();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "QUESTLINEID":
				reader.Read();
				QuestLineID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "TYPE":
				reader.Read();
				type = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.QuestType>(reader);
				break;
			case "NARRATIVEORDER":
				reader.Read();
				NarrativeOrder = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PROGRESSIVEGOTO":
				reader.Read();
				ProgressiveGoto = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "SHOWREWARDSPOPUPBYDEFAULT":
				reader.Read();
				ShowRewardsPopupByDefault = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "SURFACETYPE":
				reader.Read();
				SurfaceType = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.QuestSurfaceType>(reader);
				break;
			case "SURFACEID":
				reader.Read();
				SurfaceID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "UNLOCKLEVEL":
				reader.Read();
				UnlockLevel = global::System.Convert.ToInt32(reader.Value);
				break;
			case "UNLOCKQUESTID":
				reader.Read();
				UnlockQuestId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "QUESTPRIORITY":
				reader.Read();
				QuestPriority = global::System.Convert.ToInt32(reader.Value);
				break;
			case "QUESTVERSION":
				reader.Read();
				QuestVersion = global::System.Convert.ToInt32(reader.Value);
				break;
			case "QUESTBOOKICON":
				reader.Read();
				QuestBookIcon = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "QUESTBOOKMASK":
				reader.Read();
				QuestBookMask = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "QUESTCOMPLETEPLAYERTRAININGCATEGORYITEMID":
				reader.Read();
				QuestCompletePlayerTrainingCategoryItemId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "QUESTMODALCLOSEPLAYERTRAININGCATEGORYITEMID":
				reader.Read();
				QuestModalClosePlayerTrainingCategoryItemId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "QUESTSTEPS":
				reader.Read();
				QuestSteps = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadQuestStepDefinition, QuestSteps);
				break;
			case "REWARDTRANSACTION":
				reader.Read();
				RewardTransaction = global::System.Convert.ToInt32(reader.Value);
				break;
			case "REWARDDISPLAYCOUNT":
				reader.Read();
				RewardDisplayCount = global::System.Convert.ToInt32(reader.Value);
				break;
			case "WAYFINDERICON":
				reader.Read();
				WayFinderIcon = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "QUESTINTRO":
				reader.Read();
				QuestIntro = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "QUESTVOICE":
				reader.Read();
				QuestVoice = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "QUESTOUTRO":
				reader.Read();
				QuestOutro = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "QUESTINTROMOOD":
				reader.Read();
				QuestIntroMood = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "QUESTVOICEMOOD":
				reader.Read();
				QuestVoiceMood = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "QUESTOUTROMOOD":
				reader.Read();
				QuestOutroMood = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "FORCEENABLEREWARDEDAD2XREWARD":
				reader.Read();
				ForceEnableRewardedAd2xReward = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "FORCEDISABLEREWARDEDAD2XREWARD":
				reader.Read();
				ForceDisableRewardedAd2xReward = global::System.Convert.ToBoolean(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public virtual global::Kampai.Game.Transaction.TransactionDefinition GetReward(global::Kampai.Game.IDefinitionService definitionService)
		{
			if (RewardTransaction == 0 || definitionService == null)
			{
				return null;
			}
			global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(RewardTransaction);
			if (RewardDisplayCount == 0)
			{
				RewardDisplayCount = global::Kampai.Game.Transaction.TransactionDataExtension.GetOutputCount(transactionDefinition);
			}
			return transactionDefinition;
		}

		public static string GetProceduralQuestIcon(global::Kampai.Game.QuestStepType type)
		{
			switch (type)
			{
			case global::Kampai.Game.QuestStepType.Delivery:
				return "tempCharQuestIcon";
			case global::Kampai.Game.QuestStepType.OrderBoard:
				return "tempCharQuestIcon";
			case global::Kampai.Game.QuestStepType.MinionTask:
				return "tempCharQuestIcon";
			case global::Kampai.Game.QuestStepType.Mignette:
				return "tempCharQuestIcon";
			default:
				return string.Empty;
			}
		}

		public static string GetProceduralQuestDescription(global::Kampai.Game.QuestStepType type)
		{
			switch (type)
			{
			case global::Kampai.Game.QuestStepType.Delivery:
				return "deliveryTaskName";
			case global::Kampai.Game.QuestStepType.OrderBoard:
				return "orderBoardTaskName";
			case global::Kampai.Game.QuestStepType.MinionTask:
				return "minionTaskName";
			case global::Kampai.Game.QuestStepType.Mignette:
				return "mignetteTaskName";
			default:
				return string.Empty;
			}
		}

		public virtual global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.Quest(this);
		}
	}
}

namespace Kampai.Game.Trigger
{
	public class MignetteScoreTriggerConditionDefinition : global::Kampai.Game.Trigger.TriggerConditionDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1160;
			}
		}

		public int score { get; set; }

		public int mignetteBuildingId { get; set; }

		public bool useTotalMignetteScore { get; set; }

		public override global::Kampai.Game.Trigger.TriggerConditionType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerConditionType.Identifier.MignetteScore;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(score);
			writer.Write(mignetteBuildingId);
			writer.Write(useTotalMignetteScore);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			score = reader.ReadInt32();
			mignetteBuildingId = reader.ReadInt32();
			useTotalMignetteScore = reader.ReadBoolean();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "SCORE":
				reader.Read();
				score = global::System.Convert.ToInt32(reader.Value);
				break;
			case "MIGNETTEBUILDINGID":
				reader.Read();
				mignetteBuildingId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "USETOTALMIGNETTESCORE":
				reader.Read();
				useTotalMignetteScore = global::System.Convert.ToBoolean(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public override string ToString()
		{
			return string.Format("{0}, Operator: {1}, Type: {2}, score: {3}, mignetteBuildingId: {4}", GetType(), base.conditionOp, type, score, mignetteBuildingId);
		}

		public override bool IsTriggered(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			int actualValue = 0;
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = gameContext.injectionBinder;
			if (useTotalMignetteScore)
			{
				global::Kampai.Game.IPlayerService instance = injectionBinder.GetInstance<global::Kampai.Game.IPlayerService>();
				global::Kampai.Game.MignetteBuilding byInstanceId = instance.GetByInstanceId<global::Kampai.Game.MignetteBuilding>(mignetteBuildingId);
				if (byInstanceId != null)
				{
					actualValue = byInstanceId.TotalScore;
				}
				else
				{
					global::Kampai.Util.IKampaiLogger kampaiLogger = global::Elevation.Logging.LogManager.GetClassLogger("MignetteScoreTriggerConditionDefinition") as global::Kampai.Util.IKampaiLogger;
					if (kampaiLogger != null)
					{
						kampaiLogger.Error("Mignette {0} is not found, 0 score will be used for the triigger", mignetteBuildingId);
					}
				}
			}
			else
			{
				global::Kampai.Game.MignetteCollectionService instance2 = injectionBinder.GetInstance<global::Kampai.Game.MignetteCollectionService>();
				global::Kampai.Game.RewardCollection activeCollectionForMignette = instance2.GetActiveCollectionForMignette(mignetteBuildingId);
				actualValue = activeCollectionForMignette.CollectionScoreProgress;
			}
			return TestOperator(score, actualValue);
		}
	}
}

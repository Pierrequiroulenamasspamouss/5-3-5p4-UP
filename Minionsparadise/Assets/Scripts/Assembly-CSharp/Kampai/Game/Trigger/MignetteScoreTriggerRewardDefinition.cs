namespace Kampai.Game.Trigger
{
	public class MignetteScoreTriggerRewardDefinition : global::Kampai.Game.Trigger.TriggerRewardDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1179;
			}
		}

		public int MignetteBuildingId { get; set; }

		public int Points { get; set; }

		public override global::Kampai.Game.Trigger.TriggerRewardType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerRewardType.Identifier.MignetteScore;
			}
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(MignetteBuildingId);
			writer.Write(Points);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			MignetteBuildingId = reader.ReadInt32();
			Points = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "POINTS":
				reader.Read();
				Points = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			case "MIGNETTEBUILDINGID":
				reader.Read();
				MignetteBuildingId = global::System.Convert.ToInt32(reader.Value);
				break;
			}
			return true;
		}

		public override void RewardPlayer(global::strange.extensions.context.api.ICrossContextCapable context)
		{
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = context.injectionBinder;
			global::Kampai.Game.MignetteBuilding firstInstanceByDefinitionId = injectionBinder.GetInstance<global::Kampai.Game.IPlayerService>().GetFirstInstanceByDefinitionId<global::Kampai.Game.MignetteBuilding>(MignetteBuildingId);
			if (firstInstanceByDefinitionId != null)
			{
				global::Kampai.Game.Mignette.MignetteGameModel instance = injectionBinder.GetInstance<global::Kampai.Game.Mignette.MignetteGameModel>();
				instance.BuildingId = firstInstanceByDefinitionId.ID;
				instance.CurrentGameScore = Points;
				injectionBinder.GetInstance<global::Kampai.Game.ShowAndIncreaseMignetteScoreSignal>().Dispatch();
			}
			else
			{
				global::Kampai.Util.IKampaiLogger kampaiLogger = global::Elevation.Logging.LogManager.GetClassLogger("MignetteScoreTriggerRewardDefinition") as global::Kampai.Util.IKampaiLogger;
				kampaiLogger.Error("Cannot find mignette building {0}", MignetteBuildingId);
			}
		}
	}
}

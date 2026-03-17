namespace Kampai.Game.Trigger
{
	public class PlatformTriggerConditionDefinition : global::Kampai.Game.Trigger.TriggerConditionDefinition
	{
		private global::Kampai.Util.Boxed<global::UnityEngine.RuntimePlatform> testValue;

		public override int TypeCode
		{
			get
			{
				return 1162;
			}
		}

		public string Platform { get; set; }

		public override global::Kampai.Game.Trigger.TriggerConditionType.Identifier type
		{
			get
			{
				return global::Kampai.Game.Trigger.TriggerConditionType.Identifier.Platform;
			}
		}

		public PlatformTriggerConditionDefinition()
		{
		}

		public PlatformTriggerConditionDefinition(global::UnityEngine.RuntimePlatform testValue)
		{
			this.testValue = new global::Kampai.Util.Boxed<global::UnityEngine.RuntimePlatform>(testValue);
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Platform);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Platform = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "PLATFORM":
				reader.Read();
				Platform = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override string ToString()
		{
			return string.Format("{0}, Operator: {1}, Type: {2}, Platform: {3}", GetType(), base.conditionOp, type, Platform);
		}

		public override bool IsTriggered(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			global::Kampai.Util.IKampaiLogger kampaiLogger = global::Elevation.Logging.LogManager.GetClassLogger("PlatformTriggerConditionDefinition") as global::Kampai.Util.IKampaiLogger;
			if (string.IsNullOrEmpty(Platform))
			{
				kampaiLogger.Error("No platform.");
				return false;
			}
			global::UnityEngine.RuntimePlatform runtimePlatform = ((testValue != null) ? testValue.Value : global::UnityEngine.Application.platform);
			string text = global::Kampai.Util.StringUtil.UnifiedPlatformName(runtimePlatform);
			if (text == null)
			{
				kampaiLogger.Error("Unknown platform {0}", runtimePlatform);
				return false;
			}
			return TestOperator(Platform.ToLower(), text.ToLower());
		}
	}
}

namespace Kampai.Game
{
	public class MasterPlanOnboardDefinition : global::Kampai.Game.Definition
	{
		public override int TypeCode
		{
			get
			{
				return 1109;
			}
		}

		public int nextOnboardDefinitionId { get; set; }

		public int CustomCameraPosID { get; set; }

		public global::Kampai.Game.GhostFunctionDefinition ghostFunction { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(nextOnboardDefinitionId);
			writer.Write(CustomCameraPosID);
			global::Kampai.Util.BinarySerializationUtil.WriteGhostFunctionDefinition(writer, ghostFunction);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			nextOnboardDefinitionId = reader.ReadInt32();
			CustomCameraPosID = reader.ReadInt32();
			ghostFunction = global::Kampai.Util.BinarySerializationUtil.ReadGhostFunctionDefinition(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "NEXTONBOARDDEFINITIONID":
				reader.Read();
				nextOnboardDefinitionId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "CUSTOMCAMERAPOSID":
				reader.Read();
				CustomCameraPosID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "GHOSTFUNCTION":
				reader.Read();
				ghostFunction = global::Kampai.Util.ReaderUtil.ReadGhostFunctionDefinition(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}
	}
}

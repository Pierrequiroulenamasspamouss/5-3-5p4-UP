namespace Kampai.Game
{
	public class StageBuildingDefinition : global::Kampai.Game.AnimatingBuildingDefinition, global::Kampai.Game.ZoomableBuildingDefinition
	{
		private global::Kampai.Game.Area normalizedArea;

		public override int TypeCode
		{
			get
			{
				return 1062;
			}
		}

		public global::UnityEngine.Vector3 zoomOffset { get; set; }

		public global::UnityEngine.Vector3 zoomEulers { get; set; }

		public float zoomFOV { get; set; }

		public string backdropPrefabName { get; set; }

		public int temporaryMinionNum { get; set; }

		public int temporaryMinionAnimationCount { get; set; }

		public float temporaryMinionsOffset { get; set; }

		public string temporaryMinionASM { get; set; }

		public string AspirationalMessage { get; set; }

		public int SocialEventMinimumLevel { get; set; }

		public global::Kampai.Game.Area concertArea { get; set; }

		public float maxMinionOffsetX { get; set; }

		public float maxMinionOffsetY { get; set; }

		public float posSkipPercent { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteVector3(writer, zoomOffset);
			global::Kampai.Util.BinarySerializationUtil.WriteVector3(writer, zoomEulers);
			writer.Write(zoomFOV);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, backdropPrefabName);
			writer.Write(temporaryMinionNum);
			writer.Write(temporaryMinionAnimationCount);
			writer.Write(temporaryMinionsOffset);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, temporaryMinionASM);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, AspirationalMessage);
			writer.Write(SocialEventMinimumLevel);
			global::Kampai.Util.BinarySerializationUtil.WriteArea(writer, concertArea);
			writer.Write(maxMinionOffsetX);
			writer.Write(maxMinionOffsetY);
			writer.Write(posSkipPercent);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			zoomOffset = global::Kampai.Util.BinarySerializationUtil.ReadVector3(reader);
			zoomEulers = global::Kampai.Util.BinarySerializationUtil.ReadVector3(reader);
			zoomFOV = reader.ReadSingle();
			backdropPrefabName = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			temporaryMinionNum = reader.ReadInt32();
			temporaryMinionAnimationCount = reader.ReadInt32();
			temporaryMinionsOffset = reader.ReadSingle();
			temporaryMinionASM = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			AspirationalMessage = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			SocialEventMinimumLevel = reader.ReadInt32();
			concertArea = global::Kampai.Util.BinarySerializationUtil.ReadArea(reader);
			maxMinionOffsetX = reader.ReadSingle();
			maxMinionOffsetY = reader.ReadSingle();
			posSkipPercent = reader.ReadSingle();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "ZOOMOFFSET":
				reader.Read();
				zoomOffset = global::Kampai.Util.ReaderUtil.ReadVector3(reader, converters);
				break;
			case "ZOOMEULERS":
				reader.Read();
				zoomEulers = global::Kampai.Util.ReaderUtil.ReadVector3(reader, converters);
				break;
			case "ZOOMFOV":
				reader.Read();
				zoomFOV = global::System.Convert.ToSingle(reader.Value);
				break;
			case "BACKDROPPREFABNAME":
				reader.Read();
				backdropPrefabName = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "TEMPORARYMINIONNUM":
				reader.Read();
				temporaryMinionNum = global::System.Convert.ToInt32(reader.Value);
				break;
			case "TEMPORARYMINIONANIMATIONCOUNT":
				reader.Read();
				temporaryMinionAnimationCount = global::System.Convert.ToInt32(reader.Value);
				break;
			case "TEMPORARYMINIONSOFFSET":
				reader.Read();
				temporaryMinionsOffset = global::System.Convert.ToSingle(reader.Value);
				break;
			case "TEMPORARYMINIONASM":
				reader.Read();
				temporaryMinionASM = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "ASPIRATIONALMESSAGE":
				reader.Read();
				AspirationalMessage = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "SOCIALEVENTMINIMUMLEVEL":
				reader.Read();
				SocialEventMinimumLevel = global::System.Convert.ToInt32(reader.Value);
				break;
			case "CONCERTAREA":
				reader.Read();
				concertArea = global::Kampai.Util.ReaderUtil.ReadArea(reader, converters);
				break;
			case "MAXMINIONOFFSETX":
				reader.Read();
				maxMinionOffsetX = global::System.Convert.ToSingle(reader.Value);
				break;
			case "MAXMINIONOFFSETY":
				reader.Read();
				maxMinionOffsetY = global::System.Convert.ToSingle(reader.Value);
				break;
			case "POSSKIPPERCENT":
				reader.Read();
				posSkipPercent = global::System.Convert.ToSingle(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public override global::Kampai.Game.Building BuildBuilding()
		{
			return new global::Kampai.Game.StageBuilding(this);
		}

		private bool AssertNormalized()
		{
			if (normalizedArea == null)
			{
				if (concertArea == null)
				{
					return false;
				}
				global::Kampai.Game.Location a = concertArea.a;
				global::Kampai.Game.Location b = concertArea.b;
				int x = global::System.Math.Min(a.x, b.x);
				int y = global::System.Math.Min(a.y, b.y);
				int x2 = global::System.Math.Max(a.x, b.x);
				int y2 = global::System.Math.Max(a.y, b.y);
				normalizedArea = new global::Kampai.Game.Area(x, y, x2, y2);
			}
			return true;
		}

		public bool Contains(global::Kampai.Util.Point point)
		{
			if (!AssertNormalized())
			{
				return false;
			}
			global::Kampai.Game.Location a = normalizedArea.a;
			global::Kampai.Game.Location b = normalizedArea.b;
			return point.x >= a.x && point.y >= a.y && point.x <= b.x && point.y <= b.y;
		}
	}
}

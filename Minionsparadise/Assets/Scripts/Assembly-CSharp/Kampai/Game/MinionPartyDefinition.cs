namespace Kampai.Game
{
	public class MinionPartyDefinition : global::Kampai.Game.Definition, global::Kampai.Util.IBuilder<global::Kampai.Game.Instance>
	{
		private global::Kampai.Game.Area normalized;

		public override int TypeCode
		{
			get
			{
				return 1116;
			}
		}

		public int UnlockQuestID { get; set; }

		public global::Kampai.Game.PartyMeterDefinition partyMeterDefinition { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.MinionPartyLevelBandDefinition> LevelBands { get; set; }

		public global::Kampai.Game.CameraControlSettings cameraControlSettings { get; set; }

		public global::Kampai.Game.Area PartyArea { get; set; }

		public global::Kampai.Game.Location Center { get; set; }

		public float PartyRadius { get; set; }

		public float partyAnimationRestMin { get; set; }

		public float partyAnimationRestMax { get; set; }

		public int PartyAnimations { get; set; }

		public int PreGOHPartyDuration { get; set; }

		public int PartyDuration { get; set; }

		public float Percent { get; set; }

		public int PartyAssetDelay { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.VFXAssetDefinition> startPartyVFX { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.VFXAssetDefinition> endPartyVFX { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.VFXAssetDefinition> partyVFXDefintions { get; set; }

		public int MinionsPlayingAudioCount { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(UnlockQuestID);
			global::Kampai.Util.BinarySerializationUtil.WriteObject(writer, partyMeterDefinition);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, LevelBands);
			global::Kampai.Util.BinarySerializationUtil.WriteCameraControlSettings(writer, cameraControlSettings);
			global::Kampai.Util.BinarySerializationUtil.WriteArea(writer, PartyArea);
			global::Kampai.Util.BinarySerializationUtil.WriteLocation(writer, Center);
			writer.Write(PartyRadius);
			writer.Write(partyAnimationRestMin);
			writer.Write(partyAnimationRestMax);
			writer.Write(PartyAnimations);
			writer.Write(PreGOHPartyDuration);
			writer.Write(PartyDuration);
			writer.Write(Percent);
			writer.Write(PartyAssetDelay);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteVFXAssetDefinition, startPartyVFX);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteVFXAssetDefinition, endPartyVFX);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteVFXAssetDefinition, partyVFXDefintions);
			writer.Write(MinionsPlayingAudioCount);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			UnlockQuestID = reader.ReadInt32();
			partyMeterDefinition = global::Kampai.Util.BinarySerializationUtil.ReadObject<global::Kampai.Game.PartyMeterDefinition>(reader);
			LevelBands = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, LevelBands);
			cameraControlSettings = global::Kampai.Util.BinarySerializationUtil.ReadCameraControlSettings(reader);
			PartyArea = global::Kampai.Util.BinarySerializationUtil.ReadArea(reader);
			Center = global::Kampai.Util.BinarySerializationUtil.ReadLocation(reader);
			PartyRadius = reader.ReadSingle();
			partyAnimationRestMin = reader.ReadSingle();
			partyAnimationRestMax = reader.ReadSingle();
			PartyAnimations = reader.ReadInt32();
			PreGOHPartyDuration = reader.ReadInt32();
			PartyDuration = reader.ReadInt32();
			Percent = reader.ReadSingle();
			PartyAssetDelay = reader.ReadInt32();
			startPartyVFX = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadVFXAssetDefinition, startPartyVFX);
			endPartyVFX = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadVFXAssetDefinition, endPartyVFX);
			partyVFXDefintions = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadVFXAssetDefinition, partyVFXDefintions);
			MinionsPlayingAudioCount = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "UNLOCKQUESTID":
				reader.Read();
				UnlockQuestID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PARTYMETERDEFINITION":
				reader.Read();
				partyMeterDefinition = global::Kampai.Util.FastJSONDeserializer.Deserialize<global::Kampai.Game.PartyMeterDefinition>(reader, converters);
				break;
			case "LEVELBANDS":
				reader.Read();
				LevelBands = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, LevelBands);
				break;
			case "CAMERACONTROLSETTINGS":
				reader.Read();
				cameraControlSettings = global::Kampai.Util.ReaderUtil.ReadCameraControlSettings(reader, converters);
				break;
			case "PARTYAREA":
				reader.Read();
				PartyArea = global::Kampai.Util.ReaderUtil.ReadArea(reader, converters);
				break;
			case "CENTER":
				reader.Read();
				Center = global::Kampai.Util.ReaderUtil.ReadLocation(reader, converters);
				break;
			case "PARTYRADIUS":
				reader.Read();
				PartyRadius = global::System.Convert.ToSingle(reader.Value);
				break;
			case "PARTYANIMATIONRESTMIN":
				reader.Read();
				partyAnimationRestMin = global::System.Convert.ToSingle(reader.Value);
				break;
			case "PARTYANIMATIONRESTMAX":
				reader.Read();
				partyAnimationRestMax = global::System.Convert.ToSingle(reader.Value);
				break;
			case "PARTYANIMATIONS":
				reader.Read();
				PartyAnimations = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PREGOHPARTYDURATION":
				reader.Read();
				PreGOHPartyDuration = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PARTYDURATION":
				reader.Read();
				PartyDuration = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PERCENT":
				reader.Read();
				Percent = global::System.Convert.ToSingle(reader.Value);
				break;
			case "PARTYASSETDELAY":
				reader.Read();
				PartyAssetDelay = global::System.Convert.ToInt32(reader.Value);
				break;
			case "STARTPARTYVFX":
				reader.Read();
				startPartyVFX = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadVFXAssetDefinition, startPartyVFX);
				break;
			case "ENDPARTYVFX":
				reader.Read();
				endPartyVFX = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadVFXAssetDefinition, endPartyVFX);
				break;
			case "PARTYVFXDEFINTIONS":
				reader.Read();
				partyVFXDefintions = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadVFXAssetDefinition, partyVFXDefintions);
				break;
			case "MINIONSPLAYINGAUDIOCOUNT":
				reader.Read();
				MinionsPlayingAudioCount = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		private void AssertNormalized()
		{
			if (normalized == null)
			{
				global::Kampai.Game.Location a = PartyArea.a;
				global::Kampai.Game.Location b = PartyArea.b;
				int x = global::System.Math.Min(a.x, b.x);
				int y = global::System.Math.Min(a.y, b.y);
				int x2 = global::System.Math.Max(a.x, b.x);
				int y2 = global::System.Math.Max(a.y, b.y);
				normalized = new global::Kampai.Game.Area(x, y, x2, y2);
			}
		}

		public bool Contains(global::Kampai.Util.Point point)
		{
			AssertNormalized();
			global::Kampai.Game.Location a = normalized.a;
			global::Kampai.Game.Location b = normalized.b;
			return point.x >= a.x && point.y >= a.y && point.x <= b.x && point.y <= b.y;
		}

		public int GetPartyDuration(bool partyShouldProduceBuff)
		{
			if (!partyShouldProduceBuff)
			{
				return PreGOHPartyDuration;
			}
			return PartyDuration;
		}

		public global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.MinionParty(this);
		}
	}
}

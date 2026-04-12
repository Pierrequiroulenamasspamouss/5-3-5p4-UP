namespace Kampai.Game
{
	public class MignetteBuildingDefinition : global::Kampai.Game.TaskableBuildingDefinition
	{
		public string CollectableImage;

		public string CollectableImageMask;

		public global::System.Collections.Generic.IList<MignetteRuleDefinition> MignetteRules;

		public global::System.Collections.Generic.IList<global::Kampai.Game.MignetteChildObjectDefinition> ChildObjects;

		public global::System.Collections.Generic.IList<global::Kampai.Game.MignetteChildObjectDefinition> CooldownObjects;

		public override int TypeCode
		{
			get
			{
				return 1056;
			}
		}

		public bool ShowPlayConfirmMenu { get; set; }

		public bool ShowMignetteHUD { get; set; }

		public string ContextRootName { get; set; }

		public int CooldownInSeconds { get; set; }

		public int LevelUnlocked { get; set; }

		public float XPRewardFactor { get; set; }

		public global::System.Collections.Generic.IList<int> MainCollectionDefinitionIDs { get; set; }

		public global::System.Collections.Generic.IList<int> RepeatableCollectionDefinitionIDs { get; set; }

		public string AspirationalMessage { get; set; }

		public int LandExpansionID { get; set; }

		public MignetteBuildingDefinition()
		{
			ShowPlayConfirmMenu = false;
			ShowMignetteHUD = true;
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			writer.Write(ShowPlayConfirmMenu);
			writer.Write(ShowMignetteHUD);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, ContextRootName);
			writer.Write(CooldownInSeconds);
			writer.Write(LevelUnlocked);
			writer.Write(XPRewardFactor);
			global::Kampai.Util.BinarySerializationUtil.WriteListInt32(writer, MainCollectionDefinitionIDs);
			global::Kampai.Util.BinarySerializationUtil.WriteListInt32(writer, RepeatableCollectionDefinitionIDs);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, AspirationalMessage);
			writer.Write(LandExpansionID);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, CollectableImage);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, CollectableImageMask);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteMignetteRuleDefinition, MignetteRules);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteMignetteChildObjectDefinition, ChildObjects);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteMignetteChildObjectDefinition, CooldownObjects);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			ShowPlayConfirmMenu = reader.ReadBoolean();
			ShowMignetteHUD = reader.ReadBoolean();
			ContextRootName = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			CooldownInSeconds = reader.ReadInt32();
			LevelUnlocked = reader.ReadInt32();
			XPRewardFactor = reader.ReadSingle();
			MainCollectionDefinitionIDs = global::Kampai.Util.BinarySerializationUtil.ReadListInt32(reader, MainCollectionDefinitionIDs);
			RepeatableCollectionDefinitionIDs = global::Kampai.Util.BinarySerializationUtil.ReadListInt32(reader, RepeatableCollectionDefinitionIDs);
			AspirationalMessage = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			LandExpansionID = reader.ReadInt32();
			CollectableImage = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			CollectableImageMask = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			MignetteRules = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadMignetteRuleDefinition, MignetteRules);
			ChildObjects = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadMignetteChildObjectDefinition, ChildObjects);
			CooldownObjects = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadMignetteChildObjectDefinition, CooldownObjects);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "SHOWPLAYCONFIRMMENU":
				reader.Read();
				ShowPlayConfirmMenu = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "SHOWMIGNETTEHUD":
				reader.Read();
				ShowMignetteHUD = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "CONTEXTROOTNAME":
				reader.Read();
				ContextRootName = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "COOLDOWNINSECONDS":
				reader.Read();
				CooldownInSeconds = global::System.Convert.ToInt32(reader.Value);
				break;
			case "LEVELUNLOCKED":
				reader.Read();
				LevelUnlocked = global::System.Convert.ToInt32(reader.Value);
				break;
			case "XPREWARDFACTOR":
				reader.Read();
				XPRewardFactor = global::System.Convert.ToSingle(reader.Value);
				break;
			case "MAINCOLLECTIONDEFINITIONIDS":
				reader.Read();
				MainCollectionDefinitionIDs = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, MainCollectionDefinitionIDs);
				break;
			case "REPEATABLECOLLECTIONDEFINITIONIDS":
				reader.Read();
				RepeatableCollectionDefinitionIDs = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, RepeatableCollectionDefinitionIDs);
				break;
			case "ASPIRATIONALMESSAGE":
				reader.Read();
				AspirationalMessage = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "LANDEXPANSIONID":
				reader.Read();
				LandExpansionID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "COLLECTABLEIMAGE":
				reader.Read();
				CollectableImage = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "COLLECTABLEIMAGEMASK":
				reader.Read();
				CollectableImageMask = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "MIGNETTERULES":
				reader.Read();
				MignetteRules = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadMignetteRuleDefinition, MignetteRules);
				break;
			case "CHILDOBJECTS":
				reader.Read();
				ChildObjects = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadMignetteChildObjectDefinition, ChildObjects);
				break;
			case "COOLDOWNOBJECTS":
				reader.Read();
				CooldownObjects = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadMignetteChildObjectDefinition, CooldownObjects);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public override global::Kampai.Game.Building BuildBuilding()
		{
			return new global::Kampai.Game.MignetteBuilding(this);
		}
	}
}

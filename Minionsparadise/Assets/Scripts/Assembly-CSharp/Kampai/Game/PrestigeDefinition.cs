namespace Kampai.Game
{
	public class PrestigeDefinition : global::Kampai.Game.TaxonomyDefinition, global::Kampai.Util.IBuilder<global::Kampai.Game.Instance>
	{
		public override int TypeCode
		{
			get
			{
				return 1013;
			}
		}

		public global::Kampai.Game.PrestigeType Type { get; set; }

		public uint PreUnlockLevel { get; set; }

		public uint MaxedBadgedOrder { get; set; }

		public uint OrderBoardWeight { get; set; }

		public string CollectionTitle { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.CharacterPrestigeLevelDefinition> PrestigeLevelSettings { get; set; }

		public string UniqueTikiBarstoolASMPatron1 { get; set; }

		public string UniqueTikiBarstoolASMPatron2 { get; set; }

		public string UniqueArrivalStateMachine { get; set; }

		public int SmallAvatarResouceId { get; set; }

		public int WayFinderIconResourceId { get; set; }

		public int BigAvatarResourceId { get; set; }

		public int TrackedDefinitionID { get; set; }

		public int GuestOfHonorDefinitionID { get; set; }

		public int PlayerTrainingNonPrestigeDefinitionId { get; set; }

		public int PlayerTrainingPrestigeDefinitionId { get; set; }

		public int PlayerTrainingReprestigeDefinitionId { get; set; }

		public global::Kampai.Game.StickerbookCharacterDisplayableType StickerbookDisplayableType { get; set; }

		public global::Kampai.Game.GOHDisplayableType GuestOfHonorDisplayableType { get; set; }

		public int CostumeDefinitionID { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, Type);
			writer.Write(PreUnlockLevel);
			writer.Write(MaxedBadgedOrder);
			writer.Write(OrderBoardWeight);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, CollectionTitle);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteCharacterPrestigeLevelDefinition, PrestigeLevelSettings);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, UniqueTikiBarstoolASMPatron1);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, UniqueTikiBarstoolASMPatron2);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, UniqueArrivalStateMachine);
			writer.Write(SmallAvatarResouceId);
			writer.Write(WayFinderIconResourceId);
			writer.Write(BigAvatarResourceId);
			writer.Write(TrackedDefinitionID);
			writer.Write(GuestOfHonorDefinitionID);
			writer.Write(PlayerTrainingNonPrestigeDefinitionId);
			writer.Write(PlayerTrainingPrestigeDefinitionId);
			writer.Write(PlayerTrainingReprestigeDefinitionId);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, StickerbookDisplayableType);
			global::Kampai.Util.BinarySerializationUtil.WriteEnum(writer, GuestOfHonorDisplayableType);
			writer.Write(CostumeDefinitionID);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Type = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Game.PrestigeType>(reader);
			PreUnlockLevel = reader.ReadUInt32();
			MaxedBadgedOrder = reader.ReadUInt32();
			OrderBoardWeight = reader.ReadUInt32();
			CollectionTitle = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			PrestigeLevelSettings = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadCharacterPrestigeLevelDefinition, PrestigeLevelSettings);
			UniqueTikiBarstoolASMPatron1 = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			UniqueTikiBarstoolASMPatron2 = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			UniqueArrivalStateMachine = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			SmallAvatarResouceId = reader.ReadInt32();
			WayFinderIconResourceId = reader.ReadInt32();
			BigAvatarResourceId = reader.ReadInt32();
			TrackedDefinitionID = reader.ReadInt32();
			GuestOfHonorDefinitionID = reader.ReadInt32();
			PlayerTrainingNonPrestigeDefinitionId = reader.ReadInt32();
			PlayerTrainingPrestigeDefinitionId = reader.ReadInt32();
			PlayerTrainingReprestigeDefinitionId = reader.ReadInt32();
			StickerbookDisplayableType = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Game.StickerbookCharacterDisplayableType>(reader);
			GuestOfHonorDisplayableType = global::Kampai.Util.BinarySerializationUtil.ReadEnum<global::Kampai.Game.GOHDisplayableType>(reader);
			CostumeDefinitionID = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "TYPE":
				reader.Read();
				Type = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.PrestigeType>(reader);
				break;
			case "PREUNLOCKLEVEL":
				reader.Read();
				PreUnlockLevel = global::System.Convert.ToUInt32(reader.Value);
				break;
			case "MAXEDBADGEDORDER":
				reader.Read();
				MaxedBadgedOrder = global::System.Convert.ToUInt32(reader.Value);
				break;
			case "ORDERBOARDWEIGHT":
				reader.Read();
				OrderBoardWeight = global::System.Convert.ToUInt32(reader.Value);
				break;
			case "COLLECTIONTITLE":
				reader.Read();
				CollectionTitle = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "PRESTIGELEVELSETTINGS":
				reader.Read();
				PrestigeLevelSettings = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadCharacterPrestigeLevelDefinition, PrestigeLevelSettings);
				break;
			case "UNIQUETIKIBARSTOOLASMPATRON1":
				reader.Read();
				UniqueTikiBarstoolASMPatron1 = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "UNIQUETIKIBARSTOOLASMPATRON2":
				reader.Read();
				UniqueTikiBarstoolASMPatron2 = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "UNIQUEARRIVALSTATEMACHINE":
				reader.Read();
				UniqueArrivalStateMachine = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "SMALLAVATARRESOUCEID":
				reader.Read();
				SmallAvatarResouceId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "WAYFINDERICONRESOURCEID":
				reader.Read();
				WayFinderIconResourceId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "BIGAVATARRESOURCEID":
				reader.Read();
				BigAvatarResourceId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "TRACKEDDEFINITIONID":
				reader.Read();
				TrackedDefinitionID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "GUESTOFHONORDEFINITIONID":
				reader.Read();
				GuestOfHonorDefinitionID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PLAYERTRAININGNONPRESTIGEDEFINITIONID":
				reader.Read();
				PlayerTrainingNonPrestigeDefinitionId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PLAYERTRAININGPRESTIGEDEFINITIONID":
				reader.Read();
				PlayerTrainingPrestigeDefinitionId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "PLAYERTRAININGREPRESTIGEDEFINITIONID":
				reader.Read();
				PlayerTrainingReprestigeDefinitionId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "STICKERBOOKDISPLAYABLETYPE":
				reader.Read();
				StickerbookDisplayableType = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.StickerbookCharacterDisplayableType>(reader);
				break;
			case "GUESTOFHONORDISPLAYABLETYPE":
				reader.Read();
				GuestOfHonorDisplayableType = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.GOHDisplayableType>(reader);
				break;
			case "COSTUMEDEFINITIONID":
				reader.Read();
				CostumeDefinitionID = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.Prestige(this);
		}
	}
}

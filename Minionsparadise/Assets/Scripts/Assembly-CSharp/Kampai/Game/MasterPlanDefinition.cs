namespace Kampai.Game
{
	public class MasterPlanDefinition : global::Kampai.Game.Definition, global::Kampai.Util.IBuilder<global::Kampai.Game.Instance>
	{
		public override int TypeCode
		{
			get
			{
				return 1108;
			}
		}

		public string DescriptionKey { get; set; }

		public string Image { get; set; }

		public string Mask { get; set; }

		public string IntroDialog { get; set; }

		public int RewardTransactionID { get; set; }

		public int CooldownRewardTransactionID { get; set; }

		public int SubsequentCooldownRewardTransactionID { get; set; }

		public global::System.Collections.Generic.List<int> ComponentDefinitionIDs { get; set; }

		public global::System.Collections.Generic.List<int> CompBuildingDefinitionIDs { get; set; }

		public int BuildingDefID { get; set; }

		public int LeavebehindBuildingDefID { get; set; }

		public int BuildingCustomCameraPosID { get; set; }

		public int VillainCharacterDefID { get; set; }

		public int CooldownDuration { get; set; }

		public string CooldownRewardDialogKey { get; set; }

		public string RewardStateMachine { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, DescriptionKey);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Image);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Mask);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, IntroDialog);
			writer.Write(RewardTransactionID);
			writer.Write(CooldownRewardTransactionID);
			writer.Write(SubsequentCooldownRewardTransactionID);
			global::Kampai.Util.BinarySerializationUtil.WriteListInt32(writer, ComponentDefinitionIDs);
			global::Kampai.Util.BinarySerializationUtil.WriteListInt32(writer, CompBuildingDefinitionIDs);
			writer.Write(BuildingDefID);
			writer.Write(LeavebehindBuildingDefID);
			writer.Write(BuildingCustomCameraPosID);
			writer.Write(VillainCharacterDefID);
			writer.Write(CooldownDuration);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, CooldownRewardDialogKey);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, RewardStateMachine);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			DescriptionKey = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Image = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Mask = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			IntroDialog = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			RewardTransactionID = reader.ReadInt32();
			CooldownRewardTransactionID = reader.ReadInt32();
			SubsequentCooldownRewardTransactionID = reader.ReadInt32();
			ComponentDefinitionIDs = global::Kampai.Util.BinarySerializationUtil.ReadListInt32(reader, ComponentDefinitionIDs);
			CompBuildingDefinitionIDs = global::Kampai.Util.BinarySerializationUtil.ReadListInt32(reader, CompBuildingDefinitionIDs);
			BuildingDefID = reader.ReadInt32();
			LeavebehindBuildingDefID = reader.ReadInt32();
			BuildingCustomCameraPosID = reader.ReadInt32();
			VillainCharacterDefID = reader.ReadInt32();
			CooldownDuration = reader.ReadInt32();
			CooldownRewardDialogKey = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			RewardStateMachine = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "DESCRIPTIONKEY":
				reader.Read();
				DescriptionKey = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "IMAGE":
				reader.Read();
				Image = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "MASK":
				reader.Read();
				Mask = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "INTRODIALOG":
				reader.Read();
				IntroDialog = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "REWARDTRANSACTIONID":
				reader.Read();
				RewardTransactionID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "COOLDOWNREWARDTRANSACTIONID":
				reader.Read();
				CooldownRewardTransactionID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "SUBSEQUENTCOOLDOWNREWARDTRANSACTIONID":
				reader.Read();
				SubsequentCooldownRewardTransactionID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "COMPONENTDEFINITIONIDS":
				reader.Read();
				ComponentDefinitionIDs = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, ComponentDefinitionIDs);
				break;
			case "COMPBUILDINGDEFINITIONIDS":
				reader.Read();
				CompBuildingDefinitionIDs = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, CompBuildingDefinitionIDs);
				break;
			case "BUILDINGDEFID":
				reader.Read();
				BuildingDefID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "LEAVEBEHINDBUILDINGDEFID":
				reader.Read();
				LeavebehindBuildingDefID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "BUILDINGCUSTOMCAMERAPOSID":
				reader.Read();
				BuildingCustomCameraPosID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "VILLAINCHARACTERDEFID":
				reader.Read();
				VillainCharacterDefID = global::System.Convert.ToInt32(reader.Value);
				break;
			case "COOLDOWNDURATION":
				reader.Read();
				CooldownDuration = global::System.Convert.ToInt32(reader.Value);
				break;
			case "COOLDOWNREWARDDIALOGKEY":
				reader.Read();
				CooldownRewardDialogKey = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "REWARDSTATEMACHINE":
				reader.Read();
				RewardStateMachine = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.MasterPlan(this);
		}
	}
}

namespace Kampai.Game
{
	public class MasterPlanComponentDefinition : global::Kampai.Game.TaxonomyDefinition, global::Kampai.Util.IBuilder<global::Kampai.Game.Instance>
	{
		public global::System.Collections.Generic.List<global::Kampai.Game.MasterPlanComponentTaskDefinition> Tasks = new global::System.Collections.Generic.List<global::Kampai.Game.MasterPlanComponentTaskDefinition>();

		public override int TypeCode
		{
			get
			{
				return 1107;
			}
		}

		public string BenefitDescription { get; set; }

		public string OnClickSound { get; set; }

		public global::Kampai.Game.MasterPlanComponentRewardDefinition Reward { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, BenefitDescription);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, OnClickSound);
			global::Kampai.Util.BinarySerializationUtil.WriteMasterPlanComponentRewardDefinition(writer, Reward);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, global::Kampai.Util.BinarySerializationUtil.WriteMasterPlanComponentTaskDefinition, Tasks);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			BenefitDescription = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			OnClickSound = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Reward = global::Kampai.Util.BinarySerializationUtil.ReadMasterPlanComponentRewardDefinition(reader);
			Tasks = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, global::Kampai.Util.BinarySerializationUtil.ReadMasterPlanComponentTaskDefinition, Tasks);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "BENEFITDESCRIPTION":
				reader.Read();
				BenefitDescription = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "ONCLICKSOUND":
				reader.Read();
				OnClickSound = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "REWARD":
				reader.Read();
				Reward = global::Kampai.Util.ReaderUtil.ReadMasterPlanComponentRewardDefinition(reader, converters);
				break;
			case "TASKS":
				reader.Read();
				Tasks = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadMasterPlanComponentTaskDefinition, Tasks);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public virtual global::Kampai.Game.Instance Build()
		{
			return new global::Kampai.Game.MasterPlanComponent(this);
		}

		public bool AreTasksComplete(global::Kampai.Game.IPlayerService playerService)
		{
			global::Kampai.Game.MasterPlanComponent firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponent>(ID);
			if (firstInstanceByDefinitionId == null)
			{
				return false;
			}
			for (int i = 0; i < firstInstanceByDefinitionId.tasks.Count; i++)
			{
				if (!firstInstanceByDefinitionId.tasks[i].isComplete)
				{
					return false;
				}
			}
			return true;
		}
	}
}

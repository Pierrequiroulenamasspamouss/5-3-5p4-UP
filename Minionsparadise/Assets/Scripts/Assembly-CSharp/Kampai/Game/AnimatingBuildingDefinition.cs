namespace Kampai.Game
{
	public abstract class AnimatingBuildingDefinition : global::Kampai.Game.RepairableBuildingDefinition
	{
		public int GagFrequency;

		public override int TypeCode
		{
			get
			{
				return 1030;
			}
		}

		public global::System.Collections.Generic.IList<global::Kampai.Game.BuildingAnimationDefinition> AnimationDefinitions { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, AnimationDefinitions);
			writer.Write(GagFrequency);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			AnimationDefinitions = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, AnimationDefinitions);
			GagFrequency = reader.ReadInt32();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			default:
			{
				int num = 1; //FIX USE OF UNASSIGNED VARIABLE
				if (num == 1)
				{
					reader.Read();
					GagFrequency = global::System.Convert.ToInt32(reader.Value);
					break;
				}
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			case "ANIMATIONDEFINITIONS":
				reader.Read();
				AnimationDefinitions = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, AnimationDefinitions);
				break;
			}
			return true;
		}

		public virtual global::System.Collections.Generic.IList<string> AnimationControllerKeys()
		{
			global::System.Collections.Generic.IList<string> list = new global::System.Collections.Generic.List<string>();
			if (AnimationDefinitions != null && AnimationDefinitions.Count > 0)
			{
				foreach (global::Kampai.Game.BuildingAnimationDefinition animationDefinition in AnimationDefinitions)
				{
					if (!string.IsNullOrEmpty(animationDefinition.BuildingController))
					{
						list.Add(animationDefinition.BuildingController);
					}
					if (!string.IsNullOrEmpty(animationDefinition.MinionController))
					{
						list.Add(animationDefinition.MinionController);
					}
				}
			}
			return list;
		}
	}
}

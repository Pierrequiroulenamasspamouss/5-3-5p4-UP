namespace Kampai.Game
{
	public class UIAnimationDefinition : global::Kampai.Game.AnimationDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1084;
			}
		}

		public string AnimationClipName { get; set; }

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, AnimationClipName);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			AnimationClipName = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "ANIMATIONCLIPNAME":
				reader.Read();
				AnimationClipName = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}
	}
}

namespace Kampai.Game
{
	public class CompositeBuilding : global::Kampai.Game.Building<global::Kampai.Game.CompositeBuildingDefinition>
	{
		public global::System.Collections.Generic.IList<int> AttachedCompositePieceIDs { get; set; }

		public CompositeBuilding(global::Kampai.Game.CompositeBuildingDefinition definition)
			: base(definition)
		{
			AttachedCompositePieceIDs = new global::System.Collections.Generic.List<int>();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "ATTACHEDCOMPOSITEPIECEIDS":
				reader.Read();
				AttachedCompositePieceIDs = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, AttachedCompositePieceIDs);
				return true;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
		}

		public override void Serialize(global::Newtonsoft.Json.JsonWriter writer)
		{
			writer.WriteStartObject();
			SerializeProperties(writer);
			writer.WriteEndObject();
		}

		protected override void SerializeProperties(global::Newtonsoft.Json.JsonWriter writer)
		{
			base.SerializeProperties(writer);
			if (AttachedCompositePieceIDs == null)
			{
				return;
			}
			writer.WritePropertyName("AttachedCompositePieceIDs");
			writer.WriteStartArray();
			global::System.Collections.Generic.IEnumerator<int> enumerator = AttachedCompositePieceIDs.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.Current;
					writer.WriteValue(current);
				}
			}
			finally
			{
				enumerator.Dispose();
			}
			writer.WriteEndArray();
		}

		public override global::Kampai.Game.View.BuildingObject AddBuildingObject(global::UnityEngine.GameObject gameObject)
		{
			return gameObject.AddComponent<global::Kampai.Game.View.CompositeBuildingObject>();
		}

		public void ShufflePieceIDs()
		{
			int item = AttachedCompositePieceIDs[AttachedCompositePieceIDs.Count - 1];
			AttachedCompositePieceIDs.RemoveAt(AttachedCompositePieceIDs.Count - 1);
			AttachedCompositePieceIDs.Insert(0, item);
		}
	}
}

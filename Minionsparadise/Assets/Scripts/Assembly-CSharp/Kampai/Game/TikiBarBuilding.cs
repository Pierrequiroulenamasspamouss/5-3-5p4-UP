namespace Kampai.Game
{
	public class TikiBarBuilding : global::Kampai.Game.TaskableMinionPartyBuilding<global::Kampai.Game.TikiBarBuildingDefinition>, global::Kampai.Game.Building, global::Kampai.Game.ZoomableBuilding, global::Kampai.Game.Instance, global::Kampai.Game.Locatable, global::Kampai.Util.IFastJSONDeserializable, global::Kampai.Util.IFastJSONSerializable, global::Kampai.Util.Identifiable
	{
		public global::System.Collections.Generic.IList<int> minionQueue { get; set; }

		public global::Kampai.Game.ZoomableBuildingDefinition ZoomableDefinition
		{
			get
			{
				return base.Definition;
			}
		}

		public TikiBarBuilding(global::Kampai.Game.TikiBarBuildingDefinition def)
			: base(def)
		{
			minionQueue = new global::System.Collections.Generic.List<int>();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "MINIONQUEUE":
				reader.Read();
				minionQueue = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, minionQueue);
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
			if (minionQueue == null)
			{
				return;
			}
			writer.WritePropertyName("minionQueue");
			writer.WriteStartArray();
			global::System.Collections.Generic.IEnumerator<int> enumerator = minionQueue.GetEnumerator();
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
			return gameObject.AddComponent<global::Kampai.Game.View.TikiBarBuildingObjectView>();
		}

		public int GetMinionSlotIndex(int characterDefinitionID)
		{
			return minionQueue.IndexOf(characterDefinitionID);
		}

		public int GetOpenSlot()
		{
			for (int i = 0; i < 3; i++)
			{
				if (minionQueue[i] == -1)
				{
					return i;
				}
			}
			return -1;
		}

		public override int GetTransactionID(global::Kampai.Game.IDefinitionService definitionService)
		{
			return 0;
		}

		public override bool HasDetailMenuToShow()
		{
			return false;
		}

		public override string GetPrefab(int index = 0)
		{
			if (State == global::Kampai.Game.BuildingState.Disabled)
			{
				SetState(global::Kampai.Game.BuildingState.MissingTikiSign);
			}
			if (State == global::Kampai.Game.BuildingState.MissingTikiSign)
			{
				return base.Definition.noSignPrefab;
			}
			return base.GetPrefab(0);
		}
	}
}

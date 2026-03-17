namespace Kampai.Game
{
	public class LeisureBuilding : global::Kampai.Game.Building<global::Kampai.Game.LeisureBuildingDefintiion>
	{
		protected global::System.Collections.Generic.IList<int> minionList;

		[global::Newtonsoft.Json.JsonIgnore]
		public global::System.Collections.Generic.IList<int> MinionList
		{
			get
			{
				return minionList;
			}
			set
			{
				minionList = value;
			}
		}

		public int UTCLastTaskingTimeStarted { get; set; }

		public LeisureBuilding(global::Kampai.Game.LeisureBuildingDefintiion def)
			: base(def)
		{
			minionList = new global::System.Collections.Generic.List<int>();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "UTCLASTTASKINGTIMESTARTED":
				reader.Read();
				UTCLastTaskingTimeStarted = global::System.Convert.ToInt32(reader.Value);
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
			writer.WritePropertyName("UTCLastTaskingTimeStarted");
			writer.WriteValue(UTCLastTaskingTimeStarted);
		}

		public override global::Kampai.Game.View.BuildingObject AddBuildingObject(global::UnityEngine.GameObject gameObject)
		{
			return gameObject.AddComponent<global::Kampai.Game.View.LeisureBuildingObjectView>();
		}

		public int GetMinionsInBuilding()
		{
			return minionList.Count;
		}

		public void AddMinion(int minionID, int utcTime)
		{
			if (!minionList.Contains(minionID))
			{
				minionList.Add(minionID);
				if (UTCLastTaskingTimeStarted == 0)
				{
					UTCLastTaskingTimeStarted = utcTime;
				}
			}
		}

		public int GetMinionRouteIndex(int minionID)
		{
			return minionList.IndexOf(minionID);
		}

		public void CleanMinionQueue()
		{
			minionList.Clear();
		}

		public int GetMinionByIndex(int index)
		{
			return minionList[index];
		}
	}
}

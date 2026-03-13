namespace Kampai.Game
{
	public abstract class TaskableBuilding<T> : global::Kampai.Game.RepairableBuilding<T>, global::Kampai.Game.Building, global::Kampai.Game.RepairableBuilding, global::Kampai.Game.TaskableBuilding, global::Kampai.Game.Instance, global::Kampai.Game.Locatable, global::Kampai.Util.IFastJSONDeserializable, global::Kampai.Util.IFastJSONSerializable, global::Kampai.Util.Identifiable where T : global::Kampai.Game.TaskableBuildingDefinition
	{
		protected global::System.Collections.Generic.IList<int> minionList;

		protected int minionSlotsOwned;

		private global::System.Collections.Generic.IList<int> completeMinionQueue;

		global::Kampai.Game.TaskableBuildingDefinition global::Kampai.Game.TaskableBuilding.Definition
		{
			get
			{
				return base.Definition;
			}
		}

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

		public int NextGagTime { get; set; }

		public int CumulativeTaskingTime { get; set; }

		public int UTCLastTaskingTimeStarted { get; set; }

		public int MinionSlotsOwned
		{
			get
			{
				return minionSlotsOwned;
			}
			set
			{
				minionSlotsOwned = value;
			}
		}

		[global::Newtonsoft.Json.JsonIgnore]
		public global::System.Collections.Generic.IList<int> CompleteMinionQueue
		{
			get
			{
				return completeMinionQueue;
			}
			set
			{
				completeMinionQueue = value;
			}
		}

		public TaskableBuilding(T definition)
			: base(definition)
		{
			minionList = new global::System.Collections.Generic.List<int>();
			minionSlotsOwned = definition.DefaultSlots;
			completeMinionQueue = new global::System.Collections.Generic.List<int>();
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "NEXTGAGTIME":
				reader.Read();
				NextGagTime = global::System.Convert.ToInt32(reader.Value);
				break;
			case "CUMULATIVETASKINGTIME":
				reader.Read();
				CumulativeTaskingTime = global::System.Convert.ToInt32(reader.Value);
				break;
			case "UTCLASTTASKINGTIMESTARTED":
				reader.Read();
				UTCLastTaskingTimeStarted = global::System.Convert.ToInt32(reader.Value);
				break;
			case "MINIONSLOTSOWNED":
				reader.Read();
				MinionSlotsOwned = global::System.Convert.ToInt32(reader.Value);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
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
			writer.WritePropertyName("NextGagTime");
			writer.WriteValue(NextGagTime);
			writer.WritePropertyName("CumulativeTaskingTime");
			writer.WriteValue(CumulativeTaskingTime);
			writer.WritePropertyName("UTCLastTaskingTimeStarted");
			writer.WriteValue(UTCLastTaskingTimeStarted);
			writer.WritePropertyName("MinionSlotsOwned");
			writer.WriteValue(MinionSlotsOwned);
		}

		public int GetMinionSlotsOwned()
		{
			return minionSlotsOwned;
		}

		public int GetMinionsInBuilding()
		{
			return minionList.Count;
		}

		public bool AreAllMinionSlotsFilled()
		{
			return GetMinionsInBuilding() >= GetMinionSlotsOwned();
		}

		public void AddMinion(int minionID, int utcTime)
		{
			if (!minionList.Contains(minionID))
			{
				minionList.Add(minionID);
				if (UTCLastTaskingTimeStarted == 0 && GetNumBusyMinions() > 0)
				{
					UTCLastTaskingTimeStarted = utcTime;
				}
			}
		}

		public void RemoveMinion(int minionID, int utcTime)
		{
			minionList.Remove(minionID);
			ReconcileMinionStoppedTasking(utcTime);
		}

		private int GetNumBusyMinions()
		{
			return minionList.Count - completeMinionQueue.Count;
		}

		protected void ReconcileMinionStoppedTasking(int utcTime)
		{
			if (GetNumBusyMinions() == 0 && UTCLastTaskingTimeStarted > 0)
			{
				int num = utcTime - UTCLastTaskingTimeStarted;
				CumulativeTaskingTime += num;
				UTCLastTaskingTimeStarted = 0;
			}
		}

		public int GetMinionByIndex(int index)
		{
			return minionList[index];
		}

		public int GetIndexByMinionID(int minionID)
		{
			for (int i = 0; i < minionList.Count; i++)
			{
				if (minionList[i] == minionID)
				{
					return i;
				}
			}
			return -1;
		}

		public int GetNumCompleteMinions()
		{
			return completeMinionQueue.Count;
		}

		public void AddToCompletedMinions(int minionID, int utcTime)
		{
			completeMinionQueue.Add(minionID);
			ReconcileMinionStoppedTasking(utcTime);
		}

		public int HarvestFromCompleteMinions()
		{
			int num = completeMinionQueue[0];
			completeMinionQueue.RemoveAt(0);
			minionList.Remove(num);
			return num;
		}

		public bool IsEligibleForGag()
		{
			int result;
			if (base.Definition.GagFrequency > 0)
			{
				int count = minionList.Count;
				T definition = base.Definition;
				if (count >= definition.WorkStations)
				{
					result = ((completeMinionQueue.Count == 0) ? 1 : 0);
					goto IL_004b;
				}
			}
			result = 0;
			goto IL_004b;
			IL_004b:
			return (byte)result != 0;
		}

		public int GetCumulativeTimeTasked(int utcTime)
		{
			int num = CumulativeTaskingTime;
			if (UTCLastTaskingTimeStarted > 0)
			{
				num += utcTime - UTCLastTaskingTimeStarted;
			}
			return num;
		}

		public void GagPlayed(int utcTime)
		{
			NextGagTime = GetCumulativeTimeTasked(utcTime) + base.Definition.GagFrequency;
		}

		public int GetNextGagPlayTime(int utcTime)
		{
			int num = NextGagTime - GetCumulativeTimeTasked(utcTime);
			return (num >= 0) ? num : 0;
		}

		public override bool HasDetailMenuToShow()
		{
			return GetNumCompleteMinions() == 0;
		}

		public virtual int GetAvailableHarvest()
		{
			return GetNumCompleteMinions();
		}

		public abstract int GetTransactionID(global::Kampai.Game.IDefinitionService definitionService);
	}
	public interface TaskableBuilding : global::Kampai.Game.Building, global::Kampai.Game.RepairableBuilding, global::Kampai.Game.Instance, global::Kampai.Game.Locatable, global::Kampai.Util.IFastJSONDeserializable, global::Kampai.Util.IFastJSONSerializable, global::Kampai.Util.Identifiable
	{
		new global::Kampai.Game.TaskableBuildingDefinition Definition { get; }

		global::System.Collections.Generic.IList<int> MinionList { get; set; }

		int NextGagTime { get; set; }

		int CumulativeTaskingTime { get; set; }

		int UTCLastTaskingTimeStarted { get; set; }

		int GetMinionsInBuilding();

		void AddMinion(int minionID, int utcTime);

		void RemoveMinion(int minionID, int utcTime);

		int GetMinionSlotsOwned();

		int GetIndexByMinionID(int minionID);

		int GetMinionByIndex(int index);

		int GetNumCompleteMinions();

		void AddToCompletedMinions(int minionID, int utcTime);

		int HarvestFromCompleteMinions();

		bool IsEligibleForGag();

		void GagPlayed(int utcTime);

		int GetCumulativeTimeTasked(int utcTime);

		int GetNextGagPlayTime(int utcTime);

		int GetTransactionID(global::Kampai.Game.IDefinitionService definitionService);

		int GetAvailableHarvest();
	}
}

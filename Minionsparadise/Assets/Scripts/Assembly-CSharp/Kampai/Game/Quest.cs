namespace Kampai.Game
{
	public class Quest : global::Kampai.Game.Instance<global::Kampai.Game.QuestDefinition>, global::Kampai.Game.IGameTimeTracker, global::System.IComparable<global::Kampai.Game.Quest>
	{
		public global::System.Collections.Generic.Dictionary<string, global::Kampai.Game.QuestScriptInstance> questScriptInstances = new global::System.Collections.Generic.Dictionary<string, global::Kampai.Game.QuestScriptInstance>();

		public virtual global::System.Collections.Generic.IList<global::Kampai.Game.QuestStep> Steps { get; set; }

		public int UTCQuestStartTime { get; set; }

		public int QuestIconTrackedInstanceId { get; set; }

		public int QuestVersion { get; set; }

		public int StartGameTime { get; set; }

		public virtual global::Kampai.Game.QuestState state { get; set; }

		[global::Newtonsoft.Json.JsonIgnore]
		public virtual bool AutoGrantReward { get; set; }

		public global::Kampai.Game.DynamicQuestDefinition dynamicDefinition { get; set; }

		public Quest(global::Kampai.Game.QuestDefinition def)
			: base(def)
		{
			CheckDynamicDefinition(def);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "STEPS":
				reader.Read();
				Steps = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, global::Kampai.Util.ReaderUtil.ReadQuestStep, Steps);
				break;
			case "UTCQUESTSTARTTIME":
				reader.Read();
				UTCQuestStartTime = global::System.Convert.ToInt32(reader.Value);
				break;
			case "QUESTICONTRACKEDINSTANCEID":
				reader.Read();
				QuestIconTrackedInstanceId = global::System.Convert.ToInt32(reader.Value);
				break;
			case "QUESTVERSION":
				reader.Read();
				QuestVersion = global::System.Convert.ToInt32(reader.Value);
				break;
			case "STARTGAMETIME":
				reader.Read();
				StartGameTime = global::System.Convert.ToInt32(reader.Value);
				break;
			case "STATE":
				reader.Read();
				state = global::Kampai.Util.ReaderUtil.ReadEnum<global::Kampai.Game.QuestState>(reader);
				break;
			case "DYNAMICDEFINITION":
				reader.Read();
				dynamicDefinition = ((converters.questDefinitionConverter == null) ? global::Kampai.Util.FastJSONDeserializer.Deserialize<global::Kampai.Game.DynamicQuestDefinition>(reader, converters) : ((global::Kampai.Game.DynamicQuestDefinition)converters.questDefinitionConverter.ReadJson(reader, converters)));
				break;
			case "QUESTSCRIPTINSTANCES":
				reader.Read();
				questScriptInstances = global::Kampai.Util.ReaderUtil.ReadDictionary<global::Kampai.Game.QuestScriptInstance>(reader, converters);
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
			if (Steps != null)
			{
				writer.WritePropertyName("Steps");
				writer.WriteStartArray();
				global::System.Collections.Generic.IEnumerator<global::Kampai.Game.QuestStep> enumerator = Steps.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						global::Kampai.Game.QuestStep current = enumerator.Current;
						writer.WriteStartObject();
						writer.WritePropertyName("state");
						writer.WriteValue((int)current.state);
						writer.WritePropertyName("AmountCompleted");
						writer.WriteValue(current.AmountCompleted);
						writer.WritePropertyName("AmountReady");
						writer.WriteValue(current.AmountReady);
						writer.WritePropertyName("TrackedID");
						writer.WriteValue(current.TrackedID);
						writer.WriteEndObject();
					}
				}
				finally
				{
					enumerator.Dispose();
				}
				writer.WriteEndArray();
			}
			writer.WritePropertyName("UTCQuestStartTime");
			writer.WriteValue(UTCQuestStartTime);
			writer.WritePropertyName("QuestIconTrackedInstanceId");
			writer.WriteValue(QuestIconTrackedInstanceId);
			writer.WritePropertyName("QuestVersion");
			writer.WriteValue(QuestVersion);
			writer.WritePropertyName("StartGameTime");
			writer.WriteValue(StartGameTime);
			writer.WritePropertyName("state");
			writer.WriteValue((int)state);
			if (dynamicDefinition != null)
			{
				writer.WritePropertyName("dynamicDefinition");
				writer.WriteStartObject();
				if (dynamicDefinition.LocalizedKey != null)
				{
					writer.WritePropertyName("LocalizedKey");
					writer.WriteValue(dynamicDefinition.LocalizedKey);
				}
				writer.WritePropertyName("ID");
				writer.WriteValue(dynamicDefinition.ID);
				writer.WritePropertyName("Disabled");
				writer.WriteValue(dynamicDefinition.Disabled);
				writer.WritePropertyName("QuestLineID");
				writer.WriteValue(dynamicDefinition.QuestLineID);
				writer.WritePropertyName("type");
				writer.WriteValue((int)dynamicDefinition.type);
				writer.WritePropertyName("NarrativeOrder");
				writer.WriteValue(dynamicDefinition.NarrativeOrder);
				writer.WritePropertyName("ProgressiveGoto");
				writer.WriteValue(dynamicDefinition.ProgressiveGoto);
				writer.WritePropertyName("ShowRewardsPopupByDefault");
				writer.WriteValue(dynamicDefinition.ShowRewardsPopupByDefault);
				writer.WritePropertyName("SurfaceType");
				writer.WriteValue((int)dynamicDefinition.SurfaceType);
				writer.WritePropertyName("SurfaceID");
				writer.WriteValue(dynamicDefinition.SurfaceID);
				writer.WritePropertyName("UnlockLevel");
				writer.WriteValue(dynamicDefinition.UnlockLevel);
				writer.WritePropertyName("UnlockQuestId");
				writer.WriteValue(dynamicDefinition.UnlockQuestId);
				writer.WritePropertyName("QuestPriority");
				writer.WriteValue(dynamicDefinition.QuestPriority);
				writer.WritePropertyName("QuestVersion");
				writer.WriteValue(dynamicDefinition.QuestVersion);
				if (dynamicDefinition.QuestBookIcon != null)
				{
					writer.WritePropertyName("QuestBookIcon");
					writer.WriteValue(dynamicDefinition.QuestBookIcon);
				}
				if (dynamicDefinition.QuestBookMask != null)
				{
					writer.WritePropertyName("QuestBookMask");
					writer.WriteValue(dynamicDefinition.QuestBookMask);
				}
				writer.WritePropertyName("QuestCompletePlayerTrainingCategoryItemId");
				writer.WriteValue(dynamicDefinition.QuestCompletePlayerTrainingCategoryItemId);
				writer.WritePropertyName("QuestModalClosePlayerTrainingCategoryItemId");
				writer.WriteValue(dynamicDefinition.QuestModalClosePlayerTrainingCategoryItemId);
				if (dynamicDefinition.QuestSteps != null)
				{
					writer.WritePropertyName("QuestSteps");
					writer.WriteStartArray();
					global::System.Collections.Generic.IEnumerator<global::Kampai.Game.QuestStepDefinition> enumerator2 = dynamicDefinition.QuestSteps.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							global::Kampai.Game.QuestStepDefinition current2 = enumerator2.Current;
							writer.WriteStartObject();
							writer.WritePropertyName("Type");
							writer.WriteValue((int)current2.Type);
							writer.WritePropertyName("ItemAmount");
							writer.WriteValue(current2.ItemAmount);
							writer.WritePropertyName("ItemDefinitionID");
							writer.WriteValue(current2.ItemDefinitionID);
							writer.WritePropertyName("CostumeDefinitionID");
							writer.WriteValue(current2.CostumeDefinitionID);
							writer.WritePropertyName("ShowWayfinder");
							writer.WriteValue(current2.ShowWayfinder);
							writer.WritePropertyName("QuestStepCompletePlayerTrainingCategoryItemId");
							writer.WriteValue(current2.QuestStepCompletePlayerTrainingCategoryItemId);
							writer.WritePropertyName("UpgradeLevel");
							writer.WriteValue(current2.UpgradeLevel);
							writer.WriteEndObject();
						}
					}
					finally
					{
						enumerator2.Dispose();
					}
					writer.WriteEndArray();
				}
				writer.WritePropertyName("RewardTransaction");
				writer.WriteValue(dynamicDefinition.RewardTransaction);
				writer.WritePropertyName("RewardDisplayCount");
				writer.WriteValue(dynamicDefinition.RewardDisplayCount);
				if (dynamicDefinition.WayFinderIcon != null)
				{
					writer.WritePropertyName("WayFinderIcon");
					writer.WriteValue(dynamicDefinition.WayFinderIcon);
				}
				if (dynamicDefinition.QuestIntro != null)
				{
					writer.WritePropertyName("QuestIntro");
					writer.WriteValue(dynamicDefinition.QuestIntro);
				}
				if (dynamicDefinition.QuestVoice != null)
				{
					writer.WritePropertyName("QuestVoice");
					writer.WriteValue(dynamicDefinition.QuestVoice);
				}
				if (dynamicDefinition.QuestOutro != null)
				{
					writer.WritePropertyName("QuestOutro");
					writer.WriteValue(dynamicDefinition.QuestOutro);
				}
				if (dynamicDefinition.QuestIntroMood != null)
				{
					writer.WritePropertyName("QuestIntroMood");
					writer.WriteValue(dynamicDefinition.QuestIntroMood);
				}
				if (dynamicDefinition.QuestVoiceMood != null)
				{
					writer.WritePropertyName("QuestVoiceMood");
					writer.WriteValue(dynamicDefinition.QuestVoiceMood);
				}
				if (dynamicDefinition.QuestOutroMood != null)
				{
					writer.WritePropertyName("QuestOutroMood");
					writer.WriteValue(dynamicDefinition.QuestOutroMood);
				}
				writer.WritePropertyName("ForceEnableRewardedAd2xReward");
				writer.WriteValue(dynamicDefinition.ForceEnableRewardedAd2xReward);
				writer.WritePropertyName("ForceDisableRewardedAd2xReward");
				writer.WriteValue(dynamicDefinition.ForceDisableRewardedAd2xReward);
				if (dynamicDefinition.RewardTransactionInstance != null)
				{
					writer.WritePropertyName("RewardTransactionInstance");
					writer.WriteStartObject();
					writer.WritePropertyName("ID");
					writer.WriteValue(dynamicDefinition.RewardTransactionInstance.ID);
					if (dynamicDefinition.RewardTransactionInstance.Inputs != null)
					{
						writer.WritePropertyName("Inputs");
						writer.WriteStartArray();
						global::System.Collections.Generic.IEnumerator<global::Kampai.Util.QuantityItem> enumerator3 = dynamicDefinition.RewardTransactionInstance.Inputs.GetEnumerator();
						try
						{
							while (enumerator3.MoveNext())
							{
								global::Kampai.Util.QuantityItem current3 = enumerator3.Current;
								writer.WriteStartObject();
								writer.WritePropertyName("ID");
								writer.WriteValue(current3.ID);
								writer.WritePropertyName("Quantity");
								writer.WriteValue(current3.Quantity);
								writer.WriteEndObject();
							}
						}
						finally
						{
							enumerator3.Dispose();
						}
						writer.WriteEndArray();
					}
					if (dynamicDefinition.RewardTransactionInstance.Outputs != null)
					{
						writer.WritePropertyName("Outputs");
						writer.WriteStartArray();
						global::System.Collections.Generic.IEnumerator<global::Kampai.Util.QuantityItem> enumerator4 = dynamicDefinition.RewardTransactionInstance.Outputs.GetEnumerator();
						try
						{
							while (enumerator4.MoveNext())
							{
								global::Kampai.Util.QuantityItem current4 = enumerator4.Current;
								writer.WriteStartObject();
								writer.WritePropertyName("ID");
								writer.WriteValue(current4.ID);
								writer.WritePropertyName("Quantity");
								writer.WriteValue(current4.Quantity);
								writer.WriteEndObject();
							}
						}
						finally
						{
							enumerator4.Dispose();
						}
						writer.WriteEndArray();
					}
					writer.WriteEndObject();
				}
				writer.WritePropertyName("DropStep");
				writer.WriteValue(dynamicDefinition.DropStep);
				writer.WriteEndObject();
			}
			if (questScriptInstances == null)
			{
				return;
			}
			writer.WritePropertyName("questScriptInstances");
			writer.WriteStartObject();
			global::System.Collections.Generic.Dictionary<string, global::Kampai.Game.QuestScriptInstance>.Enumerator enumerator5 = questScriptInstances.GetEnumerator();
			try
			{
				while (enumerator5.MoveNext())
				{
					global::System.Collections.Generic.KeyValuePair<string, global::Kampai.Game.QuestScriptInstance> current5 = enumerator5.Current;
					writer.WritePropertyName(global::System.Convert.ToString(current5.Key));
					current5.Value.Serialize(writer);
				}
			}
			finally
			{
				enumerator5.Dispose();
			}
			writer.WriteEndObject();
		}

		private void CheckDynamicDefinition(global::Kampai.Game.QuestDefinition def)
		{
			global::Kampai.Game.DynamicQuestDefinition dynamicQuestDefinition = def as global::Kampai.Game.DynamicQuestDefinition;
			if (dynamicQuestDefinition != null)
			{
				base.Definition = new global::Kampai.Game.QuestDefinition();
				base.Definition.ID = 77777;
				dynamicDefinition = dynamicQuestDefinition;
				dynamicDefinition.ID = base.Definition.ID;
			}
		}

		public new void OnDefinitionHotSwap(global::Kampai.Game.Definition definition)
		{
			base.OnDefinitionHotSwap(definition);
			CheckDynamicDefinition(definition as global::Kampai.Game.QuestDefinition);
		}

		public void Initialize()
		{
			if (Steps != null)
			{
				return;
			}
			global::Kampai.Game.QuestDefinition activeDefinition = GetActiveDefinition();
			if (activeDefinition.QuestSteps != null)
			{
				Steps = new global::System.Collections.Generic.List<global::Kampai.Game.QuestStep>(activeDefinition.QuestSteps.Count);
				for (int i = 0; i < activeDefinition.QuestSteps.Count; i++)
				{
					global::Kampai.Game.QuestStep item = new global::Kampai.Game.QuestStep();
					Steps.Add(item);
				}
			}
			else
			{
				Steps = new global::System.Collections.Generic.List<global::Kampai.Game.QuestStep>();
			}
			QuestVersion = GetActiveDefinition().QuestVersion;
		}

		public void Clear()
		{
			state = global::Kampai.Game.QuestState.Notstarted;
			UTCQuestStartTime = 0;
			global::System.Collections.Generic.IList<global::Kampai.Game.QuestStep> steps = Steps;
			if (steps != null)
			{
				steps.Clear();
				global::Kampai.Game.QuestDefinition activeDefinition = GetActiveDefinition();
				if (activeDefinition.QuestSteps != null)
				{
					for (int i = 0; i < activeDefinition.QuestSteps.Count; i++)
					{
						global::Kampai.Game.QuestStep item = new global::Kampai.Game.QuestStep();
						steps.Add(item);
					}
				}
			}
			if (questScriptInstances != null)
			{
				questScriptInstances.Clear();
			}
		}

		public global::Kampai.Game.QuestDefinition GetActiveDefinition()
		{
			if (dynamicDefinition != null)
			{
				if (dynamicDefinition.ID == 0)
				{
					dynamicDefinition.ID = base.Definition.ID;
				}
				return dynamicDefinition;
			}
			return base.Definition;
		}

		public bool IsDynamic()
		{
			return dynamicDefinition != null;
		}

		public bool IsProcedurallyGenerated()
		{
			return GetActiveDefinition().SurfaceType == global::Kampai.Game.QuestSurfaceType.ProcedurallyGenerated;
		}

		public int CompareTo(global::Kampai.Game.Quest other)
		{
			global::Kampai.Game.QuestDefinition activeDefinition = GetActiveDefinition();
			global::Kampai.Game.QuestDefinition activeDefinition2 = other.GetActiveDefinition();
			if (activeDefinition.SurfaceID == activeDefinition2.SurfaceID)
			{
				return activeDefinition2.QuestPriority.CompareTo(activeDefinition.QuestPriority);
			}
			return activeDefinition.SurfaceID.CompareTo(activeDefinition2.SurfaceID);
		}
	}
}

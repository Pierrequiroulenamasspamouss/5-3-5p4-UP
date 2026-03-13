namespace Kampai.Game.Trigger
{
	[global::Kampai.Util.RequiresJsonConverter]
	public abstract class TriggerDefinition : global::Kampai.Game.Definition, global::Kampai.Util.IBuilder<global::Kampai.Game.Trigger.TriggerInstance>, global::Kampai.Game.IDisplayableDefinition, global::Kampai.Game.Trigger.IIsTriggerable, global::System.IComparable<global::Kampai.Game.Trigger.TriggerDefinition>, global::System.IEquatable<global::Kampai.Game.Trigger.TriggerDefinition>, global::System.Collections.Generic.IComparer<global::Kampai.Game.Trigger.TriggerDefinition>
	{
		public override int TypeCode
		{
			get
			{
				return 1150;
			}
		}

		public string Title { get; set; }

		public string Description { get; set; }

		public string Image { get; set; }

		public string Mask { get; set; }

		public string WayFinderIcon { get; set; }

		public abstract global::Kampai.Game.Trigger.TriggerDefinitionType.Identifier type { get; }

		public int priority { get; set; }

		public int cooldownSeconds { get; set; }

		public bool ForceOverride { get; set; }

		public bool TreasureIntro { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.Trigger.TriggerConditionDefinition> conditions { get; set; }

		public global::System.Collections.Generic.IList<int> reward { get; set; }

		[global::Newtonsoft.Json.JsonIgnore]
		public global::System.Collections.Generic.IList<global::Kampai.Game.Trigger.TriggerRewardDefinition> rewards { get; private set; }

		protected TriggerDefinition()
		{
			rewards = new global::System.Collections.Generic.List<global::Kampai.Game.Trigger.TriggerRewardDefinition>();
		}

		public override void Write(global::System.IO.BinaryWriter writer)
		{
			base.Write(writer);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Title);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Description);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Image);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, Mask);
			global::Kampai.Util.BinarySerializationUtil.WriteString(writer, WayFinderIcon);
			writer.Write(priority);
			writer.Write(cooldownSeconds);
			writer.Write(ForceOverride);
			writer.Write(TreasureIntro);
			global::Kampai.Util.BinarySerializationUtil.WriteList(writer, conditions);
			global::Kampai.Util.BinarySerializationUtil.WriteListInt32(writer, reward);
		}

		public override void Read(global::System.IO.BinaryReader reader)
		{
			base.Read(reader);
			Title = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Description = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Image = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			Mask = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			WayFinderIcon = global::Kampai.Util.BinarySerializationUtil.ReadString(reader);
			priority = reader.ReadInt32();
			cooldownSeconds = reader.ReadInt32();
			ForceOverride = reader.ReadBoolean();
			TreasureIntro = reader.ReadBoolean();
			conditions = global::Kampai.Util.BinarySerializationUtil.ReadList(reader, conditions);
			reward = global::Kampai.Util.BinarySerializationUtil.ReadListInt32(reader, reward);
		}

		protected override bool DeserializeProperty(string propertyName, global::Newtonsoft.Json.JsonReader reader, JsonConverters converters)
		{
			switch (propertyName)
			{
			case "TITLE":
				reader.Read();
				Title = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "DESCRIPTION":
				reader.Read();
				Description = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "IMAGE":
				reader.Read();
				Image = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "MASK":
				reader.Read();
				Mask = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "WAYFINDERICON":
				reader.Read();
				WayFinderIcon = global::Kampai.Util.ReaderUtil.ReadString(reader, converters);
				break;
			case "PRIORITY":
				reader.Read();
				priority = global::System.Convert.ToInt32(reader.Value);
				break;
			case "COOLDOWNSECONDS":
				reader.Read();
				cooldownSeconds = global::System.Convert.ToInt32(reader.Value);
				break;
			case "FORCEOVERRIDE":
				reader.Read();
				ForceOverride = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "TREASUREINTRO":
				reader.Read();
				TreasureIntro = global::System.Convert.ToBoolean(reader.Value);
				break;
			case "CONDITIONS":
				reader.Read();
				conditions = global::Kampai.Util.ReaderUtil.PopulateList(reader, converters, converters.triggerConditionDefinitionConverter, conditions);
				break;
			case "REWARD":
				reader.Read();
				reward = global::Kampai.Util.ReaderUtil.PopulateListInt32(reader, reward);
				break;
			default:
				return base.DeserializeProperty(propertyName, reader, converters);
			}
			return true;
		}

		public abstract global::Kampai.Game.Trigger.TriggerInstance Build();

		public virtual bool IsTriggered(global::strange.extensions.context.api.ICrossContextCapable gameContext)
		{
			global::Kampai.Game.IPlayerService instance = gameContext.injectionBinder.GetInstance<global::Kampai.Game.IPlayerService>();
			if (instance == null || conditions == null || conditions.Count == 0)
			{
				return false;
			}
			global::Kampai.Game.Trigger.TriggerInstance triggerByDefinitionId = instance.GetTriggerByDefinitionId(ID);
			if (triggerByDefinitionId != null && triggerByDefinitionId.StartGameTime != -1)
			{
				return false;
			}
			bool result = true;
			for (int i = 0; i < conditions.Count; i++)
			{
				global::Kampai.Game.Trigger.TriggerConditionDefinition triggerConditionDefinition = conditions[i];
				if (triggerConditionDefinition == null || !triggerConditionDefinition.IsTriggered(gameContext))
				{
					result = false;
					break;
				}
			}
			return result;
		}

		public virtual void PrintTriggerConditions(global::strange.extensions.context.api.ICrossContextCapable gameContext, global::System.Text.StringBuilder outBuilder)
		{
			global::Kampai.Game.IPlayerService instance = gameContext.injectionBinder.GetInstance<global::Kampai.Game.IPlayerService>();
			if (instance == null || conditions == null || conditions.Count == 0)
			{
				return;
			}
			global::Kampai.Game.Trigger.TriggerInstance triggerByDefinitionId = instance.GetTriggerByDefinitionId(ID);
			if (triggerByDefinitionId != null && triggerByDefinitionId.StartGameTime != -1)
			{
				return;
			}
			for (int i = 0; i < conditions.Count; i++)
			{
				global::Kampai.Game.Trigger.TriggerConditionDefinition triggerConditionDefinition = conditions[i];
				if (triggerConditionDefinition != null)
				{
					outBuilder.AppendLine(triggerConditionDefinition.ToString() + " is triggered: " + triggerConditionDefinition.IsTriggered(gameContext));
				}
			}
		}

		public virtual int CompareTo(global::Kampai.Game.Trigger.TriggerDefinition rhs)
		{
			if (rhs == null)
			{
				return 1;
			}
			int num = rhs.priority.CompareTo(priority);
			if (num != 0)
			{
				return num;
			}
			int num2 = type.CompareTo(rhs.type);
			if (num2 != 0)
			{
				return num2;
			}
			return ID.CompareTo(rhs.ID);
		}

		public int Compare(global::Kampai.Game.Trigger.TriggerDefinition x, global::Kampai.Game.Trigger.TriggerDefinition y)
		{
			if (x == null)
			{
				return -1;
			}
			return x.CompareTo(y);
		}

		public bool Equals(global::Kampai.Game.Trigger.TriggerDefinition obj)
		{
			return obj != null && Equals((object)obj);
		}

		public override string ToString()
		{
			return string.Format("{0}, PRIORITY: {1}, COOLDOWN: {2}, Reward: {3}", base.ToString(), priority, cooldownSeconds, rewards);
		}

		public override bool Equals(object obj)
		{
			if (object.ReferenceEquals(null, obj))
			{
				return false;
			}
			if (object.ReferenceEquals(this, obj))
			{
				return true;
			}
			global::Kampai.Game.Trigger.TriggerDefinition triggerDefinition = obj as global::Kampai.Game.Trigger.TriggerDefinition;
			return !object.ReferenceEquals(null, triggerDefinition) && CompareTo(triggerDefinition) == 0;
		}

		public override int GetHashCode()
		{
			return new { type, priority, ID }.GetHashCode();
		}
	}
}

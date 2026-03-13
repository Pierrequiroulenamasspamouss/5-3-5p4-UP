namespace Kampai.Game
{
	public class TransactionArg
	{
		private global::System.Collections.Generic.Dictionary<global::System.Type, object> arguments = new global::System.Collections.Generic.Dictionary<global::System.Type, object>();

		private global::System.Collections.Generic.ICollection<global::Kampai.Game.ItemAccumulator> accumulators = new global::System.Collections.Generic.List<global::Kampai.Game.ItemAccumulator>();

		public int InstanceId { get; set; }

		public global::UnityEngine.Vector3 StartPosition { get; set; }

		public bool fromGlass { get; set; }

		public int TransactionUTCTime { get; set; }

		public bool IsFromPremiumSource { get; set; }

		public int IsFromQuestSource { get; set; }

		public int CraftableXPEarned { get; set; }

		public string Source { get; set; }

		public TransactionArg()
		{
		}

		public TransactionArg(int instanceId)
		{
			InstanceId = instanceId;
		}

		public TransactionArg(string source)
		{
			Source = source;
		}

		public void AddAccumulator(global::Kampai.Game.ItemAccumulator itemAccumulator)
		{
			accumulators.Add(itemAccumulator);
		}

		public global::System.Collections.Generic.ICollection<global::Kampai.Game.ItemAccumulator> GetAccumulators()
		{
			return accumulators;
		}

		public T Get<T>()
		{
			if (arguments.ContainsKey(typeof(T)))
			{
				return (T)arguments[typeof(T)];
			}
			return default(T);
		}

		public global::Kampai.Game.TransactionArg Add(object value)
		{
			if (value != null)
			{
				arguments.Add(value.GetType(), value);
			}
			return this;
		}
	}
}

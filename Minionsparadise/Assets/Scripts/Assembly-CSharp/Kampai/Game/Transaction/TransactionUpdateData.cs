namespace Kampai.Game.Transaction
{
	public class TransactionUpdateData
	{
		public global::Kampai.Game.Transaction.UpdateType Type { get; set; }

		public int TransactionId { get; set; }

		public int InstanceId { get; set; }

		public global::UnityEngine.Vector3 startPosition { get; set; }

		public bool fromGlass { get; set; }

		public int taxonomyId { get; set; }

		public int craftableXPEarned { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> Inputs { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> Outputs { get; set; }

		public global::System.Collections.Generic.IList<global::Kampai.Game.Instance> NewItems { get; set; }

		public global::Kampai.Game.TransactionTarget Target { get; set; }

		public bool IsFromPremiumSource { get; set; }

		public bool IsNotForPlayerTraining { get; set; }

		public string Source { get; set; }

		public TransactionUpdateData()
		{
			Inputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
			Outputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
		}

		public void AddInput(int id, int quantity)
		{
			Inputs.Add(new global::Kampai.Util.QuantityItem(id, (uint)quantity));
		}

		public void AddOutput(int id, int quantity)
		{
			Outputs.Add(new global::Kampai.Util.QuantityItem(id, (uint)quantity));
		}
	}
}

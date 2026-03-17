namespace Kampai.Game.View
{
	public class RewardQuantity : global::Kampai.Util.Identifiable
	{
		public int ID { get; set; }

		public int Quantity { get; set; }

		public bool NewItem { get; set; }

		public bool IsReward { get; set; }

		public RewardQuantity(int id, int quantity, bool isNew, bool isReward)
		{
			ID = id;
			Quantity = quantity;
			NewItem = isNew;
			IsReward = isReward;
		}
	}
}

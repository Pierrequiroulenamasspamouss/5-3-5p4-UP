namespace Kampai.Game.Mtx
{
	public class AmazonAppStoreReceipt : global::Kampai.Game.Mtx.IMtxReceipt
	{
		public string amazonUserId { get; set; }

		public string receipt { get; set; }

		public AmazonAppStoreReceipt()
		{
			base.platformStore = global::Kampai.Game.Mtx.IMtxReceipt.PlatformStore.AmazonAppStore;
		}

		public override string ToString()
		{
			return string.Format("AmazonAppStoreReceipt: amazonUserId: {0}, receipt: {1}", amazonUserId ?? "null", receipt);
		}
	}
}

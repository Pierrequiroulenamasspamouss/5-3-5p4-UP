namespace Kampai.Game.Mtx
{
	public class AppleAppStoreReceipt : global::Kampai.Game.Mtx.IMtxReceipt
	{
		public string receipt { get; set; }

		public AppleAppStoreReceipt()
		{
			base.platformStore = global::Kampai.Game.Mtx.IMtxReceipt.PlatformStore.AppleAppStore;
		}

		public override string ToString()
		{
			return string.Format("AppleAppStoreReceipt: receipt: {0}", receipt ?? "null");
		}
	}
}

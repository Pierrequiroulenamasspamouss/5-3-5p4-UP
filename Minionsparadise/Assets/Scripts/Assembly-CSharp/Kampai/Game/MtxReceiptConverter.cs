namespace Kampai.Game
{
	public class MtxReceiptConverter : global::Newtonsoft.Json.Converters.CustomCreationConverter<global::Kampai.Game.Mtx.IMtxReceipt>
	{
		private global::Kampai.Game.Mtx.IMtxReceipt.PlatformStore platformStore;

		public override object ReadJson(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			global::Newtonsoft.Json.Linq.JObject jObject = global::Newtonsoft.Json.Linq.JObject.Load(reader);
			if (jObject.Property("platformStore") != null)
			{
				string value = jObject.Property("platformStore").Value.ToString();
				platformStore = (global::Kampai.Game.Mtx.IMtxReceipt.PlatformStore)(int)global::System.Enum.Parse(typeof(global::Kampai.Game.Mtx.IMtxReceipt.PlatformStore), value);
			}
			reader = jObject.CreateReader();
			return base.ReadJson(reader, objectType, existingValue, serializer);
		}

		public override global::Kampai.Game.Mtx.IMtxReceipt Create(global::System.Type objectType)
		{
			switch (platformStore)
			{
			case global::Kampai.Game.Mtx.IMtxReceipt.PlatformStore.AppleAppStore:
				return new global::Kampai.Game.Mtx.AppleAppStoreReceipt();
			case global::Kampai.Game.Mtx.IMtxReceipt.PlatformStore.GooglePlay:
				return new global::Kampai.Game.Mtx.GooglePlayReceipt();
			case global::Kampai.Game.Mtx.IMtxReceipt.PlatformStore.AmazonAppStore:
				return new global::Kampai.Game.Mtx.AmazonAppStoreReceipt();
			default:
				return null;
			}
		}
	}
}

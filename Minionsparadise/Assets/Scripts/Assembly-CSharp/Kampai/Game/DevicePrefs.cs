namespace Kampai.Game
{
	public class DevicePrefs
	{
		[global::Newtonsoft.Json.JsonProperty("ConstructionNotif")]
		public bool ConstructionNotif = true;

		[global::Newtonsoft.Json.JsonProperty("BlackMarketNotif")]
		public bool BlackMarketNotif = true;

		[global::Newtonsoft.Json.JsonProperty("MinionsNotif")]
		public bool MinionsParadiseNotif = true;

		[global::Newtonsoft.Json.JsonProperty("BaseResourceNotif")]
		public bool BaseResourceNotif = true;

		[global::Newtonsoft.Json.JsonProperty("CraftingNotif")]
		public bool CraftingNotif = true;

		[global::Newtonsoft.Json.JsonProperty("EventNotif")]
		public bool EventNotif = true;

		[global::Newtonsoft.Json.JsonProperty("MarketPlaceNotif")]
		public bool MarketPlaceNotif = true;

		[global::Newtonsoft.Json.JsonProperty("SocialEventNotif")]
		public bool SocialEventNotif = true;

		[global::Newtonsoft.Json.JsonProperty("MusicVolume")]
		public float MusicVolume = 1f;

		[global::Newtonsoft.Json.JsonProperty("SFXVolume")]
		public float SFXVolume = 1f;
	}
}

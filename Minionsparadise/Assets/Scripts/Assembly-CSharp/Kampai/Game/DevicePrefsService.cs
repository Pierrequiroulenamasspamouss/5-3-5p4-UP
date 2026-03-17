namespace Kampai.Game
{
	public class DevicePrefsService : global::Kampai.Game.IDevicePrefsService
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("DevicePrefsService") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.DevicePrefs DevicePrefs = new global::Kampai.Game.DevicePrefs();

		private object mutex = new object();

		public global::Kampai.Game.DevicePrefs GetDevicePrefs()
		{
			return DevicePrefs;
		}

		public void Deserialize(string serialized)
		{
			lock (mutex)
			{
				try
				{
					global::Kampai.Game.DevicePrefs devicePrefs = global::Newtonsoft.Json.JsonConvert.DeserializeObject<global::Kampai.Game.DevicePrefs>(serialized);
					if (devicePrefs != null)
					{
						DevicePrefs = devicePrefs;
					}
					else
					{
						logger.Fatal(global::Kampai.Util.FatalCode.PS_JSON_PARSE_ERR, 1, "Json Parse Err: null Device");
					}
				}
				catch (global::Newtonsoft.Json.JsonSerializationException ex)
				{
					logger.Fatal(global::Kampai.Util.FatalCode.PS_JSON_PARSE_ERR, 2, "Json Parse Err: {0}", ex);
				}
				catch (global::Newtonsoft.Json.JsonReaderException ex2)
				{
					logger.Fatal(global::Kampai.Util.FatalCode.PS_JSON_PARSE_ERR, 3, "Json Parse Err: {0}", ex2);
				}
			}
		}

		public string Serialize()
		{
			string text = null;
			lock (mutex)
			{
				try
				{
					text = global::Newtonsoft.Json.JsonConvert.SerializeObject(DevicePrefs);
					logger.Debug(text);
				}
				catch (global::Newtonsoft.Json.JsonSerializationException ex)
				{
					logger.Fatal(global::Kampai.Util.FatalCode.PS_JSON_PARSE_ERR, 4, "Json Parse Err: {0}", ex.ToString());
				}
			}
			return text;
		}
	}
}

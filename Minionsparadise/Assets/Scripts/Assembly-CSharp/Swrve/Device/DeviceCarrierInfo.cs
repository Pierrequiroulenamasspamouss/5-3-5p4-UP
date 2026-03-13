namespace Swrve.Device
{
	public class DeviceCarrierInfo : global::Swrve.Device.ICarrierInfo
	{
		private global::UnityEngine.AndroidJavaObject androidTelephonyManager;

		public DeviceCarrierInfo()
		{
			try
			{
				using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("android.content.Context"))
				{
					using (global::UnityEngine.AndroidJavaClass androidJavaClass2 = new global::UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer"))
					{
						using (global::UnityEngine.AndroidJavaObject androidJavaObject = androidJavaClass2.GetStatic<global::UnityEngine.AndroidJavaObject>("currentActivity"))
						{
							string text = androidJavaClass.GetStatic<string>("TELEPHONY_SERVICE");
							androidTelephonyManager = androidJavaObject.Call<global::UnityEngine.AndroidJavaObject>("getSystemService", new object[1] { text });
						}
					}
				}
			}
			catch (global::System.Exception ex)
			{
				SwrveLog.LogWarning("Couldn't get access to TelephonyManager: " + ex.ToString());
			}
		}

		private string AndroidGetTelephonyManagerAttribute(string method)
		{
			if (androidTelephonyManager != null)
			{
				try
				{
					return androidTelephonyManager.Call<string>(method, new object[0]);
				}
				catch (global::System.Exception ex)
				{
					SwrveLog.LogWarning("Problem accessing the TelephonyManager - " + method + ": " + ex.ToString());
				}
			}
			return null;
		}

		public string GetName()
		{
			return AndroidGetTelephonyManagerAttribute("getSimOperatorName");
		}

		public string GetIsoCountryCode()
		{
			return AndroidGetTelephonyManagerAttribute("getSimCountryIso");
		}

		public string GetCarrierCode()
		{
			return AndroidGetTelephonyManagerAttribute("getSimOperator");
		}
	}
}

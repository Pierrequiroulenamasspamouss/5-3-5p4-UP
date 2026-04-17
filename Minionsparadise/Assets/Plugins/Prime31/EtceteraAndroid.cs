namespace Prime31
{
	public class EtceteraAndroid
	{
		public enum ScalingMode
		{
			None = 0,
			AspectFit = 1,
			Fill = 2
		}

		public class Contact
		{
			public string name;

			public global::System.Collections.Generic.List<string> emails;

			public global::System.Collections.Generic.List<string> phoneNumbers;
		}

		private static global::UnityEngine.AndroidJavaObject _plugin;

		static EtceteraAndroid()
		{
			if (global::UnityEngine.Application.platform != global::UnityEngine.RuntimePlatform.Android)
			{
				return;
			}
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.prime31.EtceteraPlugin"))
			{
				_plugin = androidJavaClass.CallStatic<global::UnityEngine.AndroidJavaObject>("instance", new object[0]);
			}
		}

		public static global::UnityEngine.Texture2D textureFromFileAtPath(string filePath)
		{
#if !UNITY_WEBPLAYER
			byte[] data = global::System.IO.File.ReadAllBytes(filePath);
			global::UnityEngine.Texture2D texture2D = new global::UnityEngine.Texture2D(1, 1);
            global::UnityEngine.ImageConversion.LoadImage(texture2D, data);
            texture2D.Apply();
			global::UnityEngine.Debug.Log("texture size: " + texture2D.width + "x" + texture2D.height);
			return texture2D;
#else
			return null;
#endif
		}

		public static void setSystemUiVisibilityToLowProfile(bool useLowProfile)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("setSystemUiVisibilityToLowProfile", useLowProfile);
			}
		}

		public static void playMovie(string pathOrUrl, uint bgColor, bool showControls, global::Prime31.EtceteraAndroid.ScalingMode scalingMode, bool closeOnTouch)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("playMovie", pathOrUrl, (int)bgColor, showControls, (int)scalingMode, closeOnTouch);
			}
		}

		public static void setAlertDialogTheme(int theme)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("setAlertDialogTheme", theme);
			}
		}

		public static void showToast(string text, bool useShortDuration)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("showToast", text, useShortDuration);
			}
		}

		public static void showAlert(string title, string message, string positiveButton)
		{
			showAlert(title, message, positiveButton, string.Empty);
		}

		public static void showAlert(string title, string message, string positiveButton, string negativeButton)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("showAlert", title, message, positiveButton, negativeButton);
			}
		}

		public static void showAlertPrompt(string title, string message, string promptHint, string promptText, string positiveButton, string negativeButton)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("showAlertPrompt", title, message, promptHint, promptText, positiveButton, negativeButton);
			}
		}

		public static void showAlertPromptWithTwoFields(string title, string message, string promptHintOne, string promptTextOne, string promptHintTwo, string promptTextTwo, string positiveButton, string negativeButton)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("showAlertPromptWithTwoFields", title, message, promptHintOne, promptTextOne, promptHintTwo, promptTextTwo, positiveButton, negativeButton);
			}
		}

		public static void showProgressDialog(string title, string message)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("showProgressDialog", title, message);
			}
		}

		public static void hideProgressDialog()
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("hideProgressDialog");
			}
		}

		public static void showWebView(string url)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("showWebView", url);
			}
		}

		public static void showCustomWebView(string url, bool disableTitle, bool disableBackButton)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("showCustomWebView", url, disableTitle, disableBackButton);
			}
		}

		public static void showEmailComposer(string toAddress, string subject, string text, bool isHTML)
		{
			showEmailComposer(toAddress, subject, text, isHTML, string.Empty);
		}

		public static void showEmailComposer(string toAddress, string subject, string text, bool isHTML, string attachmentFilePath)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("showEmailComposer", toAddress, subject, text, isHTML, attachmentFilePath);
			}
		}

		public static bool isSMSComposerAvailable()
		{
			if (global::UnityEngine.Application.platform != global::UnityEngine.RuntimePlatform.Android)
			{
				return false;
			}
			return _plugin.Call<bool>("isSMSComposerAvailable", new object[0]);
		}

		public static void showSMSComposer(string body)
		{
			showSMSComposer(body, null);
		}

		public static void showSMSComposer(string body, string[] recipients)
		{
			if (global::UnityEngine.Application.platform != global::UnityEngine.RuntimePlatform.Android)
			{
				return;
			}
			string text = string.Empty;
			if (recipients != null && recipients.Length > 0)
			{
				text = "smsto:";
				foreach (string text2 in recipients)
				{
					text = text + text2 + ";";
				}
			}
			_plugin.Call("showSMSComposer", text, body);
		}

		public static void shareImageWithNativeShareIntent(string pathToImage, string chooserText)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("shareImageWithNativeShareIntent", pathToImage, chooserText);
			}
		}

		public static void shareWithNativeShareIntent(string text, string subject, string chooserText, string pathToImage = null)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("shareWithNativeShareIntent", text, subject, chooserText, pathToImage);
			}
		}

		public static void promptToTakePhoto(string name)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("promptToTakePhoto", name);
			}
		}

		public static void promptForPictureFromAlbum(string name)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("promptForPictureFromAlbum", name);
			}
		}

		public static void promptToTakeVideo(string name)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("promptToTakeVideo", name);
			}
		}

		public static bool saveImageToGallery(string pathToPhoto, string title)
		{
			if (global::UnityEngine.Application.platform != global::UnityEngine.RuntimePlatform.Android)
			{
				return false;
			}
			return _plugin.Call<bool>("saveImageToGallery", new object[2] { pathToPhoto, title });
		}

		public static void scaleImageAtPath(string pathToImage, float scale)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("scaleImageAtPath", pathToImage, scale);
			}
		}

		public static global::UnityEngine.Vector2 getImageSizeAtPath(string pathToImage)
		{
			if (global::UnityEngine.Application.platform != global::UnityEngine.RuntimePlatform.Android)
			{
				return global::UnityEngine.Vector2.zero;
			}
			string text = _plugin.Call<string>("getImageSizeAtPath", new object[1] { pathToImage });
			string[] array = text.Split(',');
			return new global::UnityEngine.Vector2(int.Parse(array[0]), int.Parse(array[1]));
		}

		public static void enableImmersiveMode(bool shouldEnable)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("enableImmersiveMode", shouldEnable ? 1 : 0);
			}
		}

		public static void loadContacts(int startingIndex, int totalToRetrieve)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("loadContacts", startingIndex, totalToRetrieve);
			}
		}

		public static void initTTS()
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("initTTS");
			}
		}

		public static void teardownTTS()
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("teardownTTS");
			}
		}

		public static void speak(string text)
		{
			speak(text, global::Prime31.TTSQueueMode.Add);
		}

		public static void speak(string text, global::Prime31.TTSQueueMode queueMode)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("speak", text, (int)queueMode);
			}
		}

		public static void stop()
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("stop");
			}
		}

		public static void playSilence(long durationInMs, global::Prime31.TTSQueueMode queueMode)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("playSilence", durationInMs, (int)queueMode);
			}
		}

		public static void setPitch(float pitch)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("setPitch", pitch);
			}
		}

		public static void setSpeechRate(float rate)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("setSpeechRate", rate);
			}
		}

		public static void askForReviewSetButtonTitles(string remindMeLaterTitle, string dontAskAgainTitle, string rateItTitle)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("askForReviewSetButtonTitles", remindMeLaterTitle, dontAskAgainTitle, rateItTitle);
			}
		}

		public static void askForReview(int launchesUntilPrompt, int hoursUntilFirstPrompt, int hoursBetweenPrompts, string title, string message, bool isAmazonAppStore = false)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				if (isAmazonAppStore)
				{
					_plugin.Set("isAmazonAppStore", true);
				}
				_plugin.Call("askForReview", launchesUntilPrompt, hoursUntilFirstPrompt, hoursBetweenPrompts, title, message);
			}
		}

		public static void askForReviewNow(string title, string message, bool isAmazonAppStore = false)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				if (isAmazonAppStore)
				{
					_plugin.Set("isAmazonAppStore", true);
				}
				_plugin.Call("askForReviewNow", title, message);
			}
		}

		public static void resetAskForReview()
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("resetAskForReview");
			}
		}

		public static void openReviewPageInPlayStore(bool isAmazonAppStore = false)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				if (isAmazonAppStore)
				{
					_plugin.Set("isAmazonAppStore", true);
				}
				_plugin.Call("openReviewPageInPlayStore");
			}
		}

		public static void inlineWebViewShow(string url, int x, int y, int width, int height)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("inlineWebViewShow", url, x, y, width, height);
			}
		}

		public static void inlineWebViewClose()
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("inlineWebViewClose");
			}
		}

		public static void inlineWebViewSetUrl(string url)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("inlineWebViewSetUrl", url);
			}
		}

		public static void inlineWebViewSetFrame(int x, int y, int width, int height)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("inlineWebViewSetFrame", x, y, width, height);
			}
		}

		[global::System.Obsolete("Use the scheduleNotification variant that accepts a AndroidNotificationConfiguration parameter")]
		public static int scheduleNotification(long secondsFromNow, string title, string subtitle, string tickerText, string extraData, int requestCode = -1)
		{
			global::Prime31.AndroidNotificationConfiguration androidNotificationConfiguration = new global::Prime31.AndroidNotificationConfiguration(secondsFromNow, title, subtitle, tickerText);
			androidNotificationConfiguration.extraData = extraData;
			androidNotificationConfiguration.requestCode = requestCode;
			return scheduleNotification(androidNotificationConfiguration);
		}

		[global::System.Obsolete("Use the scheduleNotification variant that accepts a AndroidNotificationConfiguration parameter")]
		public static int scheduleNotification(long secondsFromNow, string title, string subtitle, string tickerText, string extraData, string smallIcon, string largeIcon, int requestCode = -1)
		{
			global::Prime31.AndroidNotificationConfiguration androidNotificationConfiguration = new global::Prime31.AndroidNotificationConfiguration(secondsFromNow, title, subtitle, tickerText);
			androidNotificationConfiguration.extraData = extraData;
			androidNotificationConfiguration.smallIcon = smallIcon;
			androidNotificationConfiguration.largeIcon = largeIcon;
			androidNotificationConfiguration.requestCode = requestCode;
			return scheduleNotification(androidNotificationConfiguration);
		}

		public static int scheduleNotification(global::Prime31.AndroidNotificationConfiguration config)
		{
			if (global::UnityEngine.Application.platform != global::UnityEngine.RuntimePlatform.Android)
			{
				return -1;
			}
			config.build();
			return _plugin.Call<int>("scheduleNotification", new object[1] { global::Prime31.Json.encode(config) });
		}

		public static void cancelNotification(int notificationId)
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("cancelNotification", notificationId);
			}
		}

		public static void cancelAllNotifications()
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("cancelAllNotifications");
			}
		}

		public static void checkForNotifications()
		{
			if (global::UnityEngine.Application.platform == global::UnityEngine.RuntimePlatform.Android)
			{
				_plugin.Call("checkForNotifications");
			}
		}
	}
}

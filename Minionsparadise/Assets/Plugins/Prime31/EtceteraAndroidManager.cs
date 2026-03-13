namespace Prime31
{
	public class EtceteraAndroidManager : global::Prime31.AbstractManager
	{
		public static event global::System.Action<string> alertButtonClickedEvent;

		public static event global::System.Action alertCancelledEvent;

		public static event global::System.Action<string> promptFinishedWithTextEvent;

		public static event global::System.Action promptCancelledEvent;

		public static event global::System.Action<string, string> twoFieldPromptFinishedWithTextEvent;

		public static event global::System.Action twoFieldPromptCancelledEvent;

		public static event global::System.Action webViewCancelledEvent;

		public static event global::System.Action albumChooserCancelledEvent;

		public static event global::System.Action<string> albumChooserSucceededEvent;

		public static event global::System.Action photoChooserCancelledEvent;

		public static event global::System.Action<string> photoChooserSucceededEvent;

		public static event global::System.Action<string> videoRecordingSucceededEvent;

		public static event global::System.Action videoRecordingCancelledEvent;

		public static event global::System.Action ttsInitializedEvent;

		public static event global::System.Action ttsFailedToInitializeEvent;

		public static event global::System.Action askForReviewWillOpenMarketEvent;

		public static event global::System.Action askForReviewRemindMeLaterEvent;

		public static event global::System.Action askForReviewDontAskAgainEvent;

		public static event global::System.Action<string> inlineWebViewJSCallbackEvent;

		public static event global::System.Action<string> notificationReceivedEvent;

		public static event global::System.Action<global::System.Collections.Generic.List<global::Prime31.EtceteraAndroid.Contact>> contactsLoadedEvent;

		static EtceteraAndroidManager()
		{
			global::Prime31.AbstractManager.initialize(typeof(global::Prime31.EtceteraAndroidManager));
		}

		public void alertButtonClicked(string positiveButton)
		{
			if (global::Prime31.EtceteraAndroidManager.alertButtonClickedEvent != null)
			{
				global::Prime31.EtceteraAndroidManager.alertButtonClickedEvent(positiveButton);
			}
		}

		public void alertCancelled(string empty)
		{
			if (global::Prime31.EtceteraAndroidManager.alertCancelledEvent != null)
			{
				global::Prime31.EtceteraAndroidManager.alertCancelledEvent();
			}
		}

		public void promptFinishedWithText(string text)
		{
			string[] array = text.Split(new string[1] { "|||" }, global::System.StringSplitOptions.None);
			if (array.Length == 1 && global::Prime31.EtceteraAndroidManager.promptFinishedWithTextEvent != null)
			{
				global::Prime31.EtceteraAndroidManager.promptFinishedWithTextEvent(array[0]);
			}
			if (array.Length == 2 && global::Prime31.EtceteraAndroidManager.twoFieldPromptFinishedWithTextEvent != null)
			{
				global::Prime31.EtceteraAndroidManager.twoFieldPromptFinishedWithTextEvent(array[0], array[1]);
			}
		}

		public void promptCancelled(string empty)
		{
			if (global::Prime31.EtceteraAndroidManager.promptCancelledEvent != null)
			{
				global::Prime31.EtceteraAndroidManager.promptCancelledEvent();
			}
		}

		public void twoFieldPromptCancelled(string empty)
		{
			if (global::Prime31.EtceteraAndroidManager.twoFieldPromptCancelledEvent != null)
			{
				global::Prime31.EtceteraAndroidManager.twoFieldPromptCancelledEvent();
			}
		}

		public void webViewCancelled(string empty)
		{
			if (global::Prime31.EtceteraAndroidManager.webViewCancelledEvent != null)
			{
				global::Prime31.EtceteraAndroidManager.webViewCancelledEvent();
			}
		}

		public void albumChooserCancelled(string empty)
		{
			if (global::Prime31.EtceteraAndroidManager.albumChooserCancelledEvent != null)
			{
				global::Prime31.EtceteraAndroidManager.albumChooserCancelledEvent();
			}
		}

		public void albumChooserSucceeded(string path)
		{
			if (global::Prime31.EtceteraAndroidManager.albumChooserSucceededEvent != null)
			{
				if (global::System.IO.File.Exists(path))
				{
					global::Prime31.EtceteraAndroidManager.albumChooserSucceededEvent(path);
				}
				else if (global::Prime31.EtceteraAndroidManager.albumChooserCancelledEvent != null)
				{
					global::Prime31.EtceteraAndroidManager.albumChooserCancelledEvent();
				}
			}
		}

		public void photoChooserCancelled(string empty)
		{
			if (global::Prime31.EtceteraAndroidManager.photoChooserCancelledEvent != null)
			{
				global::Prime31.EtceteraAndroidManager.photoChooserCancelledEvent();
			}
		}

		public void photoChooserSucceeded(string path)
		{
			if (global::Prime31.EtceteraAndroidManager.photoChooserSucceededEvent != null)
			{
				if (global::System.IO.File.Exists(path))
				{
					global::Prime31.EtceteraAndroidManager.photoChooserSucceededEvent(path);
				}
				else if (global::Prime31.EtceteraAndroidManager.photoChooserCancelledEvent != null)
				{
					global::Prime31.EtceteraAndroidManager.photoChooserCancelledEvent();
				}
			}
		}

		public void videoRecordingSucceeded(string path)
		{
			if (global::Prime31.EtceteraAndroidManager.videoRecordingSucceededEvent != null)
			{
				global::Prime31.EtceteraAndroidManager.videoRecordingSucceededEvent(path);
			}
		}

		public void videoRecordingCancelled(string empty)
		{
			if (global::Prime31.EtceteraAndroidManager.videoRecordingCancelledEvent != null)
			{
				global::Prime31.EtceteraAndroidManager.videoRecordingCancelledEvent();
			}
		}

		public void ttsInitialized(string result)
		{
			bool flag = result == "1";
			if (flag && global::Prime31.EtceteraAndroidManager.ttsInitializedEvent != null)
			{
				global::Prime31.EtceteraAndroidManager.ttsInitializedEvent();
			}
			if (!flag && global::Prime31.EtceteraAndroidManager.ttsFailedToInitializeEvent != null)
			{
				global::Prime31.EtceteraAndroidManager.ttsFailedToInitializeEvent();
			}
		}

		public void ttsUtteranceCompleted(string utteranceId)
		{
			global::UnityEngine.Debug.Log("utterance completed: " + utteranceId);
		}

		public void askForReviewWillOpenMarket(string empty)
		{
			if (global::Prime31.EtceteraAndroidManager.askForReviewWillOpenMarketEvent != null)
			{
				global::Prime31.EtceteraAndroidManager.askForReviewWillOpenMarketEvent();
			}
		}

		public void askForReviewRemindMeLater(string empty)
		{
			if (global::Prime31.EtceteraAndroidManager.askForReviewRemindMeLaterEvent != null)
			{
				global::Prime31.EtceteraAndroidManager.askForReviewRemindMeLaterEvent();
			}
		}

		public void askForReviewDontAskAgain(string empty)
		{
			if (global::Prime31.EtceteraAndroidManager.askForReviewDontAskAgainEvent != null)
			{
				global::Prime31.EtceteraAndroidManager.askForReviewDontAskAgainEvent();
			}
		}

		public void inlineWebViewJSCallback(string message)
		{
			global::Prime31.EtceteraAndroidManager.inlineWebViewJSCallbackEvent.fire(message);
		}

		public void notificationReceived(string extraData)
		{
			global::Prime31.EtceteraAndroidManager.notificationReceivedEvent.fire(extraData);
		}

		private void contactsLoaded(string json)
		{
			if (global::Prime31.EtceteraAndroidManager.contactsLoadedEvent != null)
			{
				global::System.Collections.Generic.List<global::Prime31.EtceteraAndroid.Contact> obj = global::Prime31.Json.decode<global::System.Collections.Generic.List<global::Prime31.EtceteraAndroid.Contact>>(json);
				global::Prime31.EtceteraAndroidManager.contactsLoadedEvent(obj);
			}
		}
	}
}

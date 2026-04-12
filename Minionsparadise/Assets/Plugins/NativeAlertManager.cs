public class NativeAlertManager : global::UnityEngine.MonoBehaviour
{
	private static class NativeMethods
	{
#if UNITY_IOS && !UNITY_EDITOR
		[global::System.Runtime.InteropServices.DllImport("__Internal", CharSet = global::System.Runtime.InteropServices.CharSet.Auto)]
		public static extern void showAlert(string title, string message, string positiveButton, string negativeButton);
#endif
	}

	public class NativeAlertEventArgs : global::System.EventArgs
	{
		public string ButtonText { get; set; }
	}

	public static global::System.Action<bool> AudioMuted;

	public static event global::System.EventHandler<NativeAlertManager.NativeAlertEventArgs> AlertClicked;

	public static void Init()
	{
		global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("NativeAlertManager");
		gameObject.AddComponent<NativeAlertManager>();
		global::UnityEngine.Object.DontDestroyOnLoad(gameObject);
	}

	public static void ShowAlert(string title, string message, string positiveButton, string negativeButton)
	{
#if UNITY_IOS && !UNITY_EDITOR
		NativeAlertManager.NativeMethods.showAlert(title, message, positiveButton, negativeButton);
#elif UNITY_ANDROID && !UNITY_EDITOR
		global::System.Action<string> onClick = null;
		onClick = delegate(string buttonText)
		{
			global::Prime31.EtceteraAndroidManager.alertButtonClickedEvent -= onClick;
			NativeAlertManager.AlertClicked?.Invoke(null, new NativeAlertManager.NativeAlertEventArgs
			{
				ButtonText = buttonText
			});
		};
		global::Prime31.EtceteraAndroidManager.alertButtonClickedEvent += onClick;
		global::Prime31.EtceteraAndroid.showAlert(title, message, positiveButton);
#endif
	}

	protected void OnClick(string buttonText)
	{
		global::System.EventHandler<NativeAlertManager.NativeAlertEventArgs> alertClicked = NativeAlertManager.AlertClicked;
		if (alertClicked != null)
		{
			alertClicked(this, new NativeAlertManager.NativeAlertEventArgs
			{
				ButtonText = buttonText
			});
		}
	}

	protected void MuteAudio(string mute)
	{
		if (AudioMuted != null)
		{
			AudioMuted(global::System.Convert.ToBoolean(mute));
		}
	}
}

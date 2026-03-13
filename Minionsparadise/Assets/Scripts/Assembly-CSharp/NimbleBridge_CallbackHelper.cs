internal class NimbleBridge_CallbackHelper : global::UnityEngine.MonoBehaviour
{
	private delegate void BridgeDisposeCallback(global::System.IntPtr userInfoPtr);

	private static NimbleBridge_CallbackHelper s_instance;

	private global::System.Collections.Generic.List<global::System.Action> m_pendingCallbacks = new global::System.Collections.Generic.List<global::System.Action>();

	private volatile bool m_callbacksPending;

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_setDisposeCallback(NimbleBridge_CallbackHelper.BridgeDisposeCallback callback);

	private void Awake()
	{
		global::UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
	}

	internal static NimbleBridge_CallbackHelper Get()
	{
		if (object.ReferenceEquals(s_instance, null))
		{
			if (!global::UnityEngine.Application.isPlaying)
			{
				return null;
			}
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("NimbleCallbackHelper");
			s_instance = gameObject.AddComponent<NimbleBridge_CallbackHelper>();
			NimbleBridge_setDisposeCallback(DisposeCallback);
		}
		return s_instance;
	}

	[global::AOT.MonoPInvokeCallback(typeof(NimbleBridge_CallbackHelper.BridgeDisposeCallback))]
	public static void DisposeCallback(global::System.IntPtr userInfoPtr)
	{
		global::System.Runtime.InteropServices.GCHandle.FromIntPtr(userInfoPtr).Free();
	}

	internal global::System.IntPtr MakeCallbackData(object data)
	{
		return global::System.Runtime.InteropServices.GCHandle.ToIntPtr(global::System.Runtime.InteropServices.GCHandle.Alloc(data));
	}

	internal object GetData(global::System.IntPtr dataPtr)
	{
		return global::System.Runtime.InteropServices.GCHandle.FromIntPtr(dataPtr).Target;
	}

	internal void RunOnMainThread(global::System.Action action)
	{
		lock (m_pendingCallbacks)
		{
			m_pendingCallbacks.Add(action);
			m_callbacksPending = true;
		}
	}

	private void Update()
	{
		if (!m_callbacksPending)
		{
			return;
		}
		global::System.Collections.Generic.List<global::System.Action> list;
		lock (m_pendingCallbacks)
		{
			list = new global::System.Collections.Generic.List<global::System.Action>(m_pendingCallbacks);
			m_pendingCallbacks.Clear();
			m_callbacksPending = false;
		}
		foreach (global::System.Action item in list)
		{
			item();
		}
	}
}

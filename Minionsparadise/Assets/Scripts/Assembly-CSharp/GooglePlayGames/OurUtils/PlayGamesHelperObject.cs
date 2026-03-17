namespace GooglePlayGames.OurUtils
{
	public class PlayGamesHelperObject : global::UnityEngine.MonoBehaviour
	{
		private static global::GooglePlayGames.OurUtils.PlayGamesHelperObject instance = null;

		private static bool sIsDummy = false;

		private static global::System.Collections.Generic.List<global::System.Action> sQueue = new global::System.Collections.Generic.List<global::System.Action>();

		private global::System.Collections.Generic.List<global::System.Action> localQueue = new global::System.Collections.Generic.List<global::System.Action>();

		private static volatile bool sQueueEmpty = true;

		private static global::System.Collections.Generic.List<global::System.Action<bool>> sPauseCallbackList = new global::System.Collections.Generic.List<global::System.Action<bool>>();

		private static global::System.Collections.Generic.List<global::System.Action<bool>> sFocusCallbackList = new global::System.Collections.Generic.List<global::System.Action<bool>>();

		public static void CreateObject()
		{
			if (!(instance != null))
			{
				if (global::UnityEngine.Application.isPlaying)
				{
					global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("PlayGames_QueueRunner");
					global::UnityEngine.Object.DontDestroyOnLoad(gameObject);
					instance = gameObject.AddComponent<global::GooglePlayGames.OurUtils.PlayGamesHelperObject>();
				}
				else
				{
					instance = new global::GooglePlayGames.OurUtils.PlayGamesHelperObject();
					sIsDummy = true;
				}
			}
		}

		public void Awake()
		{
			global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		public void OnDisable()
		{
			if (instance == this)
			{
				instance = null;
			}
		}

		public static void RunCoroutine(global::System.Collections.IEnumerator action)
		{
			if (instance != null)
			{
				RunOnGameThread(delegate
				{
					instance.StartCoroutine(action);
				});
			}
		}

		public static void RunOnGameThread(global::System.Action action)
		{
			if (action == null)
			{
				throw new global::System.ArgumentNullException("action");
			}
			if (sIsDummy)
			{
				return;
			}
			lock (sQueue)
			{
				sQueue.Add(action);
				sQueueEmpty = false;
			}
		}

		public void Update()
		{
			if (!sIsDummy && !sQueueEmpty)
			{
				localQueue.Clear();
				lock (sQueue)
				{
					localQueue.AddRange(sQueue);
					sQueue.Clear();
					sQueueEmpty = true;
				}
				for (int i = 0; i < localQueue.Count; i++)
				{
					localQueue[i]();
				}
			}
		}

		public void OnApplicationFocus(bool focused)
		{
			foreach (global::System.Action<bool> sFocusCallback in sFocusCallbackList)
			{
				try
				{
					sFocusCallback(focused);
				}
				catch (global::System.Exception ex)
				{
					global::UnityEngine.Debug.LogError("Exception in OnApplicationFocus:" + ex.Message + "\n" + ex.StackTrace);
				}
			}
		}

		public void OnApplicationPause(bool paused)
		{
			foreach (global::System.Action<bool> sPauseCallback in sPauseCallbackList)
			{
				try
				{
					sPauseCallback(paused);
				}
				catch (global::System.Exception ex)
				{
					global::UnityEngine.Debug.LogError("Exception in OnApplicationPause:" + ex.Message + "\n" + ex.StackTrace);
				}
			}
		}

		public static void AddFocusCallback(global::System.Action<bool> callback)
		{
			if (!sFocusCallbackList.Contains(callback))
			{
				sFocusCallbackList.Add(callback);
			}
		}

		public static bool RemoveFocusCallback(global::System.Action<bool> callback)
		{
			return sFocusCallbackList.Remove(callback);
		}

		public static void AddPauseCallback(global::System.Action<bool> callback)
		{
			if (!sPauseCallbackList.Contains(callback))
			{
				sPauseCallbackList.Add(callback);
			}
		}

		public static bool RemovePauseCallback(global::System.Action<bool> callback)
		{
			return sPauseCallbackList.Remove(callback);
		}
	}
}

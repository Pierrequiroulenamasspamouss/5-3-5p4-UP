namespace Discord.Unity.Editor
{
	internal abstract class EditorFacebookMockDialog : global::UnityEngine.MonoBehaviour
	{
		private global::UnityEngine.Rect modalRect;

		private global::UnityEngine.GUIStyle modalStyle;

		public global::Discord.Unity.Utilities.Callback<global::Discord.Unity.ResultContainer> Callback { protected get; set; }

		public string CallbackID { protected get; set; }

		protected abstract string DialogTitle { get; }

		public void Start()
		{
			modalRect = new global::UnityEngine.Rect(10f, 10f, global::UnityEngine.Screen.width - 20, global::UnityEngine.Screen.height - 20);
			global::UnityEngine.Texture2D texture2D = new global::UnityEngine.Texture2D(1, 1);
			texture2D.SetPixel(0, 0, new global::UnityEngine.Color(0.2f, 0.2f, 0.2f, 1f));
			texture2D.Apply();
			modalStyle = new global::UnityEngine.GUIStyle();
			modalStyle.normal.background = texture2D;
		}

		public void OnGUI()
		{
			global::UnityEngine.GUI.ModalWindow(GetHashCode(), modalRect, OnGUIDialog, DialogTitle, modalStyle);
		}

		protected abstract void DoGui();

		protected abstract void SendSuccessResult();

		protected virtual void SendCancelResult()
		{
			global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
			dictionary["cancelled"] = true;
			if (!string.IsNullOrEmpty(CallbackID))
			{
				dictionary["callback_id"] = CallbackID;
			}
			Callback(new global::Discord.Unity.ResultContainer(dictionary.ToJson()));
		}

		protected virtual void SendErrorResult(string errorMessage)
		{
			global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
			dictionary["error"] = errorMessage;
			if (!string.IsNullOrEmpty(CallbackID))
			{
				dictionary["callback_id"] = CallbackID;
			}
			Callback(new global::Discord.Unity.ResultContainer(dictionary.ToJson()));
		}

		private void OnGUIDialog(int windowId)
		{
			global::UnityEngine.GUILayout.Space(10f);
			global::UnityEngine.GUILayout.BeginVertical();
			global::UnityEngine.GUILayout.Label("Warning! Mock dialog responses will NOT match production dialogs");
			global::UnityEngine.GUILayout.Label("Test your app on one of the supported platforms");
			DoGui();
			global::UnityEngine.GUILayout.EndVertical();
			global::UnityEngine.GUILayout.BeginHorizontal();
			global::UnityEngine.GUILayout.FlexibleSpace();
			global::UnityEngine.GUIContent content = new global::UnityEngine.GUIContent("Send Success");
			global::UnityEngine.Rect rect = global::UnityEngine.GUILayoutUtility.GetRect(content, global::UnityEngine.GUI.skin.button);
			if (global::UnityEngine.GUI.Button(rect, content))
			{
				SendSuccessResult();
				global::UnityEngine.Object.Destroy(this);
			}
			global::UnityEngine.GUIContent content2 = new global::UnityEngine.GUIContent("Send Cancel");
			global::UnityEngine.Rect rect2 = global::UnityEngine.GUILayoutUtility.GetRect(content2, global::UnityEngine.GUI.skin.button);
			if (global::UnityEngine.GUI.Button(rect2, content2, global::UnityEngine.GUI.skin.button))
			{
				SendCancelResult();
				global::UnityEngine.Object.Destroy(this);
			}
			global::UnityEngine.GUIContent content3 = new global::UnityEngine.GUIContent("Send Error");
			global::UnityEngine.Rect rect3 = global::UnityEngine.GUILayoutUtility.GetRect(content2, global::UnityEngine.GUI.skin.button);
			if (global::UnityEngine.GUI.Button(rect3, content3, global::UnityEngine.GUI.skin.button))
			{
				SendErrorResult("Error: Error button pressed");
				global::UnityEngine.Object.Destroy(this);
			}
			global::UnityEngine.GUILayout.EndHorizontal();
		}
	}
}

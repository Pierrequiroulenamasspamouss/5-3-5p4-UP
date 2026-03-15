namespace Kampai.Util
{
	public class FatalView : global::UnityEngine.MonoBehaviour
	{
		private static string code = "0000";

		private static string title = "FatalTitle";

		private static string message = "FatalMessage";

		private static string playerID = string.Empty;

		public global::UnityEngine.UI.Text ErrorCode;

		public global::UnityEngine.UI.Text ActionMessage;

		public global::UnityEngine.UI.Text TitleMessage;

		public global::UnityEngine.UI.Text PlayerID;

		private void OnEnable()
		{
			ErrorCode.text = code;
			ActionMessage.text = message;
			TitleMessage.text = title;
			PlayerID.text = playerID;
		}

		public static void SetFatalText(string code, string message, string title, string playerID)
		{
			global::Kampai.Util.FatalView.code = code;
			global::Kampai.Util.FatalView.message = message;
			global::Kampai.Util.FatalView.title = title;
			global::Kampai.Util.FatalView.playerID = playerID;
		}
	}
}

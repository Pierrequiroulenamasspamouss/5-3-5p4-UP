namespace Discord.Unity.Editor.Dialogs
{
	internal class MockLoginDialog : global::Discord.Unity.Editor.EditorFacebookMockDialog
	{
		private string accessToken = string.Empty;

		protected override string DialogTitle
		{
			get
			{
				return "Mock Login Dialog";
			}
		}

		protected override void DoGui()
		{
			global::UnityEngine.GUILayout.BeginHorizontal();
			global::UnityEngine.GUILayout.Label("User Access Token:");
			accessToken = global::UnityEngine.GUILayout.TextField(accessToken, global::UnityEngine.GUI.skin.textArea, global::UnityEngine.GUILayout.MinWidth(400f));
			global::UnityEngine.GUILayout.EndHorizontal();
			global::UnityEngine.GUILayout.Space(10f);
			if (global::UnityEngine.GUILayout.Button("Find Access Token"))
			{
				global::UnityEngine.Application.OpenURL(string.Format("https://developers.discord.com/tools/accesstoken/?app_id={0}", global::Discord.Unity.FB.AppId));
			}
			global::UnityEngine.GUILayout.Space(20f);
		}

		protected override void SendSuccessResult()
		{
			if (string.IsNullOrEmpty(accessToken))
			{
				SendErrorResult("Empty Access token string");
				return;
			}
			global::Discord.Unity.FB.API("/me?fields=id&access_token=" + accessToken, global::Discord.Unity.HttpMethod.GET, delegate(global::Discord.Unity.IGraphResult graphResult)
			{
				if (!string.IsNullOrEmpty(graphResult.Error))
				{
					SendErrorResult("Graph API error: " + graphResult.Error);
				}
				else
				{
					string facebookID = graphResult.ResultDictionary["id"] as string;
					global::Discord.Unity.FB.API("/me/permissions?access_token=" + accessToken, global::Discord.Unity.HttpMethod.GET, delegate(global::Discord.Unity.IGraphResult permResult)
					{
						if (!string.IsNullOrEmpty(permResult.Error))
						{
							SendErrorResult("Graph API error: " + permResult.Error);
						}
						else
						{
							global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
							global::System.Collections.Generic.List<string> list2 = new global::System.Collections.Generic.List<string>();
							global::System.Collections.Generic.List<object> list3 = permResult.ResultDictionary["data"] as global::System.Collections.Generic.List<object>;
							foreach (global::System.Collections.Generic.Dictionary<string, object> item in list3)
							{
								if (item["status"] as string == "granted")
								{
									list.Add(item["permission"] as string);
								}
								else
								{
									list2.Add(item["permission"] as string);
								}
							}
							global::Discord.Unity.AccessToken newAccessToken = new global::Discord.Unity.AccessToken(this.accessToken, facebookID, global::System.DateTime.UtcNow.AddDays(60.0), list, global::System.DateTime.UtcNow);
							global::System.Collections.Generic.IDictionary<string, object> dictionary2 = (global::System.Collections.Generic.IDictionary<string, object>)global::Discord.MiniJSON.Json.Deserialize(newAccessToken.ToJson());
							dictionary2.Add("granted_permissions", list);
							dictionary2.Add("declined_permissions", list2);
							if (!string.IsNullOrEmpty(base.CallbackID))
							{
								dictionary2["callback_id"] = base.CallbackID;
							}
							if (base.Callback != null)
							{
								base.Callback(new global::Discord.Unity.ResultContainer(dictionary2));
							}
						}
					}, (global::System.Collections.Generic.IDictionary<string, string>)null);
				}
			}, (global::System.Collections.Generic.IDictionary<string, string>)null);
		}
	}
}

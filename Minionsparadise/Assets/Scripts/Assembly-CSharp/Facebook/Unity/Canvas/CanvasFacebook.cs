namespace Discord.Unity.Canvas
{
	internal sealed class CanvasFacebook : global::Discord.Unity.FacebookBase, global::Discord.Unity.Canvas.ICanvasFacebook, global::Discord.Unity.Canvas.ICanvasFacebookImplementation, global::Discord.Unity.Canvas.ICanvasFacebookResultHandler, global::Discord.Unity.IFacebook, global::Discord.Unity.IFacebookResultHandler, global::Discord.Unity.IPayFacebook
	{
		private class CanvasUIMethodCall<T> : global::Discord.Unity.MethodCall<T> where T : global::Discord.Unity.IResult
		{
			private global::Discord.Unity.Canvas.CanvasFacebook canvasImpl;

			private string callbackMethod;

			public CanvasUIMethodCall(global::Discord.Unity.Canvas.CanvasFacebook canvasImpl, string methodName, string callbackMethod)
				: base((global::Discord.Unity.FacebookBase)canvasImpl, methodName)
			{
				this.canvasImpl = canvasImpl;
				this.callbackMethod = callbackMethod;
			}

			public override void Call(global::Discord.Unity.MethodArguments args)
			{
				UI(base.MethodName, args, base.Callback);
			}

			private void UI(string method, global::Discord.Unity.MethodArguments args, global::Discord.Unity.FacebookDelegate<T> callback = null)
			{
				canvasImpl.canvasJSWrapper.DisableFullScreen();
				global::Discord.Unity.MethodArguments methodArguments = new global::Discord.Unity.MethodArguments(args);
				methodArguments.AddString("app_id", canvasImpl.appId);
				methodArguments.AddString("method", method);
				string text = canvasImpl.CallbackManager.AddFacebookDelegate(callback);
				canvasImpl.canvasJSWrapper.ExternalCall("FBUnity.ui", methodArguments.ToJsonString(), text, callbackMethod);
			}
		}

		internal const string MethodAppRequests = "apprequests";

		internal const string MethodFeed = "feed";

		internal const string MethodPay = "pay";

		internal const string MethodGameGroupCreate = "game_group_create";

		internal const string MethodGameGroupJoin = "game_group_join";

		internal const string CancelledResponse = "{\"cancelled\":true}";

		internal const string FacebookConnectURL = "https://connect.discord.net";

		private const string AuthResponseKey = "authResponse";

		private string appId;

		private string appLinkUrl;

		private global::Discord.Unity.Canvas.ICanvasJSWrapper canvasJSWrapper;

		public override bool LimitEventUsage { get; set; }

		public override string SDKName
		{
			get
			{
				return "FBJSSDK";
			}
		}

		public override string SDKVersion
		{
			get
			{
				return canvasJSWrapper.GetSDKVersion();
			}
		}

		public override string SDKUserAgent
		{
			get
			{
				global::Discord.Unity.FacebookUnityPlatform currentPlatform = global::Discord.Unity.Constants.CurrentPlatform;
				string productName;
				if (currentPlatform == global::Discord.Unity.FacebookUnityPlatform.WebGL || currentPlatform == global::Discord.Unity.FacebookUnityPlatform.WebPlayer)
				{
					productName = string.Format(global::System.Globalization.CultureInfo.InvariantCulture, "FBUnity{0}", global::Discord.Unity.Constants.CurrentPlatform.ToString());
				}
				else
				{
					global::Discord.Unity.FacebookLogger.Warn("Currently running on uknown web platform");
					productName = "FBUnityWebUnknown";
				}
				return string.Format(global::System.Globalization.CultureInfo.InvariantCulture, "{0} {1}", base.SDKUserAgent, global::Discord.Unity.Utilities.GetUserAgent(productName, global::Discord.Unity.FacebookSdkVersion.Build));
			}
		}

		public CanvasFacebook()
			: this(new global::Discord.Unity.Canvas.CanvasJSWrapper(), new global::Discord.Unity.CallbackManager())
		{
		}

		public CanvasFacebook(global::Discord.Unity.Canvas.ICanvasJSWrapper canvasJSWrapper, global::Discord.Unity.CallbackManager callbackManager)
			: base(callbackManager)
		{
			this.canvasJSWrapper = canvasJSWrapper;
		}

		public void Init(string appId, bool cookie, bool logging, bool status, bool xfbml, string channelUrl, string authResponse, bool frictionlessRequests, string javascriptSDKLocale, bool loadDebugJSSDK, global::Discord.Unity.HideUnityDelegate hideUnityDelegate, global::Discord.Unity.InitDelegate onInitComplete)
		{
			if (canvasJSWrapper.IntegrationMethodJs == null)
			{
				throw new global::System.Exception("Cannot initialize discord javascript");
			}
			base.Init(hideUnityDelegate, onInitComplete);
			canvasJSWrapper.ExternalEval(canvasJSWrapper.IntegrationMethodJs);
			this.appId = appId;
			bool flag = true;
			global::Discord.Unity.MethodArguments methodArguments = new global::Discord.Unity.MethodArguments();
			methodArguments.AddString("appId", appId);
			methodArguments.AddPrimative("cookie", cookie);
			methodArguments.AddPrimative("logging", logging);
			methodArguments.AddPrimative("status", status);
			methodArguments.AddPrimative("xfbml", xfbml);
			methodArguments.AddString("channelUrl", channelUrl);
			methodArguments.AddString("authResponse", authResponse);
			methodArguments.AddPrimative("frictionlessRequests", frictionlessRequests);
			methodArguments.AddString("version", global::Discord.Unity.FB.GraphApiVersion);
			canvasJSWrapper.ExternalCall("FBUnity.init", flag ? 1 : 0, "https://connect.discord.net", javascriptSDKLocale, loadDebugJSSDK ? 1 : 0, methodArguments.ToJsonString(), status ? 1 : 0);
		}

		public override void LogInWithPublishPermissions(global::System.Collections.Generic.IEnumerable<string> permissions, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.ILoginResult> callback)
		{
			canvasJSWrapper.DisableFullScreen();
			canvasJSWrapper.ExternalCall("FBUnity.login", permissions, base.CallbackManager.AddFacebookDelegate(callback));
		}

		public override void LogInWithReadPermissions(global::System.Collections.Generic.IEnumerable<string> permissions, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.ILoginResult> callback)
		{
			canvasJSWrapper.DisableFullScreen();
			canvasJSWrapper.ExternalCall("FBUnity.login", permissions, base.CallbackManager.AddFacebookDelegate(callback));
		}

		public override void LogOut()
		{
			base.LogOut();
			canvasJSWrapper.ExternalCall("FBUnity.logout");
		}

		public override void AppRequest(string message, global::Discord.Unity.OGActionType? actionType, string objectId, global::System.Collections.Generic.IEnumerable<string> to, global::System.Collections.Generic.IEnumerable<object> filters, global::System.Collections.Generic.IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAppRequestResult> callback)
		{
			ValidateAppRequestArgs(message, actionType, objectId, to, filters, excludeIds, maxRecipients, data, title, callback);
			global::Discord.Unity.MethodArguments methodArguments = new global::Discord.Unity.MethodArguments();
			methodArguments.AddString("message", message);
			methodArguments.AddCommaSeparatedList("to", to);
			methodArguments.AddString("action_type", (!actionType.HasValue) ? null : actionType.ToString());
			methodArguments.AddString("object_id", objectId);
			methodArguments.AddList("filters", filters);
			methodArguments.AddList("exclude_ids", excludeIds);
			methodArguments.AddNullablePrimitive("max_recipients", maxRecipients);
			methodArguments.AddString("data", data);
			methodArguments.AddString("title", title);
			global::Discord.Unity.Canvas.CanvasFacebook.CanvasUIMethodCall<global::Discord.Unity.IAppRequestResult> canvasUIMethodCall = new global::Discord.Unity.Canvas.CanvasFacebook.CanvasUIMethodCall<global::Discord.Unity.IAppRequestResult>(this, "apprequests", "OnAppRequestsComplete");
			canvasUIMethodCall.Callback = callback;
			canvasUIMethodCall.Call(methodArguments);
		}

		public override void ActivateApp(string appId)
		{
			canvasJSWrapper.ExternalCall("FBUnity.activateApp");
		}

		public override void ShareLink(global::System.Uri contentURL, string contentTitle, string contentDescription, global::System.Uri photoURL, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IShareResult> callback)
		{
			global::Discord.Unity.MethodArguments methodArguments = new global::Discord.Unity.MethodArguments();
			methodArguments.AddUri("link", contentURL);
			methodArguments.AddString("name", contentTitle);
			methodArguments.AddString("description", contentDescription);
			methodArguments.AddUri("picture", photoURL);
			global::Discord.Unity.Canvas.CanvasFacebook.CanvasUIMethodCall<global::Discord.Unity.IShareResult> canvasUIMethodCall = new global::Discord.Unity.Canvas.CanvasFacebook.CanvasUIMethodCall<global::Discord.Unity.IShareResult>(this, "feed", "OnShareLinkComplete");
			canvasUIMethodCall.Callback = callback;
			canvasUIMethodCall.Call(methodArguments);
		}

		public override void FeedShare(string toId, global::System.Uri link, string linkName, string linkCaption, string linkDescription, global::System.Uri picture, string mediaSource, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IShareResult> callback)
		{
			global::Discord.Unity.MethodArguments methodArguments = new global::Discord.Unity.MethodArguments();
			methodArguments.AddString("to", toId);
			methodArguments.AddUri("link", link);
			methodArguments.AddString("name", linkName);
			methodArguments.AddString("caption", linkCaption);
			methodArguments.AddString("description", linkDescription);
			methodArguments.AddUri("picture", picture);
			methodArguments.AddString("source", mediaSource);
			global::Discord.Unity.Canvas.CanvasFacebook.CanvasUIMethodCall<global::Discord.Unity.IShareResult> canvasUIMethodCall = new global::Discord.Unity.Canvas.CanvasFacebook.CanvasUIMethodCall<global::Discord.Unity.IShareResult>(this, "feed", "OnShareLinkComplete");
			canvasUIMethodCall.Callback = callback;
			canvasUIMethodCall.Call(methodArguments);
		}

		public void Pay(string product, string action, int quantity, int? quantityMin, int? quantityMax, string requestId, string pricepointId, string testCurrency, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IPayResult> callback)
		{
			PayImpl(product, action, quantity, quantityMin, quantityMax, requestId, pricepointId, testCurrency, callback);
		}

		public override void GameGroupCreate(string name, string description, string privacy, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IGroupCreateResult> callback)
		{
			global::Discord.Unity.MethodArguments methodArguments = new global::Discord.Unity.MethodArguments();
			methodArguments.AddString("name", name);
			methodArguments.AddString("description", description);
			methodArguments.AddString("privacy", privacy);
			methodArguments.AddString("display", "async");
			global::Discord.Unity.Canvas.CanvasFacebook.CanvasUIMethodCall<global::Discord.Unity.IGroupCreateResult> canvasUIMethodCall = new global::Discord.Unity.Canvas.CanvasFacebook.CanvasUIMethodCall<global::Discord.Unity.IGroupCreateResult>(this, "game_group_create", "OnGroupCreateComplete");
			canvasUIMethodCall.Callback = callback;
			canvasUIMethodCall.Call(methodArguments);
		}

		public override void GameGroupJoin(string id, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IGroupJoinResult> callback)
		{
			global::Discord.Unity.MethodArguments methodArguments = new global::Discord.Unity.MethodArguments();
			methodArguments.AddString("id", id);
			methodArguments.AddString("display", "async");
			global::Discord.Unity.Canvas.CanvasFacebook.CanvasUIMethodCall<global::Discord.Unity.IGroupJoinResult> canvasUIMethodCall = new global::Discord.Unity.Canvas.CanvasFacebook.CanvasUIMethodCall<global::Discord.Unity.IGroupJoinResult>(this, "game_group_join", "OnJoinGroupComplete");
			canvasUIMethodCall.Callback = callback;
			canvasUIMethodCall.Call(methodArguments);
		}

		public override void GetAppLink(global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IAppLinkResult> callback)
		{
			global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
			dictionary.Add("url", appLinkUrl);
			global::System.Collections.Generic.Dictionary<string, object> dictionary2 = dictionary;
			callback(new global::Discord.Unity.AppLinkResult(new global::Discord.Unity.ResultContainer(dictionary2)));
			appLinkUrl = string.Empty;
		}

		public override void AppEventsLogEvent(string logEvent, float? valueToSum, global::System.Collections.Generic.Dictionary<string, object> parameters)
		{
			canvasJSWrapper.ExternalCall("FBUnity.logAppEvent", logEvent, valueToSum, global::Discord.MiniJSON.Json.Serialize(parameters));
		}

		public override void AppEventsLogPurchase(float logPurchase, string currency, global::System.Collections.Generic.Dictionary<string, object> parameters)
		{
			canvasJSWrapper.ExternalCall("FBUnity.logPurchase", logPurchase, currency, global::Discord.MiniJSON.Json.Serialize(parameters));
		}

		public override void OnLoginComplete(global::Discord.Unity.ResultContainer result)
		{
			FormatAuthResponse(result, delegate(global::Discord.Unity.ResultContainer formattedResponse)
			{
				OnAuthResponse(new global::Discord.Unity.LoginResult(formattedResponse));
			});
		}

		public override void OnGetAppLinkComplete(global::Discord.Unity.ResultContainer message)
		{
			throw new global::System.NotImplementedException();
		}

		public void OnFacebookAuthResponseChange(string responseJsonData)
		{
			OnFacebookAuthResponseChange(new global::Discord.Unity.ResultContainer(responseJsonData));
		}

		public void OnFacebookAuthResponseChange(global::Discord.Unity.ResultContainer resultContainer)
		{
			FormatAuthResponse(resultContainer, delegate(global::Discord.Unity.ResultContainer formattedResponse)
			{
				global::Discord.Unity.LoginResult loginResult = new global::Discord.Unity.LoginResult(formattedResponse);
				global::Discord.Unity.AccessToken.CurrentAccessToken = loginResult.AccessToken;
			});
		}

		public void OnPayComplete(string responseJsonData)
		{
			OnPayComplete(new global::Discord.Unity.ResultContainer(responseJsonData));
		}

		public void OnPayComplete(global::Discord.Unity.ResultContainer resultContainer)
		{
			global::Discord.Unity.PayResult result = new global::Discord.Unity.PayResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public override void OnAppRequestsComplete(global::Discord.Unity.ResultContainer resultContainer)
		{
			global::Discord.Unity.AppRequestResult result = new global::Discord.Unity.AppRequestResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public override void OnShareLinkComplete(global::Discord.Unity.ResultContainer resultContainer)
		{
			global::Discord.Unity.ShareResult result = new global::Discord.Unity.ShareResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public override void OnGroupCreateComplete(global::Discord.Unity.ResultContainer resultContainer)
		{
			global::Discord.Unity.GroupCreateResult result = new global::Discord.Unity.GroupCreateResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public override void OnGroupJoinComplete(global::Discord.Unity.ResultContainer resultContainer)
		{
			global::Discord.Unity.GroupJoinResult result = new global::Discord.Unity.GroupJoinResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public void OnUrlResponse(string url)
		{
			appLinkUrl = url;
		}

		private static void FormatAuthResponse(global::Discord.Unity.ResultContainer result, global::Discord.Unity.Utilities.Callback<global::Discord.Unity.ResultContainer> callback)
		{
			if (result.ResultDictionary == null)
			{
				callback(result);
				return;
			}
			global::System.Collections.Generic.IDictionary<string, object> value;
			if (result.ResultDictionary.TryGetValue<global::System.Collections.Generic.IDictionary<string, object>>("authResponse", out value))
			{
				result.ResultDictionary.Remove("authResponse");
				foreach (global::System.Collections.Generic.KeyValuePair<string, object> item in value)
				{
					result.ResultDictionary[item.Key] = item.Value;
				}
			}
			if (result.ResultDictionary.ContainsKey(global::Discord.Unity.LoginResult.AccessTokenKey) && !result.ResultDictionary.ContainsKey(global::Discord.Unity.LoginResult.PermissionsKey))
			{
				global::System.Collections.Generic.Dictionary<string, string> dictionary = new global::System.Collections.Generic.Dictionary<string, string>();
				dictionary.Add("fields", "permissions");
				dictionary.Add("access_token", (string)result.ResultDictionary[global::Discord.Unity.LoginResult.AccessTokenKey]);
				global::System.Collections.Generic.Dictionary<string, string> formData = dictionary;
				global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IGraphResult> callback2 = delegate(global::Discord.Unity.IGraphResult r)
				{
					global::System.Collections.Generic.IDictionary<string, object> value2;
					if (r.ResultDictionary != null && r.ResultDictionary.TryGetValue<global::System.Collections.Generic.IDictionary<string, object>>("permissions", out value2))
					{
						global::System.Collections.Generic.IList<string> list = new global::System.Collections.Generic.List<string>();
						global::System.Collections.Generic.IList<object> value3;
						if (value2.TryGetValue<global::System.Collections.Generic.IList<object>>("data", out value3))
						{
							foreach (object item2 in value3)
							{
								global::System.Collections.Generic.IDictionary<string, object> dictionary2 = item2 as global::System.Collections.Generic.IDictionary<string, object>;
								if (dictionary2 != null)
								{
									string value4;
									if (dictionary2.TryGetValue<string>("status", out value4) && value4.Equals("granted", global::System.StringComparison.InvariantCultureIgnoreCase))
									{
										string value5;
										if (dictionary2.TryGetValue<string>("permission", out value5))
										{
											list.Add(value5);
										}
										else
										{
											global::Discord.Unity.FacebookLogger.Warn("Didn't find permission name");
										}
									}
									else
									{
										global::Discord.Unity.FacebookLogger.Warn("Didn't find status in permissions result");
									}
								}
								else
								{
									global::Discord.Unity.FacebookLogger.Warn("Failed to case permission dictionary");
								}
							}
						}
						else
						{
							global::Discord.Unity.FacebookLogger.Warn("Failed to extract data from permissions");
						}
						result.ResultDictionary[global::Discord.Unity.LoginResult.PermissionsKey] = list.ToCommaSeparateList();
					}
					else
					{
						global::Discord.Unity.FacebookLogger.Warn("Failed to load permissions for access token");
					}
					callback(result);
				};
				global::Discord.Unity.FB.API("me", global::Discord.Unity.HttpMethod.GET, callback2, formData);
			}
			else
			{
				callback(result);
			}
		}

		private void PayImpl(string product, string action, int quantity, int? quantityMin, int? quantityMax, string requestId, string pricepointId, string testCurrency, global::Discord.Unity.FacebookDelegate<global::Discord.Unity.IPayResult> callback)
		{
			global::Discord.Unity.MethodArguments methodArguments = new global::Discord.Unity.MethodArguments();
			methodArguments.AddString("product", product);
			methodArguments.AddString("action", action);
			methodArguments.AddPrimative("quantity", quantity);
			methodArguments.AddNullablePrimitive("quantity_min", quantityMin);
			methodArguments.AddNullablePrimitive("quantity_max", quantityMax);
			methodArguments.AddString("request_id", requestId);
			methodArguments.AddString("pricepoint_id", pricepointId);
			methodArguments.AddString("test_currency", testCurrency);
			global::Discord.Unity.Canvas.CanvasFacebook.CanvasUIMethodCall<global::Discord.Unity.IPayResult> canvasUIMethodCall = new global::Discord.Unity.Canvas.CanvasFacebook.CanvasUIMethodCall<global::Discord.Unity.IPayResult>(this, "pay", "OnPayComplete");
			canvasUIMethodCall.Callback = callback;
			canvasUIMethodCall.Call(methodArguments);
		}
	}
}

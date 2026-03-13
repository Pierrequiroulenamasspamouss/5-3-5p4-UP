namespace Facebook.Unity.Canvas
{
	internal sealed class CanvasFacebook : global::Facebook.Unity.FacebookBase, global::Facebook.Unity.Canvas.ICanvasFacebook, global::Facebook.Unity.Canvas.ICanvasFacebookImplementation, global::Facebook.Unity.Canvas.ICanvasFacebookResultHandler, global::Facebook.Unity.IFacebook, global::Facebook.Unity.IFacebookResultHandler, global::Facebook.Unity.IPayFacebook
	{
		private class CanvasUIMethodCall<T> : global::Facebook.Unity.MethodCall<T> where T : global::Facebook.Unity.IResult
		{
			private global::Facebook.Unity.Canvas.CanvasFacebook canvasImpl;

			private string callbackMethod;

			public CanvasUIMethodCall(global::Facebook.Unity.Canvas.CanvasFacebook canvasImpl, string methodName, string callbackMethod)
				: base((global::Facebook.Unity.FacebookBase)canvasImpl, methodName)
			{
				this.canvasImpl = canvasImpl;
				this.callbackMethod = callbackMethod;
			}

			public override void Call(global::Facebook.Unity.MethodArguments args)
			{
				UI(base.MethodName, args, base.Callback);
			}

			private void UI(string method, global::Facebook.Unity.MethodArguments args, global::Facebook.Unity.FacebookDelegate<T> callback = null)
			{
				canvasImpl.canvasJSWrapper.DisableFullScreen();
				global::Facebook.Unity.MethodArguments methodArguments = new global::Facebook.Unity.MethodArguments(args);
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

		internal const string FacebookConnectURL = "https://connect.facebook.net";

		private const string AuthResponseKey = "authResponse";

		private string appId;

		private string appLinkUrl;

		private global::Facebook.Unity.Canvas.ICanvasJSWrapper canvasJSWrapper;

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
				global::Facebook.Unity.FacebookUnityPlatform currentPlatform = global::Facebook.Unity.Constants.CurrentPlatform;
				string productName;
				if (currentPlatform == global::Facebook.Unity.FacebookUnityPlatform.WebGL || currentPlatform == global::Facebook.Unity.FacebookUnityPlatform.WebPlayer)
				{
					productName = string.Format(global::System.Globalization.CultureInfo.InvariantCulture, "FBUnity{0}", global::Facebook.Unity.Constants.CurrentPlatform.ToString());
				}
				else
				{
					global::Facebook.Unity.FacebookLogger.Warn("Currently running on uknown web platform");
					productName = "FBUnityWebUnknown";
				}
				return string.Format(global::System.Globalization.CultureInfo.InvariantCulture, "{0} {1}", base.SDKUserAgent, global::Facebook.Unity.Utilities.GetUserAgent(productName, global::Facebook.Unity.FacebookSdkVersion.Build));
			}
		}

		public CanvasFacebook()
			: this(new global::Facebook.Unity.Canvas.CanvasJSWrapper(), new global::Facebook.Unity.CallbackManager())
		{
		}

		public CanvasFacebook(global::Facebook.Unity.Canvas.ICanvasJSWrapper canvasJSWrapper, global::Facebook.Unity.CallbackManager callbackManager)
			: base(callbackManager)
		{
			this.canvasJSWrapper = canvasJSWrapper;
		}

		public void Init(string appId, bool cookie, bool logging, bool status, bool xfbml, string channelUrl, string authResponse, bool frictionlessRequests, string javascriptSDKLocale, bool loadDebugJSSDK, global::Facebook.Unity.HideUnityDelegate hideUnityDelegate, global::Facebook.Unity.InitDelegate onInitComplete)
		{
			if (canvasJSWrapper.IntegrationMethodJs == null)
			{
				throw new global::System.Exception("Cannot initialize facebook javascript");
			}
			base.Init(hideUnityDelegate, onInitComplete);
			canvasJSWrapper.ExternalEval(canvasJSWrapper.IntegrationMethodJs);
			this.appId = appId;
			bool flag = true;
			global::Facebook.Unity.MethodArguments methodArguments = new global::Facebook.Unity.MethodArguments();
			methodArguments.AddString("appId", appId);
			methodArguments.AddPrimative("cookie", cookie);
			methodArguments.AddPrimative("logging", logging);
			methodArguments.AddPrimative("status", status);
			methodArguments.AddPrimative("xfbml", xfbml);
			methodArguments.AddString("channelUrl", channelUrl);
			methodArguments.AddString("authResponse", authResponse);
			methodArguments.AddPrimative("frictionlessRequests", frictionlessRequests);
			methodArguments.AddString("version", global::Facebook.Unity.FB.GraphApiVersion);
			canvasJSWrapper.ExternalCall("FBUnity.init", flag ? 1 : 0, "https://connect.facebook.net", javascriptSDKLocale, loadDebugJSSDK ? 1 : 0, methodArguments.ToJsonString(), status ? 1 : 0);
		}

		public override void LogInWithPublishPermissions(global::System.Collections.Generic.IEnumerable<string> permissions, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.ILoginResult> callback)
		{
			canvasJSWrapper.DisableFullScreen();
			canvasJSWrapper.ExternalCall("FBUnity.login", permissions, base.CallbackManager.AddFacebookDelegate(callback));
		}

		public override void LogInWithReadPermissions(global::System.Collections.Generic.IEnumerable<string> permissions, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.ILoginResult> callback)
		{
			canvasJSWrapper.DisableFullScreen();
			canvasJSWrapper.ExternalCall("FBUnity.login", permissions, base.CallbackManager.AddFacebookDelegate(callback));
		}

		public override void LogOut()
		{
			base.LogOut();
			canvasJSWrapper.ExternalCall("FBUnity.logout");
		}

		public override void AppRequest(string message, global::Facebook.Unity.OGActionType? actionType, string objectId, global::System.Collections.Generic.IEnumerable<string> to, global::System.Collections.Generic.IEnumerable<object> filters, global::System.Collections.Generic.IEnumerable<string> excludeIds, int? maxRecipients, string data, string title, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppRequestResult> callback)
		{
			ValidateAppRequestArgs(message, actionType, objectId, to, filters, excludeIds, maxRecipients, data, title, callback);
			global::Facebook.Unity.MethodArguments methodArguments = new global::Facebook.Unity.MethodArguments();
			methodArguments.AddString("message", message);
			methodArguments.AddCommaSeparatedList("to", to);
			methodArguments.AddString("action_type", (!actionType.HasValue) ? null : actionType.ToString());
			methodArguments.AddString("object_id", objectId);
			methodArguments.AddList("filters", filters);
			methodArguments.AddList("exclude_ids", excludeIds);
			methodArguments.AddNullablePrimitive("max_recipients", maxRecipients);
			methodArguments.AddString("data", data);
			methodArguments.AddString("title", title);
			global::Facebook.Unity.Canvas.CanvasFacebook.CanvasUIMethodCall<global::Facebook.Unity.IAppRequestResult> canvasUIMethodCall = new global::Facebook.Unity.Canvas.CanvasFacebook.CanvasUIMethodCall<global::Facebook.Unity.IAppRequestResult>(this, "apprequests", "OnAppRequestsComplete");
			canvasUIMethodCall.Callback = callback;
			canvasUIMethodCall.Call(methodArguments);
		}

		public override void ActivateApp(string appId)
		{
			canvasJSWrapper.ExternalCall("FBUnity.activateApp");
		}

		public override void ShareLink(global::System.Uri contentURL, string contentTitle, string contentDescription, global::System.Uri photoURL, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IShareResult> callback)
		{
			global::Facebook.Unity.MethodArguments methodArguments = new global::Facebook.Unity.MethodArguments();
			methodArguments.AddUri("link", contentURL);
			methodArguments.AddString("name", contentTitle);
			methodArguments.AddString("description", contentDescription);
			methodArguments.AddUri("picture", photoURL);
			global::Facebook.Unity.Canvas.CanvasFacebook.CanvasUIMethodCall<global::Facebook.Unity.IShareResult> canvasUIMethodCall = new global::Facebook.Unity.Canvas.CanvasFacebook.CanvasUIMethodCall<global::Facebook.Unity.IShareResult>(this, "feed", "OnShareLinkComplete");
			canvasUIMethodCall.Callback = callback;
			canvasUIMethodCall.Call(methodArguments);
		}

		public override void FeedShare(string toId, global::System.Uri link, string linkName, string linkCaption, string linkDescription, global::System.Uri picture, string mediaSource, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IShareResult> callback)
		{
			global::Facebook.Unity.MethodArguments methodArguments = new global::Facebook.Unity.MethodArguments();
			methodArguments.AddString("to", toId);
			methodArguments.AddUri("link", link);
			methodArguments.AddString("name", linkName);
			methodArguments.AddString("caption", linkCaption);
			methodArguments.AddString("description", linkDescription);
			methodArguments.AddUri("picture", picture);
			methodArguments.AddString("source", mediaSource);
			global::Facebook.Unity.Canvas.CanvasFacebook.CanvasUIMethodCall<global::Facebook.Unity.IShareResult> canvasUIMethodCall = new global::Facebook.Unity.Canvas.CanvasFacebook.CanvasUIMethodCall<global::Facebook.Unity.IShareResult>(this, "feed", "OnShareLinkComplete");
			canvasUIMethodCall.Callback = callback;
			canvasUIMethodCall.Call(methodArguments);
		}

		public void Pay(string product, string action, int quantity, int? quantityMin, int? quantityMax, string requestId, string pricepointId, string testCurrency, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IPayResult> callback)
		{
			PayImpl(product, action, quantity, quantityMin, quantityMax, requestId, pricepointId, testCurrency, callback);
		}

		public override void GameGroupCreate(string name, string description, string privacy, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IGroupCreateResult> callback)
		{
			global::Facebook.Unity.MethodArguments methodArguments = new global::Facebook.Unity.MethodArguments();
			methodArguments.AddString("name", name);
			methodArguments.AddString("description", description);
			methodArguments.AddString("privacy", privacy);
			methodArguments.AddString("display", "async");
			global::Facebook.Unity.Canvas.CanvasFacebook.CanvasUIMethodCall<global::Facebook.Unity.IGroupCreateResult> canvasUIMethodCall = new global::Facebook.Unity.Canvas.CanvasFacebook.CanvasUIMethodCall<global::Facebook.Unity.IGroupCreateResult>(this, "game_group_create", "OnGroupCreateComplete");
			canvasUIMethodCall.Callback = callback;
			canvasUIMethodCall.Call(methodArguments);
		}

		public override void GameGroupJoin(string id, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IGroupJoinResult> callback)
		{
			global::Facebook.Unity.MethodArguments methodArguments = new global::Facebook.Unity.MethodArguments();
			methodArguments.AddString("id", id);
			methodArguments.AddString("display", "async");
			global::Facebook.Unity.Canvas.CanvasFacebook.CanvasUIMethodCall<global::Facebook.Unity.IGroupJoinResult> canvasUIMethodCall = new global::Facebook.Unity.Canvas.CanvasFacebook.CanvasUIMethodCall<global::Facebook.Unity.IGroupJoinResult>(this, "game_group_join", "OnJoinGroupComplete");
			canvasUIMethodCall.Callback = callback;
			canvasUIMethodCall.Call(methodArguments);
		}

		public override void GetAppLink(global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IAppLinkResult> callback)
		{
			global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
			dictionary.Add("url", appLinkUrl);
			global::System.Collections.Generic.Dictionary<string, object> dictionary2 = dictionary;
			callback(new global::Facebook.Unity.AppLinkResult(new global::Facebook.Unity.ResultContainer(dictionary2)));
			appLinkUrl = string.Empty;
		}

		public override void AppEventsLogEvent(string logEvent, float? valueToSum, global::System.Collections.Generic.Dictionary<string, object> parameters)
		{
			canvasJSWrapper.ExternalCall("FBUnity.logAppEvent", logEvent, valueToSum, global::Facebook.MiniJSON.Json.Serialize(parameters));
		}

		public override void AppEventsLogPurchase(float logPurchase, string currency, global::System.Collections.Generic.Dictionary<string, object> parameters)
		{
			canvasJSWrapper.ExternalCall("FBUnity.logPurchase", logPurchase, currency, global::Facebook.MiniJSON.Json.Serialize(parameters));
		}

		public override void OnLoginComplete(global::Facebook.Unity.ResultContainer result)
		{
			FormatAuthResponse(result, delegate(global::Facebook.Unity.ResultContainer formattedResponse)
			{
				OnAuthResponse(new global::Facebook.Unity.LoginResult(formattedResponse));
			});
		}

		public override void OnGetAppLinkComplete(global::Facebook.Unity.ResultContainer message)
		{
			throw new global::System.NotImplementedException();
		}

		public void OnFacebookAuthResponseChange(string responseJsonData)
		{
			OnFacebookAuthResponseChange(new global::Facebook.Unity.ResultContainer(responseJsonData));
		}

		public void OnFacebookAuthResponseChange(global::Facebook.Unity.ResultContainer resultContainer)
		{
			FormatAuthResponse(resultContainer, delegate(global::Facebook.Unity.ResultContainer formattedResponse)
			{
				global::Facebook.Unity.LoginResult loginResult = new global::Facebook.Unity.LoginResult(formattedResponse);
				global::Facebook.Unity.AccessToken.CurrentAccessToken = loginResult.AccessToken;
			});
		}

		public void OnPayComplete(string responseJsonData)
		{
			OnPayComplete(new global::Facebook.Unity.ResultContainer(responseJsonData));
		}

		public void OnPayComplete(global::Facebook.Unity.ResultContainer resultContainer)
		{
			global::Facebook.Unity.PayResult result = new global::Facebook.Unity.PayResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public override void OnAppRequestsComplete(global::Facebook.Unity.ResultContainer resultContainer)
		{
			global::Facebook.Unity.AppRequestResult result = new global::Facebook.Unity.AppRequestResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public override void OnShareLinkComplete(global::Facebook.Unity.ResultContainer resultContainer)
		{
			global::Facebook.Unity.ShareResult result = new global::Facebook.Unity.ShareResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public override void OnGroupCreateComplete(global::Facebook.Unity.ResultContainer resultContainer)
		{
			global::Facebook.Unity.GroupCreateResult result = new global::Facebook.Unity.GroupCreateResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public override void OnGroupJoinComplete(global::Facebook.Unity.ResultContainer resultContainer)
		{
			global::Facebook.Unity.GroupJoinResult result = new global::Facebook.Unity.GroupJoinResult(resultContainer);
			base.CallbackManager.OnFacebookResponse(result);
		}

		public void OnUrlResponse(string url)
		{
			appLinkUrl = url;
		}

		private static void FormatAuthResponse(global::Facebook.Unity.ResultContainer result, global::Facebook.Unity.Utilities.Callback<global::Facebook.Unity.ResultContainer> callback)
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
			if (result.ResultDictionary.ContainsKey(global::Facebook.Unity.LoginResult.AccessTokenKey) && !result.ResultDictionary.ContainsKey(global::Facebook.Unity.LoginResult.PermissionsKey))
			{
				global::System.Collections.Generic.Dictionary<string, string> dictionary = new global::System.Collections.Generic.Dictionary<string, string>();
				dictionary.Add("fields", "permissions");
				dictionary.Add("access_token", (string)result.ResultDictionary[global::Facebook.Unity.LoginResult.AccessTokenKey]);
				global::System.Collections.Generic.Dictionary<string, string> formData = dictionary;
				global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IGraphResult> callback2 = delegate(global::Facebook.Unity.IGraphResult r)
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
											global::Facebook.Unity.FacebookLogger.Warn("Didn't find permission name");
										}
									}
									else
									{
										global::Facebook.Unity.FacebookLogger.Warn("Didn't find status in permissions result");
									}
								}
								else
								{
									global::Facebook.Unity.FacebookLogger.Warn("Failed to case permission dictionary");
								}
							}
						}
						else
						{
							global::Facebook.Unity.FacebookLogger.Warn("Failed to extract data from permissions");
						}
						result.ResultDictionary[global::Facebook.Unity.LoginResult.PermissionsKey] = list.ToCommaSeparateList();
					}
					else
					{
						global::Facebook.Unity.FacebookLogger.Warn("Failed to load permissions for access token");
					}
					callback(result);
				};
				global::Facebook.Unity.FB.API("me", global::Facebook.Unity.HttpMethod.GET, callback2, formData);
			}
			else
			{
				callback(result);
			}
		}

		private void PayImpl(string product, string action, int quantity, int? quantityMin, int? quantityMax, string requestId, string pricepointId, string testCurrency, global::Facebook.Unity.FacebookDelegate<global::Facebook.Unity.IPayResult> callback)
		{
			global::Facebook.Unity.MethodArguments methodArguments = new global::Facebook.Unity.MethodArguments();
			methodArguments.AddString("product", product);
			methodArguments.AddString("action", action);
			methodArguments.AddPrimative("quantity", quantity);
			methodArguments.AddNullablePrimitive("quantity_min", quantityMin);
			methodArguments.AddNullablePrimitive("quantity_max", quantityMax);
			methodArguments.AddString("request_id", requestId);
			methodArguments.AddString("pricepoint_id", pricepointId);
			methodArguments.AddString("test_currency", testCurrency);
			global::Facebook.Unity.Canvas.CanvasFacebook.CanvasUIMethodCall<global::Facebook.Unity.IPayResult> canvasUIMethodCall = new global::Facebook.Unity.Canvas.CanvasFacebook.CanvasUIMethodCall<global::Facebook.Unity.IPayResult>(this, "pay", "OnPayComplete");
			canvasUIMethodCall.Callback = callback;
			canvasUIMethodCall.Call(methodArguments);
		}
	}
}

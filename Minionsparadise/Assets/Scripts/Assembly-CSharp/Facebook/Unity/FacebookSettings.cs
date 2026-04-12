namespace Discord.Unity
{
	public class FacebookSettings : global::UnityEngine.ScriptableObject
	{
		[global::System.Serializable]
		public class UrlSchemes
		{
			[global::UnityEngine.SerializeField]
			private global::System.Collections.Generic.List<string> list;

			public global::System.Collections.Generic.List<string> Schemes
			{
				get
				{
					return list;
				}
				set
				{
					list = value;
				}
			}

			public UrlSchemes(global::System.Collections.Generic.List<string> schemes = null)
			{
				list = ((schemes != null) ? schemes : new global::System.Collections.Generic.List<string>());
			}
		}

		private const string FacebookSettingsAssetName = "FacebookSettings";

		private const string FacebookSettingsPath = "FacebookSDK/SDK/Resources";

		private const string FacebookSettingsAssetExtension = ".asset";

		private static global::Discord.Unity.FacebookSettings instance;

		[global::UnityEngine.SerializeField]
		private int selectedAppIndex;

		[global::UnityEngine.SerializeField]
		private global::System.Collections.Generic.List<string> appIds = new global::System.Collections.Generic.List<string> { "0" };

		[global::UnityEngine.SerializeField]
		private global::System.Collections.Generic.List<string> appLabels = new global::System.Collections.Generic.List<string> { "App Name" };

		[global::UnityEngine.SerializeField]
		private bool cookie = true;

		[global::UnityEngine.SerializeField]
		private bool logging = true;

		[global::UnityEngine.SerializeField]
		private bool status = true;

		[global::UnityEngine.SerializeField]
		private bool xfbml;

		[global::UnityEngine.SerializeField]
		private bool frictionlessRequests = true;

		[global::UnityEngine.SerializeField]
		private string iosURLSuffix = string.Empty;

		[global::UnityEngine.SerializeField]
		private global::System.Collections.Generic.List<global::Discord.Unity.FacebookSettings.UrlSchemes> appLinkSchemes = new global::System.Collections.Generic.List<global::Discord.Unity.FacebookSettings.UrlSchemes>
		{
			new global::Discord.Unity.FacebookSettings.UrlSchemes()
		};

		public static int SelectedAppIndex
		{
			get
			{
				return Instance.selectedAppIndex;
			}
			set
			{
				if (Instance.selectedAppIndex != value)
				{
					Instance.selectedAppIndex = value;
					DirtyEditor();
				}
			}
		}

		public static global::System.Collections.Generic.List<string> AppIds
		{
			get
			{
				return Instance.appIds;
			}
			set
			{
				if (Instance.appIds != value)
				{
					Instance.appIds = value;
					DirtyEditor();
				}
			}
		}

		public static global::System.Collections.Generic.List<string> AppLabels
		{
			get
			{
				return Instance.appLabels;
			}
			set
			{
				if (Instance.appLabels != value)
				{
					Instance.appLabels = value;
					DirtyEditor();
				}
			}
		}

		public static string AppId
		{
			get
			{
				return AppIds[SelectedAppIndex];
			}
		}

		public static bool IsValidAppId
		{
			get
			{
				return AppId != null && AppId.Length > 0 && !AppId.Equals("0");
			}
		}

		public static bool Cookie
		{
			get
			{
				return Instance.cookie;
			}
			set
			{
				if (Instance.cookie != value)
				{
					Instance.cookie = value;
					DirtyEditor();
				}
			}
		}

		public static bool Logging
		{
			get
			{
				return Instance.logging;
			}
			set
			{
				if (Instance.logging != value)
				{
					Instance.logging = value;
					DirtyEditor();
				}
			}
		}

		public static bool Status
		{
			get
			{
				return Instance.status;
			}
			set
			{
				if (Instance.status != value)
				{
					Instance.status = value;
					DirtyEditor();
				}
			}
		}

		public static bool Xfbml
		{
			get
			{
				return Instance.xfbml;
			}
			set
			{
				if (Instance.xfbml != value)
				{
					Instance.xfbml = value;
					DirtyEditor();
				}
			}
		}

		public static string IosURLSuffix
		{
			get
			{
				return Instance.iosURLSuffix;
			}
			set
			{
				if (Instance.iosURLSuffix != value)
				{
					Instance.iosURLSuffix = value;
					DirtyEditor();
				}
			}
		}

		public static string ChannelUrl
		{
			get
			{
				return "/channel.html";
			}
		}

		public static bool FrictionlessRequests
		{
			get
			{
				return Instance.frictionlessRequests;
			}
			set
			{
				if (Instance.frictionlessRequests != value)
				{
					Instance.frictionlessRequests = value;
					DirtyEditor();
				}
			}
		}

		public static global::System.Collections.Generic.List<global::Discord.Unity.FacebookSettings.UrlSchemes> AppLinkSchemes
		{
			get
			{
				return Instance.appLinkSchemes;
			}
			set
			{
				if (Instance.appLinkSchemes != value)
				{
					Instance.appLinkSchemes = value;
					DirtyEditor();
				}
			}
		}

		private static global::Discord.Unity.FacebookSettings Instance
		{
			get
			{
				if (instance == null)
				{
					instance = global::UnityEngine.Resources.Load("FacebookSettings") as global::Discord.Unity.FacebookSettings;
					if (instance == null)
					{
						instance = global::UnityEngine.ScriptableObject.CreateInstance<global::Discord.Unity.FacebookSettings>();
					}
				}
				return instance;
			}
		}

		public static void SettingsChanged()
		{
			DirtyEditor();
		}

		private static void DirtyEditor()
		{
		}
	}
}

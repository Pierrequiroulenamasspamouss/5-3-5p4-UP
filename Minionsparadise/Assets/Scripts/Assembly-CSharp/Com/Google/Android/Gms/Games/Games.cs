namespace Com.Google.Android.Gms.Games
{
	public class Games : global::Google.Developers.JavaObjWrapper
	{
		private const string CLASS_NAME = "com/google/android/gms/games/Games";

		public static string EXTRA_PLAYER_IDS
		{
			get
			{
				return global::Google.Developers.JavaObjWrapper.GetStaticStringField("com/google/android/gms/games/Games", "EXTRA_PLAYER_IDS");
			}
		}

		public static string EXTRA_STATUS
		{
			get
			{
				return global::Google.Developers.JavaObjWrapper.GetStaticStringField("com/google/android/gms/games/Games", "EXTRA_STATUS");
			}
		}

		public static object SCOPE_GAMES
		{
			get
			{
				return global::Google.Developers.JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "SCOPE_GAMES", "Lcom/google/android/gms/common/api/Scope;");
			}
		}

		public static object API
		{
			get
			{
				return global::Google.Developers.JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "API", "Lcom/google/android/gms/common/api/Api;");
			}
		}

		public static object GamesMetadata
		{
			get
			{
				return global::Google.Developers.JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "GamesMetadata", "Lcom/google/android/gms/games/GamesMetadata;");
			}
		}

		public static object Achievements
		{
			get
			{
				return global::Google.Developers.JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "Achievements", "Lcom/google/android/gms/games/achievement/Achievements;");
			}
		}

		public static object Events
		{
			get
			{
				return global::Google.Developers.JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "Events", "Lcom/google/android/gms/games/event/Events;");
			}
		}

		public static object Leaderboards
		{
			get
			{
				return global::Google.Developers.JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "Leaderboards", "Lcom/google/android/gms/games/leaderboard/Leaderboards;");
			}
		}

		public static object Invitations
		{
			get
			{
				return global::Google.Developers.JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "Invitations", "Lcom/google/android/gms/games/multiplayer/Invitations;");
			}
		}

		public static object TurnBasedMultiplayer
		{
			get
			{
				return global::Google.Developers.JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "TurnBasedMultiplayer", "Lcom/google/android/gms/games/multiplayer/turnbased/TurnBasedMultiplayer;");
			}
		}

		public static object RealTimeMultiplayer
		{
			get
			{
				return global::Google.Developers.JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "RealTimeMultiplayer", "Lcom/google/android/gms/games/multiplayer/realtime/RealTimeMultiplayer;");
			}
		}

		public static object Players
		{
			get
			{
				return global::Google.Developers.JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "Players", "Lcom/google/android/gms/games/Players;");
			}
		}

		public static object Notifications
		{
			get
			{
				return global::Google.Developers.JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "Notifications", "Lcom/google/android/gms/games/Notifications;");
			}
		}

		public static object Quests
		{
			get
			{
				return global::Google.Developers.JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "Quests", "Lcom/google/android/gms/games/quest/Quests;");
			}
		}

		public static object Requests
		{
			get
			{
				return global::Google.Developers.JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "Requests", "Lcom/google/android/gms/games/request/Requests;");
			}
		}

		public static object Snapshots
		{
			get
			{
				return global::Google.Developers.JavaObjWrapper.GetStaticObjectField<object>("com/google/android/gms/games/Games", "Snapshots", "Lcom/google/android/gms/games/snapshot/Snapshots;");
			}
		}

		public static global::Com.Google.Android.Gms.Games.Stats.StatsObject Stats
		{
			get
			{
				return global::Google.Developers.JavaObjWrapper.GetStaticObjectField<global::Com.Google.Android.Gms.Games.Stats.StatsObject>("com/google/android/gms/games/Games", "Stats", "Lcom/google/android/gms/games/stats/Stats;");
			}
		}

		public Games(global::System.IntPtr ptr)
			: base(ptr)
		{
		}

		public static string getAppId(global::Com.Google.Android.Gms.Common.Api.GoogleApiClient arg_GoogleApiClient_1)
		{
			return global::Google.Developers.JavaObjWrapper.StaticInvokeCall<string>("com/google/android/gms/games/Games", "getAppId", "(Lcom/google/android/gms/common/api/GoogleApiClient;)Ljava/lang/String;", new object[1] { arg_GoogleApiClient_1 });
		}

		public static string getCurrentAccountName(global::Com.Google.Android.Gms.Common.Api.GoogleApiClient arg_GoogleApiClient_1)
		{
			return global::Google.Developers.JavaObjWrapper.StaticInvokeCall<string>("com/google/android/gms/games/Games", "getCurrentAccountName", "(Lcom/google/android/gms/common/api/GoogleApiClient;)Ljava/lang/String;", new object[1] { arg_GoogleApiClient_1 });
		}

		public static int getSdkVariant(global::Com.Google.Android.Gms.Common.Api.GoogleApiClient arg_GoogleApiClient_1)
		{
			return global::Google.Developers.JavaObjWrapper.StaticInvokeCall<int>("com/google/android/gms/games/Games", "getSdkVariant", "(Lcom/google/android/gms/common/api/GoogleApiClient;)I", new object[1] { arg_GoogleApiClient_1 });
		}

		public static object getSettingsIntent(global::Com.Google.Android.Gms.Common.Api.GoogleApiClient arg_GoogleApiClient_1)
		{
			return global::Google.Developers.JavaObjWrapper.StaticInvokeCall<object>("com/google/android/gms/games/Games", "getSettingsIntent", "(Lcom/google/android/gms/common/api/GoogleApiClient;)Landroid/content/Intent;", new object[1] { arg_GoogleApiClient_1 });
		}

		public static void setGravityForPopups(global::Com.Google.Android.Gms.Common.Api.GoogleApiClient arg_GoogleApiClient_1, int arg_int_2)
		{
			global::Google.Developers.JavaObjWrapper.StaticInvokeCallVoid("com/google/android/gms/games/Games", "setGravityForPopups", "(Lcom/google/android/gms/common/api/GoogleApiClient;I)V", arg_GoogleApiClient_1, arg_int_2);
		}

		public static void setViewForPopups(global::Com.Google.Android.Gms.Common.Api.GoogleApiClient arg_GoogleApiClient_1, object arg_object_2)
		{
			global::Google.Developers.JavaObjWrapper.StaticInvokeCallVoid("com/google/android/gms/games/Games", "setViewForPopups", "(Lcom/google/android/gms/common/api/GoogleApiClient;Landroid/view/View;)V", arg_GoogleApiClient_1, arg_object_2);
		}

		public static global::Com.Google.Android.Gms.Common.Api.PendingResult<global::Com.Google.Android.Gms.Common.Api.Status> signOut(global::Com.Google.Android.Gms.Common.Api.GoogleApiClient arg_GoogleApiClient_1)
		{
			return global::Google.Developers.JavaObjWrapper.StaticInvokeCall<global::Com.Google.Android.Gms.Common.Api.PendingResult<global::Com.Google.Android.Gms.Common.Api.Status>>("com/google/android/gms/games/Games", "signOut", "(Lcom/google/android/gms/common/api/GoogleApiClient;)Lcom/google/android/gms/common/api/PendingResult;", new object[1] { arg_GoogleApiClient_1 });
		}
	}
}

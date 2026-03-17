namespace Com.Google.Android.Gms.Games.Stats
{
	public class StatsObject : global::Google.Developers.JavaObjWrapper, global::Com.Google.Android.Gms.Games.Stats.Stats
	{
		private const string CLASS_NAME = "com/google/android/gms/games/stats/Stats";

		public StatsObject(global::System.IntPtr ptr)
			: base(ptr)
		{
		}

		public global::Com.Google.Android.Gms.Common.Api.PendingResult<global::Com.Google.Android.Gms.Games.Stats.Stats_LoadPlayerStatsResultObject> loadPlayerStats(global::Com.Google.Android.Gms.Common.Api.GoogleApiClient arg_GoogleApiClient_1, bool arg_bool_2)
		{
			global::System.IntPtr ptr = InvokeCall<global::System.IntPtr>("loadPlayerStats", "(Lcom/google/android/gms/common/api/GoogleApiClient;Z)Lcom/google/android/gms/common/api/PendingResult;", new object[2] { arg_GoogleApiClient_1, arg_bool_2 });
			return new global::Com.Google.Android.Gms.Common.Api.PendingResult<global::Com.Google.Android.Gms.Games.Stats.Stats_LoadPlayerStatsResultObject>(ptr);
		}
	}
}

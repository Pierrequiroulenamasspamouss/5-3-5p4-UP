namespace Com.Google.Android.Gms.Games.Stats
{
	public class Stats_LoadPlayerStatsResultObject : global::Google.Developers.JavaObjWrapper, global::Com.Google.Android.Gms.Common.Api.Result, global::Com.Google.Android.Gms.Games.Stats.Stats_LoadPlayerStatsResult
	{
		private const string CLASS_NAME = "com/google/android/gms/games/stats/Stats$LoadPlayerStatsResult";

		public Stats_LoadPlayerStatsResultObject(global::System.IntPtr ptr)
			: base(ptr)
		{
		}

		public global::Com.Google.Android.Gms.Games.Stats.PlayerStats getPlayerStats()
		{
			global::System.IntPtr ptr = InvokeCall<global::System.IntPtr>("getPlayerStats", "()Lcom/google/android/gms/games/stats/PlayerStats;", new object[0]);
			return new global::Com.Google.Android.Gms.Games.Stats.PlayerStatsObject(ptr);
		}

		public global::Com.Google.Android.Gms.Common.Api.Status getStatus()
		{
			global::System.IntPtr ptr = InvokeCall<global::System.IntPtr>("getStatus", "()Lcom/google/android/gms/common/api/Status;", new object[0]);
			return new global::Com.Google.Android.Gms.Common.Api.Status(ptr);
		}
	}
}

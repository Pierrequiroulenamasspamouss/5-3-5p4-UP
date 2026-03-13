namespace GooglePlayGames.Native.PInvoke
{
	internal class NativePlayerStats : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal NativePlayerStats(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		internal bool Valid()
		{
			return global::GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_Valid(SelfPtr());
		}

		internal bool HasAverageSessionLength()
		{
			return global::GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasAverageSessionLength(SelfPtr());
		}

		internal float AverageSessionLength()
		{
			return global::GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_AverageSessionLength(SelfPtr());
		}

		internal bool HasChurnProbability()
		{
			return global::GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasChurnProbability(SelfPtr());
		}

		internal float ChurnProbability()
		{
			return global::GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_ChurnProbability(SelfPtr());
		}

		internal bool HasDaysSinceLastPlayed()
		{
			return global::GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasDaysSinceLastPlayed(SelfPtr());
		}

		internal int DaysSinceLastPlayed()
		{
			return global::GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_DaysSinceLastPlayed(SelfPtr());
		}

		internal bool HasNumberOfPurchases()
		{
			return global::GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasNumberOfPurchases(SelfPtr());
		}

		internal int NumberOfPurchases()
		{
			return global::GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_NumberOfPurchases(SelfPtr());
		}

		internal bool HasNumberOfSessions()
		{
			return global::GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasNumberOfSessions(SelfPtr());
		}

		internal int NumberOfSessions()
		{
			return global::GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_NumberOfSessions(SelfPtr());
		}

		internal bool HasSessionPercentile()
		{
			return global::GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasSessionPercentile(SelfPtr());
		}

		internal float SessionPercentile()
		{
			return global::GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_SessionPercentile(SelfPtr());
		}

		internal bool HasSpendPercentile()
		{
			return global::GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_HasSpendPercentile(SelfPtr());
		}

		internal float SpendPercentile()
		{
			return global::GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_SpendPercentile(SelfPtr());
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.PlayerStats.PlayerStats_Dispose(selfPointer);
		}

		internal global::GooglePlayGames.BasicApi.PlayerStats AsPlayerStats()
		{
			global::GooglePlayGames.BasicApi.PlayerStats playerStats = new global::GooglePlayGames.BasicApi.PlayerStats();
			playerStats.Valid = Valid();
			if (Valid())
			{
				playerStats.AvgSessonLength = AverageSessionLength();
				playerStats.ChurnProbability = ChurnProbability();
				playerStats.DaysSinceLastPlayed = DaysSinceLastPlayed();
				playerStats.NumberOfPurchases = NumberOfPurchases();
				playerStats.NumberOfSessions = NumberOfSessions();
				playerStats.SessPercentile = SessionPercentile();
				playerStats.SpendPercentile = SpendPercentile();
				playerStats.SpendProbability = -1f;
			}
			return playerStats;
		}
	}
}

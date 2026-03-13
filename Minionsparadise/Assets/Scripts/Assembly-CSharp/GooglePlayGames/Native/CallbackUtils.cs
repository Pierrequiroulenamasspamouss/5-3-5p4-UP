namespace GooglePlayGames.Native
{
	internal static class CallbackUtils
	{
		internal static global::System.Action<T> ToOnGameThread<T>(global::System.Action<T> toConvert)
		{
			if (toConvert == null)
			{
				return delegate
				{
				};
			}
			return delegate(T val)
			{
				global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
				{
					toConvert(val);
				});
			};
		}

		internal static global::System.Action<T1, T2> ToOnGameThread<T1, T2>(global::System.Action<T1, T2> toConvert)
		{
			if (toConvert == null)
			{
				return delegate
				{
				};
			}
			return delegate(T1 val1, T2 val2)
			{
				global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
				{
					toConvert(val1, val2);
				});
			};
		}

		internal static global::System.Action<T1, T2, T3> ToOnGameThread<T1, T2, T3>(global::System.Action<T1, T2, T3> toConvert)
		{
			if (toConvert == null)
			{
				return delegate
				{
				};
			}
			return delegate(T1 val1, T2 val2, T3 val3)
			{
				global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
				{
					toConvert(val1, val2, val3);
				});
			};
		}
	}
}

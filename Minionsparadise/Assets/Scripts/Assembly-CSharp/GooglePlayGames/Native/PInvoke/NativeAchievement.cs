namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeAchievement : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		private const ulong MinusOne = ulong.MaxValue;

		internal NativeAchievement(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		internal uint CurrentSteps()
		{
			return global::GooglePlayGames.Native.Cwrapper.Achievement.Achievement_CurrentSteps(SelfPtr());
		}

		internal string Description()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.Achievement.Achievement_Description(SelfPtr(), out_string, out_size));
		}

		internal string Id()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.Achievement.Achievement_Id(SelfPtr(), out_string, out_size));
		}

		internal string Name()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.Achievement.Achievement_Name(SelfPtr(), out_string, out_size));
		}

		internal global::GooglePlayGames.Native.Cwrapper.Types.AchievementState State()
		{
			return global::GooglePlayGames.Native.Cwrapper.Achievement.Achievement_State(SelfPtr());
		}

		internal uint TotalSteps()
		{
			return global::GooglePlayGames.Native.Cwrapper.Achievement.Achievement_TotalSteps(SelfPtr());
		}

		internal global::GooglePlayGames.Native.Cwrapper.Types.AchievementType Type()
		{
			return global::GooglePlayGames.Native.Cwrapper.Achievement.Achievement_Type(SelfPtr());
		}

		internal ulong LastModifiedTime()
		{
			if (global::GooglePlayGames.Native.Cwrapper.Achievement.Achievement_Valid(SelfPtr()))
			{
				return global::GooglePlayGames.Native.Cwrapper.Achievement.Achievement_LastModifiedTime(SelfPtr());
			}
			return 0uL;
		}

		internal ulong getXP()
		{
			return global::GooglePlayGames.Native.Cwrapper.Achievement.Achievement_XP(SelfPtr());
		}

		internal string getRevealedImageUrl()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.Achievement.Achievement_RevealedIconUrl(SelfPtr(), out_string, out_size));
		}

		internal string getUnlockedImageUrl()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.Achievement.Achievement_UnlockedIconUrl(SelfPtr(), out_string, out_size));
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.Achievement.Achievement_Dispose(selfPointer);
		}

		internal global::GooglePlayGames.BasicApi.Achievement AsAchievement()
		{
			global::GooglePlayGames.BasicApi.Achievement achievement = new global::GooglePlayGames.BasicApi.Achievement();
			achievement.Id = Id();
			achievement.Name = Name();
			achievement.Description = Description();
			global::System.DateTime dateTime = new global::System.DateTime(1970, 1, 1, 0, 0, 0, 0, global::System.DateTimeKind.Utc);
			ulong num = LastModifiedTime();
			if (num == ulong.MaxValue)
			{
				num = 0uL;
			}
			achievement.LastModifiedTime = dateTime.AddMilliseconds(num);
			achievement.Points = getXP();
			achievement.RevealedImageUrl = getRevealedImageUrl();
			achievement.UnlockedImageUrl = getUnlockedImageUrl();
			if (Type() == global::GooglePlayGames.Native.Cwrapper.Types.AchievementType.INCREMENTAL)
			{
				achievement.IsIncremental = true;
				achievement.CurrentSteps = (int)CurrentSteps();
				achievement.TotalSteps = (int)TotalSteps();
			}
			achievement.IsRevealed = State() == global::GooglePlayGames.Native.Cwrapper.Types.AchievementState.REVEALED || State() == global::GooglePlayGames.Native.Cwrapper.Types.AchievementState.UNLOCKED;
			achievement.IsUnlocked = State() == global::GooglePlayGames.Native.Cwrapper.Types.AchievementState.UNLOCKED;
			return achievement;
		}
	}
}

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeQuest : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder, global::GooglePlayGames.BasicApi.Quests.IQuest
	{
		private volatile global::GooglePlayGames.Native.PInvoke.NativeQuestMilestone mCachedMilestone;

		public string Id
		{
			get
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.Quest.Quest_Id(SelfPtr(), out_string, out_size));
			}
		}

		public string Name
		{
			get
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.Quest.Quest_Name(SelfPtr(), out_string, out_size));
			}
		}

		public string Description
		{
			get
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.Quest.Quest_Description(SelfPtr(), out_string, out_size));
			}
		}

		public string BannerUrl
		{
			get
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.Quest.Quest_BannerUrl(SelfPtr(), out_string, out_size));
			}
		}

		public string IconUrl
		{
			get
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.Quest.Quest_IconUrl(SelfPtr(), out_string, out_size));
			}
		}

		public global::System.DateTime StartTime
		{
			get
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.FromMillisSinceUnixEpoch(global::GooglePlayGames.Native.Cwrapper.Quest.Quest_StartTime(SelfPtr()));
			}
		}

		public global::System.DateTime ExpirationTime
		{
			get
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.FromMillisSinceUnixEpoch(global::GooglePlayGames.Native.Cwrapper.Quest.Quest_ExpirationTime(SelfPtr()));
			}
		}

		public global::System.DateTime? AcceptedTime
		{
			get
			{
				long num = global::GooglePlayGames.Native.Cwrapper.Quest.Quest_AcceptedTime(SelfPtr());
				if (num == 0L)
				{
					return null;
				}
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.FromMillisSinceUnixEpoch(num);
			}
		}

		public global::GooglePlayGames.BasicApi.Quests.IQuestMilestone Milestone
		{
			get
			{
				if (mCachedMilestone == null)
				{
					mCachedMilestone = global::GooglePlayGames.Native.PInvoke.NativeQuestMilestone.FromPointer(global::GooglePlayGames.Native.Cwrapper.Quest.Quest_CurrentMilestone(SelfPtr()));
				}
				return mCachedMilestone;
			}
		}

		public global::GooglePlayGames.BasicApi.Quests.QuestState State
		{
			get
			{
				global::GooglePlayGames.Native.Cwrapper.Types.QuestState questState = global::GooglePlayGames.Native.Cwrapper.Quest.Quest_State(SelfPtr());
				switch (questState)
				{
				case global::GooglePlayGames.Native.Cwrapper.Types.QuestState.UPCOMING:
					return global::GooglePlayGames.BasicApi.Quests.QuestState.Upcoming;
				case global::GooglePlayGames.Native.Cwrapper.Types.QuestState.OPEN:
					return global::GooglePlayGames.BasicApi.Quests.QuestState.Open;
				case global::GooglePlayGames.Native.Cwrapper.Types.QuestState.ACCEPTED:
					return global::GooglePlayGames.BasicApi.Quests.QuestState.Accepted;
				case global::GooglePlayGames.Native.Cwrapper.Types.QuestState.COMPLETED:
					return global::GooglePlayGames.BasicApi.Quests.QuestState.Completed;
				case global::GooglePlayGames.Native.Cwrapper.Types.QuestState.EXPIRED:
					return global::GooglePlayGames.BasicApi.Quests.QuestState.Expired;
				case global::GooglePlayGames.Native.Cwrapper.Types.QuestState.FAILED:
					return global::GooglePlayGames.BasicApi.Quests.QuestState.Failed;
				default:
					throw new global::System.InvalidOperationException("Unknown state: " + questState);
				}
			}
		}

		internal NativeQuest(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		internal bool Valid()
		{
			return global::GooglePlayGames.Native.Cwrapper.Quest.Quest_Valid(SelfPtr());
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.Quest.Quest_Dispose(selfPointer);
		}

		public override string ToString()
		{
			if (IsDisposed())
			{
				return "[NativeQuest: DELETED]";
			}
			return string.Format("[NativeQuest: Id={0}, Name={1}, Description={2}, BannerUrl={3}, IconUrl={4}, State={5}, StartTime={6}, ExpirationTime={7}, AcceptedTime={8}]", Id, Name, Description, BannerUrl, IconUrl, State, StartTime, ExpirationTime, AcceptedTime);
		}

		internal static global::GooglePlayGames.Native.PInvoke.NativeQuest FromPointer(global::System.IntPtr pointer)
		{
			if (pointer.Equals(global::System.IntPtr.Zero))
			{
				return null;
			}
			return new global::GooglePlayGames.Native.PInvoke.NativeQuest(pointer);
		}
	}
}

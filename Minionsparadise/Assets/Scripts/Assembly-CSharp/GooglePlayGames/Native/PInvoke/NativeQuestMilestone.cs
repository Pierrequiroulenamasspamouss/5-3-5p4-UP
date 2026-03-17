namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeQuestMilestone : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder, global::GooglePlayGames.BasicApi.Quests.IQuestMilestone
	{
		public string Id
		{
			get
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.QuestMilestone.QuestMilestone_Id(SelfPtr(), out_string, out_size));
			}
		}

		public string EventId
		{
			get
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.QuestMilestone.QuestMilestone_EventId(SelfPtr(), out_string, out_size));
			}
		}

		public string QuestId
		{
			get
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.QuestMilestone.QuestMilestone_QuestId(SelfPtr(), out_string, out_size));
			}
		}

		public ulong CurrentCount
		{
			get
			{
				return global::GooglePlayGames.Native.Cwrapper.QuestMilestone.QuestMilestone_CurrentCount(SelfPtr());
			}
		}

		public ulong TargetCount
		{
			get
			{
				return global::GooglePlayGames.Native.Cwrapper.QuestMilestone.QuestMilestone_TargetCount(SelfPtr());
			}
		}

		public byte[] CompletionRewardData
		{
			get
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToArray((byte[] out_bytes, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.QuestMilestone.QuestMilestone_CompletionRewardData(SelfPtr(), out_bytes, out_size));
			}
		}

		public global::GooglePlayGames.BasicApi.Quests.MilestoneState State
		{
			get
			{
				global::GooglePlayGames.Native.Cwrapper.Types.QuestMilestoneState questMilestoneState = global::GooglePlayGames.Native.Cwrapper.QuestMilestone.QuestMilestone_State(SelfPtr());
				switch (questMilestoneState)
				{
				case global::GooglePlayGames.Native.Cwrapper.Types.QuestMilestoneState.CLAIMED:
					return global::GooglePlayGames.BasicApi.Quests.MilestoneState.Claimed;
				case global::GooglePlayGames.Native.Cwrapper.Types.QuestMilestoneState.COMPLETED_NOT_CLAIMED:
					return global::GooglePlayGames.BasicApi.Quests.MilestoneState.CompletedNotClaimed;
				case global::GooglePlayGames.Native.Cwrapper.Types.QuestMilestoneState.NOT_COMPLETED:
					return global::GooglePlayGames.BasicApi.Quests.MilestoneState.NotCompleted;
				case global::GooglePlayGames.Native.Cwrapper.Types.QuestMilestoneState.NOT_STARTED:
					return global::GooglePlayGames.BasicApi.Quests.MilestoneState.NotStarted;
				default:
					throw new global::System.InvalidOperationException("Unknown state: " + questMilestoneState);
				}
			}
		}

		internal NativeQuestMilestone(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		internal bool Valid()
		{
			return global::GooglePlayGames.Native.Cwrapper.QuestMilestone.QuestMilestone_Valid(SelfPtr());
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.QuestMilestone.QuestMilestone_Dispose(selfPointer);
		}

		public override string ToString()
		{
			return string.Format("[NativeQuestMilestone: Id={0}, EventId={1}, QuestId={2}, CurrentCount={3}, TargetCount={4}, State={5}]", Id, EventId, QuestId, CurrentCount, TargetCount, State);
		}

		internal static global::GooglePlayGames.Native.PInvoke.NativeQuestMilestone FromPointer(global::System.IntPtr pointer)
		{
			if (pointer == global::System.IntPtr.Zero)
			{
				return null;
			}
			return new global::GooglePlayGames.Native.PInvoke.NativeQuestMilestone(pointer);
		}
	}
}

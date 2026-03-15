namespace GooglePlayGames.Native.PInvoke
{
	internal class MultiplayerParticipant : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		private static readonly global::System.Collections.Generic.Dictionary<global::GooglePlayGames.Native.Cwrapper.Types.ParticipantStatus, global::GooglePlayGames.BasicApi.Multiplayer.Participant.ParticipantStatus> StatusConversion = new global::System.Collections.Generic.Dictionary<global::GooglePlayGames.Native.Cwrapper.Types.ParticipantStatus, global::GooglePlayGames.BasicApi.Multiplayer.Participant.ParticipantStatus>
		{
			{
				global::GooglePlayGames.Native.Cwrapper.Types.ParticipantStatus.INVITED,
				global::GooglePlayGames.BasicApi.Multiplayer.Participant.ParticipantStatus.Invited
			},
			{
				global::GooglePlayGames.Native.Cwrapper.Types.ParticipantStatus.JOINED,
				global::GooglePlayGames.BasicApi.Multiplayer.Participant.ParticipantStatus.Joined
			},
			{
				global::GooglePlayGames.Native.Cwrapper.Types.ParticipantStatus.DECLINED,
				global::GooglePlayGames.BasicApi.Multiplayer.Participant.ParticipantStatus.Declined
			},
			{
				global::GooglePlayGames.Native.Cwrapper.Types.ParticipantStatus.LEFT,
				global::GooglePlayGames.BasicApi.Multiplayer.Participant.ParticipantStatus.Left
			},
			{
				global::GooglePlayGames.Native.Cwrapper.Types.ParticipantStatus.NOT_INVITED_YET,
				global::GooglePlayGames.BasicApi.Multiplayer.Participant.ParticipantStatus.NotInvitedYet
			},
			{
				global::GooglePlayGames.Native.Cwrapper.Types.ParticipantStatus.FINISHED,
				global::GooglePlayGames.BasicApi.Multiplayer.Participant.ParticipantStatus.Finished
			},
			{
				global::GooglePlayGames.Native.Cwrapper.Types.ParticipantStatus.UNRESPONSIVE,
				global::GooglePlayGames.BasicApi.Multiplayer.Participant.ParticipantStatus.Unresponsive
			}
		};

		internal MultiplayerParticipant(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		internal global::GooglePlayGames.Native.Cwrapper.Types.ParticipantStatus Status()
		{
			return global::GooglePlayGames.Native.Cwrapper.MultiplayerParticipant.MultiplayerParticipant_Status(SelfPtr());
		}

		internal bool IsConnectedToRoom()
		{
			return global::GooglePlayGames.Native.Cwrapper.MultiplayerParticipant.MultiplayerParticipant_IsConnectedToRoom(SelfPtr()) || Status() == global::GooglePlayGames.Native.Cwrapper.Types.ParticipantStatus.JOINED;
		}

		internal string DisplayName()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr size) => global::GooglePlayGames.Native.Cwrapper.MultiplayerParticipant.MultiplayerParticipant_DisplayName(SelfPtr(), out_string, size));
		}

		internal global::GooglePlayGames.Native.PInvoke.NativePlayer Player()
		{
			if (!global::GooglePlayGames.Native.Cwrapper.MultiplayerParticipant.MultiplayerParticipant_HasPlayer(SelfPtr()))
			{
				return null;
			}
			return new global::GooglePlayGames.Native.PInvoke.NativePlayer(global::GooglePlayGames.Native.Cwrapper.MultiplayerParticipant.MultiplayerParticipant_Player(SelfPtr()));
		}

		internal string Id()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr size) => global::GooglePlayGames.Native.Cwrapper.MultiplayerParticipant.MultiplayerParticipant_Id(SelfPtr(), out_string, size));
		}

		internal bool Valid()
		{
			return global::GooglePlayGames.Native.Cwrapper.MultiplayerParticipant.MultiplayerParticipant_Valid(SelfPtr());
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.MultiplayerParticipant.MultiplayerParticipant_Dispose(selfPointer);
		}

		internal global::GooglePlayGames.BasicApi.Multiplayer.Participant AsParticipant()
		{
			global::GooglePlayGames.Native.PInvoke.NativePlayer nativePlayer = Player();
			return new global::GooglePlayGames.BasicApi.Multiplayer.Participant(DisplayName(), Id(), StatusConversion[Status()], (nativePlayer != null) ? nativePlayer.AsPlayer() : null, IsConnectedToRoom());
		}

		internal static global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant FromPointer(global::System.IntPtr pointer)
		{
			if (global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.IsNull(pointer))
			{
				return null;
			}
			return new global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant(pointer);
		}

		internal static global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant AutomatchingSentinel()
		{
			return new global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant(global::GooglePlayGames.Native.Cwrapper.Sentinels.Sentinels_AutomatchingParticipant());
		}
	}
}

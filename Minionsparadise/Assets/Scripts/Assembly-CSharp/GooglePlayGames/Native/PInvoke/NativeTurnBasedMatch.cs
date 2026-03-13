namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeTurnBasedMatch : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal NativeTurnBasedMatch(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		internal uint AvailableAutomatchSlots()
		{
			return global::GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_AutomatchingSlotsAvailable(SelfPtr());
		}

		internal ulong CreationTime()
		{
			return global::GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_CreationTime(SelfPtr());
		}

		internal global::System.Collections.Generic.IEnumerable<global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant> Participants()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.ToEnumerable(global::GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Participants_Length(SelfPtr()), (global::System.UIntPtr index) => new global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant(global::GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Participants_GetElement(SelfPtr(), index)));
		}

		internal uint Version()
		{
			return global::GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Version(SelfPtr());
		}

		internal uint Variant()
		{
			return global::GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Variant(SelfPtr());
		}

		internal global::GooglePlayGames.Native.PInvoke.ParticipantResults Results()
		{
			return new global::GooglePlayGames.Native.PInvoke.ParticipantResults(global::GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_ParticipantResults(SelfPtr()));
		}

		internal global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant ParticipantWithId(string participantId)
		{
			foreach (global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant item in Participants())
			{
				if (item.Id().Equals(participantId))
				{
					return item;
				}
				item.Dispose();
			}
			return null;
		}

		internal global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant PendingParticipant()
		{
			global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant = new global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant(global::GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_PendingParticipant(SelfPtr()));
			if (!multiplayerParticipant.Valid())
			{
				multiplayerParticipant.Dispose();
				return null;
			}
			return multiplayerParticipant;
		}

		internal global::GooglePlayGames.Native.Cwrapper.Types.MatchStatus MatchStatus()
		{
			return global::GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Status(SelfPtr());
		}

		internal string Description()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr size) => global::GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Description(SelfPtr(), out_string, size));
		}

		internal bool HasRematchId()
		{
			string text = RematchId();
			return string.IsNullOrEmpty(text) || !text.Equals("(null)");
		}

		internal string RematchId()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr size) => global::GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_RematchId(SelfPtr(), out_string, size));
		}

		internal byte[] Data()
		{
			if (!global::GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_HasData(SelfPtr()))
			{
				global::GooglePlayGames.OurUtils.Logger.d("Match has no data.");
				return null;
			}
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToArray((byte[] bytes, global::System.UIntPtr size) => global::GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Data(SelfPtr(), bytes, size));
		}

		internal string Id()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr size) => global::GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Id(SelfPtr(), out_string, size));
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Dispose(selfPointer);
		}

		internal global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch AsTurnBasedMatch(string selfPlayerId)
		{
			global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.Multiplayer.Participant> list = new global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.Multiplayer.Participant>();
			string selfParticipantId = null;
			string pendingParticipantId = null;
			using (global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant = PendingParticipant())
			{
				if (multiplayerParticipant != null)
				{
					pendingParticipantId = multiplayerParticipant.Id();
				}
			}
			foreach (global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant item in Participants())
			{
				using (item)
				{
					using (global::GooglePlayGames.Native.PInvoke.NativePlayer nativePlayer = item.Player())
					{
						if (nativePlayer != null && nativePlayer.Id().Equals(selfPlayerId))
						{
							selfParticipantId = item.Id();
						}
					}
					list.Add(item.AsParticipant());
				}
			}
			bool canRematch = MatchStatus() == global::GooglePlayGames.Native.Cwrapper.Types.MatchStatus.COMPLETED && !HasRematchId();
			return new global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch(Id(), Data(), canRematch, selfParticipantId, list, AvailableAutomatchSlots(), pendingParticipantId, ToTurnStatus(MatchStatus()), ToMatchStatus(pendingParticipantId, MatchStatus()), Variant(), Version());
		}

		private static global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus ToTurnStatus(global::GooglePlayGames.Native.Cwrapper.Types.MatchStatus status)
		{
			switch (status)
			{
			case global::GooglePlayGames.Native.Cwrapper.Types.MatchStatus.CANCELED:
				return global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.Complete;
			case global::GooglePlayGames.Native.Cwrapper.Types.MatchStatus.COMPLETED:
				return global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.Complete;
			case global::GooglePlayGames.Native.Cwrapper.Types.MatchStatus.EXPIRED:
				return global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.Complete;
			case global::GooglePlayGames.Native.Cwrapper.Types.MatchStatus.INVITED:
				return global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.Invited;
			case global::GooglePlayGames.Native.Cwrapper.Types.MatchStatus.MY_TURN:
				return global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.MyTurn;
			case global::GooglePlayGames.Native.Cwrapper.Types.MatchStatus.PENDING_COMPLETION:
				return global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.Complete;
			case global::GooglePlayGames.Native.Cwrapper.Types.MatchStatus.THEIR_TURN:
				return global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.TheirTurn;
			default:
				return global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.Unknown;
			}
		}

		private static global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus ToMatchStatus(string pendingParticipantId, global::GooglePlayGames.Native.Cwrapper.Types.MatchStatus status)
		{
			switch (status)
			{
			case global::GooglePlayGames.Native.Cwrapper.Types.MatchStatus.CANCELED:
				return global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Cancelled;
			case global::GooglePlayGames.Native.Cwrapper.Types.MatchStatus.COMPLETED:
				return global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Complete;
			case global::GooglePlayGames.Native.Cwrapper.Types.MatchStatus.EXPIRED:
				return global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Expired;
			case global::GooglePlayGames.Native.Cwrapper.Types.MatchStatus.INVITED:
				return global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Active;
			case global::GooglePlayGames.Native.Cwrapper.Types.MatchStatus.MY_TURN:
				return global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Active;
			case global::GooglePlayGames.Native.Cwrapper.Types.MatchStatus.PENDING_COMPLETION:
				return global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Complete;
			case global::GooglePlayGames.Native.Cwrapper.Types.MatchStatus.THEIR_TURN:
				return (pendingParticipantId == null) ? global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.AutoMatching : global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Active;
			default:
				return global::GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Unknown;
			}
		}

		internal static global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch FromPointer(global::System.IntPtr selfPointer)
		{
			if (global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.IsNull(selfPointer))
			{
				return null;
			}
			return new global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch(selfPointer);
		}
	}
}

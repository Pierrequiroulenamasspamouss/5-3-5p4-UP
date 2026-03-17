namespace GooglePlayGames.Native.PInvoke
{
	internal class MultiplayerInvitation : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal MultiplayerInvitation(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		internal global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant Inviter()
		{
			global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant = new global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant(global::GooglePlayGames.Native.Cwrapper.MultiplayerInvitation.MultiplayerInvitation_InvitingParticipant(SelfPtr()));
			if (!multiplayerParticipant.Valid())
			{
				multiplayerParticipant.Dispose();
				return null;
			}
			return multiplayerParticipant;
		}

		internal uint Variant()
		{
			return global::GooglePlayGames.Native.Cwrapper.MultiplayerInvitation.MultiplayerInvitation_Variant(SelfPtr());
		}

		internal global::GooglePlayGames.Native.Cwrapper.Types.MultiplayerInvitationType Type()
		{
			return global::GooglePlayGames.Native.Cwrapper.MultiplayerInvitation.MultiplayerInvitation_Type(SelfPtr());
		}

		internal string Id()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr size) => global::GooglePlayGames.Native.Cwrapper.MultiplayerInvitation.MultiplayerInvitation_Id(SelfPtr(), out_string, size));
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.MultiplayerInvitation.MultiplayerInvitation_Dispose(selfPointer);
		}

		internal uint AutomatchingSlots()
		{
			return global::GooglePlayGames.Native.Cwrapper.MultiplayerInvitation.MultiplayerInvitation_AutomatchingSlotsAvailable(SelfPtr());
		}

		internal uint ParticipantCount()
		{
			return global::GooglePlayGames.Native.Cwrapper.MultiplayerInvitation.MultiplayerInvitation_Participants_Length(SelfPtr()).ToUInt32();
		}

		private static global::GooglePlayGames.BasicApi.Multiplayer.Invitation.InvType ToInvType(global::GooglePlayGames.Native.Cwrapper.Types.MultiplayerInvitationType invitationType)
		{
			switch (invitationType)
			{
			case global::GooglePlayGames.Native.Cwrapper.Types.MultiplayerInvitationType.REAL_TIME:
				return global::GooglePlayGames.BasicApi.Multiplayer.Invitation.InvType.RealTime;
			case global::GooglePlayGames.Native.Cwrapper.Types.MultiplayerInvitationType.TURN_BASED:
				return global::GooglePlayGames.BasicApi.Multiplayer.Invitation.InvType.TurnBased;
			default:
				global::GooglePlayGames.OurUtils.Logger.d("Found unknown invitation type: " + invitationType);
				return global::GooglePlayGames.BasicApi.Multiplayer.Invitation.InvType.Unknown;
			}
		}

		internal global::GooglePlayGames.BasicApi.Multiplayer.Invitation AsInvitation()
		{
			global::GooglePlayGames.BasicApi.Multiplayer.Invitation.InvType invType = ToInvType(Type());
			string invId = Id();
			int variant = (int)Variant();
			global::GooglePlayGames.BasicApi.Multiplayer.Participant inviter;
			using (global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant multiplayerParticipant = Inviter())
			{
				inviter = ((multiplayerParticipant != null) ? multiplayerParticipant.AsParticipant() : null);
			}
			return new global::GooglePlayGames.BasicApi.Multiplayer.Invitation(invType, invId, inviter, variant);
		}

		internal static global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation FromPointer(global::System.IntPtr selfPointer)
		{
			if (global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.IsNull(selfPointer))
			{
				return null;
			}
			return new global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation(selfPointer);
		}
	}
}

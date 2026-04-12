namespace GooglePlayGames.Native.PInvoke
{
	internal class ParticipantResults : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal ParticipantResults(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		internal bool HasResultsForParticipant(string participantId)
		{
			return global::GooglePlayGames.Native.Cwrapper.ParticipantResults.ParticipantResults_HasResultsForParticipant(SelfPtr(), participantId);
		}

		internal uint PlacingForParticipant(string participantId)
		{
			return global::GooglePlayGames.Native.Cwrapper.ParticipantResults.ParticipantResults_PlaceForParticipant(SelfPtr(), participantId);
		}

		internal global::GooglePlayGames.Native.Cwrapper.Types.MatchResult ResultsForParticipant(string participantId)
		{
			return global::GooglePlayGames.Native.Cwrapper.ParticipantResults.ParticipantResults_MatchResultForParticipant(SelfPtr(), participantId);
		}

		internal global::GooglePlayGames.Native.PInvoke.ParticipantResults WithResult(string participantId, uint placing, global::GooglePlayGames.Native.Cwrapper.Types.MatchResult result)
		{
			return new global::GooglePlayGames.Native.PInvoke.ParticipantResults(global::GooglePlayGames.Native.Cwrapper.ParticipantResults.ParticipantResults_WithResult(SelfPtr(), participantId, placing, result));
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.ParticipantResults.ParticipantResults_Dispose(selfPointer);
		}
	}
}

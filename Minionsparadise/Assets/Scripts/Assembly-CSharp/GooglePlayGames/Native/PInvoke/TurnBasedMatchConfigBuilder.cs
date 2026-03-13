namespace GooglePlayGames.Native.PInvoke
{
	internal class TurnBasedMatchConfigBuilder : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		private TurnBasedMatchConfigBuilder(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		internal global::GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder PopulateFromUIResponse(global::GooglePlayGames.Native.PInvoke.PlayerSelectUIResponse response)
		{
			global::GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_PopulateFromPlayerSelectUIResponse(SelfPtr(), response.AsPointer());
			return this;
		}

		internal global::GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder SetVariant(uint variant)
		{
			global::GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_SetVariant(SelfPtr(), variant);
			return this;
		}

		internal global::GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder AddInvitedPlayer(string playerId)
		{
			global::GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_AddPlayerToInvite(SelfPtr(), playerId);
			return this;
		}

		internal global::GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder SetExclusiveBitMask(ulong bitmask)
		{
			global::GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_SetExclusiveBitMask(SelfPtr(), bitmask);
			return this;
		}

		internal global::GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder SetMinimumAutomatchingPlayers(uint minimum)
		{
			global::GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_SetMinimumAutomatchingPlayers(SelfPtr(), minimum);
			return this;
		}

		internal global::GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder SetMaximumAutomatchingPlayers(uint maximum)
		{
			global::GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_SetMaximumAutomatchingPlayers(SelfPtr(), maximum);
			return this;
		}

		internal global::GooglePlayGames.Native.PInvoke.TurnBasedMatchConfig Build()
		{
			return new global::GooglePlayGames.Native.PInvoke.TurnBasedMatchConfig(global::GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_Create(SelfPtr()));
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_Dispose(selfPointer);
		}

		internal static global::GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder Create()
		{
			return new global::GooglePlayGames.Native.PInvoke.TurnBasedMatchConfigBuilder(global::GooglePlayGames.Native.Cwrapper.TurnBasedMatchConfigBuilder.TurnBasedMatchConfig_Builder_Construct());
		}
	}
}

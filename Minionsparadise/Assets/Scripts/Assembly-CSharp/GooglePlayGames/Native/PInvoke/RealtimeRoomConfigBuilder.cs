namespace GooglePlayGames.Native.PInvoke
{
	internal class RealtimeRoomConfigBuilder : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal RealtimeRoomConfigBuilder(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		internal global::GooglePlayGames.Native.PInvoke.RealtimeRoomConfigBuilder PopulateFromUIResponse(global::GooglePlayGames.Native.PInvoke.PlayerSelectUIResponse response)
		{
			global::GooglePlayGames.Native.Cwrapper.RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_PopulateFromPlayerSelectUIResponse(SelfPtr(), response.AsPointer());
			return this;
		}

		internal global::GooglePlayGames.Native.PInvoke.RealtimeRoomConfigBuilder SetVariant(uint variantValue)
		{
			uint variant = ((variantValue != 0) ? variantValue : uint.MaxValue);
			global::GooglePlayGames.Native.Cwrapper.RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_SetVariant(SelfPtr(), variant);
			return this;
		}

		internal global::GooglePlayGames.Native.PInvoke.RealtimeRoomConfigBuilder AddInvitedPlayer(string playerId)
		{
			global::GooglePlayGames.Native.Cwrapper.RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_AddPlayerToInvite(SelfPtr(), playerId);
			return this;
		}

		internal global::GooglePlayGames.Native.PInvoke.RealtimeRoomConfigBuilder SetExclusiveBitMask(ulong bitmask)
		{
			global::GooglePlayGames.Native.Cwrapper.RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_SetExclusiveBitMask(SelfPtr(), bitmask);
			return this;
		}

		internal global::GooglePlayGames.Native.PInvoke.RealtimeRoomConfigBuilder SetMinimumAutomatchingPlayers(uint minimum)
		{
			global::GooglePlayGames.Native.Cwrapper.RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_SetMinimumAutomatchingPlayers(SelfPtr(), minimum);
			return this;
		}

		internal global::GooglePlayGames.Native.PInvoke.RealtimeRoomConfigBuilder SetMaximumAutomatchingPlayers(uint maximum)
		{
			global::GooglePlayGames.Native.Cwrapper.RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_SetMaximumAutomatchingPlayers(SelfPtr(), maximum);
			return this;
		}

		internal global::GooglePlayGames.Native.PInvoke.RealtimeRoomConfig Build()
		{
			return new global::GooglePlayGames.Native.PInvoke.RealtimeRoomConfig(global::GooglePlayGames.Native.Cwrapper.RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_Create(SelfPtr()));
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_Dispose(selfPointer);
		}

		internal static global::GooglePlayGames.Native.PInvoke.RealtimeRoomConfigBuilder Create()
		{
			return new global::GooglePlayGames.Native.PInvoke.RealtimeRoomConfigBuilder(global::GooglePlayGames.Native.Cwrapper.RealTimeRoomConfigBuilder.RealTimeRoomConfig_Builder_Construct());
		}
	}
}

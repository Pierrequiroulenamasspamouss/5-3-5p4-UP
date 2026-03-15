namespace GooglePlayGames.Native.PInvoke
{
	internal class GameServicesBuilder : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal delegate void AuthFinishedCallback(global::GooglePlayGames.Native.Cwrapper.Types.AuthOperation operation, global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.AuthStatus status);

		internal delegate void AuthStartedCallback(global::GooglePlayGames.Native.Cwrapper.Types.AuthOperation operation);

		private GameServicesBuilder(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.InternalHooks.InternalHooks_ConfigureForUnityPlugin(SelfPtr());
		}

		internal void SetOnAuthFinishedCallback(global::GooglePlayGames.Native.PInvoke.GameServicesBuilder.AuthFinishedCallback callback)
		{
			global::GooglePlayGames.Native.Cwrapper.Builder.GameServices_Builder_SetOnAuthActionFinished(SelfPtr(), InternalAuthFinishedCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback));
		}

		internal void EnableSnapshots()
		{
			global::GooglePlayGames.Native.Cwrapper.Builder.GameServices_Builder_EnableSnapshots(SelfPtr());
		}

		internal void RequireGooglePlus()
		{
			global::GooglePlayGames.Native.Cwrapper.Builder.GameServices_Builder_RequireGooglePlus(SelfPtr());
		}

		internal void AddOauthScope(string scope)
		{
			global::GooglePlayGames.Native.Cwrapper.Builder.GameServices_Builder_AddOauthScope(SelfPtr(), scope);
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.Builder.OnAuthActionFinishedCallback))]
		private static void InternalAuthFinishedCallback(global::GooglePlayGames.Native.Cwrapper.Types.AuthOperation op, global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.AuthStatus status, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.GameServicesBuilder.AuthFinishedCallback authFinishedCallback = global::GooglePlayGames.Native.PInvoke.Callbacks.IntPtrToPermanentCallback<global::GooglePlayGames.Native.PInvoke.GameServicesBuilder.AuthFinishedCallback>(data);
			if (authFinishedCallback == null)
			{
				return;
			}
			try
			{
				authFinishedCallback(op, status);
			}
			catch (global::System.Exception ex)
			{
				global::GooglePlayGames.OurUtils.Logger.e("Error encountered executing InternalAuthFinishedCallback. Smothering to avoid passing exception into Native: " + ex);
			}
		}

		internal void SetOnAuthStartedCallback(global::GooglePlayGames.Native.PInvoke.GameServicesBuilder.AuthStartedCallback callback)
		{
			global::GooglePlayGames.Native.Cwrapper.Builder.GameServices_Builder_SetOnAuthActionStarted(SelfPtr(), InternalAuthStartedCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.Builder.OnAuthActionStartedCallback))]
		private static void InternalAuthStartedCallback(global::GooglePlayGames.Native.Cwrapper.Types.AuthOperation op, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.GameServicesBuilder.AuthStartedCallback authStartedCallback = global::GooglePlayGames.Native.PInvoke.Callbacks.IntPtrToPermanentCallback<global::GooglePlayGames.Native.PInvoke.GameServicesBuilder.AuthStartedCallback>(data);
			try
			{
				if (authStartedCallback != null)
				{
					authStartedCallback(op);
				}
			}
			catch (global::System.Exception ex)
			{
				global::GooglePlayGames.OurUtils.Logger.e("Error encountered executing InternalAuthStartedCallback. Smothering to avoid passing exception into Native: " + ex);
			}
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.Builder.GameServices_Builder_Dispose(selfPointer);
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.Builder.OnTurnBasedMatchEventCallback))]
		private static void InternalOnTurnBasedMatchEventCallback(global::GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent eventType, string matchId, global::System.IntPtr match, global::System.IntPtr userData)
		{
			global::System.Action<global::GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent, string, global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch> action = global::GooglePlayGames.Native.PInvoke.Callbacks.IntPtrToPermanentCallback<global::System.Action<global::GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent, string, global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch>>(userData);
			using (global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch arg = global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch.FromPointer(match))
			{
				try
				{
					if (action != null)
					{
						action(eventType, matchId, arg);
					}
				}
				catch (global::System.Exception ex)
				{
					global::GooglePlayGames.OurUtils.Logger.e("Error encountered executing InternalOnTurnBasedMatchEventCallback. Smothering to avoid passing exception into Native: " + ex);
				}
			}
		}

		internal void SetOnTurnBasedMatchEventCallback(global::System.Action<global::GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent, string, global::GooglePlayGames.Native.PInvoke.NativeTurnBasedMatch> callback)
		{
			global::System.IntPtr callback_arg = global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback);
			global::GooglePlayGames.Native.Cwrapper.Builder.GameServices_Builder_SetOnTurnBasedMatchEvent(SelfPtr(), InternalOnTurnBasedMatchEventCallback, callback_arg);
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.Builder.OnMultiplayerInvitationEventCallback))]
		private static void InternalOnMultiplayerInvitationEventCallback(global::GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent eventType, string matchId, global::System.IntPtr match, global::System.IntPtr userData)
		{
			global::System.Action<global::GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent, string, global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation> action = global::GooglePlayGames.Native.PInvoke.Callbacks.IntPtrToPermanentCallback<global::System.Action<global::GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent, string, global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation>>(userData);
			using (global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation arg = global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation.FromPointer(match))
			{
				try
				{
					if (action != null)
					{
						action(eventType, matchId, arg);
					}
				}
				catch (global::System.Exception ex)
				{
					global::GooglePlayGames.OurUtils.Logger.e("Error encountered executing InternalOnMultiplayerInvitationEventCallback. Smothering to avoid passing exception into Native: " + ex);
				}
			}
		}

		internal void SetOnMultiplayerInvitationEventCallback(global::System.Action<global::GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent, string, global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation> callback)
		{
			global::System.IntPtr callback_arg = global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback);
			global::GooglePlayGames.Native.Cwrapper.Builder.GameServices_Builder_SetOnMultiplayerInvitationEvent(SelfPtr(), InternalOnMultiplayerInvitationEventCallback, callback_arg);
		}

		internal global::GooglePlayGames.Native.PInvoke.GameServices Build(global::GooglePlayGames.Native.PInvoke.PlatformConfiguration configRef)
		{
			global::System.IntPtr selfPointer = global::GooglePlayGames.Native.Cwrapper.Builder.GameServices_Builder_Create(SelfPtr(), global::System.Runtime.InteropServices.HandleRef.ToIntPtr(configRef.AsHandle()));
			if (selfPointer.Equals(global::System.IntPtr.Zero))
			{
				throw new global::System.InvalidOperationException("There was an error creating a GameServices object. Check for log errors from GamesNativeSDK");
			}
			return new global::GooglePlayGames.Native.PInvoke.GameServices(selfPointer);
		}

		internal static global::GooglePlayGames.Native.PInvoke.GameServicesBuilder Create()
		{
			return new global::GooglePlayGames.Native.PInvoke.GameServicesBuilder(global::GooglePlayGames.Native.Cwrapper.Builder.GameServices_Builder_Construct());
		}
	}
}

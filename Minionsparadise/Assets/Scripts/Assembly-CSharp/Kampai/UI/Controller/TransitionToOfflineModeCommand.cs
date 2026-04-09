using System;
using Kampai.Game;
using Kampai.UI.View;
using strange.extensions.command.impl;

namespace Kampai.UI.Controller
{
    public class TransitionToOfflineModeCommand : Command
    {
        [Inject]
        public IUserSessionService userSessionService { get; set; }

        [Inject]
        public ShowOfflinePopupSignal showOfflinePopupSignal { get; set; }

        [Inject]
        public IConfigurationsService configurationsService { get; set; }

        [Inject]
        public ILocalPersistanceService localPersistService { get; set; }

        [Inject]
        public LoadPlayerSignal loadPlayerSignal { get; set; }

        [Inject]
        public LoginUserSignal loginUserSignal { get; set; }

        public override void Execute()
        {
            UnityEngine.Debug.Log("[OfflineMode] TransitionToOfflineModeCommand.Execute() started.");
            
            userSessionService.IsOffline = true;
            UnityEngine.Debug.Log("[OfflineMode] IsOffline set to true.");

            // Close the popup
            UnityEngine.Debug.Log("[OfflineMode] Dispatching showOfflinePopupSignal(false).");
            showOfflinePopupSignal.Dispatch(false);

            // If we don't have a UserID yet (first launch offline), create a dummy one
            string userId = localPersistService.GetData("UserID");
            UnityEngine.Debug.Log("[OfflineMode] Current UserID: " + userId);

            if (string.IsNullOrEmpty(userId))
            {
                string offlineUid = "OFFLINE_" + DateTime.Now.Ticks.ToString();
                localPersistService.PutData("UserID", offlineUid);
                UnityEngine.Debug.Log("[OfflineMode] Created temporary offline UID: " + offlineUid);
            }

            // Continue the loading flow using local data
            if (configurationsService.GetConfigurations() == null)
            {
                UnityEngine.Debug.Log("[OfflineMode] No configurations found, calling LoadLocalConfiguration().");
                configurationsService.LoadLocalConfiguration();
            }
            else
            {
                UnityEngine.Debug.Log("[OfflineMode] Configurations present, triggering offline login flow via loginUserSignal.");
                loginUserSignal.Dispatch();
            }
            UnityEngine.Debug.Log("[OfflineMode] TransitionToOfflineModeCommand.Execute() finished.");
        }
    }
}

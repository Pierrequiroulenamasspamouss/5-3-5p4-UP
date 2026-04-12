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
            
            userSessionService.IsOffline = true;

            // Close the popup
            showOfflinePopupSignal.Dispatch(false);

            // If we don't have a UserID yet (first launch offline), create a dummy one
            string userId = localPersistService.GetData("UserID");

            if (string.IsNullOrEmpty(userId))
            {
                string offlineUid = "OFFLINE_" + DateTime.Now.Ticks.ToString();
                localPersistService.PutData("UserID", offlineUid);
            }

            // Continue the loading flow using local data
            if (configurationsService.GetConfigurations() == null)
            {
                configurationsService.LoadLocalConfiguration();
            }
            else
            {
                loginUserSignal.Dispatch();
            }
        }
    }
}

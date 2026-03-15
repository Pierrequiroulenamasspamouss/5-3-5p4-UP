namespace Kampai.Game
{
    public class UserAgeForCOPPAReceivedCommand : global::strange.extensions.command.impl.Command
    {
        public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("UserAgeForCOPPAReceivedCommand") as global::Kampai.Util.IKampaiLogger;

        [Inject(global::Kampai.Game.GameElement.CONTEXT)]
        public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

        [Inject]
        public global::Kampai.Util.Tuple<int, int> birthdateYearMonth { get; set; }

        [Inject]
        public global::Kampai.Game.CoppaCompletedSignal coppaCompletedSignal { get; set; }

        [Inject]
        public global::Kampai.Game.SocialInitAllServicesSignal socialInitSignal { get; set; }

        [Inject]
        public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

        [Inject]
        public global::Kampai.Game.IUserSessionService userSessionService { get; set; }

        [Inject]
        public global::Kampai.Game.LoadMarketplaceOverridesSignal loadMarketPlaceOverridesSignal { get; set; }

        [Inject]
        public global::Kampai.UI.View.LoadMTXStoreSignal loadMTXStoreSignal { get; set; }

        [Inject]
        public global::Kampai.Game.LoadServerSalesSignal loadServerSalesSignal { get; set; }

        public override void Execute()
        {
            int item = birthdateYearMonth.Item1;
            int item2 = birthdateYearMonth.Item2;

            // Le log d'origine (qui va dans le fichier de log EA)
            logger.Debug("User age for COPPA has been received: birthdate {0}-{1}", item, item2);

            // NOTRE LOG UNITY (Visible dans la console)
            global::UnityEngine.Debug.Log(string.Format("<color=magenta>[COPPA TRACE]</color> Âge reçu depuis l'UI ({0}-{1}) ! DISPATCH de coppaCompletedSignal.", item, item2));

            coppaCompletedSignal.Dispatch();
            loadMarketPlaceOverridesSignal.Dispatch();
            loadMTXStoreSignal.Dispatch();
            loadServerSalesSignal.Dispatch();
            routineRunner.StartCoroutine(SocialInit());
            routineRunner.StartCoroutine(MarketplaceSlotsInit());
        }

        // ... Le reste du code (SocialInit, MarketplaceSlotsInit) reste inchangé
        private global::System.Collections.IEnumerator SocialInit()
        {
            yield return new global::UnityEngine.WaitForSeconds(1f);
            if (userSessionService.UserSession != null && !string.IsNullOrEmpty(userSessionService.UserSession.SessionID))
            {
                socialInitSignal.Dispatch();
            }
            else
            {
                logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "User Session was never initilized so social services will not be initialized");
            }
        }

        private global::System.Collections.IEnumerator MarketplaceSlotsInit()
        {
            yield return new global::UnityEngine.WaitForSeconds(1f);
            gameContext.injectionBinder.GetInstance<global::Kampai.Game.InitializeMarketplaceSlotsSignal>().Dispatch();
        }
    }
}
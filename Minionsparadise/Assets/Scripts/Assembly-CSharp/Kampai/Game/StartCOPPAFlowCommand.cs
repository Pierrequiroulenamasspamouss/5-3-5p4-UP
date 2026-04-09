namespace Kampai.Game
{
    public class StartCOPPAFlowCommand : global::strange.extensions.command.impl.Command
    {
        [Inject]
        public global::Kampai.UI.View.IGUIService guiService { get; set; }

        [Inject]
        public global::Kampai.Common.ICoppaService coppaService { get; set; }

        [Inject]
        public global::Kampai.Game.CoppaCompletedSignal coppaCompletedSignal { get; set; }

        [Inject]
        public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

        public override void Execute()
        {
            // LOG : On vérifie que la commande de départ est bien lancée
            // global::UnityEngine.Debug.Log("<color=cyan>[COPPA TRACE]</color> StartCOPPAFlowCommand a démarré.");

            if (!coppaService.IsBirthdateKnown())
            {
                // LOG : Le jeu ne connait pas l'âge, il va ouvrir le panel UI
                // global::UnityEngine.Debug.Log("<color=yellow>[COPPA TRACE]</color> Âge inconnu : Ouverture du panel COPPA_Age_Gate_Panel.");

                global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "COPPA_Age_Gate_Panel");
                iGUICommand.skrimScreen = "CoppaAgeGate";
                iGUICommand.disableSkrimButton = true;
                iGUICommand.darkSkrim = false;
                guiService.Execute(iGUICommand);
            }
            else
            {
                // LOG : L'âge est déjà connu, on saute l'UI
                // global::UnityEngine.Debug.Log("<color=green>[COPPA TRACE]</color> Âge déjà connu : On lance la coroutine CompleteCoppa directement.");
                routineRunner.StartCoroutine(CompleteCoppa());
            }
        }

        private global::System.Collections.IEnumerator CompleteCoppa()
        {
            // global::UnityEngine.Debug.Log("<color=cyan>[COPPA TRACE]</color> Attente de 2 secondes...");
            yield return new global::UnityEngine.WaitForSeconds(2f);

            // global::UnityEngine.Debug.Log("<color=green>[COPPA TRACE]</color> DISPATCH de coppaCompletedSignal (depuis StartCOPPAFlow) !");
            coppaCompletedSignal.Dispatch();
        }
    }
}
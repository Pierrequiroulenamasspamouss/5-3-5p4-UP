namespace Kampai.Util
{
	public class NamedCharacterBuilder : global::Kampai.Util.INamedCharacterBuilder
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("NamedCharacterBuilder") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Main.PlayLocalAudioSignal audioSignal { get; set; }

		[Inject]
		public global::Kampai.Main.StartLoopingAudioSignal startLoopingAudioSignal { get; set; }

		[Inject]
		public global::Kampai.Main.StopLocalAudioSignal stopAudioSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayMinionStateAudioSignal minionStateAudioSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDLCService dlcService { get; set; }

		[Inject]
		public global::Kampai.Util.IMinionBuilder minionBuilder { get; set; }

        public global::Kampai.Game.View.NamedCharacterObject Build(global::Kampai.Game.NamedCharacter character, global::UnityEngine.GameObject parent)
        {
            // === ANCIEN CODE COMMENTÉ (Sauvegarde) ===
            /*
            string arg = dlcService.GetDownloadQualityLevel().ToUpper();
            string prefab = character.Definition.Prefab;
            string text = string.Format("{0}_{1}", prefab, arg);
            global::UnityEngine.Object obj = global::Kampai.Util.KampaiResources.Load(text);
            global::UnityEngine.GameObject gameObject;
            if (obj == null)
            {
                logger.Error("NamedCharacterBuilder: Failed to load {0}.", text);
                gameObject = new global::UnityEngine.GameObject(text + "(FAILED TO LOAD)");
            }
            else
            {
                gameObject = global::UnityEngine.Object.Instantiate(obj) as global::UnityEngine.GameObject;
            }
            global::Kampai.Game.View.NamedCharacterObject namedCharacterObject = character.Setup(gameObject);
            namedCharacterObject.Build(character, parent, logger, minionBuilder);
            namedCharacterObject.Init(character, logger);
            global::Kampai.Game.View.AnimEventHandler animEventHandler = gameObject.AddComponent<global::Kampai.Game.View.AnimEventHandler>();
            animEventHandler.Init(namedCharacterObject, namedCharacterObject.localAudioEmitter, audioSignal, stopAudioSignal, minionStateAudioSignal, startLoopingAudioSignal);
            namedCharacterObject.SetupRandomizer(character.Definition.CharacterAnimations);
            return namedCharacterObject;
            */
            // ==========================================


            // === NOUVEAU CODE AVEC SMART FALLBACK ===
            string arg = dlcService.GetDownloadQualityLevel().ToUpper();
            string prefab = character.Definition.Prefab;
            string text = string.Format("{0}_{1}", prefab, arg);

            // 1. Le jeu essaie de charger sa qualité cible (ex: VERYLOW)
            global::UnityEngine.Object obj = global::Kampai.Util.KampaiResources.Load(text);

            // --- DÉBUT DU FALLBACK DYNAMIQUE ---
            if (obj == null)
            {
                // 2. S'il a raté, il essaie en priorité la qualité HIGH
                text = string.Format("{0}_HIGH", prefab);
                obj = global::Kampai.Util.KampaiResources.Load(text);
            }

            if (obj == null)
            {
                // 3. S'il rate encore, il essaie la MED
                text = string.Format("{0}_MED", prefab);
                obj = global::Kampai.Util.KampaiResources.Load(text);
            }
            // --- FIN DU FALLBACK ---

            global::UnityEngine.GameObject gameObject;
            if (obj == null)
            {
                // 4. Si vraiment AUCUNE qualité n'existe, là il fait son objet d'erreur
                logger.Error("NamedCharacterBuilder: Failed to load {0} in any quality.", prefab);
                gameObject = new global::UnityEngine.GameObject(prefab + "(FAILED TO LOAD)");
            }
            else
            {
                gameObject = global::UnityEngine.Object.Instantiate(obj) as global::UnityEngine.GameObject;
            }

            // 5. === LIGNES MANQUANTES : Configuration et Return ===
            global::Kampai.Game.View.NamedCharacterObject namedCharacterObject = character.Setup(gameObject);
            namedCharacterObject.Build(character, parent, logger, minionBuilder);
            namedCharacterObject.Init(character, logger);

            global::Kampai.Game.View.AnimEventHandler animEventHandler = gameObject.AddComponent<global::Kampai.Game.View.AnimEventHandler>();
            animEventHandler.Init(namedCharacterObject, namedCharacterObject.localAudioEmitter, audioSignal, stopAudioSignal, minionStateAudioSignal, startLoopingAudioSignal);

            namedCharacterObject.SetupRandomizer(character.Definition.CharacterAnimations);

            return namedCharacterObject; // On retourne l'objet final !
        }
    }
}

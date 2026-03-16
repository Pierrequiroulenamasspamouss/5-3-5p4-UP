namespace Kampai.Game
{
	public class LoadPartyAssetsCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("LoadPartyAssetsCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Game.SetMinionPartyBuildingStateSignal setMinionPartyBuildingStateSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IGuestOfHonorService guestService { get; set; }

		public override void Execute()
		{
			if (guestService.PartyShouldProduceBuff())
			{
				global::Kampai.Game.MinionPartyDefinition minionPartyDefinition = definitionService.Get<global::Kampai.Game.MinionPartyDefinition>();
				global::strange.extensions.injector.api.IInjectionBinding binding = gameContext.injectionBinder.GetBinding<global::UnityEngine.GameObject>(global::Kampai.Game.GameElement.PARTY_OBJECT);
				if (binding == null)
				{
					logger.Error("LoadPartyAssetsCommand: Cannot not find PARTY_OBJECT. Did you call PreloadPartyAssetsSignal first?");
					return;
				}
				global::UnityEngine.GameObject gameObject = binding.value as global::UnityEngine.GameObject;
				ActivateChild(gameObject, "StartPartyVFX");
				routineRunner.StartCoroutine(UnloadParticleSystem(gameObject, "StartPartyVFX"));
				routineRunner.StartCoroutine(StartBuildingPartyVFX(gameObject, minionPartyDefinition.PartyAssetDelay));
			}
		}

		private global::System.Collections.IEnumerator UnloadParticleSystem(global::UnityEngine.GameObject parent, string assetGroup)
		{
			global::UnityEngine.GameObject go = parent.FindChild(assetGroup);
			if (go == null)
			{
				yield break;
			}
			global::System.Collections.Generic.IList<global::UnityEngine.ParticleSystem> particleSystems = go.GetComponentsInChildren<global::UnityEngine.ParticleSystem>();
			for (int i = 0; i < particleSystems.Count; i++)
			{
				while (particleSystems[i] != null && particleSystems[i].IsAlive())
				{
					yield return null;
				}
			}
			global::UnityEngine.Object.Destroy(go);
		}

		private void ActivateChild(global::UnityEngine.GameObject parent, string childName)
		{
			if (parent == null)
			{
				logger.Warning("LoadPartyAssetsCommand: Parent is null when trying to activate child: " + childName);
				return;
			}
			global::UnityEngine.GameObject gameObject = parent.FindChild(childName);
			if (gameObject == null)
			{
				logger.Warning("LoadPartyAssetsCommand: can't find expected child: " + childName + " in parent: " + parent.name + ". This might happen if the party was unloaded prematurely.");
			}
			else
			{
				gameObject.SetActive(true);
			}
		}

		private global::System.Collections.IEnumerator StartBuildingPartyVFX(global::UnityEngine.GameObject partyObject, float delay)
		{
			yield return new global::UnityEngine.WaitForSeconds(delay);
			setMinionPartyBuildingStateSignal.Dispatch(true);
			ActivateChild(partyObject, "BuildingVFX");
		}
	}
}

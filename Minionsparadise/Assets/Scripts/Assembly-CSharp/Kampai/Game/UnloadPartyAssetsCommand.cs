namespace Kampai.Game
{
	public class UnloadPartyAssetsCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("UnloadPartyAssetsCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Game.SetMinionPartyBuildingStateSignal setMinionPartyBuildingStateSignal { get; set; }

		public override void Execute()
		{
			global::strange.extensions.injector.api.IInjectionBinding binding = gameContext.injectionBinder.GetBinding<global::UnityEngine.GameObject>(global::Kampai.Game.GameElement.PARTY_OBJECT);
			if (binding == null)
			{
				logger.Warning("Trying to unload party assets when they haven't been loaded");
				return;
			}
			global::UnityEngine.GameObject partyObject = binding.value as global::UnityEngine.GameObject;
			global::Kampai.Game.MinionPartyDefinition definition = definitionService.Get<global::Kampai.Game.MinionPartyDefinition>();
			LoadPartyEndVFX(partyObject, definition);
			routineRunner.StartCoroutine(UnloadPartyObjects(partyObject, definition));
		}

		private void LoadPartyEndVFX(global::UnityEngine.GameObject partyObject, global::Kampai.Game.MinionPartyDefinition definition)
		{
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("EndPartyVFX");
			gameObject.transform.parent = partyObject.transform;
			foreach (global::Kampai.Game.VFXAssetDefinition item in definition.endPartyVFX)
			{
				global::UnityEngine.GameObject original = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>(item.Prefab);
				global::UnityEngine.GameObject gameObject2 = global::UnityEngine.Object.Instantiate(original);
				gameObject2.transform.parent = gameObject.transform;
				gameObject2.transform.position = (global::UnityEngine.Vector3)item.location;
			}
			routineRunner.StartCoroutine(UnloadParticleSystem(partyObject, "EndPartyVFX"));
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
				while (particleSystems[i].IsAlive())
				{
					yield return null;
				}
			}
			global::UnityEngine.Object.Destroy(go);
		}

		private global::System.Collections.IEnumerator UnloadPartyObjects(global::UnityEngine.GameObject partyObject, global::Kampai.Game.MinionPartyDefinition definition)
		{
			yield return new global::UnityEngine.WaitForSeconds(definition.PartyAssetDelay);
			DestroyAssetGroup(partyObject, "BuildingVFX");
			setMinionPartyBuildingStateSignal.Dispatch(false);
		}

		private static void DestroyAssetGroup(global::UnityEngine.GameObject parent, string assetGroup)
		{
			global::UnityEngine.GameObject gameObject = parent.FindChild(assetGroup);
			if (!(gameObject == null))
			{
				global::UnityEngine.Object.Destroy(gameObject);
			}
		}
	}
}

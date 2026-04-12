namespace Kampai.Game
{
	public class PreLoadPartyAssetsCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject(global::strange.extensions.context.api.ContextKeys.CONTEXT_VIEW)]
		public global::UnityEngine.GameObject contextView { get; set; }

		public override void Execute()
		{
			global::strange.extensions.injector.api.IInjectionBinding binding = gameContext.injectionBinder.GetBinding<global::UnityEngine.GameObject>(global::Kampai.Game.GameElement.PARTY_OBJECT);
			global::UnityEngine.GameObject gameObject;
			if (binding == null)
			{
				gameObject = new global::UnityEngine.GameObject(global::Kampai.Game.GameElement.PARTY_OBJECT.ToString());
				gameObject.transform.parent = contextView.transform;
				gameContext.injectionBinder.Bind<global::UnityEngine.GameObject>().ToValue(gameObject).ToName(global::Kampai.Game.GameElement.PARTY_OBJECT);
			}
			else
			{
				gameObject = binding.value as global::UnityEngine.GameObject;
			}
			global::Kampai.Game.MinionPartyDefinition minionPartyDefinition = definitionService.Get<global::Kampai.Game.MinionPartyDefinition>();
			global::UnityEngine.GameObject gameObject2 = CreateAssetGroup(gameObject, "StartPartyVFX");
			foreach (global::Kampai.Game.VFXAssetDefinition item in minionPartyDefinition.startPartyVFX)
			{
				global::UnityEngine.GameObject original = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>(item.Prefab);
				global::UnityEngine.GameObject gameObject3 = global::UnityEngine.Object.Instantiate(original);
				gameObject3.transform.parent = gameObject2.transform;
				gameObject3.transform.position = (global::UnityEngine.Vector3)item.location;
			}
			gameObject2.SetActive(false);
			global::UnityEngine.GameObject gameObject4 = CreateAssetGroup(gameObject, "BuildingVFX");
			foreach (global::Kampai.Game.VFXAssetDefinition partyVFXDefintion in minionPartyDefinition.partyVFXDefintions)
			{
				global::UnityEngine.GameObject gameObject5 = LoadInstance(gameObject4, partyVFXDefintion.Prefab);
				gameObject5.transform.position = (global::UnityEngine.Vector3)partyVFXDefintion.location;
			}
			gameObject4.SetActive(false);
		}

		private static global::UnityEngine.GameObject CreateAssetGroup(global::UnityEngine.GameObject parent, string assetGroup)
		{
			global::UnityEngine.GameObject gameObject = parent.FindChild(assetGroup);
			if (gameObject == null)
			{
				gameObject = new global::UnityEngine.GameObject(assetGroup);
				gameObject.transform.parent = parent.transform;
			}
			return gameObject;
		}

		private static global::UnityEngine.GameObject LoadInstance(global::UnityEngine.GameObject assetGroup, string prefabPath)
		{
			global::UnityEngine.GameObject original = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>(prefabPath);
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(original);
			gameObject.transform.parent = assetGroup.transform;
			return gameObject;
		}
	}
}

namespace Kampai.Util
{
	public class DummyCharacterBuilder : global::Kampai.Util.IDummyCharacterBuilder
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("DummyCharacterBuilder") as global::Kampai.Util.IKampaiLogger;

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

		[Inject]
		public global::Kampai.Common.IRandomService randomService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		public global::Kampai.Game.View.DummyCharacterObject BuildMinion(global::Kampai.Game.Minion minion, global::Kampai.Game.CostumeItemDefinition costume, global::UnityEngine.Transform parent, bool isHigh, global::UnityEngine.Vector3 villainScale, global::UnityEngine.Vector3 villainPositionOffset)
		{
			string targetLOD = dlcService.GetDownloadQualityLevel().ToUpper();
			string name = string.Format("DUMMY_{0}", minion.Name);
			global::UnityEngine.GameObject gameObject = global::Kampai.Util.SkinnedMeshAggregator.CreateAggregateObject(name, costume.Skeleton, costume.MeshList, targetLOD);
			global::Kampai.Game.View.DummyCharacterObject dummyCharacterObject = gameObject.AddComponent<global::Kampai.Game.View.DummyCharacterObject>();
			dummyCharacterObject.Init(minion, logger, randomService, definitionService, GetWeightedInstanceList(costume.characterUIAnimationDefinition));
			dummyCharacterObject.Build(minion, costume.characterUIAnimationDefinition, parent, logger, isHigh, villainScale, villainPositionOffset, minionBuilder);
			global::Kampai.Game.View.AnimEventHandler animEventHandler = gameObject.AddComponent<global::Kampai.Game.View.AnimEventHandler>();
			animEventHandler.Init(dummyCharacterObject, dummyCharacterObject.localAudioEmitter, audioSignal, stopAudioSignal, minionStateAudioSignal, startLoopingAudioSignal);
			return dummyCharacterObject;
		}

		public global::Kampai.Game.View.DummyCharacterObject BuildNamedChacter(global::Kampai.Game.NamedCharacter namedCharacter, global::UnityEngine.Transform parent, bool isHigh, global::UnityEngine.Vector3 villainScale, global::UnityEngine.Vector3 villainPositionOffset)
		{
			string arg = dlcService.GetDownloadQualityLevel().ToUpper();
			global::Kampai.Game.NamedCharacterDefinition definition = namedCharacter.Definition;
			string prefab = definition.Prefab;
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
			global::Kampai.Game.View.DummyCharacterObject dummyCharacterObject = gameObject.AddComponent<global::Kampai.Game.View.DummyCharacterObject>();
			global::Kampai.Game.CharacterUIAnimationDefinition characterUIAnimationDefinition = definition.CharacterAnimations.characterUIAnimationDefinition;
			dummyCharacterObject.Init(namedCharacter, logger, randomService, definitionService, GetWeightedInstanceList(characterUIAnimationDefinition));
			dummyCharacterObject.Build(namedCharacter, characterUIAnimationDefinition, parent, logger, isHigh, villainScale, villainPositionOffset, minionBuilder);
			global::Kampai.Game.View.AnimEventHandler animEventHandler = gameObject.AddComponent<global::Kampai.Game.View.AnimEventHandler>();
			animEventHandler.Init(dummyCharacterObject, dummyCharacterObject.localAudioEmitter, audioSignal, stopAudioSignal, minionStateAudioSignal, startLoopingAudioSignal);
			return dummyCharacterObject;
		}

		private global::System.Collections.Generic.List<global::Kampai.Game.Transaction.WeightedInstance> GetWeightedInstanceList(global::Kampai.Game.CharacterUIAnimationDefinition characterUIAnimationDefinition)
		{
			global::System.Collections.Generic.List<global::Kampai.Game.Transaction.WeightedInstance> list = new global::System.Collections.Generic.List<global::Kampai.Game.Transaction.WeightedInstance>();
			if (!characterUIAnimationDefinition.UseLegacy)
			{
				global::Kampai.Game.Transaction.WeightedDefinition def = definitionService.Get<global::Kampai.Game.Transaction.WeightedDefinition>(characterUIAnimationDefinition.IdleWeightedAnimationID);
				global::Kampai.Game.Transaction.WeightedDefinition def2 = definitionService.Get<global::Kampai.Game.Transaction.WeightedDefinition>(characterUIAnimationDefinition.HappyWeightedAnimationID);
				global::Kampai.Game.Transaction.WeightedDefinition def3 = definitionService.Get<global::Kampai.Game.Transaction.WeightedDefinition>(characterUIAnimationDefinition.SelectedWeightedAnimationID);
				list.Add(new global::Kampai.Game.Transaction.WeightedInstance(def));
				list.Add(new global::Kampai.Game.Transaction.WeightedInstance(def2));
				list.Add(new global::Kampai.Game.Transaction.WeightedInstance(def3));
			}
			return list;
		}
	}
}

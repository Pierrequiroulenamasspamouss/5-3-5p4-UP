namespace Kampai.UI
{
	public class FancyUIService : global::Kampai.UI.IFancyUIService
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("FancyUIService") as global::Kampai.Util.IKampaiLogger;

		private global::System.Collections.Generic.Dictionary<string, global::UnityEngine.RuntimeAnimatorController> animationControllers;

		[Inject]
		public global::Kampai.Util.IDummyCharacterBuilder builder { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Common.IRandomService randomService { get; set; }

		[Inject]
		public global::Kampai.Util.IMinionBuilder minionBuilder { get; set; }

		[Inject]
		public global::Kampai.Main.PlayLocalAudioSignal audioSignal { get; set; }

		[Inject]
		public global::Kampai.Main.StartLoopingAudioSignal startLoopingAudioSignal { get; set; }

		[Inject]
		public global::Kampai.Main.StopLocalAudioSignal stopAudioSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayMinionStateAudioSignal minionStateAudioSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.Game.NetworkLostOpenSignal networkLostOpenSignal { get; set; }

		[Inject]
		public global::Kampai.Game.NetworkLostCloseSignal networkLostCloseSignal { get; set; }

		public global::Kampai.Game.View.DummyCharacterObject CreateCharacter(global::Kampai.UI.DummyCharacterType type, global::Kampai.UI.DummyCharacterAnimationState startingState, global::UnityEngine.Transform parent, global::UnityEngine.Vector3 villainScale, global::UnityEngine.Vector3 villainPositionOffset, int prestigeDefinitionID = 0, bool isHighLOD = true, bool isAudible = true, bool adjustMaterial = false)
		{
			global::Kampai.Game.PrestigeDefinition prestigeDefinition = null;
			if (prestigeDefinitionID != 0)
			{
				prestigeDefinition = definitionService.Get<global::Kampai.Game.PrestigeDefinition>(prestigeDefinitionID);
			}
			switch (type)
			{
			case global::Kampai.UI.DummyCharacterType.Minion:
			{
				global::System.Collections.Generic.IList<global::Kampai.Game.MinionDefinition> all = definitionService.GetAll<global::Kampai.Game.MinionDefinition>();
				int count = all.Count;
				int index = randomService.NextInt(count);
				global::Kampai.Game.MinionDefinition def = all[index];
				global::Kampai.Game.Minion minion = new global::Kampai.Game.Minion(def);
				int num = 99;
				if (prestigeDefinition != null)
				{
					global::Kampai.Game.Prestige prestige = prestigeService.GetPrestige(prestigeDefinitionID);
					if (prestige != null)
					{
						minion.PrestigeId = prestige.ID;
					}
					num = prestigeDefinition.TrackedDefinitionID;
				}
				global::Kampai.Game.CostumeItemDefinition costumeItemDefinition = definitionService.Get<global::Kampai.Game.CostumeItemDefinition>(num);
				if (costumeItemDefinition == null)
				{
					logger.Fatal(global::Kampai.Util.FatalCode.PS_MISSING_MINION_COSTUME, "ERROR: Minion costume ID: {0} - Could not create costume!!!", num);
				}
				global::Kampai.Game.View.DummyCharacterObject dummyCharacterObject2 = builder.BuildMinion(minion, costumeItemDefinition, parent, isHighLOD, villainScale, villainPositionOffset);
				if (!isAudible)
				{
					dummyCharacterObject2.ExecuteAction(new global::Kampai.Game.View.MuteAction(dummyCharacterObject2, true, logger));
				}
				if (adjustMaterial)
				{
					dummyCharacterObject2.SetStenciledShader();
				}
				dummyCharacterObject2.StartingState(startingState);
				dummyCharacterObject2.SetUpWifiListeners(networkLostOpenSignal, networkLostCloseSignal);
				return dummyCharacterObject2;
			}
			case global::Kampai.UI.DummyCharacterType.NamedCharacter:
			{
				global::Kampai.Game.NamedCharacterDefinition namedCharacterDefinition = definitionService.Get<global::Kampai.Game.NamedCharacterDefinition>(prestigeDefinition.TrackedDefinitionID);
				if (namedCharacterDefinition != null)
				{
					global::Kampai.Game.View.DummyCharacterObject dummyCharacterObject = CreateNamedCharacter(namedCharacterDefinition, parent, villainScale, villainPositionOffset, isHighLOD);
					if (!isAudible)
					{
						dummyCharacterObject.ExecuteAction(new global::Kampai.Game.View.MuteAction(dummyCharacterObject, true, logger));
					}
					if (adjustMaterial)
					{
						dummyCharacterObject.SetStenciledShader();
					}
					dummyCharacterObject.StartingState(startingState);
					dummyCharacterObject.SetUpWifiListeners(networkLostOpenSignal, networkLostCloseSignal);
					return dummyCharacterObject;
				}
				break;
			}
			}
			return null;
		}

		public global::Kampai.UI.DummyCharacterType GetCharacterType(int prestigeDefinitionID)
		{
			global::Kampai.Game.PrestigeDefinition prestigeDefinition = definitionService.Get<global::Kampai.Game.PrestigeDefinition>(prestigeDefinitionID);
			global::Kampai.UI.DummyCharacterType result = global::Kampai.UI.DummyCharacterType.Minion;
			if (prestigeDefinition.Type == global::Kampai.Game.PrestigeType.Minion)
			{
				global::Kampai.Game.Definition definition = definitionService.Get<global::Kampai.Game.Definition>(prestigeDefinition.TrackedDefinitionID);
				if (definition is global::Kampai.Game.NamedCharacterDefinition)
				{
					result = global::Kampai.UI.DummyCharacterType.NamedCharacter;
				}
			}
			else
			{
				result = global::Kampai.UI.DummyCharacterType.NamedCharacter;
			}
			return result;
		}

		private global::Kampai.Game.View.DummyCharacterObject CreateNamedCharacter(global::Kampai.Game.NamedCharacterDefinition namedCharacterDefinition, global::UnityEngine.Transform parent, global::UnityEngine.Vector3 villainScale, global::UnityEngine.Vector3 villainPositionOffset, bool isHighLOD)
		{
			global::Kampai.Game.NamedCharacter namedCharacter = null;
			global::Kampai.Game.PhilCharacterDefinition philCharacterDefinition = namedCharacterDefinition as global::Kampai.Game.PhilCharacterDefinition;
			if (philCharacterDefinition != null)
			{
				namedCharacter = new global::Kampai.Game.PhilCharacter(philCharacterDefinition);
			}
			global::Kampai.Game.BobCharacterDefinition bobCharacterDefinition = namedCharacterDefinition as global::Kampai.Game.BobCharacterDefinition;
			if (bobCharacterDefinition != null)
			{
				namedCharacter = new global::Kampai.Game.BobCharacter(bobCharacterDefinition);
			}
			global::Kampai.Game.KevinCharacterDefinition kevinCharacterDefinition = namedCharacterDefinition as global::Kampai.Game.KevinCharacterDefinition;
			if (kevinCharacterDefinition != null)
			{
				namedCharacter = new global::Kampai.Game.KevinCharacter(kevinCharacterDefinition);
			}
			global::Kampai.Game.StuartCharacterDefinition stuartCharacterDefinition = namedCharacterDefinition as global::Kampai.Game.StuartCharacterDefinition;
			if (stuartCharacterDefinition != null)
			{
				namedCharacter = new global::Kampai.Game.StuartCharacter(stuartCharacterDefinition);
			}
			global::Kampai.Game.SpecialEventCharacterDefinition specialEventCharacterDefinition = namedCharacterDefinition as global::Kampai.Game.SpecialEventCharacterDefinition;
			if (specialEventCharacterDefinition != null)
			{
				namedCharacter = new global::Kampai.Game.SpecialEventCharacter(specialEventCharacterDefinition);
			}
			global::Kampai.Game.VillainDefinition villainDefinition = namedCharacterDefinition as global::Kampai.Game.VillainDefinition;
			if (villainDefinition != null)
			{
				namedCharacter = new global::Kampai.Game.Villain(villainDefinition);
			}
			global::Kampai.Game.TSMCharacterDefinition tSMCharacterDefinition = namedCharacterDefinition as global::Kampai.Game.TSMCharacterDefinition;
			if (tSMCharacterDefinition != null)
			{
				namedCharacter = new global::Kampai.Game.TSMCharacter(tSMCharacterDefinition);
			}
			global::Kampai.Game.View.DummyCharacterObject dummyCharacterObject = builder.BuildNamedChacter(namedCharacter, parent, isHighLOD, villainScale, villainPositionOffset);
			dummyCharacterObject.SetUpWifiListeners(networkLostOpenSignal, networkLostCloseSignal);
			return dummyCharacterObject;
		}

		public global::Kampai.Game.View.DummyCharacterObject BuildMinion(int minionId, global::Kampai.UI.DummyCharacterAnimationState startingState, global::UnityEngine.Transform parent, bool isHighLOD = true, bool isAudible = true, int minionLevel = 0)
		{
			global::Kampai.Game.MinionDefinition def = definitionService.Get<global::Kampai.Game.MinionDefinition>(minionId);
			global::Kampai.Game.Minion minion = new global::Kampai.Game.Minion(def);
			minion.Level = minionLevel;
			global::Kampai.Game.CostumeItemDefinition costumeItemDefinition = definitionService.Get<global::Kampai.Game.CostumeItemDefinition>(minion.GetCostumeId(playerService, definitionService));
			if (costumeItemDefinition == null)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.PS_MISSING_DEFAULT_COSTUME, "ERROR: Minion costume ID: {0} - Could not create default costume!!!", 99);
			}
			global::Kampai.Game.View.DummyCharacterObject dummyCharacterObject = builder.BuildMinion(minion, costumeItemDefinition, parent, isHighLOD, global::UnityEngine.Vector3.one, global::UnityEngine.Vector3.one);
			if (!isAudible)
			{
				dummyCharacterObject.ExecuteAction(new global::Kampai.Game.View.MuteAction(dummyCharacterObject, true, logger));
			}
			dummyCharacterObject.StartBundlePackAnimation();
			dummyCharacterObject.SetUpWifiListeners(networkLostOpenSignal, networkLostCloseSignal);
			return dummyCharacterObject;
		}

		public void SetKampaiImage(global::Kampai.UI.View.KampaiImage image, string iconPath, string maskPath)
		{
			if (string.IsNullOrEmpty(iconPath))
			{
				iconPath = "btn_Main01_fill";
			}
			image.sprite = UIUtils.LoadSpriteFromPath(iconPath);
			if (string.IsNullOrEmpty(maskPath))
			{
				maskPath = "btn_Main01_mask";
			}
			image.maskSprite = UIUtils.LoadSpriteFromPath(maskPath);
		}

		public global::Kampai.Game.View.BuildingObject CreateDummyBuildingObject(global::Kampai.Game.BuildingDefinition buildingDefinition, global::UnityEngine.GameObject parent, out global::Kampai.Game.Building building, global::System.Collections.Generic.IList<global::Kampai.Game.View.MinionObject> minionsList = null, bool isAudible = true)
		{
			string prefab = buildingDefinition.GetPrefab();
			int iD = buildingDefinition.ID;
			if (string.IsNullOrEmpty(prefab))
			{
				logger.Error("Building Definition {0} doesn't have a Prefab defined, returning null", iD);
				building = null;
				return null;
			}
			if (animationControllers == null)
			{
				animationControllers = new global::System.Collections.Generic.Dictionary<string, global::UnityEngine.RuntimeAnimatorController>();
			}
			global::UnityEngine.GameObject gameObject = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>(prefab);
			if (gameObject == null)
			{
				logger.Error("Building Prefab doesn't exist for buildingDefinition {0}, Prefab: {1}", iD, prefab);
				building = null;
				return null;
			}
			global::UnityEngine.GameObject gameObject2 = global::UnityEngine.Object.Instantiate(gameObject);
			building = buildingDefinition.BuildBuilding();
			if (gameObject2 == null)
			{
				logger.Error("Could not create dummy building object from building definition id: {0}", iD);
				return null;
			}
			gameObject2.name = buildingDefinition.LocalizedKey;
			global::UnityEngine.Transform transform = gameObject2.transform;
			global::UnityEngine.Vector3 localEulerAngles = (transform.localPosition = global::UnityEngine.Vector3.zero);
			transform.localEulerAngles = localEulerAngles;
			localEulerAngles = (transform.eulerAngles = global::UnityEngine.Vector3.one);
			transform.localScale = localEulerAngles;
			transform.SetParent(parent.transform, false);
			gameObject2.SetLayerRecursively(5);
			return BuildingObjectSetup(gameObject2, buildingDefinition, parent, minionsList, building, isAudible);
		}

		public void ReleaseBuildingObject(global::Kampai.Game.View.BuildingObject buildingObj, global::Kampai.Game.Building building, global::System.Collections.Generic.IList<global::Kampai.Game.View.MinionObject> minionsList = null)
		{
			if (!(buildingObj != null) || !(buildingObj.gameObject != null))
			{
				return;
			}
			global::Kampai.Game.View.LeisureBuildingObjectView component = buildingObj.GetComponent<global::Kampai.Game.View.LeisureBuildingObjectView>();
			if (component != null)
			{
				component.FreeAllMinions();
			}
			if (minionsList != null)
			{
				global::Kampai.Game.TaskableBuilding taskableBuilding = building as global::Kampai.Game.TaskableBuilding;
				for (int i = 0; i < minionsList.Count; i++)
				{
					if (taskableBuilding != null)
					{
						taskableBuilding.RemoveMinion(minionsList[i].ID, 0);
					}
					global::UnityEngine.Object.Destroy(minionsList[i].gameObject);
				}
			}
			buildingObj.Cleanup();
			global::UnityEngine.Object.Destroy(buildingObj.gameObject);
		}

		private global::Kampai.Game.View.BuildingObject BuildingObjectSetup(global::UnityEngine.GameObject prefabInstance, global::Kampai.Game.BuildingDefinition buildingDefinition, global::UnityEngine.GameObject parent, global::System.Collections.Generic.IList<global::Kampai.Game.View.MinionObject> minionsList, global::Kampai.Game.Building building, bool isAudible)
		{
			global::Kampai.Game.View.BuildingObject buildingObject = building.AddBuildingObject(prefabInstance.gameObject);
			LoadBuildingAnimationControllers(buildingDefinition);
			LoadBuildingAnimationEventHandler(prefabInstance.transform);
			if (!isAudible)
			{
				buildingObject.ExecuteAction(new global::Kampai.Game.View.MuteAction(buildingObject, true, logger));
			}
			buildingObject.Init(building, logger, animationControllers, definitionService);
			global::Kampai.Game.View.RoutableBuildingObject routableBuildingObject = buildingObject as global::Kampai.Game.View.RoutableBuildingObject;
			if (minionsList != null && routableBuildingObject != null)
			{
				for (int i = 0; i < routableBuildingObject.GetNumberOfStations(); i++)
				{
					global::Kampai.Game.View.MinionObject minionObject = BuildBuildingMinionObject(parent);
					if (!isAudible)
					{
						minionObject.ExecuteAction(new global::Kampai.Game.View.MuteAction(minionObject, true, logger));
					}
					minionObject.ID = i;
					minionsList.Add(minionObject);
					AnimateBuildingsMinion(buildingObject, building, minionObject);
				}
			}
			global::Kampai.Game.View.AnimatingBuildingObject animatingBuildingObject = buildingObject as global::Kampai.Game.View.AnimatingBuildingObject;
			if (animatingBuildingObject != null)
			{
				animatingBuildingObject.StartAnimating();
			}
			global::Kampai.Game.View.TaskableBuildingObject taskableBuildingObj = buildingObject as global::Kampai.Game.View.TaskableBuildingObject;
			if (taskableBuildingObj != null)
			{
				taskableBuildingObj.EnqueueAction(new global::Kampai.Game.View.DelegateAction(delegate
				{
					taskableBuildingObj.SetEnabledAllStations(true);
				}, logger));
			}
			animationControllers = null;
			buildingObject.SetUpWifiListeners(networkLostOpenSignal, networkLostCloseSignal);
			return buildingObject;
		}

		private void LoadBuildingAnimationControllers(global::Kampai.Game.BuildingDefinition buildingDefinition)
		{
			global::Kampai.Game.AnimatingBuildingDefinition animatingBuildingDefinition = buildingDefinition as global::Kampai.Game.AnimatingBuildingDefinition;
			if (animatingBuildingDefinition == null)
			{
				return;
			}
			foreach (string item in animatingBuildingDefinition.AnimationControllerKeys())
			{
				if (!animationControllers.ContainsKey(item))
				{
					global::UnityEngine.RuntimeAnimatorController value = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(item);
					animationControllers.Add(item, value);
				}
			}
		}

		private void LoadBuildingAnimationEventHandler(global::UnityEngine.Transform transform)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				global::UnityEngine.Transform child = transform.GetChild(i);
				global::UnityEngine.GameObject gameObject = child.gameObject;
				global::UnityEngine.Animator component = gameObject.GetComponent<global::UnityEngine.Animator>();
				global::Kampai.Game.View.AnimEventHandler component2 = gameObject.GetComponent<global::Kampai.Game.View.AnimEventHandler>();
				if (component != null && component2 == null)
				{
					component.applyRootMotion = false;
					global::Kampai.Game.View.AnimEventHandler animEventHandler = gameObject.AddComponent<global::Kampai.Game.View.AnimEventHandler>();
					animEventHandler.Init(gameObject, audioSignal, stopAudioSignal, minionStateAudioSignal, startLoopingAudioSignal);
				}
			}
		}

		private void AnimateBuildingsMinion(global::Kampai.Game.View.BuildingObject buildingObj, global::Kampai.Game.Building building, global::Kampai.Game.View.MinionObject minionObj)
		{
			int routeIndex = GetRouteIndex(building);
			global::Kampai.Game.AnimatingBuildingDefinition animatingBuildingDefinition = null;
			global::Kampai.Game.TaskableBuilding taskableBuilding = building as global::Kampai.Game.TaskableBuilding;
			global::Kampai.Game.LeisureBuilding leisureBuilding = building as global::Kampai.Game.LeisureBuilding;
			if (taskableBuilding != null)
			{
				taskableBuilding.AddMinion(minionObj.ID, 0);
				animatingBuildingDefinition = taskableBuilding.Definition;
			}
			else if (leisureBuilding != null)
			{
				leisureBuilding.AddMinion(minionObj.ID, 0);
				animatingBuildingDefinition = leisureBuilding.Definition;
			}
			global::UnityEngine.RuntimeAnimatorController runtimeAnimatorController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(animatingBuildingDefinition.AnimationDefinitions[0].MinionController);
			minionObj.SetAnimController(runtimeAnimatorController);
			global::Kampai.Game.View.TaskableBuildingObject taskableBuildingObject = buildingObj as global::Kampai.Game.View.TaskableBuildingObject;
			global::Kampai.Game.View.LeisureBuildingObjectView leisureBuildingObjectView = buildingObj as global::Kampai.Game.View.LeisureBuildingObjectView;
			if (taskableBuildingObject != null)
			{
				taskableBuildingObject.MoveToRoutingPosition(minionObj, routeIndex);
				taskableBuildingObject.TrackChild(minionObj, runtimeAnimatorController, false);
			}
			else if (leisureBuildingObjectView != null)
			{
				leisureBuildingObjectView.TrackChild(minionObj, runtimeAnimatorController, routeIndex);
			}
		}

		private int GetRouteIndex(global::Kampai.Game.Building building)
		{
			global::Kampai.Game.TaskableBuilding taskableBuilding = building as global::Kampai.Game.TaskableBuilding;
			global::Kampai.Game.LeisureBuilding leisureBuilding = building as global::Kampai.Game.LeisureBuilding;
			int result = -1;
			if (taskableBuilding != null)
			{
				result = taskableBuilding.GetMinionsInBuilding();
			}
			else if (leisureBuilding != null)
			{
				result = leisureBuilding.GetMinionsInBuilding();
			}
			return result;
		}

		private global::Kampai.Game.View.MinionObject BuildBuildingMinionObject(global::UnityEngine.GameObject parent)
		{
			int id = global::Kampai.Util.GameConstants.MINION_DEFINITION_IDS[global::UnityEngine.Random.Range(0, global::Kampai.Util.GameConstants.MINION_DEFINITION_IDS.Length)];
			global::Kampai.Game.MinionDefinition def = definitionService.Get<global::Kampai.Game.MinionDefinition>(id);
			global::Kampai.Game.Minion minion = new global::Kampai.Game.Minion(def);
			global::Kampai.Game.CostumeItemDefinition costume = definitionService.Get<global::Kampai.Game.CostumeItemDefinition>(99);
			minionBuilder.SetLOD(minionBuilder.GetLOD());
			global::Kampai.Game.View.MinionObject minionObject = minionBuilder.BuildMinion(costume, "asm_minion_movement", parent, false);
			minionObject.transform.localScale = global::UnityEngine.Vector3.one;
			minion.Name = minionObject.name;
			minionObject.Init(minion, logger);
			global::Kampai.Util.AI.Agent component = minionObject.GetComponent<global::Kampai.Util.AI.Agent>();
			component.enabled = false;
			global::UnityEngine.RuntimeAnimatorController runtimeAnimatorController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>("asm_minion_movement");
			minionObject.SetDefaultAnimController(runtimeAnimatorController);
			minionObject.SetAnimController(runtimeAnimatorController);
			minionObject.gameObject.SetLayerRecursively(5);
			return minionObject;
		}

		public void SetStenciledShaderOnBuilding(global::UnityEngine.GameObject buildingObject)
		{
			int stencilRef = 2;
			int num = 1;
			global::UnityEngine.Renderer[] componentsInChildren = buildingObject.GetComponentsInChildren<global::UnityEngine.Renderer>();
			if (componentsInChildren == null)
			{
				return;
			}
			foreach (global::UnityEngine.Renderer renderer in componentsInChildren)
			{
				global::System.Collections.Generic.List<global::UnityEngine.Material> list = new global::System.Collections.Generic.List<global::UnityEngine.Material>();
				for (int j = 0; j < renderer.materials.Length; j++)
				{
					global::UnityEngine.Material item = renderer.materials[j];
					list.Add(item);
				}
				list.Sort((global::UnityEngine.Material x, global::UnityEngine.Material y) => x.renderQueue.CompareTo(y.renderQueue));
				for (int num2 = 0; num2 < list.Count; num2++)
				{
					global::UnityEngine.Material item = list[num2];
					global::Kampai.Util.Graphics.ShaderUtils.EnableStencilShader(item, stencilRef, num++);
				}
			}
		}
	}
}

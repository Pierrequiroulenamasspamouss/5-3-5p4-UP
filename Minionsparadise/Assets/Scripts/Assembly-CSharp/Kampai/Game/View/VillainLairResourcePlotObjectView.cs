namespace Kampai.Game.View
{
	public class VillainLairResourcePlotObjectView : global::Kampai.Game.View.AnimatingBuildingObject, global::strange.extensions.mediation.api.IView
	{
		public global::Kampai.Game.VillainLairResourcePlot resourcePlot;

		internal bool isGagable;

		internal global::strange.extensions.signal.impl.Signal<int> gagSignal = new global::strange.extensions.signal.impl.Signal<int>();

		internal global::strange.extensions.signal.impl.Signal<global::Kampai.Game.View.CharacterObject, int> addToBuildingSignal = new global::strange.extensions.signal.impl.Signal<global::Kampai.Game.View.CharacterObject, int>();

		protected global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.View.TaskingCharacterObject> childAnimators = new global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.View.TaskingCharacterObject>();

		protected global::System.Collections.Generic.List<global::Kampai.Game.View.TaskingCharacterObject> childQueue = new global::System.Collections.Generic.List<global::Kampai.Game.View.TaskingCharacterObject>();

		private global::UnityEngine.RuntimeAnimatorController buildingController;

		private global::UnityEngine.RuntimeAnimatorController minionController;

		private global::UnityEngine.RuntimeAnimatorController minionWalkStateMachine;

		private global::Kampai.Game.MinionStateChangeSignal stateChangeSignal;

		private readonly global::System.Collections.Generic.List<global::UnityEngine.Material> usableMaterials = new global::System.Collections.Generic.List<global::UnityEngine.Material>();

		private bool _requiresContext = true;

		protected bool registerWithContext = true;

		private global::strange.extensions.context.api.IContext currentContext;

		public bool requiresContext
		{
			get
			{
				return _requiresContext;
			}
			set
			{
				_requiresContext = value;
			}
		}

		public bool registeredWithContext { get; set; }

		public virtual bool autoRegisterWithContext
		{
			get
			{
				return registerWithContext;
			}
			set
			{
				registerWithContext = value;
			}
		}

		internal override void Init(global::Kampai.Game.Building building, global::Kampai.Util.IKampaiLogger logger, global::System.Collections.Generic.IDictionary<string, global::UnityEngine.RuntimeAnimatorController> controllers, global::Kampai.Game.IDefinitionService definitionService)
		{
			base.Init(building, logger, controllers, definitionService);
			resourcePlot = building as global::Kampai.Game.VillainLairResourcePlot;
			global::Kampai.Game.VillainLairResourcePlotDefinition villainLairResourcePlotDefinition = building.Definition as global::Kampai.Game.VillainLairResourcePlotDefinition;
			if (villainLairResourcePlotDefinition == null)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.BV_ILLEGAL_TASKABLE_DEFINITION, building.Definition.ID.ToString());
			}
			buildingState = building.State;
		}

		public void InitializeControllers(global::Kampai.Common.IRandomService randomService, global::Kampai.Game.MinionStateChangeSignal minionStateChangeSignal)
		{
			stateChangeSignal = minionStateChangeSignal;
			global::Kampai.Game.VillainLairResourcePlotDefinition definition = resourcePlot.Definition;
			minionWalkStateMachine = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>("asm_minion_movement");
			if (definition.AnimationDefinitions == null || definition.AnimationDefinitions.Count == 0)
			{
				logger.Error("Animation Definition NOT defined for a resource plot");
				return;
			}
			int num = randomService.NextInt(definition.AnimationDefinitions.Count);
			isGagable = num > 0;
			global::Kampai.Game.BuildingAnimationDefinition buildingAnimationDefinition = definition.AnimationDefinitions[num];
			buildingController = buildingControllers[buildingAnimationDefinition.CostumeId];
			minionController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(buildingAnimationDefinition.MinionController);
			InitializeAnimators();
		}

		public void InitializeAnimators()
		{
			ClearAnimators();
			global::UnityEngine.Animator[] componentsInChildren = base.gameObject.GetComponentsInChildren<global::UnityEngine.Animator>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].name.Contains("LOD"))
				{
					animators.Add(componentsInChildren[i]);
				}
			}
			SetAnimController(buildingController);
			InitializeVFX();
			if (childAnimators.Count > 0)
			{
				EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(this, null, logger, OnlyStateEnabled("OnLoop")));
				int hashAnimationState = GetHashAnimationState("Base Layer.Loop");
				int hashAnimationState2 = GetHashAnimationState("Base Layer.Loop");
				global::Kampai.Game.View.CharacterObject character = childAnimators[resourcePlot.MinionIDInBuilding].Character;
				character.EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(character, null, logger, OnlyStateEnabled("OnLoop")));
				character.EnqueueAction(new global::Kampai.Game.View.SkipToTimeAction(character, new global::Kampai.Game.View.SkipToTime(0, hashAnimationState2, GetCurrentAnimationTimeForState(hashAnimationState)), logger));
				SetSiblingVFXScript(character.gameObject, vfxScript);
			}
		}

		public override void SetAnimController(global::UnityEngine.RuntimeAnimatorController controller)
		{
			if (animators.Count == 0)
			{
				return;
			}
			using (global::System.Collections.Generic.List<global::UnityEngine.Animator>.Enumerator enumerator = animators.GetEnumerator())
			{
				global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>(activeProps.Keys);
				foreach (string item in list)
				{
					RemoveProp(item);
				}
				while (enumerator.MoveNext())
				{
					global::UnityEngine.Animator current2 = enumerator.Current;
					current2.runtimeAnimatorController = controller;
				}
			}
			vfxTrigger = null;
		}

		internal void TriggerGagAnimation()
		{
			if (animators.Count == 0)
			{
				return;
			}
			global::System.Collections.Generic.IList<global::Kampai.Game.View.ActionableObject> list = new global::System.Collections.Generic.List<global::Kampai.Game.View.ActionableObject>();
			list.Add(this);
			foreach (global::Kampai.Game.View.TaskingCharacterObject value in childAnimators.Values)
			{
				list.Add(value.Character);
			}
			global::Kampai.Game.View.SyncAction action = new global::Kampai.Game.View.SyncAction(list, logger);
			global::Kampai.Game.View.SyncAction action2 = new global::Kampai.Game.View.SyncAction(list, logger);
			foreach (global::Kampai.Game.View.TaskingCharacterObject value2 in childAnimators.Values)
			{
				int hashAnimationState = GetHashAnimationState("Base Layer.Loop");
				int hashAnimationState2 = GetHashAnimationState("Base Layer.Loop");
				global::Kampai.Game.View.SkipToTime skipToTime = new global::Kampai.Game.View.SkipToTime(0, hashAnimationState, GetCurrentAnimationTimeForState(hashAnimationState2));
				global::Kampai.Game.View.CharacterObject character = value2.Character;
				character.EnqueueAction(new global::Kampai.Game.View.SkipToTimeAction(character, skipToTime, logger));
				character.EnqueueAction(action);
				character.EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(character, null, logger, OnlyStateEnabled("OnGag")));
				character.EnqueueAction(action2);
				character.EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(character, null, logger, OnlyStateEnabled("OnLoop")));
			}
			EnqueueAction(action);
			EnqueueAction(new global::Kampai.Game.View.TriggerBuildingAnimationAction(this, OnlyStateEnabled("OnGag"), logger));
			EnqueueAction(new global::Kampai.Game.View.WaitForMecanimStateAction(this, GetHashAnimationState("Base Layer.Gag"), logger));
			EnqueueAction(action2);
			EnqueueAction(new global::Kampai.Game.View.TriggerBuildingAnimationAction(this, OnlyStateEnabled("OnLoop"), logger, "Base Layer.Loop"));
		}

		internal override void Highlight(bool enabled)
		{
		}

		internal void AddCharacterToBuildingActions(global::Kampai.Game.View.CharacterObject mo, int routeIndex)
		{
			TrackChild(mo, minionController, routeIndex);
			global::System.Collections.Generic.Dictionary<string, object> animationParams = OnlyStateEnabled("OnLoop");
			EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(this, null, logger, OnlyStateEnabled("OnStop")), true);
			EnqueueAction(new global::Kampai.Game.View.WaitForMecanimStateAction(this, GetHashAnimationState("Base Layer.Idle"), logger));
			EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(this, null, logger, animationParams));
		}

		public void TrackChild(global::Kampai.Game.View.CharacterObject child, global::UnityEngine.RuntimeAnimatorController controller, int routeIndex)
		{
			if (!IsAnimating)
			{
				global::System.Collections.Generic.Dictionary<string, object> animationParams = OnlyStateEnabled("OnLoop");
				EnqueueAction(new global::Kampai.Game.View.TriggerBuildingAnimationAction(this, animationParams, logger));
			}
			global::Kampai.Game.View.TaskingCharacterObject taskingCharacterObject = new global::Kampai.Game.View.TaskingCharacterObject(child, routeIndex);
			if (!childAnimators.ContainsKey(child.ID))
			{
				childAnimators.Add(child.ID, taskingCharacterObject);
				childQueue.Add(taskingCharacterObject);
				child.ShelveActionQueue();
			}
			InitializeVFX();
			SetSiblingVFXScript(child.gameObject, vfxScript);
			SetupChild(routeIndex, taskingCharacterObject, controller);
		}

		protected virtual void SetupChild(int routingIndex, global::Kampai.Game.View.TaskingCharacterObject taskingChild, global::UnityEngine.RuntimeAnimatorController controller = null)
		{
			taskingChild.RoutingIndex = routingIndex;
			global::Kampai.Game.View.CharacterObject character = taskingChild.Character;
			if (controller != null)
			{
				character.SetAnimController(controller);
			}
			character.ApplyRootMotion(true);
			character.EnableRenderers(true);
			character.SetAnimatorCullingMode(global::UnityEngine.AnimatorCullingMode.AlwaysAnimate);
			character.ExecuteAction(new global::Kampai.Game.View.SetAnimatorAction(character, null, logger, new global::System.Collections.Generic.Dictionary<string, object> { { "minionPosition", routingIndex } }));
			global::System.Collections.Generic.Dictionary<string, object> animationParams = OnlyStateEnabled("OnLoop");
			character.EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(character, null, logger, OnlyStateEnabled("OnStop")), true);
			character.EnqueueAction(new global::Kampai.Game.View.WaitForMecanimStateAction(character, GetHashAnimationState("Base Layer.Idle"), logger));
			character.EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(character, null, logger, animationParams));
			MoveToRoutingPosition(character, routingIndex);
		}

		internal void PathMinionToPlot(global::Kampai.Game.View.CharacterObject characterObject, global::strange.extensions.signal.impl.Signal<global::Kampai.Game.View.CharacterObject, int> addSignal)
		{
			characterObject.EnqueueAction(new global::Kampai.Game.View.TeleportAction(characterObject, routes[0].position, routes[0].eulerAngles, logger), true);
			characterObject.EnqueueAction(new global::Kampai.Game.View.PathToBuildingCompleteAction(characterObject, 0, addSignal, logger));
		}

		internal void FreeAllMinions(global::UnityEngine.Vector3 newPos)
		{
			global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.View.TaskingCharacterObject>.Enumerator enumerator = childAnimators.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					global::Kampai.Game.View.TaskingCharacterObject value = enumerator.Current.Value;
					UntrackChild(value.ID, newPos);
				}
			}
			finally
			{
				enumerator.Dispose();
			}
			childAnimators.Clear();
			EnqueueAction(new global::Kampai.Game.View.TriggerBuildingAnimationAction(this, OnlyStateEnabled("OnStop"), logger), true);
		}

		internal void UntrackChild(int minionId, global::UnityEngine.Vector3 newPos, global::Kampai.Game.MinionState targetState = global::Kampai.Game.MinionState.Idle)
		{
			if (childAnimators.ContainsKey(minionId))
			{
				global::Kampai.Game.View.TaskingCharacterObject taskingCharacterObject = childAnimators[minionId];
				global::Kampai.Game.View.CharacterObject character = taskingCharacterObject.Character;
				character.ApplyRootMotion(false);
				character.UnshelveActionQueue();
				character.EnableBlobShadow(true);
				character.SetAnimatorCullingMode(global::UnityEngine.AnimatorCullingMode.CullUpdateTransforms);
				UnlinkChild(minionId);
				global::Kampai.Game.View.NamedCharacterObject namedCharacterObject = character as global::Kampai.Game.View.NamedCharacterObject;
				if (namedCharacterObject != null)
				{
					character.ResetAnimationController();
				}
				else if (character is global::Kampai.Game.View.MinionObject)
				{
					character.EnqueueAction(new global::Kampai.Game.View.StateChangeAction(character.ID, stateChangeSignal, targetState, logger));
					character.EnqueueAction(new global::Kampai.Game.View.TeleportAction(character, newPos, global::UnityEngine.Vector3.zero, logger));
					character.EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(character, minionWalkStateMachine, logger));
				}
			}
		}

		protected void UnlinkChild(int minionId)
		{
			int num = -1;
			for (int i = 0; i < childQueue.Count; i++)
			{
				if (childQueue[i].ID == minionId)
				{
					num = i;
					break;
				}
			}
			if (num > -1)
			{
				childQueue.RemoveAt(num);
			}
			else
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "Not found");
			}
		}

		public void UpdateRoutes(global::UnityEngine.GameObject newInstance, global::Kampai.Game.View.MinionObject mo = null)
		{
			global::UnityEngine.GameObject gameObject = newInstance.FindChild("route0");
			if (gameObject != null)
			{
				routes[0] = gameObject.transform;
			}
			if (mo != null)
			{
				mo.transform.position = routes[0].position;
				mo.transform.rotation = routes[0].rotation;
			}
		}

		public void UpdateRenderers()
		{
			base.objectRenderers = base.gameObject.GetComponentsInChildren<global::UnityEngine.Renderer>();
			if (base.objectRenderers == null)
			{
				return;
			}
			for (int i = 0; i < base.objectRenderers.Length; i++)
			{
				global::UnityEngine.Renderer renderer = base.objectRenderers[i];
				for (int j = 0; j < renderer.materials.Length; j++)
				{
					global::UnityEngine.Material material = renderer.materials[j];
					if (!material.name.Contains("Platform") && !material.name.Contains("Shadow") && material.HasProperty("_BlendedColor"))
					{
						usableMaterials.Add(material);
					}
				}
			}
		}

		private void InitializeVFX()
		{
			global::Kampai.Util.VFXScript[] componentsInChildren = base.transform.gameObject.GetComponentsInChildren<global::Kampai.Util.VFXScript>();
			global::Kampai.Util.VFXScript[] array = componentsInChildren;
			foreach (global::Kampai.Util.VFXScript vFXScript in array)
			{
				vFXScript.Init();
				vFXScript.TriggerState("OnStop");
			}
			if (componentsInChildren.Length > 0)
			{
				if (componentsInChildren.Length > 1)
				{
					logger.Warning("Multiple VFX Scripts ({0}) found on {1}. Using the first one.", componentsInChildren.Length, base.name);
				}
				TrackVFX(componentsInChildren[0]);
				return;
			}
			logger.Info("No VFX Scripts found on this object: {0}", base.name);
		}

		private void SetSiblingVFXScript(global::UnityEngine.GameObject sibling, global::Kampai.Util.VFXScript vfxScript)
		{
			global::Kampai.Game.View.AnimEventHandler component = sibling.GetComponent<global::Kampai.Game.View.AnimEventHandler>();
			if (component != null)
			{
				component.SetSiblingVFXScript(vfxScript);
			}
		}

		protected void Awake()
		{
			global::Kampai.Util.KampaiView.BubbleToContextOnAwake(this, ref currentContext);
		}

		protected void Start()
		{
			global::Kampai.Util.KampaiView.BubbleToContextOnStart(this, ref currentContext);
			UpdateRenderers();
		}

		protected void OnDestroy()
		{
			global::Kampai.Util.KampaiView.BubbleToContextOnDestroy(this, ref currentContext);
		}
	}
}

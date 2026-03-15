namespace Kampai.Game.View
{
	public class AnimatingBuildingObject : global::Kampai.Game.View.RoutableBuildingObject
	{
		protected global::System.Collections.Generic.Dictionary<int, global::UnityEngine.RuntimeAnimatorController> buildingControllers;

		private global::UnityEngine.Renderer[] renderers;

		private global::System.Collections.Generic.Dictionary<string, int> hashCache = new global::System.Collections.Generic.Dictionary<string, int>();

		protected global::Kampai.Game.BuildingState buildingState;

		internal override void Init(global::Kampai.Game.Building building, global::Kampai.Util.IKampaiLogger logger, global::System.Collections.Generic.IDictionary<string, global::UnityEngine.RuntimeAnimatorController> controllers, global::Kampai.Game.IDefinitionService definitionService)
		{
			base.Init(building, logger, controllers, definitionService);
			global::Kampai.Game.AnimatingBuildingDefinition animatingBuildingDefinition = building.Definition as global::Kampai.Game.AnimatingBuildingDefinition;
			if (animatingBuildingDefinition == null)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.BV_ILLEGAL_ANIMATING_DEFINITION, building.Definition.ID.ToString());
			}
			buildingControllers = new global::System.Collections.Generic.Dictionary<int, global::UnityEngine.RuntimeAnimatorController>();
			if (this is global::Kampai.Game.View.MignetteBuildingObject)
			{
				return;
			}
			foreach (global::Kampai.Game.BuildingAnimationDefinition animationDefinition in animatingBuildingDefinition.AnimationDefinitions)
			{
				if (controllers.ContainsKey(animationDefinition.BuildingController))
				{
					buildingControllers.Add(animationDefinition.CostumeId, controllers[animationDefinition.BuildingController]);
				}
			}
			if (!buildingControllers.ContainsKey(-1))
			{
				logger.Fatal(global::Kampai.Util.FatalCode.BV_NO_DEFAULT_ANIMATION_CONTROLLER, animatingBuildingDefinition.ID.ToString());
			}
			animators = InitAnimators();
			if (building.IsBuildingRepaired())
			{
				if (!(this is global::Kampai.Game.View.VillainLairResourcePlotObjectView))
				{
					EnqueueAction(new global::Kampai.Game.View.ControllerBuildingAction(this, buildingControllers[-1], logger));
				}
				renderers = base.gameObject.GetComponentsInChildren<global::UnityEngine.Renderer>();
			}
		}

		protected bool HasVisibleRenderers()
		{
			if (renderers == null)
			{
				return false;
			}
			for (int i = 0; i < renderers.Length; i++)
			{
				if (renderers[i].isVisible)
				{
					return true;
				}
			}
			return false;
		}

		protected virtual global::System.Collections.Generic.List<global::UnityEngine.Animator> InitAnimators()
		{
			animators = new global::System.Collections.Generic.List<global::UnityEngine.Animator>();
			for (int i = 0; i < base.transform.childCount; i++)
			{
				global::UnityEngine.Transform child = base.transform.GetChild(i);
				if (child.name.Contains("LOD"))
				{
					global::UnityEngine.Animator component = child.GetComponent<global::UnityEngine.Animator>();
					if (component != null)
					{
						animators.Add(component);
					}
				}
			}
			return animators;
		}

		public virtual void StartAnimating()
		{
			if (IsInAnimatorState(GetHashAnimationState("Base Layer.Wait")))
			{
				EnqueueAction(new global::Kampai.Game.View.TriggerBuildingAnimationAction(this, "OnStop", logger));
			}
			EnqueueAction(new global::Kampai.Game.View.TriggerBuildingAnimationAction(this, "OnLoop", logger));
		}

		protected global::System.Func<float> GetCurrentAnimationTimeForState(int stateHash)
		{
			return () => IsInAnimatorState(stateHash) ? GetCurrentAnimationTime() : 0f;
		}

		protected float GetCurrentAnimationTime()
		{
			if (GetAnimatorStateInfo(0).HasValue)
			{
				return GetAnimatorStateInfo(0).Value.normalizedTime;
			}
			return 0f;
		}

		protected int GetHashAnimationState(string stateName, int index = -1)
		{
			if (index > -1)
			{
				stateName += index;
			}
			if (!hashCache.ContainsKey(stateName))
			{
				hashCache.Add(stateName, global::UnityEngine.Animator.StringToHash(stateName));
			}
			return hashCache[stateName];
		}

		internal void EnableAnimators(bool enable)
		{
			foreach (global::UnityEngine.Animator animator in animators)
			{
				animator.enabled = enable;
			}
		}

		protected global::System.Collections.Generic.Dictionary<string, object> OnlyStateEnabled(string enabled)
		{
			global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
			foreach (string aLL_TRIGGER in global::Kampai.Util.GameConstants.Animation.ALL_TRIGGERS)
			{
				dictionary.Add(aLL_TRIGGER, aLL_TRIGGER.Equals(enabled));
			}
			return dictionary;
		}

		public virtual void SetState(global::Kampai.Game.BuildingState newState)
		{
			if (newState == global::Kampai.Game.BuildingState.Idle)
			{
				base.StopBuildingAudioInIdleStateSignal.Dispatch();
			}
		}

		protected override void Show()
		{
			base.Show();
			StartAnimating();
		}
	}
}

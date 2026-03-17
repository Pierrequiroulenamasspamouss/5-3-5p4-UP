namespace Kampai.Game.View
{
	public class MasterPlanComponentBuildingObject : global::Kampai.Game.View.BuildingObject, global::strange.extensions.mediation.api.IView, global::Kampai.Game.View.IRequiresBuildingScaffolding, global::Kampai.Game.View.IStartAudio
	{
		internal global::Kampai.Game.CleanupMasterPlanComponentsSignal cleanupComponentSignal;

		private global::Kampai.Main.PlayLocalAudioSignal playLocalAudioSignal;

		private bool audioPlaying;

		private global::Kampai.Game.MasterPlanComponentBuilding planBuilding;

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
			planBuilding = building as global::Kampai.Game.MasterPlanComponentBuilding;
			string animationController = planBuilding.Definition.animationController;
			if (!string.IsNullOrEmpty(animationController))
			{
				global::UnityEngine.RuntimeAnimatorController runtimeAnimatorController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(animationController);
				if (runtimeAnimatorController == null)
				{
					logger.Error("MasterPlanComponentDefinition has bad animationController value: {0}", animationController);
				}
				else
				{
					InitAnimators();
					SetAnimController(runtimeAnimatorController);
				}
			}
		}

		private void InitAnimators()
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
		}

		public void TriggerVFX()
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				global::UnityEngine.Transform child = base.transform.GetChild(i);
				global::UnityEngine.ParticleSystem component = child.GetComponent<global::UnityEngine.ParticleSystem>();
				if (component != null)
				{
					component.Play();
				}
			}
		}

		public void TriggerMasterPlanCompleteAnimation(string controllerName)
		{
			global::UnityEngine.Animator animator = base.gameObject.GetComponent<global::UnityEngine.Animator>();
			if (animator == null)
			{
				animator = base.gameObject.AddComponent<global::UnityEngine.Animator>();
			}
			animators.Insert(0, animator);
			animator.runtimeAnimatorController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(controllerName);
			EnqueueAction(new global::Kampai.Game.View.WaitForMecanimStateAction(this, global::UnityEngine.Animator.StringToHash("Base Layer.Exit"), logger));
			EnqueueAction(new global::Kampai.Game.View.SendIDSignalAction(this, cleanupComponentSignal, logger));
			EnqueueAction(new global::Kampai.Game.View.DestroyObjectAction(this, logger));
		}

		protected void Awake()
		{
			global::Kampai.Util.KampaiView.BubbleToContextOnAwake(this, ref currentContext);
		}

		protected void Start()
		{
			global::Kampai.Util.KampaiView.BubbleToContextOnStart(this, ref currentContext);
		}

		protected void OnDestroy()
		{
			global::Kampai.Util.KampaiView.BubbleToContextOnDestroy(this, ref currentContext);
		}

		public void InitAudio(global::Kampai.Game.BuildingState creationState, global::Kampai.Main.PlayLocalAudioSignal playLocalAudioSignal)
		{
			this.playLocalAudioSignal = playLocalAudioSignal;
			StartCoroutine(WaitForGameToStart(creationState));
		}

		public void NotifyBuildingState(global::Kampai.Game.BuildingState newState)
		{
			if (newState != global::Kampai.Game.BuildingState.Broken && newState != global::Kampai.Game.BuildingState.Inaccessible && newState != global::Kampai.Game.BuildingState.Complete)
			{
				PlaySFX();
			}
		}

		private void PlaySFX()
		{
			string environmentalAudio = planBuilding.Definition.environmentalAudio;
			if (!string.IsNullOrEmpty(environmentalAudio) && playLocalAudioSignal != null && !audioPlaying)
			{
				playLocalAudioSignal.Dispatch(global::Kampai.Util.Audio.GetAudioEmitter.Get(base.gameObject, "MasterPlanComponentEmitter"), environmentalAudio, null);
				audioPlaying = true;
			}
		}

		private global::System.Collections.IEnumerator WaitForGameToStart(global::Kampai.Game.BuildingState creationState)
		{
			yield return new global::UnityEngine.WaitForEndOfFrame();
			NotifyBuildingState(creationState);
		}
	}
}

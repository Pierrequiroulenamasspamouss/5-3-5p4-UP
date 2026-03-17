namespace Kampai.Game.View
{
	public class VillainView : global::Kampai.Game.View.NamedCharacterObject, global::strange.extensions.mediation.api.IView
	{
		private const string ASM_PARAM_INTRO = "Intro";

		private const string ASM_PARAM_ACTIVE = "Active";

		private const string ASM_PARAM_MOVE_INTO_CABANA = "MoveIntoCabana";

		private const string ASM_PARAM_CABANA_INDEX = "CabanaIndex";

		private global::strange.extensions.signal.impl.Signal<string, global::System.Type, object> villainAnimSignal = new global::strange.extensions.signal.impl.Signal<string, global::System.Type, object>();

		private int loopCountMin;

		private int loopCountMax;

		private global::UnityEngine.Vector3 cabanaPosition;

		private global::UnityEngine.RuntimeAnimatorController cabanaController;

		private global::UnityEngine.RuntimeAnimatorController farewellController;

		private bool _requiresContext = true;

		protected bool registerWithContext = true;

		private global::strange.extensions.context.api.IContext currentContext;

		public override global::strange.extensions.signal.impl.Signal<string, global::System.Type, object> AnimSignal
		{
			get
			{
				return villainAnimSignal;
			}
		}

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

		public override global::Kampai.Game.View.NamedCharacterObject Build(global::Kampai.Game.NamedCharacter character, global::UnityEngine.GameObject parent, global::Kampai.Util.IKampaiLogger logger, global::Kampai.Util.IMinionBuilder minionBuilder)
		{
			base.Build(character, parent, logger, minionBuilder);
			global::Kampai.Game.VillainDefinition villainDefinition = character.Definition as global::Kampai.Game.VillainDefinition;
			if (!string.IsNullOrEmpty(villainDefinition.AsmCabana))
			{
				cabanaController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(villainDefinition.AsmCabana);
			}
			if (!string.IsNullOrEmpty(villainDefinition.AsmFarewell))
			{
				farewellController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(villainDefinition.AsmFarewell);
			}
			global::Kampai.Game.VillainDefinition villainDefinition2 = character.Definition as global::Kampai.Game.VillainDefinition;
			loopCountMin = villainDefinition2.LoopCountMin;
			loopCountMax = villainDefinition2.LoopCountMax;
			return this;
		}

		public override global::UnityEngine.Vector3 GetIndicatorPosition()
		{
			if (cabanaPosition == global::UnityEngine.Vector3.zero)
			{
				return base.GetIndicatorPosition();
			}
			return cabanaPosition + global::Kampai.Util.GameConstants.UI.VILLAIN_UI_OFFSET;
		}

		public void PlayWelcome()
		{
			SetAnimController(defaultController);
		}

		public void GotoCabana(int index, global::UnityEngine.Transform transform)
		{
			cabanaPosition = transform.position;
			setLocation(transform.position);
			setRotation(transform.eulerAngles);
			SetAnimController(cabanaController);
			SetAnimInteger("CabanaIndex", index);
		}

		public void PlayFarewell()
		{
			SetAnimController(farewellController);
		}

		internal void SetMasterPlanRewardAnimation(global::Kampai.Game.MasterPlanDefinition planDefinition)
		{
			if (planDefinition.VillainCharacterDefID == base.DefinitionID)
			{
				global::UnityEngine.RuntimeAnimatorController runtimeAnimatorController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(planDefinition.RewardStateMachine);
				if (runtimeAnimatorController != null)
				{
					global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
					dictionary.Add("isKevin", false);
					EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(this, runtimeAnimatorController, logger, dictionary));
					EnqueueAction(new global::Kampai.Game.View.DelayAction(this, 4f, logger));
					EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(this, defaultController, logger));
				}
			}
		}

		internal void DisplayVillain(int charDefID, bool enable)
		{
			if (charDefID == base.DefinitionID)
			{
				EnableRenderers(enable);
			}
		}

		internal void InitializeVillain(global::Kampai.Game.VillainLair lair, int charDefID)
		{
			if (charDefID == base.DefinitionID)
			{
				base.transform.position = (global::UnityEngine.Vector3)lair.Definition.Location + lair.Definition.VillainOffset;
				base.transform.rotation = global::UnityEngine.Quaternion.Euler(0f, lair.Definition.VillainRotation, 0f);
				global::UnityEngine.Collider[] componentsInChildren = base.gameObject.GetComponentsInChildren<global::UnityEngine.Collider>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					global::UnityEngine.Object.Destroy(componentsInChildren[i]);
				}
				if (lair.hasVisited)
				{
					SetAnimController(defaultController);
					return;
				}
				LoadAnimationController(lair.Definition.IntroAnimController);
				SetAnimBool("isKevin", false);
				string text = string.Format("Base Layer.Exit_{0}", "Villain");
				EnqueueAction(new global::Kampai.Game.View.WaitForMecanimStateAction(this, global::UnityEngine.Animator.StringToHash(text), logger), true);
				EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(this, defaultController, logger));
			}
		}

		protected override void TransitionComplete()
		{
			global::UnityEngine.AnimatorStateInfo? animatorStateInfo = GetAnimatorStateInfo(0);
			if (animatorStateInfo.HasValue)
			{
				int num = base.randomService.NextInt(loopCountMin, loopCountMax + 1);
				StartCoroutine(ReturnToRelaxation(animatorStateInfo.Value.length * (float)num));
			}
		}

		protected override void SetupLODs()
		{
		}

		protected override string GetRootJointPath(global::Kampai.Game.Character character)
		{
			return string.Format("{0}:{0}/{0}:ROOT/{0}:Pelvis_M", (character as global::Kampai.Game.NamedCharacter).Definition.Prefab);
		}

		private global::System.Collections.IEnumerator ReturnToRelaxation(float time)
		{
			yield return new global::UnityEngine.WaitForSeconds(time);
			SetAnimBool(PlayAlternateString, false);
		}

		private void LoadAnimationController(string animationController)
		{
			global::UnityEngine.RuntimeAnimatorController runtimeAnimatorController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(animationController);
			if (runtimeAnimatorController == null)
			{
				logger.Error("Failed to load Villain's animation controller: {0}", animationController);
			}
			else
			{
				SetAnimController(runtimeAnimatorController);
			}
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
	}
}

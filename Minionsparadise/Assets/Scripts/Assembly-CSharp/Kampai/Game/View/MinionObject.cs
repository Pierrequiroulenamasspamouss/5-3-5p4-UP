namespace Kampai.Game.View
{
	public class MinionObject : global::Kampai.Game.View.CharacterObject
	{
		public enum MinionGachaState
		{
			Inactive = 0,
			Active = 1,
			Deviant = 2,
			IndividualTap = 3
		}

		private const float importScale = 0.01f;

		protected global::Kampai.Util.AI.Agent agent;

		protected float defaultMaxSpeed;

		protected bool isInParty;

		protected global::UnityEngine.Vector3 partyLocation;

		protected float partyRadius;

		protected global::Kampai.Util.AI.SteerToTether steerToTether;

		protected bool isInMinionParty;

		protected float partyRestMin;

		protected float partyRestMax;

		protected float partyRestTimer;

		private global::Kampai.Game.Minion minion;

		[Inject]
		public global::Kampai.Game.View.CameraUtils cameraUtils { get; set; }

		[Inject]
		public global::Kampai.Game.View.MinionIdleNotifier minionIdleNotifier { get; set; }

		[Inject]
		public global::Kampai.Game.MinionPartyAnimationSignal minionPartyAnimationSignal { get; set; }

		public bool isTemporaryMinion { get; set; }

		public global::Kampai.Game.View.MinionObject.MinionGachaState GachaState { get; set; }

		public int Level
		{
			get
			{
				return (minion != null) ? minion.Level : 0;
			}
		}

		public override void Init(global::Kampai.Game.Character character, global::Kampai.Util.IKampaiLogger logger)
		{
			base.Init(character, logger);
			isTemporaryMinion = false;
			minion = character as global::Kampai.Game.Minion;
			if (minion == null)
			{
				logger.Error("Init Minion {0}:{1} Failed!", character.Name, character.ID);
			}
			else
			{
				SetMinionShape();
				agent = base.gameObject.GetComponent<global::Kampai.Util.AI.Agent>();
				defaultMaxSpeed = agent.MaxSpeed;
			}
		}

		public global::Kampai.Game.Minion GetMinion()
		{
			return minion;
		}

		public override void OnDefinitionsHotSwap(global::Kampai.Game.IDefinitionService definitionService)
		{
			base.OnDefinitionsHotSwap(definitionService);
			minion.OnDefinitionHotSwap(definitionService.Get(base.DefinitionID));
			SetMinionShape();
		}

		private void SetMinionShape()
		{
			if (!minion.HasPrestige)
			{
				SetEyes(base.transform, minion.Definition.Eyes);
				SetBody(base.transform, minion.Definition.Body);
				SetHair(base.transform, minion.Definition.Hair);
			}
		}

		public override void LateUpdate()
		{
			base.LateUpdate();
			if (!(animators[0].runtimeAnimatorController != null) || !animatorControllersAreEqual || shelvedQueue != null)
			{
				return;
			}
			if (actionQueue.Count == 0 && !(currentAction is global::Kampai.Game.View.GotoSideWalkAction))
			{
				SetAnimBool("isMoving", agent.MaxSpeed > 0.0001f);
				SetAnimFloat("speed", agent.Speed);
			}
			if (isTemporaryMinion)
			{
				return;
			}
			if (isInMinionParty)
			{
				if ((global::UnityEngine.Vector3.Distance(base.gameObject.transform.position, partyLocation) <= partyRadius + 1f) ? true : false)
				{
					partyRestTimer -= global::UnityEngine.Time.deltaTime;
					if (partyRestTimer <= 0f && actionQueue.Count == 0)
					{
						minionPartyAnimationSignal.Dispatch(ID);
						partyRestTimer = global::UnityEngine.Random.Range(partyRestMin, partyRestMax);
						return;
					}
				}
				SetTether(partyLocation, partyRadius);
			}
			else if (isInParty)
			{
				SetTether(partyLocation, partyRadius);
			}
			else
			{
				SetTether(cameraUtils.CameraCenterRaycast(), 20f);
			}
		}

		public override void Idle()
		{
			if (minionIdleNotifier != null)
			{
				minionIdleNotifier.MinionIdle(this);
				base.currentAction = null;
			}
		}

		public void EnterParty(global::UnityEngine.Vector3 partyLocation, float radius)
		{
			isInParty = true;
			this.partyLocation = partyLocation;
			partyRadius = radius;
		}

		public void LeaveParty()
		{
			isInParty = false;
		}

		public void EnterMinionParty(global::UnityEngine.Vector3 partyLocation, float radius, float restMin, float restMax)
		{
			isInMinionParty = true;
			this.partyLocation = partyLocation;
			partyRadius = radius;
			partyRestMin = restMin;
			partyRestMax = restMax;
		}

		public void LeaveMinionParty()
		{
			isInMinionParty = false;
		}

		public void SetTether(global::UnityEngine.Vector3 tether, float dist)
		{
			if (steerToTether == null)
			{
				steerToTether = GetComponent<global::Kampai.Util.AI.SteerToTether>();
			}
			if (steerToTether != null)
			{
				steerToTether.Tether = tether;
				steerToTether.MaxDist = dist;
			}
		}

		public void Wander()
		{
			agent.MaxSpeed = defaultMaxSpeed;
			SetAnimController(defaultController);
		}

		public void SeekTarget(global::UnityEngine.Vector3 pos, float threshold)
		{
			global::Kampai.Util.AI.SteerCharacterToSeek component = GetComponent<global::Kampai.Util.AI.SteerCharacterToSeek>();
			component.Target = pos;
			component.Threshold = threshold;
			component.enabled = true;
		}

		public global::Kampai.Util.AI.Agent GetAgent()
		{
			return agent;
		}

		public override void ExecuteAction(global::Kampai.Game.View.KampaiAction action)
		{
			agent.MaxSpeed = 0f;
			base.ExecuteAction(action);
		}

		public static void SetEyes(global::UnityEngine.Transform t, uint count)
		{
			if (count != 1)
			{
				global::UnityEngine.Transform transform = t.gameObject.FindChild("minion:eyeMain_jnt").transform;
				global::UnityEngine.GameObject gameObject = t.gameObject.FindChild("minion:browMorph_jnt");
				transform.localRotation = global::UnityEngine.Quaternion.identity;
				if (gameObject != null)
				{
					gameObject.transform.localScale = new global::UnityEngine.Vector3(1.65f, 0.4f, 1f);
				}
			}
		}

		public static global::System.Collections.Generic.Dictionary<string, TransformLite> GetMinionTransforms(global::UnityEngine.Transform t)
		{
			global::System.Collections.Generic.Dictionary<string, TransformLite> dictionary = new global::System.Collections.Generic.Dictionary<string, TransformLite>();
			string[] array = new string[16]
			{
				"minion:eyeMain_jnt", "minion:browMorph_jnt", "minion:neckStretch_jnt", "minion:L_shoulderOffset_jnt", "minion:R_shoulderOffset_jnt", "minion:pelvisOffset_jnt", "minion:mouth_base_jnt", "minion:neckOffset_jnt", "minion:headOffset_jnt", "minion:spineOffset_jnt",
				"minion:mouthOffset_jnt", "minion:L_hipOffset_jnt", "minion:R_hipOffset_jnt", "minion:hatScale_jnt", "minion:hair1_jnt", "minion:hair2_jnt"
			};
			string[] array2 = array;
			foreach (string key in array2)
			{
				global::UnityEngine.GameObject gameObject = t.gameObject.FindChild(key);
				if (gameObject != null)
				{
					global::UnityEngine.Transform transform = gameObject.transform;
					dictionary.Add(key, new TransformLite(transform.localPosition, transform.localRotation, transform.localScale));
				}
			}
			return dictionary;
		}

		public static void SetTransforms(global::UnityEngine.Transform t, global::System.Collections.Generic.Dictionary<string, TransformLite> xforms)
		{
			foreach (string key in xforms.Keys)
			{
				global::UnityEngine.GameObject gameObject = t.gameObject.FindChild(key);
				if (gameObject != null)
				{
					global::UnityEngine.Transform transform = gameObject.transform;
					transform.localPosition = xforms[key].position;
					transform.localRotation = xforms[key].rotation;
					transform.localScale = xforms[key].scale;
				}
			}
		}

		public static void SetBody(global::UnityEngine.Transform t, global::Kampai.Game.MinionBody body)
		{
			if (body == global::Kampai.Game.MinionBody.NORMAL)
			{
				return;
			}
			global::UnityEngine.Transform transform = t.gameObject.FindChild("minion:neckStretch_jnt").transform;
			global::UnityEngine.Transform transform2 = t.gameObject.FindChild("minion:L_shoulderOffset_jnt").transform;
			global::UnityEngine.Transform transform3 = t.gameObject.FindChild("minion:R_shoulderOffset_jnt").transform;
			global::UnityEngine.Transform transform4 = t.gameObject.FindChild("minion:pelvisOffset_jnt").transform;
			global::UnityEngine.Transform transform5 = t.gameObject.FindChild("minion:mouth_base_jnt").transform;
			global::UnityEngine.Transform transform6 = t.gameObject.FindChild("minion:neckOffset_jnt").transform;
			global::UnityEngine.Transform transform7 = t.gameObject.FindChild("minion:headOffset_jnt").transform;
			global::UnityEngine.Transform transform8 = t.gameObject.FindChild("minion:spineOffset_jnt").transform;
			global::UnityEngine.Transform transform9 = t.gameObject.FindChild("minion:mouthOffset_jnt").transform;
			global::UnityEngine.Transform transform10 = t.gameObject.FindChild("minion:eyeMain_jnt").transform;
			global::UnityEngine.GameObject gameObject = t.gameObject.FindChild("minion:browMorph_jnt");
			switch (body)
			{
			case global::Kampai.Game.MinionBody.TALL:
				transform4.localScale = new global::UnityEngine.Vector3(0.825f, 1f, 0.9f);
				transform8.localScale = new global::UnityEngine.Vector3(0.8f, 1f, 0.9f);
				transform5.localPosition = new global::UnityEngine.Vector3(transform5.localPosition.x, 0.32999998f, transform5.localPosition.z);
				transform5.localScale = new global::UnityEngine.Vector3(0.65f, 0.9f, 0.7f);
				transform.localPosition = new global::UnityEngine.Vector3(transform.localPosition.x, 0.3875f, transform.localPosition.z);
				transform6.localPosition = new global::UnityEngine.Vector3(transform6.localPosition.x, transform6.localPosition.y, 0.225f);
				transform6.localScale = new global::UnityEngine.Vector3(0.8f, transform6.localScale.y, 0.85f);
				transform7.localPosition = new global::UnityEngine.Vector3(transform7.localPosition.x, transform7.localPosition.y, 0.19999999f);
				transform7.localScale = new global::UnityEngine.Vector3(0.82f, 1f, 0.8f);
				transform10.localScale = new global::UnityEngine.Vector3(1.1f, 0.9f, 1.15f);
				transform10.localPosition = new global::UnityEngine.Vector3(transform10.localPosition.x, transform10.localPosition.y, -0.147f);
				transform3.localPosition = new global::UnityEngine.Vector3(0.205f, transform3.localPosition.y, transform3.localPosition.z);
				transform2.localPosition = new global::UnityEngine.Vector3(-0.205f, transform2.localPosition.y, transform2.localPosition.z);
				transform9.localPosition = new global::UnityEngine.Vector3(transform9.localPosition.x, 0.32999998f, transform9.localPosition.z);
				if (gameObject != null)
				{
					gameObject.transform.localScale = new global::UnityEngine.Vector3(0.01f, 0.01f, 0.01f);
				}
				break;
			case global::Kampai.Game.MinionBody.WIDE:
			{
				global::UnityEngine.Transform transform11 = t.gameObject.FindChild("minion:L_hipOffset_jnt").transform;
				global::UnityEngine.Transform transform12 = t.gameObject.FindChild("minion:R_hipOffset_jnt").transform;
				global::UnityEngine.Transform transform13 = t.gameObject.FindChild("minion:hatScale_jnt").transform;
				transform4.localScale = new global::UnityEngine.Vector3(1.3f, 1f, 1.33f);
				transform8.localScale = new global::UnityEngine.Vector3(1.3f, 1.35f, 1.3f);
				transform.localPosition = new global::UnityEngine.Vector3(transform.localPosition.x, 0.17181998f, transform.localPosition.z);
				transform6.localPosition = new global::UnityEngine.Vector3(transform6.localPosition.x, transform6.localPosition.y, 0.25108f);
				transform6.localScale = new global::UnityEngine.Vector3(1.3f, 1.35f, 1.3f);
				transform7.localPosition = new global::UnityEngine.Vector3(transform7.localPosition.x, transform7.localPosition.y, 0.25108f);
				transform7.localScale = new global::UnityEngine.Vector3(1.3f, 0.9f, 1.3f);
				transform10.localScale = new global::UnityEngine.Vector3(20f / 27f, 1f, 20f / 27f);
				transform10.localPosition = new global::UnityEngine.Vector3(transform10.localPosition.x, transform10.localPosition.y, -0.099999994f);
				transform3.localPosition = new global::UnityEngine.Vector3(0.32f, transform3.localPosition.y, -0.03f);
				transform2.localPosition = new global::UnityEngine.Vector3(-0.32f, transform2.localPosition.y, -0.03f);
				transform12.localPosition = new global::UnityEngine.Vector3(transform12.localPosition.x, transform12.localPosition.y, -0.089999996f);
				transform11.localPosition = new global::UnityEngine.Vector3(transform11.localPosition.x, transform11.localPosition.y, -0.089999996f);
				transform5.localScale = new global::UnityEngine.Vector3(1.2f, 0.8f, 1f);
				transform5.localPosition = new global::UnityEngine.Vector3(transform5.localPosition.x, 0.22711f, transform5.localPosition.z);
				transform13.localScale = new global::UnityEngine.Vector3(1f, 1.444f, 1f);
				break;
			}
			case global::Kampai.Game.MinionBody.BOB:
				break;
			}
		}

		public static void SetHair(global::UnityEngine.Transform t, global::Kampai.Game.MinionHair hair)
		{
			global::UnityEngine.GameObject gameObject = t.gameObject.FindChild("minion:hair1_jnt");
			global::UnityEngine.GameObject gameObject2 = t.gameObject.FindChild("minion:hair2_jnt");
			switch (hair)
			{
			case global::Kampai.Game.MinionHair.PARTED:
				if (gameObject != null)
				{
					global::UnityEngine.Transform transform7 = gameObject.transform;
					transform7.localPosition = new global::UnityEngine.Vector3(transform7.localPosition.x, 0.12364999f, transform7.localPosition.z);
					transform7.localEulerAngles = new global::UnityEngine.Vector3(0f, 0f, 0f);
					transform7.localScale = new global::UnityEngine.Vector3(0.5f, 0.5f, 0.5f);
				}
				if (gameObject2 != null)
				{
					global::UnityEngine.Transform transform8 = gameObject2.transform;
					transform8.localPosition = new global::UnityEngine.Vector3(transform8.localPosition.x, 0.12364999f, transform8.localPosition.z);
					transform8.localEulerAngles = new global::UnityEngine.Vector3(0f, 0f, 0f);
					transform8.localScale = new global::UnityEngine.Vector3(1f, 1f, 1f);
				}
				break;
			case global::Kampai.Game.MinionHair.BALD:
				if (gameObject != null)
				{
					global::UnityEngine.Transform transform5 = gameObject.transform;
					transform5.localPosition = new global::UnityEngine.Vector3(transform5.localPosition.x, 0.12364999f, transform5.localPosition.z);
					transform5.localEulerAngles = new global::UnityEngine.Vector3(0f, 0f, 0f);
					transform5.localScale = new global::UnityEngine.Vector3(0.5f, 0.5f, 0.5f);
				}
				if (gameObject2 != null)
				{
					global::UnityEngine.Transform transform6 = gameObject2.transform;
					transform6.localPosition = new global::UnityEngine.Vector3(transform6.localPosition.x, 0.12364999f, transform6.localPosition.z);
					transform6.localEulerAngles = new global::UnityEngine.Vector3(0f, 0f, 0f);
					transform6.localScale = new global::UnityEngine.Vector3(0.5f, 0.5f, 0.5f);
				}
				break;
			case global::Kampai.Game.MinionHair.SPROUT:
				if (gameObject != null)
				{
					global::UnityEngine.Transform transform3 = gameObject.transform;
					transform3.localPosition = new global::UnityEngine.Vector3(transform3.localPosition.x, 0.15365f, transform3.localPosition.z);
					transform3.localEulerAngles = new global::UnityEngine.Vector3(0f, 180f, 180f);
					transform3.localScale = new global::UnityEngine.Vector3(0.5f, 1f, 0.5f);
				}
				if (gameObject2 != null)
				{
					global::UnityEngine.Transform transform4 = gameObject2.transform;
					transform4.localPosition = new global::UnityEngine.Vector3(transform4.localPosition.x, 0.12364999f, transform4.localPosition.z);
					transform4.localEulerAngles = new global::UnityEngine.Vector3(0f, 0f, 0f);
					transform4.localScale = new global::UnityEngine.Vector3(0.5f, 0.5f, 0.5f);
				}
				break;
			case global::Kampai.Game.MinionHair.WILD:
				if (gameObject != null)
				{
					global::UnityEngine.Transform transform9 = gameObject.transform;
					transform9.localPosition = new global::UnityEngine.Vector3(transform9.localPosition.x, 0.15365f, transform9.localPosition.z);
					transform9.localEulerAngles = new global::UnityEngine.Vector3(0f, 0f, 0f);
					transform9.localScale = new global::UnityEngine.Vector3(1f, 1f, 1f);
				}
				if (gameObject2 != null)
				{
					global::UnityEngine.Transform transform10 = gameObject2.transform;
					transform10.localPosition = new global::UnityEngine.Vector3(transform10.localPosition.x, 0.12364999f, transform10.localPosition.z);
					transform10.localEulerAngles = new global::UnityEngine.Vector3(0f, 0f, 0f);
					transform10.localScale = new global::UnityEngine.Vector3(0.5f, 0.5f, 0.5f);
				}
				break;
			case global::Kampai.Game.MinionHair.SPIKE:
				if (gameObject != null)
				{
					global::UnityEngine.Transform transform = gameObject.transform;
					transform.localPosition = new global::UnityEngine.Vector3(transform.localPosition.x, 0.12364999f, transform.localPosition.z);
					transform.localEulerAngles = new global::UnityEngine.Vector3(0f, 0f, 0f);
					transform.localScale = new global::UnityEngine.Vector3(0.5f, 0.5f, 0.5f);
				}
				if (gameObject2 != null)
				{
					global::UnityEngine.Transform transform2 = gameObject2.transform;
					transform2.localPosition = new global::UnityEngine.Vector3(transform2.localPosition.x, 0.12364999f, transform2.localPosition.z);
					transform2.localEulerAngles = new global::UnityEngine.Vector3(0f, 180f, 180f);
					transform2.localScale = new global::UnityEngine.Vector3(1f, 1f, 1f);
				}
				break;
			}
		}

		public void DisableSelection()
		{
			foreach (global::UnityEngine.Transform item in base.transform)
			{
				global::UnityEngine.Renderer component = item.GetComponent<global::UnityEngine.Renderer>();
				if (component != null && item.name.StartsWith("selectIcon"))
				{
					logger.Debug("Disable Selection");
					component.enabled = false;
				}
			}
		}
	}
}

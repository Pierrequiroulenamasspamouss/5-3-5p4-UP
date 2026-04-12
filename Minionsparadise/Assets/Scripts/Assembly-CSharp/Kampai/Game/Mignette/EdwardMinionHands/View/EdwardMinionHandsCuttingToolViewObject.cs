namespace Kampai.Game.Mignette.EdwardMinionHands.View
{
	public class EdwardMinionHandsCuttingToolViewObject : global::UnityEngine.MonoBehaviour
	{
		private const string IDLE_ANIM_STATE_NAME = "Base Layer.Idle";

		private const string IDLE_POLE_ANIM_STATE_NAME = "Base Layer.IdleWithPole";

		private const string CHAT_ANIM_LEFT_TRIGGER_NAME = "OnChatLeft01";

		private const string CHAT_ANIM_RIGHT_TRIGGER_NAME = "OnChatRight01";

		private const string RUN_ANIM_STATE_NAME = "Base Layer.Run";

		private const string RUN_POLE_ANIM_STATE_NAME = "Base Layer.RunningWithTool";

		private const string RUN_ANIM_BOOL_NAME = "IsRunning";

		private const string CHEER_ANIM_TRIGGER_NAME = "OnCheer";

		private const string CUT_ANIM_BOOL_NAME = "IsCutting";

		public global::UnityEngine.Animator ToolAnimator;

		public global::UnityEngine.GameObject[] PoleList;

		private global::UnityEngine.ParticleSystem[] poleParticles;

		public global::Kampai.Game.View.MinionObject myMinionToUpdate;

		public global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsMignetteManagerView myParentView;

		private global::UnityEngine.Vector3 myInitialPos = global::UnityEngine.Vector3.zero;

		private global::UnityEngine.Quaternion myInitialRot = global::UnityEngine.Quaternion.identity;

		private float minionSpeed = 1f;

		private global::UnityEngine.Renderer[] renderers;

		private global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCollectableViewObject currentTargetCollectable;

		private bool poleIsShown;

		private void Start()
		{
			renderers = GetComponentsInChildren<global::UnityEngine.Renderer>(true);
			int num = global::UnityEngine.Random.Range(0, PoleList.Length);
			for (int i = 0; i < PoleList.Length; i++)
			{
				if (i == num)
				{
					PoleList[i].SetActive(true);
					poleParticles = PoleList[i].GetComponentsInChildren<global::UnityEngine.ParticleSystem>();
				}
				else
				{
					PoleList[i].SetActive(false);
				}
			}
			ShowPole(false);
		}

		public void Setup(global::Kampai.Game.View.MinionObject minionToUpdate, float minionRunSpeed, global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsMignetteManagerView parentView)
		{
			myMinionToUpdate = minionToUpdate;
			base.transform.position = myMinionToUpdate.transform.position;
			base.transform.rotation = myMinionToUpdate.transform.rotation;
			myInitialPos = myMinionToUpdate.transform.position;
			myInitialRot = myMinionToUpdate.transform.rotation;
			myParentView = parentView;
			minionSpeed = minionRunSpeed;
		}

		public void GoPickupDoober(global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsCollectableViewObject collectableViewObject)
		{
			StopCutting();
			myMinionToUpdate.SetAnimBool("IsRunning", true);
			ToolAnimator.SetBool("IsRunning", true);
			myMinionToUpdate.PlayAnimation(global::UnityEngine.Animator.StringToHash("Base Layer.Run"), 0, 0f);
			ToolAnimator.Play(global::UnityEngine.Animator.StringToHash("Base Layer.RunningWithTool"), 0);
			currentTargetCollectable = collectableViewObject;
			EmitParticles(false);
		}

		private void EmitParticles(bool emit)
		{
			global::UnityEngine.ParticleSystem[] array = poleParticles;
			foreach (global::UnityEngine.ParticleSystem ps in array)
			{
				global::UnityEngine.UnityUtils.SetEmissionEnabled(ps, emit);
			}
		}

		public void Cheer()
		{
			EmitParticles(false);
			myMinionToUpdate.SetAnimTrigger("OnCheer");
			ToolAnimator.SetTrigger("OnCheer");
		}

		public void StartCutting()
		{
			ShowPole(true);
			myMinionToUpdate.SetAnimBool("IsCutting", true);
			myMinionToUpdate.SetAnimBool("IsRunning", false);
			ToolAnimator.SetBool("IsCutting", true);
			ToolAnimator.SetBool("IsRunning", false);
		}

		public void StopCutting()
		{
			myMinionToUpdate.SetAnimBool("IsCutting", false);
			ToolAnimator.SetBool("IsCutting", false);
		}

		public bool IsCollecting()
		{
			return currentTargetCollectable != null;
		}

		public bool IsMinionIdle()
		{
			return myMinionToUpdate.IsInAnimatorState(global::UnityEngine.Animator.StringToHash("Base Layer.Idle"));
		}

		public bool IsMinionIdleWithPole()
		{
			return myMinionToUpdate.IsInAnimatorState(global::UnityEngine.Animator.StringToHash("Base Layer.IdleWithPole"));
		}

		public void StartMinionChat(bool lookLeft)
		{
			if (lookLeft)
			{
				myMinionToUpdate.SetAnimTrigger("OnChatLeft01");
			}
			else
			{
				myMinionToUpdate.SetAnimTrigger("OnChatRight01");
			}
		}

		public void ClearCollectable()
		{
			currentTargetCollectable = null;
		}

		public void ShowPole(bool show)
		{
			global::UnityEngine.Renderer[] array = renderers;
			foreach (global::UnityEngine.Renderer renderer in array)
			{
				renderer.enabled = show;
			}
			if (show)
			{
				if (poleIsShown != show)
				{
					base.transform.localScale = global::UnityEngine.Vector3.one * 0.0001f;
					EmitParticles(false);
				}
				else
				{
					EmitParticles(true);
				}
			}
			else
			{
				EmitParticles(false);
			}
			poleIsShown = show;
		}

		private void Update()
		{
			if (myParentView.IsPaused)
			{
				return;
			}
			if (myParentView.TimeElapsed >= myParentView.TotalEventTime)
			{
				myMinionToUpdate.SetAnimBool("IsRunning", false);
				myMinionToUpdate.SetAnimBool("IsCutting", false);
				ToolAnimator.SetBool("IsRunning", false);
				ToolAnimator.SetBool("IsCutting", false);
				return;
			}
			UpdateLocation();
			if (myMinionToUpdate != null)
			{
				myMinionToUpdate.transform.position = base.transform.position;
				myMinionToUpdate.transform.rotation = base.transform.rotation;
			}
			if (base.transform.localScale != global::UnityEngine.Vector3.one)
			{
				base.transform.localScale = global::UnityEngine.Vector3.MoveTowards(base.transform.localScale, global::UnityEngine.Vector3.one, global::UnityEngine.Time.deltaTime * 2f);
				if (base.transform.localScale == global::UnityEngine.Vector3.one)
				{
					EmitParticles(true);
				}
			}
		}

		private void UpdateLocation()
		{
			if (!(myMinionToUpdate != null) || !myMinionToUpdate.IsInAnimatorState(global::UnityEngine.Animator.StringToHash("Base Layer.Run")))
			{
				return;
			}
			if (currentTargetCollectable != null)
			{
				global::UnityEngine.Vector3 vector = currentTargetCollectable.transform.position - base.transform.position;
				base.transform.forward = vector.normalized;
				base.transform.position = global::UnityEngine.Vector3.MoveTowards(base.transform.position, currentTargetCollectable.transform.position, global::UnityEngine.Time.deltaTime * minionSpeed);
				if (global::UnityEngine.Vector3.Distance(base.transform.position, currentTargetCollectable.transform.position) < 0.1f)
				{
					myParentView.CollectableHasBeenCollected(currentTargetCollectable);
					currentTargetCollectable = null;
					Cheer();
				}
			}
			else if (global::UnityEngine.Vector3.Distance(base.transform.position, myInitialPos) > 0f)
			{
				global::UnityEngine.Vector3 vector2 = myInitialPos - base.transform.position;
				base.transform.forward = vector2.normalized;
				base.transform.position = global::UnityEngine.Vector3.MoveTowards(base.transform.position, myInitialPos, global::UnityEngine.Time.deltaTime * minionSpeed);
				if (global::UnityEngine.Vector3.Distance(base.transform.position, myInitialPos) <= 0.1f)
				{
					StartCutting();
					base.transform.position = myInitialPos;
					base.transform.rotation = myInitialRot;
				}
			}
		}
	}
}

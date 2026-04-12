public class PathAgent : global::UnityEngine.MonoBehaviour
{
	public const string SLIDE_ANIM_STATE_NAME = "Sliding";

	public const string JUMP_ANIM_STATE_NAME = "Jump";

	public const string CLIMB_LADDER_ANIM_STATE_NAME = "ClimbLadder";

	public const string JUMP_CANNON_ANIM_STATE_NAME = "JumpIntoCannon";

	public const string DIVE_ANIM_INT_NAME = "InDive";

	public const string HEADBONE_ATTACH_BONE = "minion:headOffset_jnt";

	public const float StartingSpeed = 5f;

	private const int pathLengthResolution = 100;

	private global::UnityEngine.Vector3 zeroOutYRotation = new global::UnityEngine.Vector3(1f, 0f, 1f);

	public WaterslideCameraController CameraController;

	public global::UnityEngine.Transform MinionHardpoint;

	public global::Kampai.Game.View.MinionObject MinionObject;

	public PathController Path;

	public WaterslideVFXManager VFXManager;

	public bool FollowSplineRotation = true;

	private float maxAccelleration = 0.05f;

	private float maxSpeed = 1f;

	private float currentSpeed = 1f;

	private float totalPathLength;

	private float elapsedTravelTime;

	private GoSpline spline;

	private bool followingPath;

	private GoTween minionRotationTween;

	public global::Kampai.Game.Mignette.WaterSlide.View.WaterSlideMignetteManagerView View { get; set; }

	public global::Kampai.Game.Mignette.WaterSlide.WaterSlideMignettePathCompletedSignal pathCompletedSignal { get; set; }

	public global::Kampai.Game.Mignette.WaterSlide.WaterslideMignetteOnDiveTriggerSignal diveTriggerSignal { get; set; }

	public global::Kampai.Game.Mignette.WaterSlide.WaterslideMignetteOnPlayDiveTriggerSignal playDiveAnimation { get; set; }

	private void Update()
	{
		if (!View.IsPaused && followingPath)
		{
			UpdateSpeed();
			UpdateAgentPosition();
		}
	}

	public void SetAtPathStart()
	{
		base.transform.position = spline.getPointOnPath(0f);
		global::UnityEngine.Vector3 pointOnPath = spline.getPointOnPath(0.01f);
		base.transform.rotation = global::UnityEngine.Quaternion.LookRotation(pointOnPath - base.transform.position);
	}

	public void StartFollowPath()
	{
		followingPath = true;
		MinionObject.PlayAnimation(global::UnityEngine.Animator.StringToHash("Sliding"), 0, 0f);
		VFXManager.DisplayMinionWake(true);
	}

	public void BuildPath()
	{
		spline = Path.GetPathSpline();
		totalPathLength = GetPathLength(spline);
		currentSpeed = 5f;
	}

	private float GetPathLength(GoSpline spline)
	{
		global::UnityEngine.Vector3 pointOnPath = spline.getPointOnPath(0f);
		float num = 0f;
		global::UnityEngine.Vector3 b = global::UnityEngine.Vector3.zero;
		for (int i = 1; i < 100; i++)
		{
			global::UnityEngine.Vector3 pointOnPath2 = spline.getPointOnPath((float)i / 100f);
			if (i == 1)
			{
				num = global::UnityEngine.Vector3.Distance(pointOnPath, pointOnPath2);
				b = pointOnPath;
			}
			else
			{
				num += global::UnityEngine.Vector3.Distance(pointOnPath2, b);
				b = pointOnPath2;
			}
		}
		return num;
	}

	private void UpdateAgentPosition()
	{
		elapsedTravelTime += global::UnityEngine.Time.deltaTime * currentSpeed;
		float num = elapsedTravelTime / totalPathLength;
		global::UnityEngine.Vector3 pointOnPath = spline.getPointOnPath(num);
		base.transform.rotation = ((!pointOnPath.Equals(base.transform.position)) ? global::UnityEngine.Quaternion.LookRotation(pointOnPath - base.transform.position) : global::UnityEngine.Quaternion.identity);
		if (!FollowSplineRotation)
		{
			zeroOutYRotation = base.transform.rotation.eulerAngles;
			zeroOutYRotation.z = 0f;
			zeroOutYRotation.x = 0f;
			base.transform.rotation = global::UnityEngine.Quaternion.Euler(zeroOutYRotation);
		}
		base.transform.position = pointOnPath;
		if (num >= 1f)
		{
			followingPath = false;
			pathCompletedSignal.Dispatch();
		}
	}

	public void ChangeSpeed(float speed, float acceleration)
	{
		maxSpeed = speed;
		maxAccelleration = acceleration;
	}

	private void UpdateSpeed()
	{
		if (currentSpeed < maxSpeed)
		{
			if (currentSpeed + maxAccelleration > maxSpeed)
			{
				currentSpeed = maxSpeed;
			}
			else
			{
				currentSpeed += maxAccelleration;
			}
		}
		else if (currentSpeed > maxSpeed)
		{
			if (currentSpeed - maxAccelleration < maxSpeed)
			{
				currentSpeed = maxSpeed;
			}
			else
			{
				currentSpeed -= maxAccelleration;
			}
		}
	}

	public float GetPctComplete()
	{
		if (totalPathLength > 0f)
		{
			return elapsedTravelTime / totalPathLength;
		}
		return 0f;
	}

	public float GetCurrentSpeed()
	{
		return currentSpeed;
	}

	public void OnCameraOverride(global::UnityEngine.Transform CameraOverrideTransform, float fieldOfView, float duration, GoEaseType easeType)
	{
		if (CameraController != null)
		{
			CameraController.AlignWithTransform(CameraOverrideTransform.transform, duration, easeType);
			if (fieldOfView > 0f)
			{
				CameraController.AlignWithFOV(fieldOfView, duration, easeType);
			}
		}
	}

	public void OnMinionTurn()
	{
		MinionObject.SetAnimBool("InTurn", true);
	}

	public void OnMinionOutofTurn()
	{
		MinionObject.SetAnimBool("InTurn", false);
	}

	public void OnRotateMinion(global::UnityEngine.Quaternion rot)
	{
		if (!rot.Equals(MinionHardpoint.localRotation))
		{
			if (minionRotationTween != null && minionRotationTween.state == GoTweenState.Running)
			{
				Go.removeTween(minionRotationTween);
			}
			GoTweenConfig goTweenConfig = new GoTweenConfig();
			RotationQuaternionTweenProperty tweenProp = new RotationQuaternionTweenProperty(rot, false, true);
			goTweenConfig.addTweenProperty(tweenProp);
			minionRotationTween = new GoTween(MinionHardpoint, 0.25f, goTweenConfig);
			Go.addTween(minionRotationTween);
		}
	}

	public void OnDiveTrigger(bool startRoll)
	{
		diveTriggerSignal.Dispatch(startRoll);
	}

	public void OnPlayDiveAnimation()
	{
		playDiveAnimation.Dispatch();
	}
}

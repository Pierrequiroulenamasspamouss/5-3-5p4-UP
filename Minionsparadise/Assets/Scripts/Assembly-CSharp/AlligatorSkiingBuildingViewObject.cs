public class AlligatorSkiingBuildingViewObject : global::Kampai.Game.Mignette.View.MignetteBuildingViewObject
{
	public global::UnityEngine.Transform IntroCamera;

	public global::UnityEngine.GameObject[] ObjectsOnDuringCooldown;

	public global::UnityEngine.GameObject[] ObjectsRandomOnDuringCooldown;

	public global::UnityEngine.Vector2 minMaxRandomObjs = new global::UnityEngine.Vector2(4f, 7f);

	public global::UnityEngine.GameObject[] ObjectsOffDuringCooldown;

	public global::UnityEngine.GameObject NeonFishGameObject;

	public float FishClickStartDelay = 0.1f;

	public float FishClickTime = 1f;

	private float fishClickTimer;

	private float fishClickDelayTimer;

	private bool isInCooldown;

	private global::Kampai.Main.PlayLocalAudioSignal myLocalAudioSignal;

	public void Start()
	{
		base.gameObject.AddComponent<global::Kampai.Game.Mignette.View.MignetteBuildingCooldownView>();
	}

	public override void ResetCooldownView(global::Kampai.Main.PlayLocalAudioSignal localAudioSignal)
	{
		myLocalAudioSignal = localAudioSignal;
		SetCooldownObjects(localAudioSignal, false);
	}

	private void SetCooldownObjects(global::Kampai.Main.PlayLocalAudioSignal localAudioSignal, bool inCooldown)
	{
		isInCooldown = inCooldown;
		global::UnityEngine.GameObject[] objectsOnDuringCooldown = ObjectsOnDuringCooldown;
		foreach (global::UnityEngine.GameObject gameObject in objectsOnDuringCooldown)
		{
			gameObject.SetActive(inCooldown);
		}
		if (ObjectsRandomOnDuringCooldown.Length > 0)
		{
			int num = global::UnityEngine.Random.Range((int)minMaxRandomObjs.x, (int)minMaxRandomObjs.y);
			int num2 = global::UnityEngine.Random.Range(0, ObjectsRandomOnDuringCooldown.Length);
			for (int j = num2; j < num; j++)
			{
				if (CheckAvailable() && j == num - 1)
				{
					RandomAnimPlayer component = ObjectsRandomOnDuringCooldown[num2].GetComponent<RandomAnimPlayer>();
					if (component != null)
					{
						component.followObj = FindRandomMinionHead();
						component.following = true;
					}
				}
				int num3 = global::UnityEngine.Random.Range(0, ObjectsRandomOnDuringCooldown.Length);
				if (j == num3)
				{
					num3 = global::UnityEngine.Random.Range(0, ObjectsRandomOnDuringCooldown.Length);
				}
				ObjectsRandomOnDuringCooldown[num3].SetActive(inCooldown);
			}
		}
		global::UnityEngine.GameObject[] objectsOffDuringCooldown = ObjectsOffDuringCooldown;
		foreach (global::UnityEngine.GameObject gameObject2 in objectsOffDuringCooldown)
		{
			gameObject2.SetActive(!inCooldown);
		}
		if (!(NeonFishGameObject != null))
		{
			return;
		}
		if (!inCooldown)
		{
			if (localAudioSignal != null)
			{
				localAudioSignal.Dispatch(global::Kampai.Util.Audio.GetAudioEmitter.Get(NeonFishGameObject, "NeonSignBuzz"), "Play_fish_neonBuzz_01", null);
			}
			return;
		}
		CustomFMOD_StudioEventEmitter component2 = NeonFishGameObject.GetComponent<CustomFMOD_StudioEventEmitter>();
		if (component2 != null)
		{
			component2.Stop();
		}
	}

	public override void UpdateCooldownView(global::Kampai.Main.PlayLocalAudioSignal localAudioSignal, int buildingData, float pctDone)
	{
		if (pctDone < 1f)
		{
			SetCooldownObjects(localAudioSignal, true);
		}
		else
		{
			SetCooldownObjects(localAudioSignal, false);
		}
	}

	public virtual void Update()
	{
		if (!(NeonFishGameObject != null) || isInCooldown)
		{
			return;
		}
		if (fishClickDelayTimer < FishClickStartDelay)
		{
			fishClickDelayTimer += global::UnityEngine.Time.deltaTime;
			return;
		}
		fishClickTimer += global::UnityEngine.Time.deltaTime;
		if (fishClickTimer >= FishClickTime)
		{
			fishClickTimer = 0f;
			if (myLocalAudioSignal != null)
			{
				myLocalAudioSignal.Dispatch(global::Kampai.Util.Audio.GetAudioEmitter.Get(NeonFishGameObject, "NeonSignCrackle"), "Play_fish_neonCrackle_01", null);
			}
		}
	}

	private bool CheckAvailable()
	{
		bool result = false;
		global::System.DateTime today = global::System.DateTime.Today;
		if (today.Day == 1 && today.Month == 4)
		{
			result = true;
		}
		return result;
	}

	private global::UnityEngine.GameObject FindRandomMinionHead()
	{
		global::UnityEngine.GameObject result = null;
		global::UnityEngine.GameObject gameObject = global::UnityEngine.GameObject.Find("Minions");
		if (gameObject != null)
		{
			global::UnityEngine.Animator[] componentsInChildren = gameObject.GetComponentsInChildren<global::UnityEngine.Animator>();
			if (componentsInChildren.Length > 0)
			{
				int num = global::UnityEngine.Random.Range(0, componentsInChildren.Length - 1);
				string text = componentsInChildren[num].gameObject.name;
				text += "/minion:ROOT/minion:pelvis_jnt/minion:spine_jnt/minion:neckStretch_jnt/minion:neck_jnt/minion:head_jnt";
				result = global::UnityEngine.GameObject.Find(text);
			}
		}
		return result;
	}
}

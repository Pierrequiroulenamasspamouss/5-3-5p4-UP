public class GhostComponentFadeHelperObject : global::UnityEngine.MonoBehaviour
{
	public global::Kampai.UI.GhostBuildingDisplayType ghostDisplayType;

	private float fadeTime = 0.5f;

	private float openDuration;

	private float BuildingFadeToAmt = 0.6f;

	private GoTween buildingTween;

	private global::Kampai.Util.Graphics.MaterialModifier materialModifier;

	private global::UnityEngine.Renderer[] buildingRenderers;

	private global::Kampai.UI.View.PopupMessageSignal popupMessageSignal;

	private global::Kampai.UI.View.CloseAllMessageDialogs closeAllDialogsSignal;

	private global::Kampai.Main.PlayGlobalSoundFXSignal sfxSignal;

	private global::Kampai.UI.IGhostComponentService ghostService;

	private bool autoFadeOut;

	private global::UnityEngine.Coroutine isClosing;

	private global::System.Collections.IEnumerator fadeCoroutine;

	public global::Kampai.Game.View.BuildingObject buildingObject { get; set; }

	public float FadeAlpha
	{
		get
		{
			return (materialModifier == null) ? 0f : global::Kampai.Util.Graphics.MaterialModifierExtensions.GetFadeAlpha(materialModifier);
		}
		set
		{
			if (materialModifier != null)
			{
				global::Kampai.Util.Graphics.MaterialModifierExtensions.SetFadeAlpha(materialModifier, value);
			}
		}
	}

	public void SetupAndDisplay(global::Kampai.UI.IGhostComponentService ghostService, global::Kampai.Game.View.BuildingObject obj, global::Kampai.UI.GhostBuildingDisplayType displayType, bool fadeIn = true)
	{
		ghostDisplayType = displayType;
		this.ghostService = ghostService;
		buildingObject = obj;
		SetListeners();
		buildingRenderers = base.gameObject.GetComponentsInChildren<global::UnityEngine.Renderer>();
		materialModifier = new global::Kampai.Util.Graphics.MaterialModifier(buildingRenderers);
		if (fadeIn)
		{
			global::Kampai.Util.Graphics.MaterialModifierExtensions.SetFadeAlpha(materialModifier, 0f);
			FadeIn();
		}
		else
		{
			global::Kampai.Util.Graphics.MaterialModifierExtensions.SetFadeAlpha(materialModifier, 1f);
		}
	}

	public void SetupAndAutoFadeWithMessage(float fadeTime, float openTime, global::Kampai.UI.View.PopupMessageSignal popupMessageSignal, global::Kampai.UI.View.CloseAllMessageDialogs closeAllDialogsSignal, global::Kampai.UI.IGhostComponentService ghostService, global::Kampai.UI.GhostBuildingDisplayType displayType, global::Kampai.Game.View.BuildingObject obj)
	{
		ghostDisplayType = displayType;
		buildingObject = obj;
		this.fadeTime = fadeTime;
		openDuration = openTime;
		this.popupMessageSignal = popupMessageSignal;
		this.closeAllDialogsSignal = closeAllDialogsSignal;
		this.ghostService = ghostService;
		SetListeners();
		buildingRenderers = base.gameObject.GetComponentsInChildren<global::UnityEngine.Renderer>();
		materialModifier = new global::Kampai.Util.Graphics.MaterialModifier(buildingRenderers);
		global::Kampai.Util.Graphics.MaterialModifierExtensions.SetFadeAlpha(materialModifier, 0f);
		autoFadeOut = true;
		FadeIn();
	}

	private void SetListeners()
	{
		if (popupMessageSignal != null)
		{
			popupMessageSignal.AddListener(CloseEarly);
		}
		if (closeAllDialogsSignal != null)
		{
			closeAllDialogsSignal.AddListener(CloseDialogReceived);
		}
	}

	private void RemoveListeners()
	{
		if (popupMessageSignal != null)
		{
			popupMessageSignal.RemoveListener(CloseEarly);
		}
		if (closeAllDialogsSignal != null)
		{
			closeAllDialogsSignal.RemoveListener(CloseDialogReceived);
		}
	}

	public void TriggerFTUEDropAnimation(string controllerName, global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal)
	{
		sfxSignal = soundFXSignal;
		global::UnityEngine.Animator animator = base.gameObject.GetComponent<global::UnityEngine.Animator>();
		if (animator == null)
		{
			animator = base.gameObject.AddComponent<global::UnityEngine.Animator>();
		}
		animator.runtimeAnimatorController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(controllerName);
	}

	private void CloseDialogReceived()
	{
		if (autoFadeOut)
		{
			if (buildingTween != null && isClosing == null)
			{
				buildingTween.complete();
				buildingTween.destroy();
			}
			StartFadeOut(false);
		}
	}

	public void PlayFTUEFX_Drop()
	{
		if (sfxSignal != null)
		{
			sfxSignal.Dispatch("Play_componentFall_woosh_01");
		}
	}

	public void PlayFTUEFX_Hit()
	{
		if (sfxSignal != null)
		{
			sfxSignal.Dispatch("Play_componentFall_metalThud_01");
		}
	}

	public void StartFadeOut(bool immediate)
	{
		if (immediate)
		{
			CleanupBuilding();
			return;
		}
		if (isClosing != null && fadeCoroutine != null)
		{
			if ((int)openDuration == 0)
			{
				return;
			}
			StopCoroutine(fadeCoroutine);
		}
		openDuration = 0f;
		fadeCoroutine = FadeOut();
		isClosing = StartCoroutine(fadeCoroutine);
	}

	private void FadeIn()
	{
		if (buildingTween != null && buildingTween.isValid())
		{
			buildingTween.destroy();
		}
		if (materialModifier == null)
		{
			return;
		}
		buildingTween = Go.to(this, fadeTime, new GoTweenConfig().floatProp("FadeAlpha", BuildingFadeToAmt).onComplete(delegate
		{
			if (autoFadeOut)
			{
				fadeCoroutine = FadeOut();
				isClosing = StartCoroutine(fadeCoroutine);
			}
		}));
	}

	private global::System.Collections.IEnumerator FadeOut()
	{
		yield return new global::UnityEngine.WaitForSeconds(openDuration);
		openDuration = 0f;
		if (buildingTween != null && buildingTween.isValid())
		{
			buildingTween.destroy();
		}
		if (materialModifier != null)
		{
			buildingTween = Go.to(this, fadeTime, new GoTweenConfig().floatProp("FadeAlpha", 0f).onComplete(delegate
			{
				CleanupBuilding();
			}));
		}
	}

	private void CloseEarly(string s, global::Kampai.UI.View.PopupMessageType messageType)
	{
		CleanupBuilding();
	}

	private void CleanupBuilding()
	{
		if (fadeCoroutine != null)
		{
			StopCoroutine(fadeCoroutine);
			fadeCoroutine = null;
		}
		RemoveListeners();
		if (buildingTween != null)
		{
			buildingTween.destroy();
			buildingTween = null;
		}
		if (materialModifier != null)
		{
			materialModifier.Destroy();
			materialModifier = null;
		}
		ghostService.GhostBuildingAutoRemoved(buildingObject.DefinitionID, this);
		global::UnityEngine.Object.Destroy(base.gameObject);
	}
}

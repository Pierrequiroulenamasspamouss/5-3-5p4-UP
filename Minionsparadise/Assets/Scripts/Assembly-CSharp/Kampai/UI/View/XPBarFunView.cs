namespace Kampai.UI.View
{
	public class XPBarFunView : global::strange.extensions.mediation.impl.View
	{
		public global::UnityEngine.ParticleSystem XPStarVFX;

		public global::UnityEngine.ParticleSystem XPImageVFX;

		public global::UnityEngine.RectTransform FillImage;

		public global::UnityEngine.UI.Image FillBarBacking;

		public global::UnityEngine.UI.Text XPAmount;

		public global::UnityEngine.UI.Text LevelAmount;

		public global::UnityEngine.UI.Text AnnouncementText;

		[global::UnityEngine.Header("FTUE Highlight")]
		public global::UnityEngine.RectTransform FTUEHighlightMeter;

		public global::UnityEngine.RectTransform FTUEHighlightXp;

		[global::UnityEngine.Header("Party Time Highlight")]
		public global::UnityEngine.RectTransform PartyTimeHighlightMeter;

		public global::UnityEngine.RectTransform PartyTimeHighlightXp;

		public global::UnityEngine.RectTransform InspirationLevelPanelHighlight;

		public global::UnityEngine.GameObject inspirationMeterSeparator;

		public global::Kampai.UI.View.KampaiImage funIcon;

		[global::UnityEngine.Header("Animation Variables")]
		public float pulseScale = 0.85f;

		public float pulseRate = 0.5f;

		internal bool expTweenAudio = true;

		internal bool expIsTweening;

		internal global::strange.extensions.signal.impl.Signal animateXP = new global::strange.extensions.signal.impl.Signal();

		private global::System.Collections.Generic.List<global::UnityEngine.GameObject> dividers;

		private GoTween _pointsTextTween;

		private GoTween _pointsFillTween;

		public int expTweenCount { get; set; }

		public void Init(global::Kampai.UI.IPositionService positionService)
		{
			dividers = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();
			positionService.AddHUDElementToAvoid(base.gameObject);
		}

		public void SetXP(uint xp, uint maxXPThisLevel)
		{
			if (maxXPThisLevel != 0)
			{
				TweenXPText(xp, maxXPThisLevel);
				TweenXPBar(xp, maxXPThisLevel);
			}
		}

		internal void TweenXPText(uint xp, uint maxXP)
		{
			if (xp > maxXP)
			{
				xp = maxXP;
			}
			if (_pointsTextTween != null)
			{
				_pointsTextTween.destroy();
			}
			_pointsTextTween = Go.to(this, 1f, new GoTweenConfig().intProp("expTweenCount", (int)xp).onUpdate(delegate
			{
				SetXPText((uint)expTweenCount, maxXP);
			}).onComplete(delegate
			{
				_pointsTextTween.destroy();
				_pointsTextTween = null;
			}));
		}

		internal void TweenXPBar(uint xp, uint maxXP, float speed = 1f)
		{
			if (xp >= maxXP)
			{
				if (FillImage.anchorMax.x >= 1f)
				{
					return;
				}
				xp = maxXP;
			}
			if (!AnnouncementText.gameObject.activeSelf)
			{
				if (_pointsFillTween != null)
				{
					_pointsFillTween.destroy();
				}
				_pointsFillTween = Go.to(FillImage, speed, new GoTweenConfig().vector2Prop("anchorMax", new global::UnityEngine.Vector2((float)xp / (float)maxXP, 1f)).onComplete(delegate
				{
					expTweenAudio = false;
					_pointsFillTween.destroy();
					_pointsFillTween = null;
				}));
			}
		}

		internal void SetXPText(uint xp, uint maxXP)
		{
			XPAmount.text = string.Format("{0}/{1}", (int)xp, (int)maxXP);
		}

		public void SetLevel(global::System.Collections.Generic.List<int> pointsEachPartyNeeds, int level)
		{
			LevelAmount.text = level.ToString();
			ClearSegments();
			SetSegments(pointsEachPartyNeeds);
			ClearBar();
		}

		public void ClearBar()
		{
			expTweenCount = 0;
			FillImage.anchorMax = new global::UnityEngine.Vector2(0f, 1f);
		}

		public void SetSegments(global::System.Collections.Generic.List<int> pointsPerEachParty)
		{
			float x = 0f;
			float num = global::System.Linq.Enumerable.Sum(pointsPerEachParty);
			float num2 = 0f;
			global::UnityEngine.Transform parent = FillBarBacking.transform;
			for (int i = 0; i < pointsPerEachParty.Count; i++)
			{
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(inspirationMeterSeparator);
				global::UnityEngine.RectTransform component = gameObject.GetComponent<global::UnityEngine.RectTransform>();
				component.SetParent(parent, false);
				component.SetAsFirstSibling();
				component.localScale = global::UnityEngine.Vector2.one;
				component.sizeDelta = global::UnityEngine.Vector2.zero;
				num2 += (float)pointsPerEachParty[i];
				component.anchorMin = new global::UnityEngine.Vector2(x, 0f);
				x = num2 / num;
				component.anchorMax = new global::UnityEngine.Vector2(x, 1f);
				dividers.Add(gameObject);
			}
		}

		public void ClearSegments()
		{
			foreach (global::UnityEngine.GameObject divider in dividers)
			{
				global::UnityEngine.Object.Destroy(divider);
			}
		}

		internal void PlayInitialVFX()
		{
			XPStarVFX.Play();
			if (!expIsTweening)
			{
				Go.to(FillBarBacking, 0.5f, new GoTweenConfig().colorProp("color", global::UnityEngine.Color.white).setIterations(2, GoLoopType.PingPong).onBegin(delegate
				{
					expIsTweening = true;
					ShowPartyTimeHighlight(true);
				})
					.onComplete(delegate
					{
						expIsTweening = false;
						ShowPartyTimeHighlight(false);
					}));
			}
		}

		internal void PlayXPVFX()
		{
			XPStarVFX.Play();
		}

		public void ShowFTUEXP(bool show)
		{
			FTUEHighlightMeter.gameObject.SetActive(show);
			FTUEHighlightXp.gameObject.SetActive(show);
		}

		public void ShowPartyTimeHighlight(bool show)
		{
			PartyTimeHighlightMeter.gameObject.SetActive(show);
		}

		public void SetAnnouncementText(string newAnnouncementText)
		{
			AnnouncementText.text = newAnnouncementText;
			AnnouncementText.gameObject.SetActive(true);
			XPAmount.gameObject.SetActive(false);
			FillImage.gameObject.SetActive(false);
			ShowPartyTimeHighlight(true);
		}

		public void ClearAnnouncement()
		{
			AnnouncementText.gameObject.SetActive(false);
			XPAmount.gameObject.SetActive(true);
			FillImage.gameObject.SetActive(true);
			ShowPartyTimeHighlight(false);
		}
	}
}

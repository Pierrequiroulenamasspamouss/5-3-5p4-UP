namespace Kampai.UI.View
{
	public class BuffInfoPopupView : global::Kampai.UI.View.PopupMenuView, IGenericPopupView
	{
		public global::Kampai.UI.View.KampaiImage BuffIcon;

		public global::UnityEngine.UI.Text BuffAmount;

		public global::UnityEngine.UI.Text BuffDescription;

		public global::UnityEngine.UI.Text CharacterName;

		public global::UnityEngine.GameObject pointer;

		public global::Kampai.UI.View.MinionSlotModal MinionSlot;

		public float Duration = 3f;

		private global::Kampai.Main.ILocalizationService localService;

		private float xBuffer = 0.02f;

		public void Init(global::Kampai.Main.ILocalizationService localizationService)
		{
			base.Init();
			localService = localizationService;
		}

		public void Display(global::UnityEngine.Vector3 itemCenter)
		{
			base.gameObject.transform.position = itemCenter;
			base.Open();
		}

		public void SetOffset(float yInput, global::UnityEngine.GameObject glassCanvas, global::UnityEngine.Vector3 itemCenter)
		{
			global::UnityEngine.Vector3 zero = global::UnityEngine.Vector3.zero;
			base.transform.localPosition = zero;
			global::UnityEngine.RectTransform rectTransform = base.transform as global::UnityEngine.RectTransform;
			rectTransform.anchorMin = itemCenter;
			rectTransform.anchorMax = itemCenter;
			global::UnityEngine.RectTransform rectTransform2 = glassCanvas.transform as global::UnityEngine.RectTransform;
			float x = rectTransform2.sizeDelta.x;
			float y = rectTransform2.sizeDelta.y;
			float num = rectTransform.sizeDelta.x / x / 2f;
			float num2 = rectTransform.sizeDelta.y / y / 2f;
			global::UnityEngine.RectTransform rectTransform3 = pointer.transform as global::UnityEngine.RectTransform;
			float num3 = rectTransform3.sizeDelta.y / y / 2f;
			float num4 = 0f;
			float num5 = rectTransform.anchorMin.x - num;
			float num6 = rectTransform.anchorMax.x + num;
			if (num5 < 0f)
			{
				num4 = 0f - num5 + xBuffer;
			}
			else if (num6 > 1f)
			{
				num4 = 1f - num6 - xBuffer;
			}
			float num7 = 0f - num2 - num3 * 2f + yInput / y;
			float num8 = 90f;
			float num9 = rectTransform3.anchorMin.y;
			if (yInput > 0f)
			{
				num7 = 0f - num7;
				num8 = 0f - num8;
				num9 = 0f - (num9 - 1f);
			}
			rectTransform.anchorMin += new global::UnityEngine.Vector2(num4, num7);
			rectTransform.anchorMax += new global::UnityEngine.Vector2(num4, num7);
			pointer.transform.eulerAngles = new global::UnityEngine.Vector3(0f, 0f, num8);
			float num10 = num4 * x / rectTransform.sizeDelta.x;
			rectTransform3.anchorMin = new global::UnityEngine.Vector2(rectTransform3.anchorMin.x - num10, num9);
			rectTransform3.anchorMax = new global::UnityEngine.Vector2(rectTransform3.anchorMax.x - num10, num9);
		}

		internal void SetBuff(global::Kampai.Game.BuffDefinition def, float currentMultiplier)
		{
			BuffIcon.maskSprite = UIUtils.LoadSpriteFromPath(def.buffSimpleMask);
			BuffDescription.text = localService.GetString(def.buffDetailLocalizedKey);
			BuffAmount.text = localService.GetString("partyBuffMultiplier", currentMultiplier);
		}

		internal void SetGuestName(string localizationKey)
		{
			CharacterName.text = localService.GetString(localizationKey);
		}
	}
}

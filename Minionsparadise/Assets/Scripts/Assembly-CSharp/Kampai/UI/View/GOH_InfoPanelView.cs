namespace Kampai.UI.View
{
	public class GOH_InfoPanelView : global::Kampai.Util.KampaiView
	{
		public global::Kampai.UI.View.MinionSlotModal minionSlot;

		public ScrollableButtonView gohPanelButton;

		public global::UnityEngine.GameObject selectedGroup;

		public global::UnityEngine.UI.Text minionName;

		[global::UnityEngine.Header("")]
		public global::UnityEngine.GameObject lockedGroup;

		public global::Kampai.UI.View.KampaiImage lockedCharacterIcon;

		public global::UnityEngine.UI.Text lockedDescription;

		[global::UnityEngine.Header("")]
		public global::UnityEngine.GameObject cooldownGroup;

		public global::UnityEngine.UI.Text cooldownDescription;

		public ScrollableButtonView cooldownRushButton;

		public global::UnityEngine.UI.Text cooldownRushCost;

		[global::UnityEngine.Header("")]
		public global::UnityEngine.GameObject availableGroup;

		public global::UnityEngine.UI.Text characterAvailDesc;

		[global::UnityEngine.Header("")]
		public global::UnityEngine.UI.Text buffTypeAmount;

		public global::Kampai.UI.View.KampaiImage buffTypeIcon;

		public global::UnityEngine.UI.Text buffDuration;

		internal int prestigeDefID;

		internal bool isLocked;

		internal int cooldown;

		internal int myIndex;

		internal bool initiallySelected;

		private global::Kampai.Game.View.DummyCharacterObject dummyCharacterObject;

		private float origZPosition;

		private bool minionAtOrigPosition;

		private global::Kampai.UI.View.GOHCardClickedSignal IAmClicked;

		internal global::strange.extensions.signal.impl.Signal rushCallBack;

		public void Init(string name, global::Kampai.UI.View.GOHCardClickedSignal gohClickedSignal)
		{
			cooldownRushButton.EnableDoubleConfirm();
			SetName(name);
			SetIAmClickedSignal(gohClickedSignal);
		}

		internal void CreateAnimatedCharacter(global::Kampai.UI.IFancyUIService fancyUIService)
		{
			global::Kampai.UI.DummyCharacterType characterType = fancyUIService.GetCharacterType(prestigeDefID);
			dummyCharacterObject = fancyUIService.CreateCharacter(characterType, global::Kampai.UI.DummyCharacterAnimationState.Happy, minionSlot.transform, minionSlot.VillainScale, minionSlot.VillainPositionOffset, prestigeDefID);
			origZPosition = (minionSlot.transform as global::UnityEngine.RectTransform).anchoredPosition3D.z;
			minionAtOrigPosition = true;
		}

		internal void SetName(string name)
		{
			minionName.text = name;
		}

		internal void SetIAmClickedSignal(global::Kampai.UI.View.GOHCardClickedSignal signal)
		{
			IAmClicked = signal;
		}

		internal void SetBuffInfo(string currentModifier, global::UnityEngine.Sprite buffMaskIcon, string duration)
		{
			buffTypeAmount.text = currentModifier;
			buffTypeIcon.maskSprite = buffMaskIcon;
			buffDuration.text = duration;
		}

		internal void SetCharacterLocked(global::UnityEngine.Sprite mask, string text)
		{
			lockedGroup.SetActive(true);
			lockedCharacterIcon.maskSprite = mask;
			lockedDescription.text = text;
		}

		internal void SetCharacterInCooldown(string cooldownText, string rushCost)
		{
			cooldownGroup.SetActive(true);
			cooldownDescription.text = cooldownText;
			cooldownRushCost.text = rushCost;
		}

		internal void SetCharacterAvailable()
		{
			availableGroup.SetActive(true);
		}

		internal void SetAvailabilityText(string availText)
		{
			characterAvailDesc.text = availText;
		}

		public void RegisterClicked()
		{
			IndicateSelected(true);
			IAmClicked.Dispatch(myIndex, !isLocked && cooldown == 0);
		}

		public void IndicateSelected(bool selected)
		{
			selectedGroup.SetActive(selected);
			if (dummyCharacterObject == null)
			{
				return;
			}
			if (selected)
			{
				if (minionAtOrigPosition)
				{
					global::UnityEngine.RectTransform rectTransform = minionSlot.transform as global::UnityEngine.RectTransform;
					rectTransform.anchoredPosition3D = new global::UnityEngine.Vector3(rectTransform.anchoredPosition3D.x, rectTransform.anchoredPosition3D.y, rectTransform.anchoredPosition3D.z + -900f);
					minionAtOrigPosition = false;
				}
			}
			else if (!minionAtOrigPosition)
			{
				global::UnityEngine.RectTransform rectTransform2 = minionSlot.transform as global::UnityEngine.RectTransform;
				rectTransform2.anchoredPosition3D = new global::UnityEngine.Vector3(rectTransform2.anchoredPosition3D.x, rectTransform2.anchoredPosition3D.y, origZPosition);
				minionAtOrigPosition = true;
			}
		}

		public void RushCharacterCooldown()
		{
			cooldownGroup.SetActive(false);
			cooldown = 0;
			SetCharacterAvailable();
		}

		public void DestroyDummyObject()
		{
			if (dummyCharacterObject != null)
			{
				dummyCharacterObject.RemoveCoroutine();
				global::UnityEngine.Object.Destroy(dummyCharacterObject.gameObject);
			}
		}
	}
}

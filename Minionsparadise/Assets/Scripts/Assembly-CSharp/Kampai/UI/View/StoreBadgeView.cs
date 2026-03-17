namespace Kampai.UI.View
{
	public class StoreBadgeView : global::Kampai.Util.KampaiView
	{
		public global::UnityEngine.UI.Text TempBadgeCount;

		public global::UnityEngine.GameObject NewUnlockImage;

		public global::UnityEngine.GameObject InventoryCountImage;

		public bool ClearOnDisable;

		private int newUnlockCounter;

		private int inventoryCounter;

		public int CurrentBadgeCount
		{
			get
			{
				return inventoryCounter + newUnlockCounter;
			}
		}

		internal void IncreaseBadgeCounter()
		{
			HideInventoryObjects(newUnlockCounter == 0);
			TempBadgeCount.text = (++newUnlockCounter).ToString();
		}

		internal void SetBadgeCount(int BadgeCount)
		{
			newUnlockCounter += BadgeCount;
			if (newUnlockCounter > 0)
			{
				HideInventoryObjects(newUnlockCounter == BadgeCount);
				TempBadgeCount.text = newUnlockCounter.ToString();
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}

		internal void SetInventoryCount(int inventoryCount)
		{
			inventoryCounter = inventoryCount;
			if (newUnlockCounter == 0)
			{
				if (inventoryCount > 0)
				{
					HideInventoryObjects(true);
					TempBadgeCount.text = inventoryCounter.ToString();
				}
				else
				{
					base.gameObject.SetActive(false);
				}
			}
		}

		internal void HideInventoryObjects(bool condition)
		{
			base.gameObject.SetActive(true);
			if (condition)
			{
				NewUnlockImage.SetActive(false);
				InventoryCountImage.SetActive(true);
			}
			TempBadgeCount.gameObject.SetActive(true);
		}

		internal void RemoveUnlockBadge(int count)
		{
			newUnlockCounter -= count;
			if (newUnlockCounter < 0)
			{
				newUnlockCounter = 0;
			}
			TempBadgeCount.text = newUnlockCounter.ToString();
		}

		internal void SetNewUnlockCounter(int count, bool showCounter = true)
		{
			newUnlockCounter = count;
			if (newUnlockCounter == 0)
			{
				NewUnlockImage.SetActive(false);
				if (inventoryCounter > 0)
				{
					base.gameObject.SetActive(true);
					InventoryCountImage.SetActive(true);
					TempBadgeCount.gameObject.SetActive(true);
					TempBadgeCount.text = newUnlockCounter.ToString();
				}
				else
				{
					base.gameObject.SetActive(false);
				}
			}
			else
			{
				base.gameObject.SetActive(true);
				NewUnlockImage.SetActive(true);
				if (showCounter)
				{
					TempBadgeCount.text = newUnlockCounter.ToString();
					TempBadgeCount.gameObject.SetActive(true);
				}
				else
				{
					TempBadgeCount.gameObject.SetActive(false);
				}
				InventoryCountImage.SetActive(false);
			}
		}

		internal void HideNew()
		{
			SetNewUnlockCounter(0);
		}

		internal void ToggleBadgeCounterVisibility(bool isHide)
		{
			inventoryCounter = 0;
			base.gameObject.SetActive(!isHide && newUnlockCounter != 0);
		}

		internal void EnableBadge(bool isEnabled)
		{
			int currentBadgeCount = CurrentBadgeCount;
			if (currentBadgeCount > 0)
			{
				base.gameObject.SetActive(isEnabled);
				TempBadgeCount.text = currentBadgeCount.ToString();
			}
		}

		private void OnDisable()
		{
			if (ClearOnDisable)
			{
				ToggleBadgeCounterVisibility(true);
			}
		}
	}
}

namespace Kampai.UI.View
{
	public class QuestDialogView : global::Kampai.Util.KampaiView
	{
		public global::UnityEngine.UI.Text DialogText;

		public global::UnityEngine.RectTransform DialogBackground;

		public global::UnityEngine.RectTransform QuestTextTransform;

		public global::Kampai.UI.View.ButtonView QuestButton;

		public global::Kampai.UI.View.KampaiImage DialogIcon;

		public global::UnityEngine.RectTransform NextArrow;

		private string dialogText;

		private int currentPageIndex;

		private global::System.Collections.Generic.IList<string> dialogPages;

		private global::Kampai.Main.ILocalizationService localizationService;

		private global::System.Collections.IEnumerator processDialogEnumerator;

		public void Init(global::Kampai.Main.ILocalizationService localizationService)
		{
			this.localizationService = localizationService;
			global::UnityEngine.RectTransform rectTransform = base.transform as global::UnityEngine.RectTransform;
			rectTransform.offsetMax = global::UnityEngine.Vector2.zero;
			rectTransform.offsetMin = global::UnityEngine.Vector2.zero;
			dialogPages = new global::System.Collections.Generic.List<string>();
			base.gameObject.SetActive(false);
		}

		internal void SetDialogIcon(string maskPath)
		{
			DialogIcon.maskSprite = UIUtils.LoadSpriteFromPath(maskPath);
		}

		public void ShowPreviousDialog()
		{
			if (dialogPages.Count != 0)
			{
				currentPageIndex = 0;
				UpdateDialog();
			}
			base.gameObject.SetActive(true);
		}

		public void ShowDialog(string dialog, global::Kampai.Util.IKampaiLogger logger)
		{
			currentPageIndex = 0;
			dialogPages.Clear();
			dialogText = dialog;
			DialogText.text = dialog;
			MoveOffDialog();
			base.gameObject.SetActive(true);
			processDialogEnumerator = ProcessDialog(logger);
			StartCoroutine(processDialogEnumerator);
		}

		protected override void OnDestroy()
		{
			if (processDialogEnumerator != null)
			{
				StopCoroutine(processDialogEnumerator);
				processDialogEnumerator = null;
			}
			base.OnDestroy();
		}

		public bool IsPageOver()
		{
			if (dialogPages.Count == 0 || currentPageIndex == dialogPages.Count)
			{
				return true;
			}
			return false;
		}

		public void UpdateDialog()
		{
			DialogText.text = dialogPages[currentPageIndex++];
			StartCoroutine(UpdateDisplay());
		}

		private void ProcessText(global::Kampai.Util.IKampaiLogger logger)
		{
			if (QuestTextTransform != null)
			{
				float height = QuestTextTransform.rect.height;
				if (DialogSizeCheck(height))
				{
					ProcessDialog_Normal();
				}
				else
				{
					BreakDialogByTextBoxSize(height, logger);
				}
			}
		}

		private void ProcessDialog_Normal()
		{
			dialogText = string.Empty;
			MoveBackDialog();
		}

		private void BreakDialogByTextBoxSize(float height, global::Kampai.Util.IKampaiLogger logger)
		{
			int pieceCount = global::UnityEngine.Mathf.CeilToInt(height / DialogBackground.rect.height);
			string text = localizationService.GetString("PunctuationDelimiters");
			if (string.IsNullOrEmpty(text))
			{
				text = ",.?!";
			}
			text += " \n\r\t>";
			char[] languageDelimiters = text.ToCharArray();
			dialogPages = UIUtils.BreakDialog(dialogText, pieceCount, languageDelimiters, logger);
			UpdateDialog();
		}

		private bool DialogSizeCheck(float height)
		{
			if (string.IsNullOrEmpty(dialogText))
			{
				return true;
			}
			if (DialogBackground.rect.height < height)
			{
				return false;
			}
			return true;
		}

		private global::System.Collections.IEnumerator ProcessDialog(global::Kampai.Util.IKampaiLogger logger)
		{
			yield return null;
			yield return null;
			ProcessText(logger);
			processDialogEnumerator = null;
		}

		private global::System.Collections.IEnumerator UpdateDisplay()
		{
			yield return null;
			if (currentPageIndex == 1)
			{
				MoveBackDialog();
			}
		}

		private void MoveOffDialog()
		{
			base.transform.localPosition = new global::UnityEngine.Vector3(0f, 2 * global::UnityEngine.Screen.height, 0f);
		}

		private void MoveBackDialog()
		{
			base.transform.localPosition = new global::UnityEngine.Vector3(0f, 0f, 0f);
		}
	}
}

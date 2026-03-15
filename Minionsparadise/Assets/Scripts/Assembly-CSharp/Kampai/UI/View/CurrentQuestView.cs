namespace Kampai.UI.View
{
	public class CurrentQuestView : global::Kampai.Util.KampaiView
	{
		public global::Kampai.UI.View.KampaiImage icon;

		public global::Kampai.UI.View.MinionSlotModal MinionSlot;

		private global::UnityEngine.Animator animator;

		private global::Kampai.UI.IFancyUIService fancyUIService;

		private global::Kampai.Game.View.DummyCharacterObject characterObject;

		private global::System.Collections.IEnumerator moveCompleteCoRoutine;

		internal void Init(int characterDefId, global::Kampai.UI.IFancyUIService fancyUIService, global::Kampai.UI.DummyCharacterType characterType)
		{
			this.fancyUIService = fancyUIService;
			animator = base.gameObject.GetComponent<global::UnityEngine.Animator>();
			global::UnityEngine.RectTransform rectTransform = MinionSlot.transform as global::UnityEngine.RectTransform;
			rectTransform.anchoredPosition3D = new global::UnityEngine.Vector3(rectTransform.anchoredPosition3D.x, rectTransform.anchoredPosition3D.y, rectTransform.anchoredPosition3D.z + -900f);
			characterObject = fancyUIService.CreateCharacter(characterType, global::Kampai.UI.DummyCharacterAnimationState.SelectedIdle, MinionSlot.transform, MinionSlot.VillainScale, MinionSlot.VillainPositionOffset, characterDefId);
		}

		internal void UpdateQuest(int characterDefId, global::Kampai.UI.DummyCharacterType characterType)
		{
			animator.Play("Close");
			moveCompleteCoRoutine = MoveComplete(characterDefId, characterType);
			StartCoroutine(moveCompleteCoRoutine);
		}

		public global::System.Collections.IEnumerator MoveComplete(int characterDefId, global::Kampai.UI.DummyCharacterType characterType)
		{
			yield return new global::UnityEngine.WaitForSeconds(1f);
			RemoveCoroutine();
			if (MinionSlot != null)
			{
				characterObject = fancyUIService.CreateCharacter(characterType, global::Kampai.UI.DummyCharacterAnimationState.SelectedIdle, MinionSlot.transform, MinionSlot.VillainScale, MinionSlot.VillainPositionOffset, characterDefId);
				animator.Play("Open");
			}
			moveCompleteCoRoutine = null;
		}

		internal void RemoveCoroutine()
		{
			if (moveCompleteCoRoutine != null)
			{
				StopCoroutine(moveCompleteCoRoutine);
				moveCompleteCoRoutine = null;
			}
			if (characterObject != null)
			{
				characterObject.RemoveCoroutine();
				global::UnityEngine.Object.Destroy(characterObject.gameObject);
			}
		}
	}
}

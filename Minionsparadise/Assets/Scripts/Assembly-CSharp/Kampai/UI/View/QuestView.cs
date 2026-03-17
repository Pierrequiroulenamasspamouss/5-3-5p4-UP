namespace Kampai.UI.View
{
	public class QuestView : global::Kampai.Util.KampaiView
	{
		public ScrollableButtonView button;

		public global::Kampai.UI.View.MinionSlotModal MinionSlot;

		[global::System.NonSerialized]
		public global::Kampai.Game.Quest quest;

		public float PaddingInPixels;

		private global::UnityEngine.Animator animator;

		private global::Kampai.Game.View.DummyCharacterObject characterObject;

		private global::Kampai.UI.IFancyUIService fancyUIService;

		private global::System.Collections.IEnumerator moveCompleteCoRoutine;

		internal void Init(global::Kampai.UI.IFancyUIService fancyUIService)
		{
			this.fancyUIService = fancyUIService;
			animator = GetComponent<global::UnityEngine.Animator>();
			global::Kampai.UI.DummyCharacterType characterType = fancyUIService.GetCharacterType(quest.GetActiveDefinition().SurfaceID);
			characterObject = fancyUIService.CreateCharacter(characterType, global::Kampai.UI.DummyCharacterAnimationState.Idle, MinionSlot.transform, MinionSlot.VillainScale, MinionSlot.VillainPositionOffset, quest.GetActiveDefinition().SurfaceID, true, true, true);
		}

		internal void UpdateQuest(global::System.Action moveCompleteAction)
		{
			animator.Play("Close");
			moveCompleteCoRoutine = MoveComplete(moveCompleteAction);
			StartCoroutine(moveCompleteCoRoutine);
		}

		public global::System.Collections.IEnumerator MoveComplete(global::System.Action moveCompleteAction)
		{
			yield return new global::UnityEngine.WaitForSeconds(1f);
			moveCompleteAction();
			RemoveCoroutine();
			global::Kampai.UI.DummyCharacterType characterType = fancyUIService.GetCharacterType(quest.GetActiveDefinition().SurfaceID);
			characterObject = fancyUIService.CreateCharacter(characterType, global::Kampai.UI.DummyCharacterAnimationState.Idle, MinionSlot.transform, MinionSlot.VillainScale, MinionSlot.VillainPositionOffset, quest.GetActiveDefinition().SurfaceID, true, true, true);
			animator.Play("Open");
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

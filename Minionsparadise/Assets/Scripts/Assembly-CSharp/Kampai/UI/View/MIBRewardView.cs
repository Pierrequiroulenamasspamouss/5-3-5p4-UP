namespace Kampai.UI.View
{
	public class MIBRewardView : global::strange.extensions.mediation.impl.View
	{
		private const string animationStateName = "anim_RewardSpinner";

		public global::Kampai.UI.View.KampaiImage image;

		public global::UnityEngine.Animator animator;

		private global::Kampai.Game.ItemDefinition[] itemDefs;

		private global::Kampai.Game.ItemDefinition pickedItemDef;

		private global::Kampai.Util.SpinnerAudio spinnerAudio;

		internal global::strange.extensions.signal.impl.Signal<global::Kampai.Game.Transaction.TransactionDefinition, global::UnityEngine.Vector3> mibRewardAnimationCompleteSignal = new global::strange.extensions.signal.impl.Signal<global::Kampai.Game.Transaction.TransactionDefinition, global::UnityEngine.Vector3>();

		internal void Init(global::Kampai.Game.Transaction.TransactionDefinition pickedTransactionDef, global::Kampai.Game.ItemDefinition pickedItemDef, global::Kampai.Game.ItemDefinition[] itemDefs, global::strange.extensions.signal.impl.Signal<string> playGlobalSFXSignal)
		{
			this.pickedItemDef = pickedItemDef;
			this.itemDefs = itemDefs;
			animator.Play("anim_RewardSpinner");
			spinnerAudio = base.gameObject.AddComponent<global::Kampai.Util.SpinnerAudio>();
			spinnerAudio.PlaySFXSignal = playGlobalSFXSignal;
			StartCoroutine(WaitTillAnimationCompletes(pickedTransactionDef));
		}

		private void SetImage(global::Kampai.Game.ItemDefinition itemDef)
		{
			image.sprite = UIUtils.LoadSpriteFromPath(itemDef.Image);
			image.maskSprite = UIUtils.LoadSpriteFromPath(itemDef.Mask);
		}

		private global::System.Collections.IEnumerator WaitTillAnimationCompletes(global::Kampai.Game.Transaction.TransactionDefinition pickedTransactionDef)
		{
			int spriteAnimationIndex = 0;
			spinnerAudio.StartSpinningSound();
			while (IsAnimationPlaying())
			{
				SetImage(itemDefs[spriteAnimationIndex]);
				int num = spriteAnimationIndex + 1;
				spriteAnimationIndex = num % itemDefs.Length;
				yield return new global::UnityEngine.WaitForSeconds(0.2f);
			}
			spinnerAudio.StopSpinningSound();
			SetImage(pickedItemDef);
			yield return new global::UnityEngine.WaitForSeconds(1f);
			mibRewardAnimationCompleteSignal.Dispatch(pickedTransactionDef, image.transform.position);
		}

		private bool IsAnimationPlaying()
		{
			global::UnityEngine.AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
			return currentAnimatorStateInfo.IsName("anim_RewardSpinner") && currentAnimatorStateInfo.normalizedTime < 1f;
		}
	}
}

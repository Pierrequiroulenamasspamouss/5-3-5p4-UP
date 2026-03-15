namespace Kampai.Util.AnimatorStateInfo
{
	public class AnimatorStateMediator : global::strange.extensions.mediation.impl.Mediator
	{
		private const string unknownState = "Unknown";

		[Inject(global::Kampai.Util.UtilElement.ANIMATOR_STATE_DEBUG_INFO)]
		public global::System.Collections.Generic.Dictionary<int, string> animatorStateInfo { get; set; }

		[Inject]
		public global::Kampai.Util.AnimatorStateInfo.AnimatorStateView view { get; set; }

		private void Update()
		{
			if (animatorStateInfo == null)
			{
				return;
			}
			view.UpdatePosition();
			int? nameHash = view.GetNameHash();
			if (!nameHash.HasValue)
			{
				view.UpdateStateName(string.Empty);
				return;
			}
			int value = nameHash.Value;
			if (!animatorStateInfo.ContainsKey(value))
			{
				view.UpdateStateName("Unknown");
			}
			else
			{
				view.UpdateStateName(animatorStateInfo[value]);
			}
		}
	}
}

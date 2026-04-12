namespace Kampai.UI.View
{
	public class TSMItemPanelMediator : global::Kampai.UI.View.KampaiMediator
	{
		private global::Kampai.Game.Trigger.TriggerRewardDefinition m_reward;

		private global::UnityEngine.Transform parent;

		[Inject]
		public global::Kampai.UI.View.TSMItemPanelView view { get; set; }

		[Inject]
		public global::Kampai.Game.ICurrencyService currencyService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			m_reward = args.Get<global::Kampai.Game.Trigger.TriggerRewardDefinition>();
			parent = args.Get<global::UnityEngine.Transform>();
			global::System.Action<global::Kampai.Game.Trigger.TriggerRewardDefinition> onPurchaseCallback = args.Get<global::System.Action<global::Kampai.Game.Trigger.TriggerRewardDefinition>>();
			bool flag = args.Get<bool>();
			view.Init(m_reward, parent, currencyService, gameContext, onPurchaseCallback);
			if (flag)
			{
				view.Disable();
			}
		}

		public override void OnRegister()
		{
		}

		public override void OnRemove()
		{
		}
	}
}

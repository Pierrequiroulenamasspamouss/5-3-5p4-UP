public class RushDialogPurchaseHelper
{
	internal global::strange.extensions.signal.impl.Signal actionSuccessfulSignal = new global::strange.extensions.signal.impl.Signal();

	private int transactionId;

	private global::System.Action transactionAction;

	private bool waitingForAdditionalGoods;

	private global::Kampai.Game.TransactionTarget transactionTarget;

	private global::Kampai.Game.TransactionArg transactionArg;

	[Inject]
	public global::Kampai.Game.IPlayerService playerService { get; set; }

	[Inject]
	public global::Kampai.Game.IDefinitionService definitionService { get; set; }

	[Inject]
	public global::Kampai.UI.View.RushDialogConfirmationSignal confirmedSignal { get; set; }

	[Inject]
	public global::Kampai.Game.LoadRushDialogSignal loadRushDialogSignal { get; set; }

	[Inject]
	public global::Kampai.UI.View.SetPremiumCurrencySignal setPremiumCurrencySignal { get; set; }

	[Inject]
	public global::Kampai.UI.View.SetGrindCurrencySignal setGrindCurrencySignal { get; set; }

	public void Init(int transactionId, global::Kampai.Game.TransactionTarget target, global::Kampai.Game.TransactionArg args = null)
	{
		this.transactionId = transactionId;
		transactionTarget = target;
		transactionArg = args;
		transactionAction = RunTransaction;
		confirmedSignal.AddListener(ConfirmClicked);
	}

	public void TryAction(bool forceConfirmation = false)
	{
		global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(transactionId);
		if (!playerService.VerifyTransaction(transactionDefinition) || forceConfirmation)
		{
			loadRushDialogSignal.Dispatch(new global::Kampai.Game.PendingCurrencyTransaction(transactionDefinition, true, 0, null, null, PurchaseButtonCallback), (transactionTarget == global::Kampai.Game.TransactionTarget.STORAGEBUILDING) ? global::Kampai.UI.View.RushDialogView.RushDialogType.STORAGE_EXPAND : global::Kampai.UI.View.RushDialogView.RushDialogType.DEFAULT);
			waitingForAdditionalGoods = true;
		}
		else
		{
			transactionAction();
		}
	}

	private void ConfirmClicked()
	{
		if (waitingForAdditionalGoods)
		{
			transactionAction();
		}
	}

	private void PurchaseButtonCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
	{
		if (pct.Success)
		{
			OnTransactionSuccess();
		}
	}

	public void StartTransaction()
	{
		playerService.StartTransaction(transactionId, transactionTarget, PurchaseButtonCallback, transactionArg);
	}

	public void RunTransaction()
	{
		playerService.RunEntireTransaction(transactionId, transactionTarget, PurchaseButtonCallback, transactionArg);
	}

	private void OnTransactionSuccess()
	{
		waitingForAdditionalGoods = false;
		actionSuccessfulSignal.Dispatch();
		setPremiumCurrencySignal.Dispatch();
		setGrindCurrencySignal.Dispatch();
	}

	public void SetTransactionAction(global::System.Action transactionAction)
	{
		this.transactionAction = transactionAction;
	}

	public void Detach()
	{
		confirmedSignal.RemoveListener(ConfirmClicked);
	}

	public void Cleanup()
	{
		Detach();
	}
}

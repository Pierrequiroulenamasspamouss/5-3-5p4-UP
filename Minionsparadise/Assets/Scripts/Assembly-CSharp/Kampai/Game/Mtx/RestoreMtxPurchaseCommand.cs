namespace Kampai.Game.Mtx
{
	public class RestoreMtxPurchaseCommand : global::strange.extensions.command.impl.Command, global::System.IDisposable
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("RestoreMtxPurchaseCommand") as global::Kampai.Util.IKampaiLogger;

		private bool _isDisposed;

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageSignal popupMessageSignal { get; set; }

		public override void Execute()
		{
			DisposedCheck();
			logger.Info("RestoreMtxPurchaseCommand executed but Nimble is disabled.");
			popupMessageSignal.Dispatch(localService.GetString("RestorePurchasesFail"), global::Kampai.UI.View.PopupMessageType.NORMAL);
		}

		public void Dispose()
		{
			Dispose(true);
			global::System.GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool fromDispose)
		{
			if (fromDispose)
			{
				DisposedCheck();
			}
			_isDisposed = true;
		}

		private void DisposedCheck()
		{
			if (_isDisposed)
			{
				throw new global::System.ObjectDisposedException(ToString());
			}
		}

		~RestoreMtxPurchaseCommand()
		{
			Dispose(false);
		}
	}
}

namespace Kampai.UI.View
{
	public class BobPointsAtStuffWayFinderView : global::Kampai.UI.View.AbstractWayFinderView
	{
		private global::Kampai.Game.Transaction.TransactionDefinition transactionDef;

		private bool _isMissingItem;

		private bool _initialSet;

		protected override string UIName
		{
			get
			{
				return "BobPointsAtStuffWayFinder";
			}
		}

		protected override string WayFinderDefaultIcon
		{
			get
			{
				return wayFinderDefinition.BobPointsAtStuffDefaultIcon;
			}
		}

		private void UpdateMissingItem(bool value)
		{
			if (_isMissingItem != value || !_initialSet)
			{
				_initialSet = true;
				_isMissingItem = value;
				if (_isMissingItem)
				{
					UpdateIcon(WayFinderDefaultIcon);
				}
				else
				{
					UpdateIcon(wayFinderDefinition.BobPointsAtStuffLandExpansionIcon);
				}
			}
		}

		internal void Init(global::Kampai.Game.ILandExpansionConfigService landExpansionService, global::Kampai.Game.IDefinitionService definitionService)
		{
			global::Kampai.Game.LandExpansionConfig expansionConfig = landExpansionService.GetExpansionConfig(playerService.GetTargetExpansion());
			transactionDef = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(expansionConfig.transactionId);
			StartCoroutine(CheckMissingItems());
		}

		private global::System.Collections.IEnumerator CheckMissingItems()
		{
			while (true)
			{
				UpdateMissingItem(playerService.IsMissingItemFromTransaction(transactionDef));
				yield return new global::UnityEngine.WaitForSeconds(0.5f);
			}
		}

		public override global::UnityEngine.Vector3 GetIndicatorPosition()
		{
			global::UnityEngine.Vector3 indicatorPosition = base.GetIndicatorPosition();
			indicatorPosition.y += wayFinderDefinition.BobPointsAtStuffYWorldOffset;
			return indicatorPosition;
		}

		protected override bool OnCanUpdate()
		{
			if (zoomCameraModel.ZoomedIn)
			{
				return false;
			}
			return true;
		}
	}
}

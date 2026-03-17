namespace Kampai.Game
{
	public interface IUpsellService
	{
		global::Kampai.Util.QuantityItem GetItemDef(int index, global::Kampai.Game.SalePackDefinition saleDefinition);

		global::Kampai.Util.QuantityItem GetItemDef(int index, global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> outputs);

		uint SumOutput(global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> inputs, int id);

		int GetInputsCount(global::Kampai.Game.SalePackDefinition saleDefinition);

		void SetupFreeItemButton(global::Kampai.UI.View.LocalizeView localizeView, global::Kampai.UI.View.ButtonView buttonView, string buttonLocKey, global::UnityEngine.RuntimeAnimatorController freeCollectButtonAnimator);

		global::Kampai.Game.Sale GetSaleInstanceFromID(global::System.Collections.Generic.List<global::Kampai.Game.Sale> playerSales, int id);

		void SetupBuyButton(global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> inputs, global::Kampai.UI.View.KampaiImage inputItemIcon, global::Kampai.Game.Definition definition);

		global::Kampai.Game.SalePackDefinition GetSalePackDefinition(int upsellDefId);

		void ScheduleSale(global::Kampai.Game.Sale saleInstance);

		void ScheduleSales(global::System.Collections.Generic.IList<global::Kampai.Game.Sale> saleInstances);

		bool IsSaleUpsellInstance(global::Kampai.Game.Sale saleInstance);

		global::Kampai.Game.Sale AddNewSaleInstance(global::Kampai.Game.SalePackDefinition salePackDefinition);
	}
}

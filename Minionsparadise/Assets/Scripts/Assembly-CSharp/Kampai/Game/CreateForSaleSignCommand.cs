namespace Kampai.Game
{
	public class CreateForSaleSignCommand : global::strange.extensions.command.impl.Command
	{
		private const int AVAILABLE_SIGN_DEFINITION_ID = 3551;

		private const int LOCKED_SIGN_DEFINITION_ID = 3561;

		[Inject]
		public int expansionID { get; set; }

		[Inject]
		public bool available { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.ILandExpansionConfigService landExpansionConfigService { get; set; }

		[Inject]
		public global::Kampai.Game.ILandExpansionService landExpansionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Game.GameElement.FOR_SALE_SIGN_PARENT)]
		public global::UnityEngine.GameObject parent { get; set; }

		public override void Execute()
		{
			if (!playerService.IsExpansionPurchased(expansionID))
			{
				global::Kampai.Game.BuildingDefinition buildingDefinition = ((!available) ? (definitionService.Get(3561) as global::Kampai.Game.BuildingDefinition) : (definitionService.Get(3551) as global::Kampai.Game.BuildingDefinition));
				if (landExpansionService.HasForSaleSign(expansionID))
				{
					landExpansionService.RemoveForSaleSign(expansionID);
				}
				global::Kampai.Game.LandExpansionConfig expansionConfig = landExpansionConfigService.GetExpansionConfig(expansionID);
				if (expansionConfig.routingSlot != null)
				{
					global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>(buildingDefinition.Prefab));
					gameObject.transform.parent = parent.transform;
					gameObject.transform.position = new global::UnityEngine.Vector3(expansionConfig.routingSlot.x, 0f, expansionConfig.routingSlot.y);
					landExpansionService.AddForSaleSign(expansionID, gameObject);
				}
			}
		}
	}
}

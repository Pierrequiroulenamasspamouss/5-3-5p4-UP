namespace Kampai.Game
{
	public class InterpolateSaleTimeCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("InterpolateSaleTimeCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IMarketplaceService marketplaceService { get; set; }

		[Inject]
		public global::Kampai.Game.MarketplaceSaleItem marketplaceItem { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.MarketplaceDefinition marketplaceDefinition = definitionService.Get<global::Kampai.Game.MarketplaceDefinition>();
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = (global::System.Collections.Generic.List<global::UnityEngine.Vector3>)marketplaceDefinition.buyTimeSpline;
			if (list == null)
			{
				logger.Error("Definition for the marketplace sale time spline is null");
				return;
			}
			AbstractGoSplineSolver abstractGoSplineSolver = CreateSplineSolver(list);
			abstractGoSplineSolver.buildPath();
			float normalizedPrice = GetNormalizedPrice();
			global::UnityEngine.Vector3 point = abstractGoSplineSolver.getPoint(normalizedPrice);
			marketplaceItem.LengthOfSale = GetTimeFromParameter(point.y);
		}

		private float GetNormalizedPrice()
		{
			int num = marketplaceItem.Definition.MinStrikePrice * marketplaceItem.QuantitySold;
			int num2 = marketplaceItem.Definition.MaxStrikePrice * marketplaceItem.QuantitySold;
			return (float)(marketplaceItem.SalePrice - num) / (float)(num2 - num);
		}

		private int GetTimeFromParameter(float t)
		{
			t = global::UnityEngine.Mathf.Clamp01(t);
			int lowPriceBuyTimeSeconds = marketplaceItem.Definition.LowPriceBuyTimeSeconds;
			int highPriceBuyTimeSeconds = marketplaceItem.Definition.HighPriceBuyTimeSeconds;
			return (int)((float)global::UnityEngine.Mathf.FloorToInt((float)lowPriceBuyTimeSeconds + (float)(highPriceBuyTimeSeconds - lowPriceBuyTimeSeconds) * t) * marketplaceService.DebugMultiplier);
		}

		private AbstractGoSplineSolver CreateSplineSolver(global::System.Collections.Generic.List<global::UnityEngine.Vector3> nodes)
		{
			if (nodes.Count == 2)
			{
				return new GoSplineStraightLineSolver(nodes);
			}
			if (nodes.Count == 3)
			{
				return new GoSplineQuadraticBezierSolver(nodes);
			}
			if (nodes.Count == 4)
			{
				return new GoSplineCubicBezierSolver(nodes);
			}
			return new GoSplineCatmullRomSolver(nodes);
		}
	}
}

namespace Kampai.Game
{
	public class ExchangeRate
	{
		private int exchangeRate = 10;

		public ExchangeRate(global::Kampai.Game.IDefinitionService service)
		{
			global::Kampai.Game.ItemDefinition itemDefinition = service.Get<global::Kampai.Game.ItemDefinition>(0);
			exchangeRate = global::UnityEngine.Mathf.FloorToInt(1f / itemDefinition.BasePremiumCost);
		}

		public int GrindToPremium(int from)
		{
			if (from > 0)
			{
				int num = from / exchangeRate;
				int num2 = ((from % exchangeRate > 0) ? 1 : 0);
				num += num2;
				if (num < 1)
				{
					num = 1;
				}
				return num;
			}
			return 0;
		}

		public int PremiumToGrind(int from)
		{
			return from * exchangeRate;
		}
	}
}

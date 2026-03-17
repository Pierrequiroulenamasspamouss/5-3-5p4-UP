namespace Kampai.Game
{
	public class Scaffolding : global::Kampai.Game.Locatable
	{
		public global::Kampai.Game.Building Building { get; set; }

		public global::Kampai.Game.Location Location { get; set; }

		public global::Kampai.Game.BuildingDefinition Definition { get; set; }

		public global::Kampai.Game.Transaction.TransactionDefinition Transaction { get; set; }

		public bool Lifted { get; set; }
	}
}

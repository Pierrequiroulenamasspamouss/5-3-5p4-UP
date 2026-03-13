namespace Kampai.Game
{
	public class GetBuffStateCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.BuffType buffType { get; set; }

		[Inject]
		public global::System.Action<float> callback { get; set; }

		[Inject]
		public global::Kampai.Game.IGuestOfHonorService gohService { get; set; }

		public override void Execute()
		{
			float currentBuffMultiplierForBuffType = gohService.GetCurrentBuffMultiplierForBuffType(buffType);
			callback(currentBuffMultiplierForBuffType);
		}
	}
}

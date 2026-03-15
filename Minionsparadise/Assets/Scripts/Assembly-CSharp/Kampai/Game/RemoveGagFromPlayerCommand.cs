namespace Kampai.Game
{
	public class RemoveGagFromPlayerCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int id { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		public override void Execute()
		{
			global::System.Collections.Generic.List<global::Kampai.Game.Item> list = playerService.GetByDefinitionId<global::Kampai.Game.Item>(id) as global::System.Collections.Generic.List<global::Kampai.Game.Item>;
			if (list != null && list.Count > 0)
			{
				playerService.Remove(list[0]);
			}
		}
	}
}

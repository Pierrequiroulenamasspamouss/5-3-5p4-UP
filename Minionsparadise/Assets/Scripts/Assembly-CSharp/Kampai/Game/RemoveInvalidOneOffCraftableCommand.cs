namespace Kampai.Game
{
	public class RemoveInvalidOneOffCraftableCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		public override void Execute()
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.Instance> instancesByDefinition = playerService.GetInstancesByDefinition<global::Kampai.Game.DynamicIngredientsDefinition>();
			foreach (global::Kampai.Game.Instance item in instancesByDefinition)
			{
				if ((item.Definition as global::Kampai.Game.DynamicIngredientsDefinition).Depreciated)
				{
					playerService.Remove(item);
				}
			}
		}
	}
}

namespace Kampai.Game
{
	public class AddFootprintCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.Locatable locatable { get; set; }

		[Inject]
		public global::Kampai.Game.Location location { get; set; }

		[Inject]
		public global::Kampai.Util.PathFinder pathFinder { get; set; }

		[Inject]
		public global::Kampai.Game.Environment environment { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		public override void Execute()
		{
			int x = location.x;
			int num = x;
			int num2 = location.y;
			string text = string.Empty;
			global::Kampai.Game.Instance instance = null;
			if (locatable is global::Kampai.Game.Building)
			{
				global::Kampai.Game.Building building = locatable as global::Kampai.Game.Building;
				text = definitionService.GetBuildingFootprint(building.Definition.FootprintID);
				instance = building;
			}
			else if (locatable is global::Kampai.Game.Plot)
			{
				global::Kampai.Game.Plot plot = locatable as global::Kampai.Game.Plot;
				text = definitionService.GetBuildingFootprint(plot.Definition.FootprintID);
				instance = plot;
			}
			string text2 = text;
			for (int i = 0; i < text2.Length; i++)
			{
				switch (text2[i])
				{
				case '.':
					environment.PlayerGrid[num, num2].Walkable = true;
					environment.PlayerGrid[num, num2].Occupied = true;
					environment.PlayerGrid[num, num2].Instance = instance;
					num++;
					break;
				case '?':
					num++;
					break;
				case '|':
					num = x;
					num2--;
					break;
				default:
					environment.PlayerGrid[num, num2].Walkable = false;
					environment.PlayerGrid[num, num2].Occupied = true;
					environment.PlayerGrid[num, num2].Instance = instance;
					num++;
					break;
				}
			}
			pathFinder.UpdateWalkableRegion();
		}
	}
}

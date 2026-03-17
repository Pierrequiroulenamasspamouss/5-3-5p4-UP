namespace Kampai.Game
{
	public class EnvironmentBuilder
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("EnvironmentBuilder") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.Environment environment { get; set; }

		public void Build(global::System.Collections.Generic.Dictionary<string, object> dict)
		{
			if (dict == null)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.DS_NULL_DEF, "The environment dictionary is null");
				return;
			}
			int num = global::System.Convert.ToInt32(dict["x"]);
			int num2 = global::System.Convert.ToInt32(dict["y"]);
			global::System.Collections.Generic.List<int> list = null;
			list = dict["definitionLayout"] as global::System.Collections.Generic.List<int>;
			if (list.Count != num * num2)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.DS_DEF_CORRUPTION, "The environment data in the json file has incorrect size.");
				return;
			}
			environment.OnDefinitionHotSwap(SetupDefinition(num, num2, list));
			environment.PlayerGrid = new global::Kampai.Game.EnvironmentGridSquare[num, num2];
			global::Kampai.Game.EnvironmentGridSquare[,] playerGrid = environment.PlayerGrid;
			global::Kampai.Game.EnvironmentGridSquareDefinition[,] definitionGrid = environment.Definition.DefinitionGrid;
			for (int num3 = num2 - 1; num3 >= 0; num3--)
			{
				for (int i = 0; i < num; i++)
				{
					playerGrid[i, num3] = new global::Kampai.Game.EnvironmentGridSquare();
					playerGrid[i, num3].Position = new global::UnityEngine.Vector2(i, num3);
					int modifier = 0;
					if (definitionGrid[i, num3].CharacterPathable)
					{
						modifier = 8;
					}
					else if (!definitionGrid[i, num3].Water && definitionGrid[i, num3].Pathable)
					{
						modifier = ((!definitionGrid[i, num3].Usable) ? 4 : 5);
					}
					playerGrid[i, num3].Modifier = modifier;
				}
			}
		}

		private global::Kampai.Game.EnvironmentDefinition SetupDefinition(int xSize, int ySize, global::System.Collections.Generic.List<int> definitionLayout)
		{
			global::Kampai.Game.EnvironmentDefinition environmentDefinition = new global::Kampai.Game.EnvironmentDefinition();
			environmentDefinition.DefinitionGrid = new global::Kampai.Game.EnvironmentGridSquareDefinition[xSize, ySize];
			global::Kampai.Game.EnvironmentGridSquareDefinition[,] definitionGrid = environmentDefinition.DefinitionGrid;
			int num = 0;
			for (int num2 = ySize - 1; num2 >= 0; num2--)
			{
				for (int i = 0; i < xSize; i++)
				{
					definitionGrid[i, num2] = new global::Kampai.Game.EnvironmentGridSquareDefinition();
					definitionGrid[i, num2].SetModifiers(definitionLayout[num]);
					definitionGrid[i, num2].Position = new global::UnityEngine.Vector2(i, num2);
					num++;
				}
			}
			return environmentDefinition;
		}

		public void DebugBuild(int xSize, int ySize, int definitionFlag, int playerFlag)
		{
			global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
			dictionary.Add("x", xSize);
			dictionary.Add("y", ySize);
			global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
			for (int i = 0; i < xSize * ySize; i++)
			{
				list.Add(definitionFlag);
			}
			global::System.Collections.Generic.List<int> list2 = new global::System.Collections.Generic.List<int>();
			for (int j = 0; j < xSize * ySize; j++)
			{
				list2.Add(playerFlag);
			}
			dictionary.Add("definitionLayout", list);
			dictionary.Add("playerLayout", list2);
			Build(dictionary);
		}
	}
}

namespace Kampai.Game
{
	public class EnableVillainIslandCollidersCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public bool enable { get; set; }

		public override void Execute()
		{
			global::UnityEngine.GameObject gameObject = global::UnityEngine.GameObject.Find("Terrain_PreFab/Terrain_VillainIsland");
			if (gameObject != null)
			{
				global::Kampai.Game.VillainIslandLocation component = gameObject.GetComponent<global::Kampai.Game.VillainIslandLocation>();
				if (component != null)
				{
					component.EnableColliders(enable);
				}
			}
		}
	}
}

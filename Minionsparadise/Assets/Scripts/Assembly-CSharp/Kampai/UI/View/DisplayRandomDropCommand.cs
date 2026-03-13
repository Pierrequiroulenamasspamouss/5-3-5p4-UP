namespace Kampai.UI.View
{
	public class DisplayRandomDropCommand : global::strange.extensions.command.impl.Command
	{
		private global::UnityEngine.Vector3 offset = new global::UnityEngine.Vector3(1.5f, 4f, 0f);

		[Inject]
		public global::Kampai.Util.Tuple<int, int> values { get; set; }

		[Inject(global::Kampai.Main.MainElement.UI_WORLDCANVAS)]
		public global::UnityEngine.GameObject worldCanvas { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Building byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Building>(values.Item2);
			if (byInstanceId != null)
			{
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(global::Kampai.Util.KampaiResources.Load("HarvestButton")) as global::UnityEngine.GameObject;
				global::Kampai.UI.View.RandomDropView component = gameObject.GetComponent<global::Kampai.UI.View.RandomDropView>();
				component.ItemDefinitionId = values.Item1;
				global::UnityEngine.Transform transform = gameObject.transform;
				transform.SetParent(worldCanvas.transform, false);
				transform.position = new global::UnityEngine.Vector3((float)byInstanceId.Location.x + offset.x, offset.y, byInstanceId.Location.y);
				global::Kampai.UI.View.KampaiImage image = component.image;
				global::Kampai.Game.ItemDefinition itemDefinition = definitionService.Get<global::Kampai.Game.ItemDefinition>(values.Item1);
				image.sprite = UIUtils.LoadSpriteFromPath(itemDefinition.Image);
				image.maskSprite = UIUtils.LoadSpriteFromPath(itemDefinition.Mask);
			}
		}
	}
}

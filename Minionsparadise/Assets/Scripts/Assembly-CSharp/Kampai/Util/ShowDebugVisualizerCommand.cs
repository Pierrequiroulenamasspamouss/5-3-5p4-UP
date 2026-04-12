namespace Kampai.Util
{
	public class ShowDebugVisualizerCommand : global::strange.extensions.command.impl.Command
	{
		private static string VISUALIZER = "popup_visualizer";

		[Inject]
		public global::UnityEngine.GameObject hitObject { get; set; }

		[Inject]
		public int ID { get; set; }

		[Inject]
		public float offset { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.IPositionService positionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService defService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.View.ActionableObject actionableObject = hitObject.GetComponent<global::Kampai.Game.View.ActionableObject>();
			if (actionableObject == null)
			{
				actionableObject = hitObject.GetComponentInParent<global::Kampai.Game.View.ActionableObject>();
			}
			if (!(actionableObject != null))
			{
				return;
			}
			global::UnityEngine.GameObject gameObject = guiService.Execute(global::Kampai.UI.View.GUIOperation.LoadUntrackedInstance, VISUALIZER);
			global::Kampai.UI.PositionData positionData = positionService.GetPositionData(hitObject.transform.position);
			gameObject.transform.position = positionData.WorldPositionInUI + new global::UnityEngine.Vector3(offset, 0f, 0f);
			global::Kampai.Util.DebugVisualizerView component = gameObject.GetComponent<global::Kampai.Util.DebugVisualizerView>();
			component.Init(positionService, hitObject, offset);
			if (ID <= 0)
			{
				global::UnityEngine.UI.Text rightText;
				component.CreateProperty(global::Kampai.Util.DebugElement.Title, "Name", actionableObject.gameObject.name, out rightText);
				component.CreateProperty(global::Kampai.Util.DebugElement.Title, "Type", actionableObject.GetType().Name, out rightText);
				global::UnityEngine.UI.Text rightText2;
				global::UnityEngine.GameObject go = component.CreateProperty(global::Kampai.Util.DebugElement.Value, "CurrentAction", actionableObject.currentAction, out rightText2, 0, -1, false);
				component.AddValueData(go, rightText2, actionableObject, actionableObject.GetType().GetProperty("currentAction"));
			}
			int id = ((ID > 0) ? ID : actionableObject.ID);
			global::Kampai.Game.Instance byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Instance>(id);
			if (byInstanceId != null)
			{
				component.CreateNoneValueProperty(global::Kampai.Util.DebugElement.Expandable, byInstanceId.GetType().Name, 0, -1, byInstanceId);
				return;
			}
			global::Kampai.Game.Definition definition = null;
			defService.TryGet<global::Kampai.Game.Definition>(id, out definition);
			if (definition != null)
			{
				component.CreateNoneValueProperty(global::Kampai.Util.DebugElement.Expandable, definition.GetType().Name, 0, -1, definition);
			}
		}
	}
}

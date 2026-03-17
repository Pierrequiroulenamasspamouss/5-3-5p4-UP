namespace Kampai.Game
{
	public class DeselectPickController : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel model { get; set; }

		[Inject]
		public global::Kampai.Game.Environment environment { get; set; }

		[Inject]
		public global::UnityEngine.Vector3 inputPosition { get; set; }

		[Inject]
		public global::Kampai.Game.MinionMoveToSignal moveSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playFXSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MinionStateChangeSignal stateChangeSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Game.View.CameraUtils cameraUtils { get; set; }

		[Inject(global::Kampai.Game.GameElement.MINION_MANAGER)]
		public global::UnityEngine.GameObject minionManager { get; set; }

		public override void Execute()
		{
			global::UnityEngine.Vector3 xZProjection = cameraUtils.GroundPlaneRaycast(inputPosition);
			global::Kampai.Game.View.MinionManagerView component = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerView>();
			global::Kampai.Util.Point p = new global::Kampai.Util.Point
			{
				XZProjection = xZProjection
			};
			if (environment.Contains(p))
			{
				MoveMinion(p, component);
			}
		}

		private void MoveMinion(global::Kampai.Util.Point p, global::Kampai.Game.View.MinionManagerView view)
		{
			if (model.SelectedMinions.Count > 0)
			{
				if (model.Points == null)
				{
					model.Points = new global::System.Collections.Generic.Queue<global::Kampai.Util.Point>(playerService.GetMinionCount());
				}
				else
				{
					model.Points.Clear();
				}
				environment.GetClosestWalkableGridSquares(p.x, p.y, playerService.GetMinionCount(), model.Points);
				routineRunner.StopTimer("MinionSelectionComplete");
				SortAndMoveMinions(view, p);
				playFXSignal.Dispatch("Play_minion_confirm_select_01");
			}
		}

		private global::System.Collections.IEnumerator MoveMinion(int id, int x, int y, bool mute, float delay)
		{
			yield return new global::UnityEngine.WaitForSeconds(delay);
			moveSignal.Dispatch(id, new global::UnityEngine.Vector3(x, 0f, y), mute);
		}

		private void SortAndMoveMinions(global::Kampai.Game.View.MinionManagerView view, global::Kampai.Util.Point p)
		{
			global::System.Collections.Generic.List<global::Kampai.Util.Tuple<int, float>> list = new global::System.Collections.Generic.List<global::Kampai.Util.Tuple<int, float>>(model.SelectedMinions.Count);
			foreach (int key in model.SelectedMinions.Keys)
			{
				global::UnityEngine.Vector3 objectPosition = view.GetObjectPosition(key);
				list.Add(new global::Kampai.Util.Tuple<int, float>(key, global::Kampai.Util.Point.DistanceSquared(p, global::Kampai.Util.Point.FromVector3(objectPosition))));
			}
			list.Sort((global::Kampai.Util.Tuple<int, float> a, global::Kampai.Util.Tuple<int, float> b) => a.Item2.CompareTo(b.Item2));
			bool mute = false;
			float num = 0f;
			bool flag = true;
			foreach (global::Kampai.Util.Tuple<int, float> item2 in list)
			{
				int item = item2.Item1;
				global::Kampai.Util.Point point = model.Points.Dequeue();
				if (flag)
				{
					playFXSignal.Dispatch("Play_minion_confirm_path_01");
					flag = false;
				}
				stateChangeSignal.Dispatch(item, global::Kampai.Game.MinionState.Selected);
				routineRunner.StartCoroutine(MoveMinion(item, point.x, point.y, mute, num));
				num += global::UnityEngine.Random.Range(0.01f, 0.2f);
				mute = true;
			}
			playFXSignal.Dispatch("Play_minion_confirm_select_01");
		}
	}
}

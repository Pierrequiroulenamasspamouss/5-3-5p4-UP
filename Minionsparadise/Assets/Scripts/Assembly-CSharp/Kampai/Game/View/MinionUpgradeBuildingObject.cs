namespace Kampai.Game.View
{
	public class MinionUpgradeBuildingObject : global::Kampai.Game.View.BuildingObject, global::Kampai.Game.View.IStartAudio
	{
		private global::Kampai.Main.PlayLocalAudioSignal playLocalAudioSignal;

		private bool playing;

		public void InitAudio(global::Kampai.Game.BuildingState creationState, global::Kampai.Main.PlayLocalAudioSignal playLocalAudioSignal)
		{
			this.playLocalAudioSignal = playLocalAudioSignal;
			StartCoroutine(WaitForGameToStart(creationState));
		}

		public void NotifyBuildingState(global::Kampai.Game.BuildingState newState)
		{
			if (newState != global::Kampai.Game.BuildingState.Broken && newState != global::Kampai.Game.BuildingState.Inaccessible)
			{
				StartLoop();
			}
		}

		private void StartLoop()
		{
			if (playLocalAudioSignal != null && !playing)
			{
				playLocalAudioSignal.Dispatch(global::Kampai.Util.Audio.GetAudioEmitter.Get(base.gameObject, "MinionUpgradeBuildingBuzz"), "Play_minionUpgradeBuilding_neon_01", null);
				playing = true;
			}
		}

		private global::System.Collections.IEnumerator WaitForGameToStart(global::Kampai.Game.BuildingState creationState)
		{
			yield return new global::UnityEngine.WaitForEndOfFrame();
			NotifyBuildingState(creationState);
		}

		protected override global::UnityEngine.Vector3 GetIndicatorPosition(bool centerY)
		{
			return new global::UnityEngine.Vector3(minColliderY.bounds.center.x, minColliderY.bounds.max.y, minColliderY.bounds.center.z);
		}
	}
}

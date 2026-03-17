namespace Kampai.Game.Mignette.BalloonBarrage.View
{
	public class BalloonBarrageBuildingViewObject : global::Kampai.Game.Mignette.View.MignetteBuildingViewObject
	{
		public enum BalloonBarrageThrowTypes
		{
			Push = 0,
			Pull = 1
		}

		public global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageBuildingViewObject.BalloonBarrageThrowTypes BalloonBarrageThrowType = global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageBuildingViewObject.BalloonBarrageThrowTypes.Pull;

		public global::UnityEngine.Transform BalloonPilotIntroLocator;

		public global::UnityEngine.GameObject BasketPrefab;

		public global::UnityEngine.GameObject MangoPrefab;

		public global::UnityEngine.GameObject MangoHitBodyVFXPrefab;

		public global::UnityEngine.GameObject MinionFaceSplatVFXPrefab;

		public global::UnityEngine.GameObject MinionHitGroundVFXPrefab;

		public global::UnityEngine.GameObject BalloonPopVFXPrefab;

		public global::UnityEngine.GameObject MangoCaughtVfxPrefab;

		public global::UnityEngine.Transform CameraTransform;

		public float FieldOfView;

		public float NearClipPlane;

		public bool BalloonIsTakingOff;

		public global::UnityEngine.GameObject[] ObjectsDisabledDuringCooldown;

		public void Start()
		{
			base.gameObject.AddComponent<global::Kampai.Game.Mignette.View.MignetteBuildingCooldownView>();
		}

		public override void ResetCooldownView(global::Kampai.Main.PlayLocalAudioSignal localAudioSignal)
		{
			for (int i = 0; i < ObjectsDisabledDuringCooldown.Length; i++)
			{
				ObjectsDisabledDuringCooldown[i].SetActive(true);
			}
		}

		public override void UpdateCooldownView(global::Kampai.Main.PlayLocalAudioSignal localAudioSignal, int buildingData, float pctDone)
		{
			int num = (int)(pctDone * (float)ObjectsDisabledDuringCooldown.Length);
			for (int i = 0; i < ObjectsDisabledDuringCooldown.Length; i++)
			{
				if (i < num)
				{
					ObjectsDisabledDuringCooldown[i].SetActive(true);
				}
				else
				{
					ObjectsDisabledDuringCooldown[i].SetActive(false);
				}
			}
		}

		public void BalloonTakingOff()
		{
			BalloonIsTakingOff = true;
		}
	}
}

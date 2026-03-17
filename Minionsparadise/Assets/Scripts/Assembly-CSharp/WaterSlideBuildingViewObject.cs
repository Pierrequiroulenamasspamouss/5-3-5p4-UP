public class WaterSlideBuildingViewObject : global::Kampai.Game.Mignette.View.MignetteBuildingViewObject
{
	public global::UnityEngine.Vector3 SpinnerBallOffset = global::UnityEngine.Vector3.forward;

	public global::UnityEngine.GameObject ClimbPoint;

	public global::UnityEngine.GameObject[] PartsDisabledDuringCooldown;

	public void Start()
	{
		base.gameObject.AddComponent<global::Kampai.Game.Mignette.View.MignetteBuildingCooldownView>();
	}

	public override void ResetCooldownView(global::Kampai.Main.PlayLocalAudioSignal localAudioSignal)
	{
		global::UnityEngine.GameObject[] partsDisabledDuringCooldown = PartsDisabledDuringCooldown;
		foreach (global::UnityEngine.GameObject gameObject in partsDisabledDuringCooldown)
		{
			gameObject.SetActive(true);
		}
		localAudioSignal.Dispatch(global::Kampai.Util.Audio.GetAudioEmitter.Get(base.gameObject, "WaterslideBuilding"), "Play_waterslide_active_loop_01", null);
	}

	public override void UpdateCooldownView(global::Kampai.Main.PlayLocalAudioSignal localAudioSignal, int buildingData, float pctDone)
	{
		if (pctDone < 1f)
		{
			global::UnityEngine.GameObject[] partsDisabledDuringCooldown = PartsDisabledDuringCooldown;
			foreach (global::UnityEngine.GameObject gameObject in partsDisabledDuringCooldown)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(false);
				}
			}
			CustomFMOD_StudioEventEmitter component = base.gameObject.GetComponent<CustomFMOD_StudioEventEmitter>();
			if (component != null)
			{
				component.Stop();
			}
		}
		else
		{
			localAudioSignal.Dispatch(global::Kampai.Util.Audio.GetAudioEmitter.Get(base.gameObject, "WaterslideBuilding"), "Play_waterslide_active_loop_01", null);
		}
	}
}

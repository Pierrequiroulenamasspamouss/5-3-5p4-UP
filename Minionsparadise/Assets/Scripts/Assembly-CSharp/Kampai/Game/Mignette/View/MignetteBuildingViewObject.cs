namespace Kampai.Game.Mignette.View
{
	public class MignetteBuildingViewObject : global::UnityEngine.MonoBehaviour
	{
		public bool UsesTimerHUD = true;

		public bool UsesProgressHUD;

		public bool UsesCounterHUD;

		public float PreCountdownDelay;

		public bool UseCountDownTimer = true;

		protected global::System.Collections.Generic.ICollection<global::UnityEngine.GameObject> DynamicCoolDownObjects = new global::System.Collections.Generic.LinkedList<global::UnityEngine.GameObject>();

		public virtual void UpdateCooldownView(global::Kampai.Main.PlayLocalAudioSignal localAudioSignal, int buildingData, float pctDone)
		{
		}

		public virtual void ResetCooldownView(global::Kampai.Main.PlayLocalAudioSignal localAudioSignal)
		{
		}

		public void AddDynamicCoolDownObject(global::UnityEngine.GameObject go)
		{
			DynamicCoolDownObjects.Add(go);
		}

		public void DestroyDynamicCoolDownObjects()
		{
			foreach (global::UnityEngine.GameObject dynamicCoolDownObject in DynamicCoolDownObjects)
			{
				global::UnityEngine.Object.Destroy(dynamicCoolDownObject);
			}
			if (DynamicCoolDownObjects.Count > 0)
			{
				DynamicCoolDownObjects = new global::System.Collections.Generic.LinkedList<global::UnityEngine.GameObject>();
			}
		}

		public bool IsDynamicCooldownObjectsLoaded()
		{
			return DynamicCoolDownObjects.Count > 0;
		}
	}
}

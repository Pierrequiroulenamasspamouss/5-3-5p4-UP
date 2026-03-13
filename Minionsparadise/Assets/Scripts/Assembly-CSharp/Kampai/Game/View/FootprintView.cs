namespace Kampai.Game.View
{
	public class FootprintView : global::Kampai.Util.KampaiView
	{
		private global::UnityEngine.Transform cachedTransform;

		private global::UnityEngine.GameObject green;

		private global::UnityEngine.GameObject red;

		public void Init()
		{
			cachedTransform = base.transform;
			green = base.transform.Find("Green").gameObject;
			red = base.transform.Find("Red").gameObject;
			green.gameObject.SetLayerRecursively(14);
			red.gameObject.SetLayerRecursively(14);
		}

		public void ToggleFootprint(bool enable)
		{
			base.gameObject.SetActive(enable);
		}

		public void ParentFootprint(global::Kampai.Game.View.ActionableObject parentObject, global::UnityEngine.Transform parentTransform, int width, int height)
		{
			if (null != parentObject)
			{
				parentObject.gameObject.SetLayerRecursively(14);
			}
			cachedTransform.parent = parentTransform;
			cachedTransform.localPosition = new global::UnityEngine.Vector3((float)width / 2f - 0.5f, 0f, (float)(-height) / 2f + 0.5f);
			cachedTransform.position = new global::UnityEngine.Vector3(cachedTransform.position.x, 0f, cachedTransform.position.z);
			cachedTransform.localScale = new global::UnityEngine.Vector3(width, height, 1f);
		}

		public void Reset()
		{
			cachedTransform.parent = null;
			cachedTransform.position = global::UnityEngine.Vector3.zero;
			cachedTransform.localScale = global::UnityEngine.Vector3.one;
		}

		public void UpdateFootprint(bool valid)
		{
			green.SetActive(valid);
			red.SetActive(!valid);
		}
	}
}

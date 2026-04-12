namespace Kampai.Util
{
	public class ParticleSystemSortOrder : global::UnityEngine.MonoBehaviour
	{
		public int SortingLayer = 1;

		private void Awake()
		{
			if (GetComponent<global::UnityEngine.ParticleSystem>() != null && GetComponent<global::UnityEngine.ParticleSystem>().GetComponent<global::UnityEngine.Renderer>() != null)
			{
				GetComponent<global::UnityEngine.ParticleSystem>().GetComponent<global::UnityEngine.Renderer>().sortingOrder = SortingLayer;
			}
		}
	}
}

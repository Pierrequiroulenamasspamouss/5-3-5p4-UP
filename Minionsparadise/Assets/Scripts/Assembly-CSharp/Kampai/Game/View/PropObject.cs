namespace Kampai.Game.View
{
	public class PropObject : global::UnityEngine.Object
	{
		public global::UnityEngine.GameObject gameObject { get; set; }

		public global::System.Collections.Generic.List<global::UnityEngine.Transform> transforms { get; set; }

		public PropObject()
		{
			gameObject = null;
			transforms = new global::System.Collections.Generic.List<global::UnityEngine.Transform>();
		}
	}
}

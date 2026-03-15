public class UnlockableScrollableButtonView : ScrollableButtonView
{
	public global::UnityEngine.GameObject AnimatableObject;

	protected override void Start()
	{
		base.Start();
		animator = AnimatableObject.GetComponent<global::UnityEngine.Animator>();
	}
}

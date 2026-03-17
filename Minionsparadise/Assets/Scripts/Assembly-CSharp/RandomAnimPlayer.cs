public class RandomAnimPlayer : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Animator anim;

	public global::UnityEngine.AnimatorStateInfo currentBaseState;

	public global::System.Collections.Generic.List<string> animNames = new global::System.Collections.Generic.List<string>();

	private int curAnimIndex;

	private int firstAnimHash;

	public float endTime = 0.7f;

	public global::UnityEngine.GameObject followObj;

	public bool following;

	public global::UnityEngine.Vector3 startPos = global::UnityEngine.Vector3.zero;

	public global::UnityEngine.Quaternion startQua = global::UnityEngine.Quaternion.identity;

	private void Start()
	{
		if (anim == null)
		{
			anim = GetComponent<global::UnityEngine.Animator>();
		}
		firstAnimHash = global::UnityEngine.Animator.StringToHash("Base Layer." + animNames[0]);
		if (followObj == null)
		{
			following = false;
			return;
		}
		startPos = base.transform.position;
		startQua = base.transform.rotation;
	}

	private void Update()
	{
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
		int fullPathHash = anim.GetCurrentAnimatorStateInfo(0).fullPathHash;
		if (fullPathHash == firstAnimHash && currentBaseState.normalizedTime > endTime)
		{
			curAnimIndex = global::UnityEngine.Random.Range(0, animNames.Count - 1);
			if (curAnimIndex < animNames.Count)
			{
				anim.Play(animNames[curAnimIndex]);
			}
		}
		if (following)
		{
			if (followObj != null)
			{
				global::UnityEngine.Vector3 position = followObj.transform.position;
				global::UnityEngine.Quaternion rotation = followObj.transform.rotation;
				base.transform.position = position;
				base.transform.rotation = rotation;
			}
			else
			{
				base.transform.position = startPos;
				base.transform.rotation = startQua;
				following = false;
			}
		}
	}
}

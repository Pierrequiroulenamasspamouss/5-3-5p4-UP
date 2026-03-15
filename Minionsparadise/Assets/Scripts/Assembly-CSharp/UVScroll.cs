public class UVScroll : global::UnityEngine.MonoBehaviour
{
	public int _uvTieX = 1;

	public int _uvTieY = 1;

	public int _seconds = 1;

	private int _fps;

	private global::UnityEngine.Vector2 _size;

	private global::UnityEngine.Renderer _myRenderer;

	private int _lastIndex = -1;

	private float time;

	private void Start()
	{
		_fps = _uvTieX * _uvTieY / _seconds;
		_size = new global::UnityEngine.Vector2(1f / (float)_uvTieX, 1f / (float)_uvTieY);
		_myRenderer = GetComponent<global::UnityEngine.Renderer>();
		if (_myRenderer == null)
		{
			base.enabled = false;
		}
		_myRenderer.material.SetTextureScale("_MainTex", _size);
		_myRenderer.material.SetTextureScale("_AlphaTex", _size);
		_myRenderer.material.SetTextureOffset("_MainTex", new global::UnityEngine.Vector2(0f, 0.75f));
		_myRenderer.material.SetTextureOffset("_AlphaTex", new global::UnityEngine.Vector2(0f, 0.75f));
	}

	private void Update()
	{
		time += global::UnityEngine.Time.deltaTime;
		int num = (int)(time * (float)_fps) % (_uvTieX * _uvTieY);
		if (num != _lastIndex)
		{
			int num2 = num % _uvTieX;
			int num3 = num / _uvTieY;
			global::UnityEngine.Vector2 offset = new global::UnityEngine.Vector2((float)num2 * _size.x, 1f - _size.y - (float)num3 * _size.y);
			_myRenderer.material.SetTextureOffset("_MainTex", offset);
			_myRenderer.material.SetTextureOffset("_AlphaTex", offset);
			_lastIndex = num;
		}
	}
}

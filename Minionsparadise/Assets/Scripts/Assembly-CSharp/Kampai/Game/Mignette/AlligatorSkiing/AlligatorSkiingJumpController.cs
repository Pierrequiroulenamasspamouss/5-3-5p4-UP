namespace Kampai.Game.Mignette.AlligatorSkiing
{
	public class AlligatorSkiingJumpController : global::UnityEngine.MonoBehaviour
	{
		public int MaxDoubleJumps = 1;

		public float Gravity = 1f;

		public float JumpHeight = 1f;

		public float SlowFallCoefficient = 1f;

		public float HangTime;

		public AlligatorAgent AlligatorAgent;

		private float _verticalSpeed;

		private bool _jumping;

		private bool _jumpingReachedApex;

		private global::UnityEngine.Vector3 _localStartPos;

		private float _slowfall = 1f;

		private float _hangTimer;

		private int _jumpCount;

		private bool _mouseUp = true;

		private void Start()
		{
			_localStartPos = base.transform.localPosition;
		}

		private void FixedUpdate()
		{
			if (_jumping)
			{
				ApplyGravity();
				MoveTransform();
			}
		}

		private void MoveTransform()
		{
			base.transform.Translate(global::UnityEngine.Vector3.up * _verticalSpeed, global::UnityEngine.Space.World);
		}

		public float CalculateJumpVerticalSpeed(float targetJumpHeight)
		{
			return global::UnityEngine.Mathf.Sqrt(2f * targetJumpHeight * Gravity);
		}

		public void ApplyJumping()
		{
			_verticalSpeed = CalculateJumpVerticalSpeed(JumpHeight);
		}

		public void ApplyGravity()
		{
			if (_jumpingReachedApex && _localStartPos.y > base.transform.localPosition.y)
			{
				_verticalSpeed = 0f;
				base.transform.localPosition = _localStartPos;
				_jumping = false;
				_jumpCount = 0;
				_mouseUp = true;
				AlligatorAgent.OnMinionLand();
			}
			if (_jumping && !_jumpingReachedApex && (double)_verticalSpeed <= 0.0)
			{
				if (!_mouseUp && _hangTimer < HangTime)
				{
					_hangTimer += global::UnityEngine.Time.deltaTime;
				}
				else
				{
					_jumpingReachedApex = true;
				}
			}
			else
			{
				_verticalSpeed -= Gravity * global::UnityEngine.Time.deltaTime * _slowfall;
			}
		}

		public void DoJump()
		{
			if (_jumpCount <= MaxDoubleJumps && _mouseUp)
			{
				_jumping = true;
				_mouseUp = false;
				_jumpingReachedApex = false;
				_slowfall = 1f;
				_hangTimer = 0f;
				_jumpCount++;
				AlligatorAgent.OnMinionJump();
				ApplyJumping();
			}
		}

		public void JumpReleased()
		{
			_mouseUp = true;
		}

		public bool IsJumping()
		{
			return _jumping;
		}
	}
}

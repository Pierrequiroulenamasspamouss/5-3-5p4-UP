namespace Kampai.Game.Mignette.BalloonBarrage.View
{
	public class BalloonBarrageMangoViewObject : global::UnityEngine.MonoBehaviour
	{
		private global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageMignetteManagerView parentView;

		public global::UnityEngine.GameObject MangoModel;

		public global::UnityEngine.ParticleSystem[] MangoImpactParticleSystems;

		public global::UnityEngine.ParticleSystem[] MangoImpactSplashParticles;

		public bool waterSplash;

		public bool hasHitGround;

		private void Start()
		{
			global::UnityEngine.ParticleSystem[] mangoImpactSplashParticles = MangoImpactSplashParticles;
			foreach (global::UnityEngine.ParticleSystem particleSystem in mangoImpactSplashParticles)
			{
				particleSystem.Stop();
				particleSystem.Clear();
			}
			MangoModel.SetActive(true);
			hasHitGround = false;
		}

		private void Update()
		{
			if (parentView.IsPaused || !(base.transform.position.y <= 0f))
			{
				return;
			}
			GetComponent<global::UnityEngine.Rigidbody>().isKinematic = true;
			global::UnityEngine.Vector3 position = base.transform.position;
			position.y = 0f;
			base.transform.position = position;
			base.transform.rotation = global::UnityEngine.Quaternion.identity;
			MangoModel.SetActive(false);
			if (!hasHitGround)
			{
				parentView.MangoHasBeenResolved(true);
				hasHitGround = true;
			}
			if (!waterSplash)
			{
				global::UnityEngine.ParticleSystem[] mangoImpactSplashParticles = MangoImpactSplashParticles;
				foreach (global::UnityEngine.ParticleSystem particleSystem in mangoImpactSplashParticles)
				{
					particleSystem.Play();
					waterSplash = true;
				}
			}
		}

		public void ThrowMango(global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageMignetteManagerView parent, global::UnityEngine.GameObject referenceGO, global::UnityEngine.Vector3 target, float force)
		{
			parentView = parent;
			base.transform.position = referenceGO.transform.position;
			target.y = base.transform.position.y;
			global::UnityEngine.Vector3 vector = target - base.transform.position;
			base.transform.rotation = referenceGO.transform.rotation;
			GetComponent<global::UnityEngine.Rigidbody>().AddForce(vector.normalized * force);
			GetComponent<global::UnityEngine.Rigidbody>().AddTorque(base.transform.right * 2000f);
		}

		private void OnTriggerEnter(global::UnityEngine.Collider other)
		{
			if (hasHitGround)
			{
				return;
			}
			global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageColliderViewObject component = other.gameObject.GetComponent<global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageColliderViewObject>();
			if (component != null && parentView.MangoHitMovingTarget(this, component))
			{
				parentView.MangoHasBeenResolved(false);
				global::UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject component2 = other.gameObject.GetComponent<global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject>();
			if (component2 != null && !component2.IsAFlyer())
			{
				parentView.MangoHitStaticTarget(this, component2);
				parentView.MangoHasBeenResolved(false);
				global::UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageGroundCollider component3 = other.gameObject.GetComponent<global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageGroundCollider>();
			if (component3 != null)
			{
				GetComponent<global::UnityEngine.Rigidbody>().isKinematic = true;
				global::UnityEngine.Vector3 position = base.transform.position;
				position.y = 0f;
				base.transform.position = position;
				base.transform.rotation = global::UnityEngine.Quaternion.identity;
				MangoModel.SetActive(false);
				if (!hasHitGround)
				{
					parentView.MangoHasBeenResolved(true);
					hasHitGround = true;
					GetComponent<global::UnityEngine.Collider>().enabled = false;
				}
				global::UnityEngine.ParticleSystem[] mangoImpactParticleSystems = MangoImpactParticleSystems;
				foreach (global::UnityEngine.ParticleSystem particleSystem in mangoImpactParticleSystems)
				{
					particleSystem.Stop();
					particleSystem.Clear();
					particleSystem.Play();
					waterSplash = true;
				}
				global::UnityEngine.ParticleSystem[] mangoImpactSplashParticles = MangoImpactSplashParticles;
				foreach (global::UnityEngine.ParticleSystem particleSystem2 in mangoImpactSplashParticles)
				{
					particleSystem2.Stop();
				}
			}
		}
	}
}

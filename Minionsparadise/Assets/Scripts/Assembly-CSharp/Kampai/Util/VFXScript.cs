namespace Kampai.Util
{
	[global::System.Serializable]
	public class VFXScript : global::UnityEngine.MonoBehaviour
	{
		private const string TRIGGER_PREFIX = "trigger_";

		public global::System.Collections.Generic.List<global::UnityEngine.GameObject> state1FXs;

		public global::System.Collections.Generic.List<global::UnityEngine.GameObject> state2FXs;

		public global::System.Collections.Generic.List<global::UnityEngine.GameObject> state3FXs;

		public global::System.Collections.Generic.List<global::UnityEngine.GameObject> state4FXs;

		public global::System.Collections.Generic.List<global::UnityEngine.GameObject> state5FXs;

		public global::System.Collections.Generic.List<global::UnityEngine.GameObject> state6FXs;

		public global::System.Collections.Generic.List<global::UnityEngine.GameObject> state7FXs;

		public global::System.Collections.Generic.List<global::UnityEngine.GameObject> state8FXs;

		public global::System.Collections.Generic.List<global::UnityEngine.GameObject> state9FXs;

		public global::System.Collections.Generic.List<global::UnityEngine.GameObject> state10FXs;

		public global::System.Collections.Generic.List<global::UnityEngine.GameObject> state11FXs;

		public global::System.Collections.Generic.List<global::UnityEngine.GameObject> state12FXs;

		public string state1ID;

		public string state2ID;

		public string state3ID;

		public string state4ID;

		public string state5ID;

		public string state6ID;

		public string state7ID;

		public string state8ID;

		public string state9ID;

		public string state10ID;

		public string state11ID;

		public string state12ID;

		private global::System.Collections.Generic.Dictionary<string, global::System.Collections.Generic.IList<global::UnityEngine.ParticleSystem>> states;

		public void Init()
		{
			states = new global::System.Collections.Generic.Dictionary<string, global::System.Collections.Generic.IList<global::UnityEngine.ParticleSystem>>();
			if (!string.IsNullOrEmpty(state1ID))
			{
				AddParticleSystems(state1ID, state1FXs);
			}
			if (!string.IsNullOrEmpty(state2ID))
			{
				AddParticleSystems(state2ID, state2FXs);
			}
			if (!string.IsNullOrEmpty(state3ID))
			{
				AddParticleSystems(state3ID, state3FXs);
			}
			if (!string.IsNullOrEmpty(state4ID))
			{
				AddParticleSystems(state4ID, state4FXs);
			}
			if (!string.IsNullOrEmpty(state5ID))
			{
				AddParticleSystems(state5ID, state5FXs);
			}
			if (!string.IsNullOrEmpty(state6ID))
			{
				AddParticleSystems(state6ID, state6FXs);
			}
			if (!string.IsNullOrEmpty(state7ID))
			{
				AddParticleSystems(state7ID, state7FXs);
			}
			if (!string.IsNullOrEmpty(state8ID))
			{
				AddParticleSystems(state8ID, state8FXs);
			}
			if (!string.IsNullOrEmpty(state9ID))
			{
				AddParticleSystems(state9ID, state9FXs);
			}
			if (!string.IsNullOrEmpty(state10ID))
			{
				AddParticleSystems(state10ID, state10FXs);
			}
			if (!string.IsNullOrEmpty(state11ID))
			{
				AddParticleSystems(state11ID, state11FXs);
			}
			if (!string.IsNullOrEmpty(state12ID))
			{
				AddParticleSystems(state12ID, state12FXs);
			}
		}

		private void AddParticleSystems(string stateName, global::System.Collections.Generic.IEnumerable<global::UnityEngine.GameObject> parents)
		{
			foreach (global::UnityEngine.GameObject parent in parents)
			{
				if (!(parent != null))
				{
					continue;
				}
				global::UnityEngine.ParticleSystem[] components = parent.GetComponents<global::UnityEngine.ParticleSystem>();
				foreach (global::UnityEngine.ParticleSystem item in components)
				{
					if (!states.ContainsKey(stateName))
					{
						states.Add(stateName, new global::System.Collections.Generic.List<global::UnityEngine.ParticleSystem>());
					}
					states[stateName].Add(item);
				}
			}
		}

		public void AnimVFX(string stateName)
		{
			TriggerState(stateName);
		}

		internal void TriggerState(string stateName)
		{
			if (stateName.StartsWith("trigger_"))
			{
				stateName = stateName.Substring("trigger_".Length);
				TriggerAnimVFX(stateName);
				return;
			}
			global::System.Collections.Generic.ICollection<global::UnityEngine.ParticleSystem> collection = new global::System.Collections.Generic.List<global::UnityEngine.ParticleSystem>();
			global::System.Collections.Generic.ICollection<global::UnityEngine.ParticleSystem> collection2 = ((!states.ContainsKey(stateName)) ? new global::System.Collections.Generic.List<global::UnityEngine.ParticleSystem>() : states[stateName]);
			foreach (string key in states.Keys)
			{
				if (key.Equals(stateName))
				{
					continue;
				}
				foreach (global::UnityEngine.ParticleSystem item in states[key])
				{
					if (!collection2.Contains(item) && !collection.Contains(item))
					{
						collection.Add(item);
					}
				}
			}
			UpdateFX(collection2, collection);
		}

		private void TriggerAnimVFX(string key)
		{
			if (states.ContainsKey(key))
			{
				foreach (global::UnityEngine.ParticleSystem item in states[key])
				{
					item.Play();
				}
				return;
			}
			global::UnityEngine.Debug.LogWarning("No such AnimVFX trigger, skipping: " + key);
		}

		private void UpdateFX(global::System.Collections.Generic.IEnumerable<global::UnityEngine.ParticleSystem> enabled, global::System.Collections.Generic.IEnumerable<global::UnityEngine.ParticleSystem> disabled)
		{
			foreach (global::UnityEngine.ParticleSystem item in enabled)
			{
				item.Play();
			}
			foreach (global::UnityEngine.ParticleSystem item2 in disabled)
			{
				item2.Stop();
			}
		}
	}
}

namespace Kampai.Game.View
{
	public class ObjectManagerView : global::Kampai.Game.View.ActionableObjectManagerView
	{
		protected global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("ObjectManagerView") as global::Kampai.Util.IKampaiLogger;

		protected global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.View.ActionableObject> objects = new global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.View.ActionableObject>();

		protected global::System.Collections.Generic.Dictionary<string, global::UnityEngine.RuntimeAnimatorController> animationControllers = new global::System.Collections.Generic.Dictionary<string, global::UnityEngine.RuntimeAnimatorController>();

		public virtual void Init()
		{
			global::UnityEngine.RuntimeAnimatorController value = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>("asm_minion_movement");
			animationControllers.Add("asm_minion_movement", value);
			global::Kampai.Game.View.ActionableObjectManagerView.ClearAllObjects();
		}

		public void CacheAnimations(global::System.Collections.Generic.IEnumerable<global::Kampai.Game.AnimationDefinition> animationDefinitions)
		{
			foreach (global::Kampai.Game.AnimationDefinition animationDefinition in animationDefinitions)
			{
				CacheAnimation(animationDefinition);
			}
		}

		public global::System.Collections.IEnumerator CacheAnimationsCoroutine<T>(global::System.Collections.Generic.IEnumerable<T> animationDefinitions) where T : global::Kampai.Game.AnimationDefinition
		{
			global::System.Diagnostics.Stopwatch sw = global::System.Diagnostics.Stopwatch.StartNew();
			foreach (T animDef in animationDefinitions)
			{
				CacheAnimation(animDef);
				if (sw.ElapsedMilliseconds > 1000)
				{
					sw = global::System.Diagnostics.Stopwatch.StartNew();
					yield return null;
				}
			}
		}

		protected virtual void CacheAnimation(global::Kampai.Game.AnimationDefinition animDef)
		{
			string animationStateMachine = GetAnimationStateMachine(animDef);
			if (animationStateMachine.Length == 0)
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "Empty State Machine");
			}
			else if (!animationControllers.ContainsKey(animationStateMachine))
			{
				animationControllers.Add(animationStateMachine, global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(animationStateMachine));
			}
		}

		protected virtual string GetAnimationStateMachine(global::Kampai.Game.AnimationDefinition animDef)
		{
			global::Kampai.Game.MinionAnimationDefinition minionAnimationDefinition = animDef as global::Kampai.Game.MinionAnimationDefinition;
			if (minionAnimationDefinition != null)
			{
				return minionAnimationDefinition.StateMachine;
			}
			return string.Empty;
		}

		public virtual void Add(int id, global::Kampai.Game.View.ActionableObject obj)
		{
			if (objects.ContainsKey(id))
			{
				logger.Error("ObjectManagerView: Tried to add an object with an id that has already been added ({0})", id);
				return;
			}
			objects.Add(id, obj);
			if (global::Kampai.Game.View.ActionableObjectManagerView.allObjects.ContainsKey(id))
			{
				logger.Error("ObjectManagerView: Global objects dictionary already has an object with id {0}. Expect bugs.", id);
			}
			else
			{
				global::Kampai.Game.View.ActionableObjectManagerView.allObjects.Add(id, obj);
			}
		}

		public virtual void Add(global::Kampai.Game.View.ActionableObject obj)
		{
			Add(obj.ID, obj);
		}

		public virtual void Remove(int id)
		{
			objects.Remove(id);
			global::Kampai.Game.View.ActionableObjectManagerView.allObjects.Remove(id);
		}

		public void EnableRenderer(int minionID, bool enable)
		{
			global::Kampai.Game.View.ActionableObject value;
			if (objects.TryGetValue(minionID, out value))
			{
				value.EnableRenderers(enable);
			}
		}

		public bool HasObject(int objectId)
		{
			return objects.ContainsKey(objectId);
		}

		public global::UnityEngine.Vector3 GetObjectPosition(int objectId)
		{
			global::Kampai.Game.View.ActionableObject value;
			if (objects.TryGetValue(objectId, out value))
			{
				return value.gameObject.transform.position;
			}
			return global::UnityEngine.Vector3.zero;
		}

		public global::Kampai.Game.View.ActionableObject Get(int objectId)
		{
			global::Kampai.Game.View.ActionableObject value;
			objects.TryGetValue(objectId, out value);
			return value;
		}

		public global::UnityEngine.GameObject GetGameObject(int objectId)
		{
			global::Kampai.Game.View.ActionableObject value;
			if (objects.TryGetValue(objectId, out value))
			{
				return value.gameObject;
			}
			return null;
		}

		public void GetPathingObjects(global::System.Collections.Generic.IList<global::Kampai.Util.Tuple<int, global::UnityEngine.Vector3>> pathingObjects)
		{
			foreach (global::System.Collections.Generic.KeyValuePair<int, global::Kampai.Game.View.ActionableObject> @object in objects)
			{
				global::Kampai.Game.View.PathAction pathAction = @object.Value.currentAction as global::Kampai.Game.View.PathAction;
				if (pathAction != null)
				{
					pathingObjects.Add(new global::Kampai.Util.Tuple<int, global::UnityEngine.Vector3>(@object.Key, pathAction.GoalPosition));
				}
			}
		}

		public void GetObjectsInArea(global::Kampai.Util.Point ll, global::Kampai.Util.Point ur, global::System.Collections.Generic.IList<global::Kampai.Game.View.ActionableObject> objectList)
		{
			foreach (global::Kampai.Game.View.ActionableObject value in objects.Values)
			{
				global::UnityEngine.Vector3 position = value.gameObject.transform.position;
				if ((float)ll.x <= position.x && (float)ll.y <= position.z && (float)ur.x >= position.x && (float)ur.y >= position.z)
				{
					objectList.Add(value);
				}
			}
		}

		public virtual void StartPathing(int minionID, global::System.Collections.Generic.IList<global::UnityEngine.Vector3> path, float speed, bool muteStatus, global::Kampai.Game.MoveMinionFinishedSignal moveFinished)
		{
			global::Kampai.Game.View.ActionableObject value;
			if (!objects.TryGetValue(minionID, out value))
			{
				logger.Error("Cannot start pathing for minion {0}, ActionableObject not in selected minions dictionary.", minionID);
			}
			else
			{
				StartPathing(value, path, speed, muteStatus, moveFinished, -180f);
			}
		}

		public virtual void StartPathing(global::Kampai.Game.View.ActionableObject mo, global::System.Collections.Generic.IList<global::UnityEngine.Vector3> path, float speed, bool muteStatus, global::Kampai.Game.MoveMinionFinishedSignal moveFinished, float rotateOffset)
		{
			if (mo == null)
			{
				logger.Error("Cannot start pathing for minion.");
				return;
			}
			mo.EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(mo, animationControllers["asm_minion_movement"], logger, new global::System.Collections.Generic.Dictionary<string, object> { { "IdleRandom", 0 } }), true);
			mo.EnqueueAction(new global::Kampai.Game.View.MuteAction(mo, muteStatus, logger));
			mo.EnqueueAction(new global::Kampai.Game.View.ConstantSpeedPathAction(mo, path, speed, logger));
			mo.EnqueueAction(new global::Kampai.Game.View.RotateAction(mo, global::UnityEngine.Camera.main.transform.eulerAngles.y + rotateOffset, 360f, logger));
			mo.EnqueueAction(new global::Kampai.Game.View.MuteAction(mo, false, logger));
			mo.EnqueueAction(new global::Kampai.Game.View.SendIDSignalAction(mo, moveFinished, logger));
		}

		protected global::System.Collections.Generic.ICollection<global::Kampai.Game.View.ActionableObject> ToActionableObjects(global::System.Collections.Generic.ICollection<int> minionIds)
		{
			global::System.Collections.Generic.List<global::Kampai.Game.View.ActionableObject> list = new global::System.Collections.Generic.List<global::Kampai.Game.View.ActionableObject>();
			foreach (int minionId in minionIds)
			{
				list.Add(objects[minionId]);
			}
			return list;
		}
	}
}

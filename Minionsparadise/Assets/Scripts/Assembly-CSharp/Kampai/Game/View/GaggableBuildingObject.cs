namespace Kampai.Game.View
{
	public class GaggableBuildingObject : global::Kampai.Game.View.TaskableBuildingObject, global::Kampai.Game.View.IRequiresBuildingScaffolding
	{
		internal virtual bool TriggerGagAnimation()
		{
			if (GetActiveMinionCount() == stations)
			{
				global::System.Collections.Generic.IList<global::Kampai.Game.View.TaskingMinionObject> list = new global::System.Collections.Generic.List<global::Kampai.Game.View.TaskingMinionObject>();
				global::System.Collections.Generic.IList<global::Kampai.Game.View.ActionableObject> list2 = new global::System.Collections.Generic.List<global::Kampai.Game.View.ActionableObject>();
				list2.Add(this);
				for (int i = 0; i < stations; i++)
				{
					global::Kampai.Game.View.TaskingMinionObject byRouteSlot = GetByRouteSlot(i);
					if (byRouteSlot == null)
					{
						return false;
					}
					list.Add(byRouteSlot);
					list2.Add(byRouteSlot.Minion);
				}
				global::Kampai.Game.View.SyncAction action = new global::Kampai.Game.View.SyncAction(list2, logger);
				global::Kampai.Game.View.SyncAction action2 = new global::Kampai.Game.View.SyncAction(list2, logger);
				int hashAnimationState = GetHashAnimationState("Base Layer.Gag");
				for (int j = 0; j < stations; j++)
				{
					global::Kampai.Game.View.TaskingMinionObject taskingMinionObject = list[j];
					global::Kampai.Game.View.MinionObject minion = taskingMinionObject.Minion;
					minion.EnqueueAction(action);
					minion.EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(minion, null, logger, OnlyStateEnabled("OnGag")));
					string stateName = "Base Layer.Gag_Pos" + (taskingMinionObject.RoutingIndex + 1);
					int hashAnimationState2 = GetHashAnimationState(stateName);
					global::Kampai.Game.View.SkipToTime skipToTime = new global::Kampai.Game.View.SkipToTime(0, hashAnimationState2, GetCurrentAnimationTimeForState(hashAnimationState));
					minion.EnqueueAction(new global::Kampai.Game.View.SkipToTimeAction(minion, skipToTime, logger));
					minion.EnqueueAction(action2);
					minion.EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(minion, null, logger, OnlyStateEnabled("OnLoop")));
				}
				EnqueueAction(action);
				EnqueueAction(new global::Kampai.Game.View.TriggerBuildingAnimationAction(this, OnlyStateEnabled("OnGag"), logger));
				EnqueueAction(new global::Kampai.Game.View.WaitForMecanimStateAction(this, GetHashAnimationState("Base Layer.Gag"), logger));
				EnqueueAction(action2);
				EnqueueAction(new global::Kampai.Game.View.TriggerBuildingAnimationAction(this, OnlyStateEnabled("OnLoop"), logger, "Base Layer.Loop"));
				return true;
			}
			return false;
		}

		internal bool IsGagAnimationPlaying()
		{
			return IsInAnimatorState(GetHashAnimationState("Base Layer.Gag"));
		}

		internal void StopGagAnimation(bool resetTrigger = false)
		{
			for (int i = 0; i < stations; i++)
			{
				global::Kampai.Game.View.TaskingMinionObject byRouteSlot = GetByRouteSlot(i);
				if (byRouteSlot != null)
				{
					global::Kampai.Game.View.MinionObject minion = byRouteSlot.Minion;
					if (resetTrigger)
					{
						minion.ClearActionQueue();
						minion.ExecuteAction(new global::Kampai.Game.View.SetAnimatorAction(minion, null, logger, OnlyStateEnabled("OnLoop")));
					}
					else
					{
						minion.EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(minion, null, logger, OnlyStateEnabled("OnStop")), true);
						minion.EnqueueAction(new global::Kampai.Game.View.SetAnimatorAction(minion, null, logger, OnlyStateEnabled("OnLoop")));
					}
				}
			}
			if (resetTrigger)
			{
				ClearActionQueue();
				ExecuteAction(new global::Kampai.Game.View.TriggerBuildingAnimationAction(this, OnlyStateEnabled("OnLoop"), logger, "Base Layer.Loop"));
			}
			else
			{
				EnqueueAction(new global::Kampai.Game.View.TriggerBuildingAnimationAction(this, OnlyStateEnabled("OnStop"), logger), true);
				EnqueueAction(new global::Kampai.Game.View.TriggerBuildingAnimationAction(this, OnlyStateEnabled("OnLoop"), logger, "Base Layer.Loop"));
			}
		}

		public override void UntrackChild(int minionId, global::Kampai.Game.TaskableBuilding building)
		{
			base.UntrackChild(minionId, building);
			if (!building.IsEligibleForGag())
			{
				return;
			}
			foreach (global::UnityEngine.Animator animator in animators)
			{
				if (animator.GetBool("OnGag"))
				{
					StopGagAnimation(true);
				}
			}
		}
	}
}

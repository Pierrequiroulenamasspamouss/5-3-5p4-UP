namespace Kampai.Util
{
	public class PartyFavorAnimationService : global::Kampai.Util.IPartyFavorAnimationService
	{
		private readonly global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.View.PartyFavorAnimationView> partyFavorCache = new global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.View.PartyFavorAnimationView>();

		private global::System.Collections.Generic.HashSet<int> allPartyFavorItems;

		private global::System.Collections.Generic.List<int> availablePartyFavorItems = new global::System.Collections.Generic.List<int>();

		private bool initialized;

		private bool randomPartyFavorStarted;

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.Environment environment { get; set; }

		[Inject]
		public global::Kampai.Util.PathFinder pathFinder { get; set; }

		[Inject]
		public global::Kampai.Game.DebugUpdateGridSignal debugUpdateGridSignal { get; set; }

		[Inject]
		public global::Kampai.Game.AddMinionsPartyFavorSignal addMinionsPartyFavorSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.MinionPartyAnimationSignal minionPartyAnimationSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MinionPartyFavorIncidentalAnimationSignal incidentalAnimationSignal { get; set; }

		[Inject]
		public global::Kampai.Common.IRandomService randomService { get; set; }

		[Inject]
		public global::Kampai.Game.AddSpecificMinionPartyFavorSignal addSpecificMinionSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PartyFavorTrackChildSignal trackChildSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PartyFavorFreeAllMinionsSignal freeMinionsSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Game.IncidentalPartyFavorAnimationCompletedSignal incidentalPartyFavorAnimationCompletedSignal { get; set; }

		public global::System.Collections.Generic.HashSet<int> GetAllPartyFavorItems()
		{
			if (allPartyFavorItems == null)
			{
				global::System.Collections.Generic.List<global::Kampai.Game.PartyFavorAnimationDefinition> all = definitionService.GetAll<global::Kampai.Game.PartyFavorAnimationDefinition>();
				allPartyFavorItems = new global::System.Collections.Generic.HashSet<int>();
				if (all != null)
				{
					foreach (global::Kampai.Game.PartyFavorAnimationDefinition item in all)
					{
						if (!allPartyFavorItems.Contains(item.ItemID))
						{
							allPartyFavorItems.Add(item.ItemID);
						}
					}
				}
			}
			return allPartyFavorItems;
		}

		private global::Kampai.Game.View.PartyFavorAnimationView CreatePartyFavor(int partyFavorId, int minionID, bool specificMinion = false)
		{
			return CreatePartyFavor(definitionService.Get<global::Kampai.Game.PartyFavorAnimationDefinition>(partyFavorId), minionID, specificMinion);
		}

		private global::Kampai.Game.View.PartyFavorAnimationView CreatePartyFavor(global::Kampai.Game.PartyFavorAnimationDefinition animationDefinition, int minionID, bool specificMinion = false)
		{
			int iD = animationDefinition.ID;
			if (partyFavorCache.ContainsKey(iD))
			{
				return null;
			}
			global::Kampai.Game.View.PartyFavorAnimationView partyFavorAnimationView = InstantiatePartyFavor(animationDefinition);
			partyFavorCache.Add(iD, partyFavorAnimationView);
			if (specificMinion)
			{
				addSpecificMinionSignal.Dispatch(iD, minionID);
			}
			else
			{
				addMinionsPartyFavorSignal.Dispatch(iD);
			}
			return partyFavorAnimationView;
		}

		private global::Kampai.Game.View.PartyFavorAnimationView InstantiatePartyFavor(global::Kampai.Game.PartyFavorAnimationDefinition def)
		{
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject();
			global::Kampai.Game.View.PartyFavorAnimationView partyFavorAnimationView = gameObject.AddComponent<global::Kampai.Game.View.PartyFavorAnimationView>();
			gameObject.name = def.LocalizedKey;
			global::UnityEngine.Transform transform = gameObject.transform;
			gameObject.transform.SetParent(buildingManager.transform, false);
			global::UnityEngine.Vector3 localEulerAngles = (transform.localPosition = global::UnityEngine.Vector3.zero);
			transform.localEulerAngles = localEulerAngles;
			localEulerAngles = (transform.eulerAngles = global::UnityEngine.Vector3.one);
			transform.localScale = localEulerAngles;
			gameObject.SetLayerRecursively(9);
			partyFavorAnimationView.Init(def, definitionService, pathFinder, debugUpdateGridSignal, environment, delegate(int minionId)
			{
				if (randomPartyFavorStarted)
				{
					PlayRandomIncidentalAnimation(minionId);
				}
				else
				{
					incidentalPartyFavorAnimationCompletedSignal.Dispatch(def.ID);
				}
			});
			return partyFavorAnimationView;
		}

		private void Init()
		{
			global::System.Collections.Generic.List<global::Kampai.Game.PartyFavorAnimationDefinition> all = definitionService.GetAll<global::Kampai.Game.PartyFavorAnimationDefinition>();
			if (all == null)
			{
				return;
			}
			foreach (global::Kampai.Game.PartyFavorAnimationDefinition item in all)
			{
				if (playerService.GetQuantityByDefinitionId(item.UnlockId) != 0)
				{
					AddAvailablePartyFavorItem(item.ID);
				}
			}
		}

		public global::System.Collections.Generic.List<int> GetAvailablePartyFavorItems()
		{
			if (!initialized)
			{
				initialized = true;
				Init();
			}
			return availablePartyFavorItems;
		}

		public void AddAvailablePartyFavorItem(int ID)
		{
			if (!availablePartyFavorItems.Contains(ID))
			{
				availablePartyFavorItems.Add(ID);
			}
		}

		public bool PlayRandomIncidentalAnimation(int minionID)
		{
			int count = GetAvailablePartyFavorItems().Count;
			if (count <= 0)
			{
				return false;
			}
			int index = randomService.NextInt(count);
			int id = GetAvailablePartyFavorItems()[index];
			global::Kampai.Game.PartyFavorAnimationDefinition partyFavorAnimationDefinition = definitionService.Get<global::Kampai.Game.PartyFavorAnimationDefinition>(id);
			global::Kampai.Game.View.PartyFavorAnimationView partyFavorAnimationView = CreatePartyFavor(partyFavorAnimationDefinition, minionID, true);
			if (partyFavorAnimationView == null)
			{
				return false;
			}
			GetAvailablePartyFavorItems().Remove(partyFavorAnimationDefinition.ID);
			if (minionID > 0)
			{
				incidentalAnimationSignal.Dispatch(minionID, partyFavorAnimationDefinition);
			}
			return true;
		}

		public void CreateRandomPartyFavor(int minionId = -1)
		{
			randomPartyFavorStarted = true;
			foreach (int availablePartyFavorItem in GetAvailablePartyFavorItems())
			{
				CreatePartyFavor(availablePartyFavorItem, -1);
			}
			if (minionId > 0)
			{
				minionPartyAnimationSignal.Dispatch(minionId);
			}
		}

		public void RemoveAllPartyFavorAnimations()
		{
			global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
			foreach (int key in partyFavorCache.Keys)
			{
				list.Add(key);
			}
			foreach (int item in list)
			{
				ReleasePartyFavor(item);
			}
			partyFavorCache.Clear();
			randomPartyFavorStarted = false;
		}

		public void ReleasePartyFavor(int partyFavorId)
		{
			global::Kampai.Game.View.PartyFavorAnimationView partyFavorView = GetPartyFavorView(partyFavorId);
			GetAvailablePartyFavorItems().Add(partyFavorId);
			ReleasePartyFavor(partyFavorView);
		}

		private global::Kampai.Game.View.PartyFavorAnimationView GetPartyFavorView(int partyFavorId)
		{
			return (!partyFavorCache.ContainsKey(partyFavorId)) ? null : partyFavorCache[partyFavorId];
		}

		public void ReleasePartyFavor(global::Kampai.Game.View.PartyFavorAnimationView partyFavor)
		{
			if (partyFavor != null)
			{
				int iD = partyFavor.PartyFavorDefinition.ID;
				freeMinionsSignal.Dispatch(iD);
				if (partyFavor.gameObject != null)
				{
					global::UnityEngine.Object.Destroy(partyFavor.gameObject);
				}
				if (partyFavorCache.ContainsKey(iD))
				{
					partyFavorCache.Remove(iD);
				}
			}
		}

		public void AddMinionsToPartyFavor(int partyFavorId, global::Kampai.Game.View.MinionObject minion)
		{
			global::Kampai.Game.View.PartyFavorAnimationView partyFavorView = GetPartyFavorView(partyFavorId);
			if (minion != null && partyFavorView != null)
			{
				routineRunner.StartCoroutine(WaitAFrame(partyFavorView.PartyFavorDefinition.ID, minion));
			}
		}

		private global::System.Collections.IEnumerator WaitAFrame(int partyFavorID, global::Kampai.Game.View.MinionObject minion)
		{
			yield return new global::UnityEngine.WaitForEndOfFrame();
			trackChildSignal.Dispatch(partyFavorID, minion);
		}
	}
}

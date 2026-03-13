namespace Kampai.Game
{
	public class IncidentalAnimationCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("IncidentalAnimationCommand") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.View.MinionManagerView mm;

		private int minionCount;

		[Inject]
		public int minionID { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Common.IRandomService randomService { get; set; }

		[Inject(global::Kampai.Game.GameElement.MINION_MANAGER)]
		public global::UnityEngine.GameObject minionManager { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.Game.StartIncidentalAnimationSignal incidentalSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MinionAcknowledgeSignal acknowledgeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.Environment environment { get; set; }

		[Inject]
		public global::Kampai.Game.StartGroupGachaSignal startGroupGachaSignal { get; set; }

		[Inject]
		public global::Kampai.Game.FrolicSignal frolicSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IPartyFavorAnimationService partyFavorAnimationService { get; set; }

		public override void Execute()
		{
			mm = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerView>();
			if (!mm.HasObject(minionID))
			{
				global::Kampai.Game.BobCharacter byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.BobCharacter>(minionID);
				if (byInstanceId != null)
				{
					frolicSignal.Dispatch(minionID);
				}
				return;
			}
			global::UnityEngine.Vector3 objectPosition = mm.GetObjectPosition(minionID);
			global::Kampai.Util.Point point = global::Kampai.Util.Point.FromVector3(objectPosition);
			global::Kampai.Util.Point ur = point;
			bool party = definitionService.Get<global::Kampai.Game.MinionPartyDefinition>(80000).Contains(point);
			global::Kampai.Game.Minion byInstanceId2 = playerService.GetByInstanceId<global::Kampai.Game.Minion>(minionID);
			if (!byInstanceId2.IsInMinionParty)
			{
				int num = 2;
				point.x -= num;
				point.y -= num;
				ur.x += num;
				ur.y += num;
				global::System.Collections.Generic.List<global::Kampai.Game.View.MinionObject> idleMinions = GetIdleMinions(point, ur);
				minionCount = idleMinions.Count;
				if (minionCount >= 2)
				{
					MinionInteraction(objectPosition, idleMinions, party);
				}
				else if (!BuildingInteraction(objectPosition, point, ur))
				{
					PlayIncidental(party);
				}
			}
		}

		private void PlayIncidental(bool party)
		{
			global::Kampai.Game.StaticItem defId = ((!party) ? global::Kampai.Game.StaticItem.WEIGHTED_INCIDENTAL_ANIM : global::Kampai.Game.StaticItem.WEIGHTED_PARTY_ANIM);
			global::Kampai.Util.QuantityItem quantityItem = playerService.GetWeightedInstance((int)defId).NextPick(randomService);
			if (quantityItem.ID == 4036)
			{
				partyFavorAnimationService.PlayRandomIncidentalAnimation(minionID);
			}
			else
			{
				incidentalSignal.Dispatch(minionID, quantityItem.ID);
			}
		}

		private global::System.Collections.Generic.List<global::Kampai.Game.View.MinionObject> GetIdleMinions(global::Kampai.Util.Point ll, global::Kampai.Util.Point ur)
		{
			global::System.Collections.Generic.List<global::Kampai.Game.View.ActionableObject> list = new global::System.Collections.Generic.List<global::Kampai.Game.View.ActionableObject>();
			mm.GetObjectsInArea(ll, ur, list);
			global::System.Collections.Generic.List<global::Kampai.Game.View.MinionObject> list2 = new global::System.Collections.Generic.List<global::Kampai.Game.View.MinionObject>(list.Count);
			foreach (global::Kampai.Game.View.MinionObject item in list)
			{
				global::Kampai.Game.Minion byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Minion>(item.ID);
				if (byInstanceId.State != global::Kampai.Game.MinionState.Idle || byInstanceId.IsInIncidental)
				{
					continue;
				}
				if (byInstanceId.HasPrestige)
				{
					global::Kampai.Game.Prestige byInstanceId2 = playerService.GetByInstanceId<global::Kampai.Game.Prestige>(byInstanceId.PrestigeId);
					if (byInstanceId2 != null && byInstanceId2.state == global::Kampai.Game.PrestigeState.Questing)
					{
						continue;
					}
				}
				byInstanceId.IsInIncidental = true;
				list2.Add(item);
			}
			return list2;
		}

		private void MinionInteraction(global::UnityEngine.Vector3 pos, global::System.Collections.Generic.IEnumerable<global::Kampai.Game.View.MinionObject> idleMinions, bool party)
		{
			global::Kampai.Game.StaticItem defId = ((!party) ? global::Kampai.Game.StaticItem.WEIGHTED_ACK_ANIM : global::Kampai.Game.StaticItem.WEIGHTED_ACK_ANIM_PARTY);
			global::Kampai.Game.Transaction.WeightedInstance weightedInstance = playerService.GetWeightedInstance((int)defId);
			global::Kampai.Util.QuantityItem quantityItem = weightedInstance.NextPick(randomService);
			if (quantityItem.ID == 11000)
			{
				GachaAwareness(pos, idleMinions, party);
			}
			else
			{
				MinionAwareness(pos, idleMinions, quantityItem);
			}
		}

		private void GachaAwareness(global::UnityEngine.Vector3 pos, global::System.Collections.Generic.IEnumerable<global::Kampai.Game.View.MinionObject> idleMinions, bool party)
		{
			global::System.Collections.Generic.HashSet<int> hashSet = new global::System.Collections.Generic.HashSet<int>();
			foreach (global::Kampai.Game.View.MinionObject idleMinion in idleMinions)
			{
				hashSet.Add(idleMinion.ID);
			}
			startGroupGachaSignal.Dispatch(new global::Kampai.Game.MinionAnimationInstructions(hashSet, new global::Kampai.Util.Boxed<global::UnityEngine.Vector3>(pos), party));
		}

		private void MinionAwareness(global::UnityEngine.Vector3 pos, global::System.Collections.Generic.IEnumerable<global::Kampai.Game.View.MinionObject> idleMinions, global::Kampai.Util.QuantityItem pick)
		{
			int num = -1;
			global::Kampai.Game.DefinitionGroup definitionGroup = definitionService.Get<global::Kampai.Game.DefinitionGroup>(pick.ID);
			if (definitionGroup.Group.Count < 2)
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Warning, "Minion Awareness: Invalid group {0}", pick.ID);
				return;
			}
			float angleFacingObject;
			foreach (global::Kampai.Game.View.MinionObject idleMinion in idleMinions)
			{
				if (idleMinion.ID == minionID)
				{
					continue;
				}
				num = idleMinion.ID;
				angleFacingObject = GetAngleFacingObject(mm.GetObjectPosition(num), pos);
				acknowledgeSignal.Dispatch(num, angleFacingObject, definitionGroup.Group[0]);
				break;
			}
			angleFacingObject = GetAngleFacingObject(pos, mm.GetObjectPosition(num));
			acknowledgeSignal.Dispatch(minionID, angleFacingObject, definitionGroup.Group[1]);
		}

		private bool BuildingInteraction(global::UnityEngine.Vector3 pos, global::Kampai.Util.Point ll, global::Kampai.Util.Point ur)
		{
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Util.QuantityItem quantityItem = playerService.GetWeightedInstance(4004).NextPick(randomService);
			bool flag = false;
			for (int i = ll.x; i <= ur.x; i++)
			{
				if (flag)
				{
					break;
				}
				for (int j = ll.y; j <= ur.y; j++)
				{
					if (flag)
					{
						break;
					}
					global::Kampai.Game.Building building = null;
					if (environment.IsOccupied(i, j) && (building = environment.GetBuilding(i, j)) != null && !(building is global::Kampai.Game.ConnectableBuilding))
					{
						global::Kampai.Game.View.BuildingObject buildingObject = component.GetBuildingObject(building.ID);
						global::UnityEngine.Vector3 position = buildingObject.transform.position;
						global::UnityEngine.BoxCollider component2 = buildingObject.GetComponent<global::UnityEngine.BoxCollider>();
						if (component2 != null)
						{
							position += component2.center;
						}
						float angleFacingObject = GetAngleFacingObject(pos, position);
						acknowledgeSignal.Dispatch(minionID, angleFacingObject, quantityItem.ID);
						flag = true;
						break;
					}
				}
			}
			return flag;
		}

		private float GetAngleFacingObject(global::UnityEngine.Vector3 pos, global::UnityEngine.Vector3 other)
		{
			global::UnityEngine.Vector3 vector = other - pos;
			return global::UnityEngine.Mathf.Atan2(vector.x, vector.z) * 57.29578f;
		}
	}
}

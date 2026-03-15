namespace Kampai.Game
{
	public class CreateMinionCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CreateMinionCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.Minion minion { get; set; }

		[Inject(global::Kampai.Game.GameElement.MINION_MANAGER)]
		public global::UnityEngine.GameObject minionManager { get; set; }

		[Inject]
		public global::Kampai.Util.IMinionBuilder minionBuilder { get; set; }

		[Inject]
		public global::Kampai.Util.PathFinder pathFinder { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.InitCharacterObjectSignal initSignal { get; set; }

		[Inject]
		public global::Kampai.Game.AddMinionSignal addMinionSignal { get; set; }

		public override void Execute()
		{
			int costumeId = minion.GetCostumeId(playerService, definitionService);
			global::Kampai.Game.CostumeItemDefinition costumeItemDefinition = definitionService.Get<global::Kampai.Game.CostumeItemDefinition>(99);
			if (costumeItemDefinition == null)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.PS_MISSING_DEFAULT_COSTUME, "ERROR: Minion costume ID: {0} - Could not create default costume!!!", costumeId);
			}
			global::Kampai.Game.CostumeItemDefinition costume = costumeItemDefinition;
			if (costumeId > 0 && costumeId != 99)
			{
				global::Kampai.Game.CostumeItemDefinition costumeItemDefinition2 = definitionService.Get<global::Kampai.Game.CostumeItemDefinition>(costumeId);
				if (costumeItemDefinition2 != null)
				{
					costume = costumeItemDefinition2;
				}
				else
				{
					logger.Warning("Minion costume ID: {0} is not a costume item, Reverting back to generic minion", costumeId);
				}
			}
			global::Kampai.Game.View.MinionObject minionObject = minionBuilder.BuildMinion(costume, "asm_minion_movement");
			initSignal.Dispatch(minionObject, minion);
			minionObject.transform.parent = minionManager.transform;
			minionObject.transform.position = pathFinder.RandomPosition(minion.Partying);
			addMinionSignal.Dispatch(minionObject);
			global::Kampai.Game.MinionPartyDefinition minionPartyDefinition = definitionService.Get<global::Kampai.Game.MinionPartyDefinition>(80000);
			if (minion.IsInMinionParty)
			{
				minionObject.EnterMinionParty((global::UnityEngine.Vector3)minionPartyDefinition.Center, minionPartyDefinition.PartyRadius, minionPartyDefinition.partyAnimationRestMin, minionPartyDefinition.partyAnimationRestMax);
			}
		}
	}
}

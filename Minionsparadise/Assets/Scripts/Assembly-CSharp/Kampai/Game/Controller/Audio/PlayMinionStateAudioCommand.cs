namespace Kampai.Game.Controller.Audio
{
	public class PlayMinionStateAudioCommand
	{
		public enum StateParameter
		{
			Unselected = 0,
			GroupGacha = 1,
			Selected = 2,
			Deviant = 3
		}

		private const string CUE_PARAM_NAME = "Cue";

		private const string STATE_PARAM_NAME = "State";

		private static readonly global::System.Collections.Generic.Dictionary<global::Kampai.Game.Controller.Audio.PlayMinionStateAudioCommand.StateParameter, float> stateLookup = new global::System.Collections.Generic.Dictionary<global::Kampai.Game.Controller.Audio.PlayMinionStateAudioCommand.StateParameter, float>
		{
			{
				global::Kampai.Game.Controller.Audio.PlayMinionStateAudioCommand.StateParameter.Unselected,
				1f
			},
			{
				global::Kampai.Game.Controller.Audio.PlayMinionStateAudioCommand.StateParameter.GroupGacha,
				2f
			},
			{
				global::Kampai.Game.Controller.Audio.PlayMinionStateAudioCommand.StateParameter.Selected,
				3f
			},
			{
				global::Kampai.Game.Controller.Audio.PlayMinionStateAudioCommand.StateParameter.Deviant,
				2f
			}
		};

		private global::System.Collections.Generic.Dictionary<string, float> parameters = new global::System.Collections.Generic.Dictionary<string, float>(2);

		[Inject]
		public global::Kampai.Main.PlayLocalAudioSignal playLocalAudioSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		public void Execute(MinionStateAudioArgs args)
		{
			global::Kampai.Game.View.ActionableObject source = args.source;
			string audioEvent = args.audioEvent;
			string emitterKey = args.emitterKey;
			float cueId = args.cueId;
			global::Kampai.Game.Controller.Audio.PlayMinionStateAudioCommand.StateParameter state = GetState(source);
			float value = 0f;
			if (stateLookup.TryGetValue(state, out value))
			{
				parameters["Cue"] = cueId;
				parameters["State"] = value;
				playLocalAudioSignal.Dispatch(source.GetAudioEmitter(emitterKey), audioEvent, parameters);
			}
		}

		private global::Kampai.Game.Controller.Audio.PlayMinionStateAudioCommand.StateParameter GetState(global::Kampai.Game.View.ActionableObject source)
		{
			global::Kampai.Game.Controller.Audio.PlayMinionStateAudioCommand.StateParameter result = global::Kampai.Game.Controller.Audio.PlayMinionStateAudioCommand.StateParameter.Unselected;
			global::Kampai.Game.View.MinionObject minionObject = source as global::Kampai.Game.View.MinionObject;
			if (minionObject != null)
			{
				int iD = minionObject.ID;
				global::Kampai.Game.Minion byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Minion>(iD);
				if (byInstanceId != null)
				{
					global::Kampai.Game.MinionState state = byInstanceId.State;
					global::Kampai.Game.View.MinionObject.MinionGachaState gachaState = minionObject.GachaState;
					switch (gachaState)
					{
					case global::Kampai.Game.View.MinionObject.MinionGachaState.Deviant:
						result = global::Kampai.Game.Controller.Audio.PlayMinionStateAudioCommand.StateParameter.Deviant;
						break;
					case global::Kampai.Game.View.MinionObject.MinionGachaState.Active:
						result = global::Kampai.Game.Controller.Audio.PlayMinionStateAudioCommand.StateParameter.GroupGacha;
						break;
					default:
						if (state == global::Kampai.Game.MinionState.Selected || gachaState == global::Kampai.Game.View.MinionObject.MinionGachaState.IndividualTap)
						{
							result = global::Kampai.Game.Controller.Audio.PlayMinionStateAudioCommand.StateParameter.Selected;
						}
						break;
					}
				}
			}
			return result;
		}
	}
}

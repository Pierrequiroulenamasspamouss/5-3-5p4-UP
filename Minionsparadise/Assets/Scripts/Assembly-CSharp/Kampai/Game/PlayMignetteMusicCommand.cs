namespace Kampai.Game
{
	public class PlayMignetteMusicCommand : global::strange.extensions.command.impl.Command
	{
		public enum MusicEvent
		{
			Start = 0,
			Stop = 1
		}

		[Inject]
		public string audioSource { get; set; }

		[Inject]
		public global::Kampai.Game.PlayMignetteMusicCommand.MusicEvent musicEvent { get; set; }

		[Inject]
		public global::Kampai.Common.Service.Audio.IFMODService fmodService { get; set; }

		public override void Execute()
		{
			string guid = fmodService.GetGuid(audioSource);
			string name = string.Format("/{0}", guid);
			global::UnityEngine.GameObject gameObject = global::UnityEngine.GameObject.Find(name);
			if (gameObject == null)
			{
				gameObject = new global::UnityEngine.GameObject(guid);
			}
			global::Kampai.Game.MignetteMusicEmitter component = gameObject.GetComponent<global::Kampai.Game.MignetteMusicEmitter>();
			if (component == null && musicEvent == global::Kampai.Game.PlayMignetteMusicCommand.MusicEvent.Start)
			{
				component = gameObject.AddComponent<global::Kampai.Game.MignetteMusicEmitter>();
				if (!(component == null))
				{
					component.SetEventGUID(guid);
					component.StartEvent();
				}
			}
			else if (component != null)
			{
				component.StopEvent();
			}
		}
	}
}

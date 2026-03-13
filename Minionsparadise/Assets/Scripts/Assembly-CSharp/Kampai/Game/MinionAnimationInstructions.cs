namespace Kampai.Game
{
	public class MinionAnimationInstructions
	{
		public global::System.Collections.Generic.HashSet<int> MinionIds { get; set; }

		public global::Kampai.Util.Boxed<global::UnityEngine.Vector3> Center { get; set; }

		public bool Party { get; set; }

		public MinionAnimationInstructions(global::System.Collections.Generic.HashSet<int> MinionIds = null, global::Kampai.Util.Boxed<global::UnityEngine.Vector3> Center = null, bool Party = false)
		{
			this.MinionIds = MinionIds;
			this.Center = Center;
			this.Party = Party;
		}
	}
}

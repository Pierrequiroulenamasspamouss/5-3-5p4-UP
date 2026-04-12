namespace Kampai.Tools.AnimationToolKit
{
	public class ToggleMeshCommand : global::strange.extensions.command.impl.Command
	{
		[Inject(global::Kampai.Tools.AnimationToolKit.AnimationToolKitElement.MINIONS)]
		public global::UnityEngine.GameObject MinionGroup { get; set; }

		[Inject(global::Kampai.Tools.AnimationToolKit.AnimationToolKitElement.CHARACTERS)]
		public global::UnityEngine.GameObject CharacterGroup { get; set; }

		public override void Execute()
		{
			DisableGroup(MinionGroup.transform);
			DisableGroup(CharacterGroup.transform);
		}

		private void DisableGroup(global::UnityEngine.Transform transform)
		{
			bool enabled = false;
			int childCount = transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				global::UnityEngine.Transform child = transform.GetChild(i);
				int childCount2 = child.childCount;
				for (int j = 0; j < childCount2; j++)
				{
					global::UnityEngine.Transform child2 = child.GetChild(j);
					if (!child2.name.Contains("LOD"))
					{
						continue;
					}
					global::UnityEngine.SkinnedMeshRenderer component = child2.GetComponent<global::UnityEngine.SkinnedMeshRenderer>();
					if (!(component == null))
					{
						if (i == 0)
						{
							enabled = !component.enabled;
						}
						component.enabled = enabled;
					}
				}
			}
		}
	}
}

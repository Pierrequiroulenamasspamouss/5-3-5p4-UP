namespace Kampai.Util
{
	public static class SkinnedMeshAggregator
	{
		private static global::System.Collections.Generic.Dictionary<string, global::UnityEngine.Transform> bonesLookup = new global::System.Collections.Generic.Dictionary<string, global::UnityEngine.Transform>(128);

		private static global::System.Collections.Generic.Stack<global::UnityEngine.Transform> bonesStack = new global::System.Collections.Generic.Stack<global::UnityEngine.Transform>(16);

		public static global::UnityEngine.GameObject CreateAggregateObject(string name, string skeletonName, global::System.Collections.Generic.IEnumerable<string> meshNames, string targetLOD)
		{
			if (string.IsNullOrEmpty(skeletonName))
			{
				return null;
			}
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject(name);
			AggregateSkinnedMeshes(gameObject.transform, skeletonName, meshNames, targetLOD);
			return gameObject;
		}

		private static void buildBonesLookup(global::UnityEngine.Transform rootBone)
		{
			bonesStack.Push(rootBone);
			while (bonesStack.Count > 0)
			{
				global::UnityEngine.Transform transform = bonesStack.Pop();
				if (!(transform == null))
				{
					bonesLookup[transform.name] = transform;
					for (int i = 0; i < transform.childCount; i++)
					{
						bonesStack.Push(transform.GetChild(i));
					}
				}
			}
		}

		public static void AggregateSkinnedMeshes(global::UnityEngine.Transform parent, string skeletonName, global::System.Collections.Generic.IEnumerable<string> meshNames, string targetLOD)
		{
			global::UnityEngine.GameObject original = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>(string.Format("{0}_{1}", skeletonName, targetLOD));
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(original);
			while (gameObject.transform.childCount > 0)
			{
				global::UnityEngine.Transform child = gameObject.transform.GetChild(0);
				child.parent = parent;
				buildBonesLookup(child);
			}
			global::UnityEngine.Animator animator = parent.gameObject.GetComponent<global::UnityEngine.Animator>();
			if (animator == null)
			{
				animator = parent.gameObject.AddComponent<global::UnityEngine.Animator>();
			}
			animator.avatar = gameObject.GetComponent<global::UnityEngine.Animator>().avatar;
			AddSubModels(meshNames, parent, targetLOD);
			global::UnityEngine.Object.Destroy(gameObject);
			bonesLookup.Clear();
		}

		public static void AddSubModels(global::System.Collections.Generic.IEnumerable<string> meshNames, global::UnityEngine.Transform modelRoot, string targetLOD)
		{
			if (meshNames == null)
			{
				return;
			}
			foreach (string meshName in meshNames)
			{
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(global::Kampai.Util.KampaiResources.Load(string.Format("{0}_{1}", meshName, targetLOD))) as global::UnityEngine.GameObject;
				global::UnityEngine.SkinnedMeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<global::UnityEngine.SkinnedMeshRenderer>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					AddSubModel(componentsInChildren[i], modelRoot);
				}
				global::UnityEngine.Object.Destroy(gameObject);
			}
		}

		public static void AddSubModel(global::UnityEngine.SkinnedMeshRenderer rendererToCopy, global::UnityEngine.Transform modelRoot)
		{
			global::UnityEngine.SkinnedMeshRenderer skinnedMeshRenderer = CopyRenderer(rendererToCopy, modelRoot);
			skinnedMeshRenderer.bones = ReconstructBoneList(rendererToCopy, modelRoot);
			skinnedMeshRenderer.sharedMesh = rendererToCopy.sharedMesh;
			skinnedMeshRenderer.materials = rendererToCopy.materials;
		}

		private static global::UnityEngine.SkinnedMeshRenderer CopyRenderer(global::UnityEngine.SkinnedMeshRenderer rendererToCopy, global::UnityEngine.Transform modelRoot)
		{
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject(rendererToCopy.name);
			gameObject.transform.parent = modelRoot;
			gameObject.transform.ResetLocal();
			return gameObject.AddComponent<global::UnityEngine.SkinnedMeshRenderer>();
		}

		private static global::UnityEngine.Transform[] ReconstructBoneList(global::UnityEngine.SkinnedMeshRenderer rendererToCopy, global::UnityEngine.Transform modelRoot)
		{
			global::UnityEngine.Transform[] array = new global::UnityEngine.Transform[rendererToCopy.bones.Length];
			global::UnityEngine.Transform[] bones = rendererToCopy.bones;
			for (int i = 0; i < bones.Length; i++)
			{
				global::UnityEngine.Transform transform = FindBoneInHierarchy(bones[i].name, modelRoot);
				array[i] = transform;
			}
			return array;
		}

		private static global::UnityEngine.Transform FindBoneInHierarchy(string boneName, global::UnityEngine.Transform parentBone)
		{
			if (parentBone.name == boneName)
			{
				return parentBone;
			}
			global::UnityEngine.Transform value;
			if (bonesLookup.TryGetValue(boneName, out value))
			{
				return value;
			}
			foreach (global::UnityEngine.Transform item in parentBone)
			{
				value = FindBoneInHierarchy(boneName, item);
				if (value != null)
				{
					return value;
				}
			}
			return null;
		}
	}
}

namespace Kampai.Main
{
	internal sealed class AssetsPreloadService : global::Kampai.Main.IAssetsPreloadService
	{
		private readonly global::System.Collections.Generic.Dictionary<string, global::System.Type> KNOWN_TYPES = new global::System.Collections.Generic.Dictionary<string, global::System.Type>
		{
			{
				"UnityEngine.Camera",
				typeof(global::UnityEngine.Camera)
			},
			{
				"UnityEngine.AnimationClip",
				typeof(global::UnityEngine.AnimationClip)
			},
			{
				"UnityEngine.ParticleSystem",
				typeof(global::UnityEngine.ParticleSystem)
			},
			{
				"UnityEngine.ParticleSystemRenderer",
				typeof(global::UnityEngine.ParticleSystemRenderer)
			},
			{
				"UnityEngine.Mesh",
				typeof(global::UnityEngine.Mesh)
			},
			{
				"UnityEngine.Avatar",
				typeof(global::UnityEngine.Avatar)
			},
			{
				"UnityEngine.Animator",
				typeof(global::UnityEngine.Animator)
			},
			{
				"UnityEngine.SkinnedMeshRenderer",
				typeof(global::UnityEngine.SkinnedMeshRenderer)
			},
			{
				"UnityEngine.Shader",
				typeof(global::UnityEngine.Shader)
			},
			{
				"UnityEngine.LODGroup",
				typeof(global::UnityEngine.LODGroup)
			},
			{
				"UnityEngine.MeshRenderer",
				typeof(global::UnityEngine.MeshRenderer)
			},
			{
				"UnityEngine.MeshFilter",
				typeof(global::UnityEngine.MeshFilter)
			},
			{
				"UnityEngine.BoxCollider",
				typeof(global::UnityEngine.BoxCollider)
			},
			{
				"UnityEngine.SphereCollider",
				typeof(global::UnityEngine.SphereCollider)
			},
			{
				"UnityEngine.TextAsset",
				typeof(global::UnityEngine.TextAsset)
			},
			{
				"UnityEngine.AnimatorOverrideController",
				typeof(global::UnityEngine.AnimatorOverrideController)
			},
			{
				"UnityEngine.MeshCollider",
				typeof(global::UnityEngine.MeshCollider)
			},
			{
				"UnityEngine.Rigidbody",
				typeof(global::UnityEngine.Rigidbody)
			},
			{
				"UnityEngine.LineRenderer",
				typeof(global::UnityEngine.LineRenderer)
			},
			{
				"UnityEngine.CapsuleCollider",
				typeof(global::UnityEngine.CapsuleCollider)
			},
			{
				"UnityEngine.TrailRenderer",
				typeof(global::UnityEngine.TrailRenderer)
			},
			{
				"UnityEngine.Animation",
				typeof(global::UnityEngine.Animation)
			},
			{
				"UnityEngine.MonoBehaviour",
				typeof(global::UnityEngine.MonoBehaviour)
			},
			{
				"UnityEngine.GameObject",
				typeof(global::UnityEngine.GameObject)
			},
			{
				"UnityEngine.Transform",
				typeof(global::UnityEngine.Transform)
			},
			{
				"UnityEngine.RectTransform",
				typeof(global::UnityEngine.RectTransform)
			},
			{
				"UnityEngine.CanvasRenderer",
				typeof(global::UnityEngine.CanvasRenderer)
			},
			{
				"UnityEngine.Font",
				typeof(global::UnityEngine.Font)
			},
			{
				"UnityEngine.Material",
				typeof(global::UnityEngine.Material)
			},
			{
				"UnityEngine.Texture2D",
				typeof(global::UnityEngine.Texture2D)
			},
			{
				"UnityEngine.Sprite",
				typeof(global::UnityEngine.Sprite)
			},
			{
				"UnityEngine.Object",
				typeof(global::UnityEngine.Object)
			},
			{
				"UnityEngine.RuntimeAnimatorController",
				typeof(global::UnityEngine.RuntimeAnimatorController)
			}
		};

		private global::System.Collections.Generic.List<global::Kampai.Main.PreloadableAsset> assetsQueue = new global::System.Collections.Generic.List<global::Kampai.Main.PreloadableAsset>(512);

		private global::System.Collections.IEnumerator integrationCoroutine;

		private int integrationStepLength = 150;

		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("AssetsPreloadService") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		public void AddAssetToPreloadQueue(global::Kampai.Main.PreloadableAsset asset)
		{
			assetsQueue.Add(asset);
			if (integrationCoroutine == null)
			{
				integrationCoroutine = IntegratePreloadQueue();
				routineRunner.StartCoroutine(integrationCoroutine);
			}
		}

		public void PreloadAllAssets()
		{
			global::UnityEngine.TextAsset textAsset = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.TextAsset>("PreloadedAssetsList");
			if (textAsset == null)
			{
				logger.Info("Assets preload list was not found. This message is harmful only to the load time.");
				return;
			}
			try
			{
				using (global::System.IO.MemoryStream stream = new global::System.IO.MemoryStream(textAsset.bytes))
				{
					using (global::System.IO.StreamReader reader = new global::System.IO.StreamReader(stream))
					{
						using (global::Newtonsoft.Json.JsonTextReader reader2 = new global::Newtonsoft.Json.JsonTextReader(reader))
						{
							global::Kampai.Main.AssetsPreloadList assetsPreloadList = global::Kampai.Util.FastJSONDeserializer.Deserialize<global::Kampai.Main.AssetsPreloadList>(reader2);
							global::System.Collections.Generic.List<global::Kampai.Main.PreloadableAsset> assetsList = assetsPreloadList.AssetsList;
							for (int num = assetsList.Count - 1; num >= 0; num--)
							{
								AddAssetToPreloadQueue(assetsList[num]);
							}
						}
					}
				}
			}
			catch (global::Newtonsoft.Json.JsonSerializationException ex)
			{
				logger.Error("AssetsPreloadList Json Parse Err: {0}", ex);
			}
			catch (global::Newtonsoft.Json.JsonReaderException ex2)
			{
				logger.Error("AssetsPreloadList Json Read Err: {0}", ex2);
			}
			catch (global::System.Exception ex3)
			{
				logger.Error("AssetsPreloadList load error: {0}", ex3);
			}
		}

		public void StopAssetsPreload()
		{
			if (integrationCoroutine != null)
			{
				assetsQueue.Clear();
				routineRunner.StopCoroutine(integrationCoroutine);
				integrationCoroutine = null;
				logger.Info("Preload has stopped");
			}
		}

		public void SetIntegrationStepLength(int msec)
		{
			integrationStepLength = msec;
		}

		private global::System.Collections.IEnumerator IntegratePreloadQueue()
		{
			yield return null;
			global::System.Diagnostics.Stopwatch sw = global::System.Diagnostics.Stopwatch.StartNew();
			while (assetsQueue.Count > 0)
			{
				if (sw.ElapsedMilliseconds > integrationStepLength)
				{
					yield return null;
					if (assetsQueue.Count == 0)
					{
						integrationCoroutine = null;
						yield break;
					}
					sw.Reset();
					sw.Start();
				}
				int idx = assetsQueue.Count - 1;
				global::Kampai.Main.PreloadableAsset assetInfo = assetsQueue[idx];
				assetsQueue.RemoveAt(idx);
				global::System.Type assetType;
				KNOWN_TYPES.TryGetValue(assetInfo.type, out assetType);
				if (assetType != null)
				{
					global::Kampai.Util.KampaiResources.Load(assetInfo.name, assetType);
					logger.Info("Preload asset '{0}'", assetInfo.name);
				}
				else
				{
					logger.Info("Failed to preload asset '{0}': type '{1}' is unknown", assetInfo.name, assetInfo.type);
				}
			}
			integrationCoroutine = null;
		}
	}
}

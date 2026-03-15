namespace Kampai.Util
{
	public static class GameObjectUtil
	{
		private static global::System.Collections.Generic.Dictionary<string, global::System.Type> typesLookup = new global::System.Collections.Generic.Dictionary<string, global::System.Type>
		{
			{
				"EdwardMinionHandsMignetteRoot",
				typeof(global::Kampai.Game.Mignette.EdwardMinionHands.EdwardMinionHandsMignetteRoot)
			},
			{
				"ButterflyCatchMignetteRoot",
				typeof(global::Kampai.Game.Mignette.ButterflyCatch.ButterflyCatchMignetteRoot)
			},
			{
				"BalloonBarrageMignetteRoot",
				typeof(global::Kampai.Game.Mignette.BalloonBarrage.BalloonBarrageMignetteRoot)
			},
			{
				"WaterSlideMignetteRoot",
				typeof(global::Kampai.Game.Mignette.WaterSlide.WaterSlideMignetteRoot)
			},
			{
				"AlligatorSkiingMignetteRoot",
				typeof(global::Kampai.Game.Mignette.AlligatorSkiing.AlligatorSkiingMignetteRoot)
			}
		};

		public static void TryRemoveComponent<T>(global::UnityEngine.GameObject obj) where T : global::UnityEngine.Component
		{
			T component = obj.GetComponent<T>();
			if (component != null)
			{
				global::UnityEngine.Object.Destroy(component);
			}
		}

		public static void TryEnableBehaviour<T>(global::UnityEngine.GameObject obj, bool enable) where T : global::UnityEngine.MonoBehaviour
		{
			T component = obj.GetComponent<T>();
			if (component != null)
			{
				component.enabled = enable;
			}
		}

		public static global::UnityEngine.Component AddComponent(global::UnityEngine.GameObject obj, string componentClassName, global::Kampai.Util.IKampaiLogger logger)
		{
			global::System.Type type = resolveComponentType(componentClassName, logger);
			return (type == null) ? null : obj.AddComponent(type);
		}

		[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
		private static global::System.Type resolveComponentType(string componentClassName, global::Kampai.Util.IKampaiLogger logger)
		{
			global::System.Type value;
			if (typesLookup.TryGetValue(componentClassName, out value))
			{
				return value;
			}
			global::System.Reflection.Assembly callingAssembly = global::System.Reflection.Assembly.GetCallingAssembly();
			value = callingAssembly.GetType(componentClassName, false, false);
			if (value != null)
			{
				logger.Error("SLOW OPERATION: component type {0} found using reflection. Please add me to GameObjectUtil.typesLookup!", componentClassName);
				typesLookup.Add(componentClassName, value);
				return value;
			}
			global::System.Reflection.Assembly[] assemblies = global::System.AppDomain.CurrentDomain.GetAssemblies();
			foreach (global::System.Reflection.Assembly assembly in assemblies)
			{
				global::System.Type[] types = assembly.GetTypes();
				foreach (global::System.Type type in types)
				{
					if (type.Name == componentClassName || type.FullName == componentClassName)
					{
						logger.Error("(EVEN) SLOW(ER) OPERATION: component type {0} found using reflection. Please add me to GameObjectUtil.typesLookup!", componentClassName);
						typesLookup.Add(componentClassName, value);
						return value;
					}
				}
			}
			return null;
		}
	}
}

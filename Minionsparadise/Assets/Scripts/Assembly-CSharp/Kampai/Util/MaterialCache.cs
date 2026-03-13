namespace Kampai.Util
{
	public class MaterialCache
	{
		private sealed class CacheEntry
		{
			public int refCount;

			public global::UnityEngine.Material material;
		}

		private const int CheckCount = 50;

		private readonly global::System.Collections.Generic.Dictionary<int, global::Kampai.Util.MaterialCache.CacheEntry> m_materialCache;

		public MaterialCache()
		{
			m_materialCache = new global::System.Collections.Generic.Dictionary<int, global::Kampai.Util.MaterialCache.CacheEntry>();
		}

		public void RemoveReference(int hashCode)
		{
			if (m_materialCache.ContainsKey(hashCode))
			{
				global::Kampai.Util.MaterialCache.CacheEntry cacheEntry = m_materialCache[hashCode];
				if (--cacheEntry.refCount <= 0)
				{
					global::UnityEngine.Object.DestroyImmediate(cacheEntry.material, true);
					m_materialCache.Remove(hashCode);
				}
			}
		}

		public global::UnityEngine.Material GetMaterial(int hashCode, global::UnityEngine.Material defaultMaterial)
		{
			if (m_materialCache.ContainsKey(hashCode))
			{
				global::Kampai.Util.MaterialCache.CacheEntry cacheEntry = m_materialCache[hashCode];
				cacheEntry.refCount++;
				return cacheEntry.material;
			}
			global::Kampai.Util.MaterialCache.CacheEntry cacheEntry2 = new global::Kampai.Util.MaterialCache.CacheEntry();
			cacheEntry2.refCount = 1;
			cacheEntry2.material = new global::UnityEngine.Material(defaultMaterial);
			global::Kampai.Util.MaterialCache.CacheEntry cacheEntry3 = cacheEntry2;
			m_materialCache.Add(hashCode, cacheEntry3);
			return cacheEntry3.material;
		}
	}
}

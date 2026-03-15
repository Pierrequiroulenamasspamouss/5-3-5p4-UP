namespace Kampai.UI
{
	[global::UnityEngine.AddComponentMenu("UI/Effects/Transform Offset", 14)]
	[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.RectTransform))]
	public class TransformOffset : global::UnityEngine.UI.BaseMeshEffect
	{
		public enum Layout
		{
			None = 0,
			Center = 1,
			Vertical = 2,
			Horizontal = 3,
			Top = 4,
			Bottom = 5
		}

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Vector3 m_offsetPosition = global::UnityEngine.Vector3.zero;

		[global::UnityEngine.SerializeField]
		private global::Kampai.UI.TransformOffset.Layout m_layout = global::Kampai.UI.TransformOffset.Layout.Center;

		private global::UnityEngine.RectTransform m_rectTransform;

		public global::UnityEngine.RectTransform rectTransform
		{
			get
			{
				return m_rectTransform;
			}
		}

		public global::UnityEngine.Vector3 offsetPosition
		{
			get
			{
				return m_offsetPosition;
			}
			set
			{
				if (!(m_offsetPosition == value))
				{
					m_offsetPosition = value;
					if (base.graphic != null)
					{
						base.graphic.SetVerticesDirty();
					}
					global::UnityEngine.UI.LayoutRebuilder.MarkLayoutForRebuild((global::UnityEngine.RectTransform)base.transform);
				}
			}
		}

		protected override void Awake()
		{
			base.Awake();
			m_rectTransform = base.transform as global::UnityEngine.RectTransform;
		}

		public override void ModifyMesh(global::UnityEngine.UI.VertexHelper vh)
		{
			if (IsActive())
			{
				global::System.Collections.Generic.List<global::UnityEngine.UIVertex> list = new global::System.Collections.Generic.List<global::UnityEngine.UIVertex>();
				vh.GetUIVertexStream(list);
				ModifyVertices(list);
				vh.Clear();
				vh.AddUIVertexTriangleStream(list);
			}
		}

		public void ModifyVertices(global::System.Collections.Generic.List<global::UnityEngine.UIVertex> verts)
		{
			if (!IsActive())
			{
				return;
			}
			m_rectTransform = m_rectTransform ?? (base.transform as global::UnityEngine.RectTransform);
			if (rectTransform == null)
			{
				return;
			}
			global::UnityEngine.Rect rect = rectTransform.rect;
			global::UnityEngine.Vector3 vector = global::UnityEngine.Vector3.zero;
			if (m_layout != global::Kampai.UI.TransformOffset.Layout.None)
			{
				float num = float.MaxValue;
				float num2 = float.MaxValue;
				float num3 = 0f;
				float num4 = 0f;
				for (int i = 0; i < verts.Count; i++)
				{
					global::UnityEngine.UIVertex uIVertex = verts[i];
					num = global::UnityEngine.Mathf.Min(num, uIVertex.position.x);
					num3 = global::UnityEngine.Mathf.Max(num3, uIVertex.position.x);
					num2 = global::UnityEngine.Mathf.Min(num2, uIVertex.position.y);
					num4 = global::UnityEngine.Mathf.Max(num4, uIVertex.position.y);
				}
				global::UnityEngine.Vector2 center = rect.center;
				global::UnityEngine.Vector2 vector2 = new global::UnityEngine.Vector2(num, num2);
				global::UnityEngine.Vector2 vector3 = new global::UnityEngine.Vector2(num3 - num, num4 - num2);
				global::UnityEngine.Vector2 vector4 = vector3 / 2f;
				switch (m_layout)
				{
				case global::Kampai.UI.TransformOffset.Layout.Top:
					center.y = rect.yMax;
					vector4 *= 2f;
					break;
				case global::Kampai.UI.TransformOffset.Layout.Bottom:
					center.y = rect.yMin;
					vector4 = global::UnityEngine.Vector2.zero;
					break;
				}
				global::UnityEngine.Vector2 vector5 = center - vector4;
				global::UnityEngine.Vector2 vector6 = vector5 - vector2;
				switch (m_layout)
				{
				case global::Kampai.UI.TransformOffset.Layout.Center:
					vector = new global::UnityEngine.Vector3(vector6.x, vector6.y, 0f);
					break;
				case global::Kampai.UI.TransformOffset.Layout.Vertical:
				case global::Kampai.UI.TransformOffset.Layout.Top:
				case global::Kampai.UI.TransformOffset.Layout.Bottom:
					vector = new global::UnityEngine.Vector3(0f, vector6.y, 0f);
					break;
				case global::Kampai.UI.TransformOffset.Layout.Horizontal:
					vector = new global::UnityEngine.Vector3(vector6.x, 0f, 0f);
					break;
				}
			}
			vector += m_offsetPosition;
			for (int j = 0; j < verts.Count; j++)
			{
				global::UnityEngine.UIVertex value = verts[j];
				value.position += vector;
				verts[j] = value;
			}
		}
	}
}

namespace Kampai.UI
{
	[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.UI.LayoutElement))]
	[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.RectTransform))]
	[global::UnityEngine.ExecuteInEditMode]
	[global::UnityEngine.AddComponentMenu("UI/Layout/Aspect Ratio Layout Element", 14)]
	public class AspectRatioLayoutElement : global::UnityEngine.EventSystems.UIBehaviour
	{
		private global::UnityEngine.RectTransform m_rectTransform;

		private global::UnityEngine.UI.LayoutElement m_layoutElement;

		public bool useWidth = true;

		public float m_paddingPercent = 1f;

		public global::UnityEngine.UI.LayoutElement LayoutElement
		{
			get
			{
				global::UnityEngine.UI.LayoutElement obj = m_layoutElement ?? GetComponent<global::UnityEngine.UI.LayoutElement>();
				global::UnityEngine.UI.LayoutElement result = obj;
				m_layoutElement = obj;
				return result;
			}
		}

		public global::UnityEngine.RectTransform rectTransform
		{
			get
			{
				global::UnityEngine.RectTransform obj = m_rectTransform ?? GetComponent<global::UnityEngine.RectTransform>();
				global::UnityEngine.RectTransform result = obj;
				m_rectTransform = obj;
				return result;
			}
		}

		protected override void Start()
		{
			base.Start();
			UpdateRect();
		}

		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			UpdateRect();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			UpdateRect();
		}

		protected override void OnDisable()
		{
			UpdateRect();
			base.OnDisable();
		}

		protected override void OnDidApplyAnimationProperties()
		{
			UpdateRect();
		}

		protected override void OnBeforeTransformParentChanged()
		{
			ResetRect();
		}

		private void ResetRect()
		{
			if (!(LayoutElement == null) && !(rectTransform == null))
			{
				if (useWidth)
				{
					global::UnityEngine.UI.LayoutElement layoutElement = LayoutElement;
					float num = 0f;
					LayoutElement.minWidth = num;
					layoutElement.preferredWidth = num;
				}
				else
				{
					global::UnityEngine.UI.LayoutElement layoutElement2 = LayoutElement;
					float num = 0f;
					LayoutElement.minHeight = num;
					layoutElement2.preferredHeight = num;
				}
			}
		}

		private void UpdateRect()
		{
			if (!(LayoutElement == null) && !(rectTransform == null))
			{
				if (useWidth)
				{
					global::UnityEngine.UI.LayoutElement layoutElement = LayoutElement;
					float num = rectTransform.rect.height * m_paddingPercent;
					LayoutElement.minWidth = num;
					layoutElement.preferredWidth = num;
				}
				else
				{
					global::UnityEngine.UI.LayoutElement layoutElement2 = LayoutElement;
					float num = rectTransform.rect.width * m_paddingPercent;
					LayoutElement.minHeight = num;
					layoutElement2.preferredHeight = num;
				}
				global::UnityEngine.UI.LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
			}
		}
	}
}

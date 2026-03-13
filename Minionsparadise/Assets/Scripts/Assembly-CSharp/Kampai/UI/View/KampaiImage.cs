namespace Kampai.UI.View
{
	public class KampaiImage : global::UnityEngine.UI.Image
	{
		[global::System.Flags]
		private enum HashCodeFlags
		{
			None = 1,
			IsMaskable = 2,
			HasDesaturate = 4,
			HasTint = 8,
			HasOverlay = 0x10
		}

		public interface IImageCreationArgs
		{
			void SetValues(global::Kampai.UI.View.KampaiImage kampaiImage);
		}

		public class ImageCreationArgs<T> : global::Kampai.UI.View.KampaiImage.IImageCreationArgs where T : global::Kampai.UI.View.KampaiImage.ImageCreationArgs<T>
		{
			internal global::UnityEngine.UI.Image.Type type;

			protected string image;

			protected string mask;

			protected global::UnityEngine.Transform parentTransform;

			protected global::UnityEngine.UI.LayoutElement layoutElement;

			protected bool isActive = true;

			protected bool isEnabled = true;

			public T SetImage(string imagePath)
			{
				image = imagePath;
				return this as T;
			}

			public T SetEnabled(bool isActive)
			{
				isEnabled = isActive;
				return this as T;
			}

			public T SetActive(bool isActive)
			{
				this.isActive = isActive;
				return this as T;
			}

			public T SetMask(string maskPath)
			{
				mask = maskPath;
				return this as T;
			}

			public T SetParent(global::UnityEngine.Transform parent)
			{
				parentTransform = parent;
				return this as T;
			}

			public T SetLayoutElement(global::UnityEngine.UI.LayoutElement layoutElement)
			{
				this.layoutElement = layoutElement;
				return this as T;
			}

			public virtual void SetValues(global::Kampai.UI.View.KampaiImage kampaiImage)
			{
				global::UnityEngine.RectTransform rectTransform = kampaiImage.GetComponent<global::UnityEngine.RectTransform>() ?? kampaiImage.gameObject.AddComponent<global::UnityEngine.RectTransform>();
				rectTransform.SetParent(parentTransform, false);
				kampaiImage.gameObject.SetActive(isActive);
				kampaiImage.enabled = isEnabled;
				kampaiImage.type = type;
				kampaiImage.sprite = UIUtils.LoadSpriteFromPath(image);
				kampaiImage.maskSprite = UIUtils.LoadSpriteFromPath(mask);
				if (!(this.layoutElement == null))
				{
					global::UnityEngine.UI.LayoutElement layoutElement = kampaiImage.GetComponent<global::UnityEngine.UI.LayoutElement>() ?? kampaiImage.gameObject.AddComponent<global::UnityEngine.UI.LayoutElement>();
					layoutElement.ignoreLayout = this.layoutElement.ignoreLayout;
					if (!layoutElement.ignoreLayout)
					{
						layoutElement.flexibleHeight = this.layoutElement.flexibleHeight;
						layoutElement.flexibleWidth = this.layoutElement.flexibleWidth;
						layoutElement.minHeight = this.layoutElement.minHeight;
						layoutElement.minWidth = this.layoutElement.minWidth;
						layoutElement.preferredHeight = this.layoutElement.preferredHeight;
						layoutElement.preferredWidth = this.layoutElement.preferredWidth;
					}
				}
			}

			private global::Kampai.UI.View.KampaiImage Build(global::UnityEngine.GameObject imageGameObject)
			{
				if (imageGameObject == null)
				{
					return null;
				}
				global::Kampai.UI.View.KampaiImage kampaiImage = imageGameObject.GetComponent<global::Kampai.UI.View.KampaiImage>() ?? imageGameObject.AddComponent<global::Kampai.UI.View.KampaiImage>();
				if (kampaiImage == null)
				{
					return null;
				}
				SetValues(kampaiImage);
				return kampaiImage;
			}

			public global::Kampai.UI.View.KampaiImage CreateKampaiImage(string name)
			{
				return Build(new global::UnityEngine.GameObject(name));
			}

			public global::Kampai.UI.View.KampaiImage CloneKampaiImage(global::UnityEngine.GameObject imageGameObject)
			{
				return Build(global::UnityEngine.Object.Instantiate(imageGameObject, global::UnityEngine.Vector3.zero, global::UnityEngine.Quaternion.identity) as global::UnityEngine.GameObject);
			}

			public global::Kampai.UI.View.KampaiImage CloneKampaiImage(global::UnityEngine.GameObject imageGameObject, string name)
			{
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(imageGameObject, global::UnityEngine.Vector3.zero, global::UnityEngine.Quaternion.identity) as global::UnityEngine.GameObject;
				if (gameObject == null)
				{
					return null;
				}
				gameObject.name = name;
				return Build(gameObject);
			}
		}

		public class SimpleImageCreationArgs : global::Kampai.UI.View.KampaiImage.ImageCreationArgs<global::Kampai.UI.View.KampaiImage.SimpleImageCreationArgs>
		{
			protected bool preserveAspect;

			public global::Kampai.UI.View.KampaiImage.SimpleImageCreationArgs ShouldPreserveAspect(bool preserveAspect)
			{
				this.preserveAspect = preserveAspect;
				return this;
			}

			public override void SetValues(global::Kampai.UI.View.KampaiImage kampaiImage)
			{
				base.SetValues(kampaiImage);
				kampaiImage.preserveAspect = preserveAspect;
			}
		}

		public class SlicedImageCreationArgs : global::Kampai.UI.View.KampaiImage.ImageCreationArgs<global::Kampai.UI.View.KampaiImage.SlicedImageCreationArgs>
		{
			protected bool fillCenter;

			public global::Kampai.UI.View.KampaiImage.SlicedImageCreationArgs ShouldFillCenter(bool fillCenter)
			{
				this.fillCenter = fillCenter;
				return this;
			}

			public override void SetValues(global::Kampai.UI.View.KampaiImage kampaiImage)
			{
				base.SetValues(kampaiImage);
				kampaiImage.fillCenter = fillCenter;
			}
		}

		public class FilledImageCreationArgs : global::Kampai.UI.View.KampaiImage.SimpleImageCreationArgs
		{
			protected bool fillClockwise;

			protected float fillAmount;

			protected global::UnityEngine.UI.Image.FillMethod fillMethod;

			protected global::UnityEngine.UI.Image.FillMethod fillOrigin;

			public global::Kampai.UI.View.KampaiImage.FilledImageCreationArgs SetFillClockwise(bool fillClockwise)
			{
				this.fillClockwise = fillClockwise;
				return this;
			}

			public global::Kampai.UI.View.KampaiImage.FilledImageCreationArgs SetFillOrigin(global::UnityEngine.UI.Image.FillMethod fillOrigin)
			{
				this.fillOrigin = fillOrigin;
				return this;
			}

			public global::Kampai.UI.View.KampaiImage.FilledImageCreationArgs SetFillMethod(global::UnityEngine.UI.Image.FillMethod fillMethod)
			{
				this.fillMethod = fillMethod;
				return this;
			}

			public global::Kampai.UI.View.KampaiImage.FilledImageCreationArgs SetFillAmount(float fillAmount)
			{
				this.fillAmount = fillAmount;
				return this;
			}

			public override void SetValues(global::Kampai.UI.View.KampaiImage kampaiImage)
			{
				base.SetValues(kampaiImage);
				kampaiImage.fillMethod = fillMethod;
				kampaiImage.fillOrigin = (int)fillOrigin;
				kampaiImage.fillAmount = fillAmount;
				kampaiImage.fillClockwise = fillClockwise;
			}
		}

		public class ImageCreationBuilder
		{
			private global::Kampai.UI.View.KampaiImage.IImageCreationArgs args;

			private global::Kampai.UI.View.KampaiImage.IImageCreationArgs SetType(global::UnityEngine.UI.Image.Type type)
			{
				switch (type)
				{
				case global::UnityEngine.UI.Image.Type.Simple:
					args = new global::Kampai.UI.View.KampaiImage.SimpleImageCreationArgs
					{
						type = type
					};
					break;
				case global::UnityEngine.UI.Image.Type.Sliced:
					args = new global::Kampai.UI.View.KampaiImage.SlicedImageCreationArgs
					{
						type = type
					};
					break;
				case global::UnityEngine.UI.Image.Type.Filled:
					args = new global::Kampai.UI.View.KampaiImage.FilledImageCreationArgs
					{
						type = type
					};
					break;
				}
				return args;
			}

			public global::Kampai.UI.View.KampaiImage.SimpleImageCreationArgs CreateSimpleImage()
			{
				return SetType(global::UnityEngine.UI.Image.Type.Simple) as global::Kampai.UI.View.KampaiImage.SimpleImageCreationArgs;
			}

			public global::Kampai.UI.View.KampaiImage.SlicedImageCreationArgs CreateSlicedImage()
			{
				return SetType(global::UnityEngine.UI.Image.Type.Sliced) as global::Kampai.UI.View.KampaiImage.SlicedImageCreationArgs;
			}

			public global::Kampai.UI.View.KampaiImage.FilledImageCreationArgs CreateFilledImage()
			{
				return SetType(global::UnityEngine.UI.Image.Type.Filled) as global::Kampai.UI.View.KampaiImage.FilledImageCreationArgs;
			}
		}

		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("KampaiImage") as global::Kampai.Util.IKampaiLogger;

		[global::UnityEngine.SerializeField]
		private global::UnityEngine.Sprite m_maskSprite;

		private static readonly global::UnityEngine.Vector2[] s_VertScratch = new global::UnityEngine.Vector2[4];

		private static readonly global::UnityEngine.Vector2[] s_Uv = new global::UnityEngine.Vector2[4];

		private static readonly global::UnityEngine.Vector2[] s_Xy = new global::UnityEngine.Vector2[4];

		private static readonly global::UnityEngine.Vector2[] s_UVScratch = new global::UnityEngine.Vector2[4];

		private static readonly global::UnityEngine.Vector2[] s_MaskUVScratch = new global::UnityEngine.Vector2[4];

		private static readonly global::UnityEngine.Vector2[] s_MaskUv = new global::UnityEngine.Vector2[4];

		private static readonly global::Kampai.Util.MaterialCache m_cache = new global::Kampai.Util.MaterialCache();

		private int m_hashCode;

		private global::Kampai.UI.View.KampaiImage.HashCodeFlags m_hashCodeFlags = global::Kampai.UI.View.KampaiImage.HashCodeFlags.None;

		private global::UnityEngine.Material myMaterial;

		private float m_Desat;

		private global::UnityEngine.Color m_Tint = global::UnityEngine.Color.white;

		private global::UnityEngine.Color m_Overlay;

		public override global::UnityEngine.Material materialForRendering
		{
			get
			{
				if (myMaterial == null && mainTexture != null)
				{
					m_hashCodeFlags |= (global::Kampai.UI.View.KampaiImage.HashCodeFlags)((!base.maskable) ? 1 : 2);
					m_hashCode = string.Format("{0}{1}{2}{3}{4}{5}", mainTexture.name, (!(m_maskSprite == null)) ? m_maskSprite.name : string.Empty, base.maskable, m_Desat, m_Overlay, m_hashCodeFlags).GetHashCode() + material.GetHashCode();
					myMaterial = m_cache.GetMaterial(m_hashCode, material);
				}
				return GetModifiedMaterial(myMaterial ?? material);
			}
		}

		public float Desaturate
		{
			get
			{
				return m_Desat;
			}
			set
			{
				m_Desat = value;
				m_hashCodeFlags |= global::Kampai.UI.View.KampaiImage.HashCodeFlags.HasDesaturate;
				myMaterial = null;
				SetMaterialDirty();
			}
		}

		public global::UnityEngine.Color Tint
		{
			get
			{
				return m_Tint;
			}
			set
			{
				m_Tint = value;
				m_hashCodeFlags |= global::Kampai.UI.View.KampaiImage.HashCodeFlags.HasTint;
				myMaterial = null;
				SetMaterialDirty();
			}
		}

		public global::UnityEngine.Color Overlay
		{
			get
			{
				return m_Overlay;
			}
			set
			{
				m_Overlay = value;
				m_hashCodeFlags |= global::Kampai.UI.View.KampaiImage.HashCodeFlags.HasOverlay;
				myMaterial = null;
				SetMaterialDirty();
			}
		}

		public global::UnityEngine.Sprite maskSprite
		{
			get
			{
				return m_maskSprite;
			}
			set
			{
				if (!(m_maskSprite == value))
				{
					m_maskSprite = value;
					myMaterial = null;
					SetAllDirty();
				}
			}
		}

		protected override void OnDestroy()
		{
			m_cache.RemoveReference(m_hashCode);
			base.OnDestroy();
		}

		protected override void OnPopulateMesh(global::UnityEngine.UI.VertexHelper vbo)
		{
			if (base.overrideSprite == null)
			{
				base.OnPopulateMesh(vbo);
				return;
			}
			vbo.Clear();
			switch (base.type)
			{
			case global::UnityEngine.UI.Image.Type.Simple:
				GenerateSimpleSprite(vbo, base.preserveAspect);
				break;
			case global::UnityEngine.UI.Image.Type.Sliced:
				GenerateSlicedSprite(vbo);
				break;
			case global::UnityEngine.UI.Image.Type.Tiled:
				logger.Error("Tiled image type is not supported on KampaiImage");
				break;
			case global::UnityEngine.UI.Image.Type.Filled:
				GenerateFilledSprite(vbo, base.preserveAspect);
				break;
			}
		}

		protected override void UpdateMaterial()
		{
			base.UpdateMaterial();
			global::UnityEngine.Material material = materialForRendering;
			if (IsActive() && !(material == null))
			{
				material.SetTexture(global::Kampai.Util.GameConstants.ShaderProperties.UI.AlphaTex, (!(m_maskSprite != null)) ? global::UnityEngine.Texture2D.whiteTexture : m_maskSprite.texture);
				if (material.HasProperty(global::Kampai.Util.GameConstants.ShaderProperties.UI.Overlay))
				{
					material.SetColor(global::Kampai.Util.GameConstants.ShaderProperties.UI.Overlay, m_Overlay);
				}
				else if (logger != null)
				{
					logger.Error("Unable to set overlay: property on material for texture {0} does not exist.", GetSafeSpriteTextureName(base.sprite));
				}
				if (material.HasProperty(global::Kampai.Util.GameConstants.ShaderProperties.UI.Desaturation))
				{
					material.SetFloat(global::Kampai.Util.GameConstants.ShaderProperties.UI.Desaturation, m_Desat);
				}
				else if (logger != null)
				{
					logger.Error("Unable to set desaturation: property on material for texture {0} does not exist.", GetSafeSpriteTextureName(base.sprite));
				}
				material.color = m_Tint;
			}
		}

		private static string GetSafeSpriteTextureName(global::UnityEngine.Sprite sprite)
		{
			if (sprite == null)
			{
				return "(sprite is null)";
			}
			if (sprite.texture == null)
			{
				return "(sprite.texture is null)";
			}
			if (sprite.texture.name == null)
			{
				return "(sprite.texture.name is null)";
			}
			return sprite.texture.name;
		}

		private void GenerateSimpleSprite(global::UnityEngine.UI.VertexHelper vbo, bool preserveAspect)
		{
			global::UnityEngine.Vector4 drawingDimensions = GetDrawingDimensions(preserveAspect);
			global::UnityEngine.Vector4 vector = ((!(base.overrideSprite != null)) ? global::UnityEngine.Vector4.zero : global::UnityEngine.Sprites.DataUtility.GetOuterUV(base.overrideSprite));
			global::UnityEngine.Vector4 vector2 = ((m_maskSprite == null) ? global::UnityEngine.Vector4.zero : ((!m_maskSprite.packed) ? vector : global::UnityEngine.Sprites.DataUtility.GetOuterUV(m_maskSprite)));
			vbo.Clear();
			vbo.AddVert(new global::UnityEngine.Vector3(drawingDimensions.x, drawingDimensions.y), base.color, new global::UnityEngine.Vector2(vector.x, vector.y), new global::UnityEngine.Vector2(vector2.x, vector2.y));
			vbo.AddVert(new global::UnityEngine.Vector3(drawingDimensions.x, drawingDimensions.w), base.color, new global::UnityEngine.Vector2(vector.x, vector.w), new global::UnityEngine.Vector2(vector2.x, vector2.w));
			vbo.AddVert(new global::UnityEngine.Vector3(drawingDimensions.z, drawingDimensions.w), base.color, new global::UnityEngine.Vector2(vector.z, vector.w), new global::UnityEngine.Vector2(vector2.z, vector2.w));
			vbo.AddVert(new global::UnityEngine.Vector3(drawingDimensions.z, drawingDimensions.y), base.color, new global::UnityEngine.Vector2(vector.z, vector.y), new global::UnityEngine.Vector2(vector2.z, vector2.y));
			vbo.AddTriangle(0, 1, 2);
			vbo.AddTriangle(2, 3, 0);
		}

		private void GenerateSlicedSprite(global::UnityEngine.UI.VertexHelper vbo)
		{
			if (!base.hasBorder)
			{
				GenerateSimpleSprite(vbo, false);
				return;
			}
			global::UnityEngine.Vector4 zero = global::UnityEngine.Vector4.zero;
			global::UnityEngine.Vector4 vector;
			global::UnityEngine.Vector4 vector2;
			global::UnityEngine.Vector4 vector3;
			global::UnityEngine.Vector4 vector4;
			if (base.overrideSprite != null)
			{
				vector = global::UnityEngine.Sprites.DataUtility.GetOuterUV(base.overrideSprite);
				vector2 = global::UnityEngine.Sprites.DataUtility.GetInnerUV(base.overrideSprite);
				vector3 = global::UnityEngine.Sprites.DataUtility.GetPadding(base.overrideSprite);
				vector4 = base.overrideSprite.border;
			}
			else
			{
				vector = zero;
				vector2 = zero;
				vector3 = zero;
				vector4 = zero;
			}
			global::UnityEngine.Vector4 vector5;
			global::UnityEngine.Vector4 vector6;
			if (m_maskSprite != null)
			{
				if (m_maskSprite.packed)
				{
					vector5 = global::UnityEngine.Sprites.DataUtility.GetOuterUV(m_maskSprite);
					vector6 = global::UnityEngine.Sprites.DataUtility.GetInnerUV(m_maskSprite);
				}
				else
				{
					vector5 = vector;
					vector6 = vector2;
				}
			}
			else
			{
				vector5 = zero;
				vector6 = zero;
			}
			global::UnityEngine.Rect pixelAdjustedRect = GetPixelAdjustedRect();
			vector4 = GetAdjustedBorders(vector4 / base.pixelsPerUnit, pixelAdjustedRect);
			vector3 /= base.pixelsPerUnit;
			s_VertScratch[0] = new global::UnityEngine.Vector2(vector3.x, vector3.y);
			s_VertScratch[3] = new global::UnityEngine.Vector2(pixelAdjustedRect.width - vector3.z, pixelAdjustedRect.height - vector3.w);
			s_VertScratch[1].x = vector4.x;
			s_VertScratch[1].y = vector4.y;
			s_VertScratch[2].x = pixelAdjustedRect.width - vector4.z;
			s_VertScratch[2].y = pixelAdjustedRect.height - vector4.w;
			for (int i = 0; i < 4; i++)
			{
				s_VertScratch[i].x += pixelAdjustedRect.x;
				s_VertScratch[i].y += pixelAdjustedRect.y;
			}
			s_UVScratch[0] = new global::UnityEngine.Vector2(vector.x, vector.y);
			s_UVScratch[1] = new global::UnityEngine.Vector2(vector2.x, vector2.y);
			s_UVScratch[2] = new global::UnityEngine.Vector2(vector2.z, vector2.w);
			s_UVScratch[3] = new global::UnityEngine.Vector2(vector.z, vector.w);
			s_MaskUVScratch[0] = new global::UnityEngine.Vector2(vector5.x, vector5.y);
			s_MaskUVScratch[1] = new global::UnityEngine.Vector2(vector6.x, vector6.y);
			s_MaskUVScratch[2] = new global::UnityEngine.Vector2(vector6.z, vector6.w);
			s_MaskUVScratch[3] = new global::UnityEngine.Vector2(vector5.z, vector5.w);
			vbo.Clear();
			for (int j = 0; j < 3; j++)
			{
				int num = j + 1;
				for (int k = 0; k < 3; k++)
				{
					if (base.fillCenter || j != 1 || k != 1)
					{
						int num2 = k + 1;
						AddQuad(vbo, new global::UnityEngine.Vector2(s_VertScratch[j].x, s_VertScratch[k].y), new global::UnityEngine.Vector2(s_VertScratch[num].x, s_VertScratch[num2].y), base.color, new global::UnityEngine.Vector2(s_UVScratch[j].x, s_UVScratch[k].y), new global::UnityEngine.Vector2(s_UVScratch[num].x, s_UVScratch[num2].y), new global::UnityEngine.Vector2(s_MaskUVScratch[j].x, s_MaskUVScratch[k].y), new global::UnityEngine.Vector2(s_MaskUVScratch[num].x, s_MaskUVScratch[num2].y));
					}
				}
			}
		}

		private void GenerateFilledSprite(global::UnityEngine.UI.VertexHelper vbo, bool preserveAspect)
		{
			if (base.fillAmount < 0.001f)
			{
				return;
			}
			global::UnityEngine.Vector4 drawingDimensions = GetDrawingDimensions(preserveAspect);
			global::UnityEngine.Vector4 vector = ((base.overrideSprite != null) ? global::UnityEngine.Sprites.DataUtility.GetOuterUV(base.overrideSprite) : global::UnityEngine.Vector4.zero);
			global::UnityEngine.Vector4 vector2 = ((!m_maskSprite.packed) ? vector : ((!(m_maskSprite == null)) ? global::UnityEngine.Sprites.DataUtility.GetOuterUV(m_maskSprite) : global::UnityEngine.Vector4.zero));
			global::UnityEngine.UIVertex simpleVert = global::UnityEngine.UIVertex.simpleVert;
			simpleVert.color = base.color;
			float x = vector.x;
			float y = vector.y;
			float z = vector.z;
			float w = vector.w;
			float x2 = vector2.x;
			float y2 = vector2.y;
			float z2 = vector2.z;
			float w2 = vector2.w;
			ProcessHorizontalOrVerticleFill(x, y, z, w, x2, y2, z2, w2, drawingDimensions);
			if (base.fillAmount < 1f)
			{
				switch (base.fillMethod)
				{
				case global::UnityEngine.UI.Image.FillMethod.Radial90:
					ProcessRadialFill_90(vbo, simpleVert);
					return;
				case global::UnityEngine.UI.Image.FillMethod.Radial180:
					ProcessRadialFill_180(vbo, simpleVert, x, y, z, w, x2, y2, z2, w2, drawingDimensions);
					return;
				case global::UnityEngine.UI.Image.FillMethod.Radial360:
					ProcessRadialFill_360(vbo, simpleVert, x, y, z, w, x2, y2, z2, w2, drawingDimensions);
					return;
				}
			}
			SetupVBO(vbo, simpleVert);
		}

		private void ProcessHorizontalOrVerticleFill(float num, float num2, float num3, float num4, float maskNum, float maskNum2, float maskNum3, float maskNum4, global::UnityEngine.Vector4 drawingDimensions)
		{
			if (base.fillMethod == global::UnityEngine.UI.Image.FillMethod.Horizontal || base.fillMethod == global::UnityEngine.UI.Image.FillMethod.Vertical)
			{
				switch (base.fillMethod)
				{
				case global::UnityEngine.UI.Image.FillMethod.Horizontal:
				{
					float num7 = (num3 - num) * base.fillAmount;
					float num8 = (maskNum3 - maskNum) * base.fillAmount;
					if (base.fillOrigin == 1)
					{
						drawingDimensions.x = drawingDimensions.z - (drawingDimensions.z - drawingDimensions.x) * base.fillAmount;
						num = num3 - num7;
						maskNum = maskNum3 - num8;
					}
					else
					{
						drawingDimensions.z = drawingDimensions.x + (drawingDimensions.z - drawingDimensions.x) * base.fillAmount;
						num3 = num + num7;
						maskNum3 = maskNum + num8;
					}
					break;
				}
				case global::UnityEngine.UI.Image.FillMethod.Vertical:
				{
					float num5 = (num4 - num2) * base.fillAmount;
					float num6 = (maskNum4 - maskNum2) * base.fillAmount;
					if (base.fillOrigin == 1)
					{
						drawingDimensions.y = drawingDimensions.w - (drawingDimensions.w - drawingDimensions.y) * base.fillAmount;
						num2 = num4 - num5;
						maskNum2 = maskNum4 - num6;
					}
					else
					{
						drawingDimensions.w = drawingDimensions.y + (drawingDimensions.w - drawingDimensions.y) * base.fillAmount;
						num4 = num2 + num5;
						maskNum4 = maskNum2 + num6;
					}
					break;
				}
				}
			}
			s_Xy[0] = new global::UnityEngine.Vector2(drawingDimensions.x, drawingDimensions.y);
			s_Xy[1] = new global::UnityEngine.Vector2(drawingDimensions.x, drawingDimensions.w);
			s_Xy[2] = new global::UnityEngine.Vector2(drawingDimensions.z, drawingDimensions.w);
			s_Xy[3] = new global::UnityEngine.Vector2(drawingDimensions.z, drawingDimensions.y);
			s_Uv[0] = new global::UnityEngine.Vector2(num, num2);
			s_Uv[1] = new global::UnityEngine.Vector2(num, num4);
			s_Uv[2] = new global::UnityEngine.Vector2(num3, num4);
			s_Uv[3] = new global::UnityEngine.Vector2(num3, num2);
			s_MaskUv[0] = new global::UnityEngine.Vector2(maskNum, maskNum2);
			s_MaskUv[1] = new global::UnityEngine.Vector2(maskNum, maskNum4);
			s_MaskUv[2] = new global::UnityEngine.Vector2(maskNum3, maskNum4);
			s_MaskUv[3] = new global::UnityEngine.Vector2(maskNum3, maskNum2);
		}

		private void ProcessRadialFill_90(global::UnityEngine.UI.VertexHelper vbo, global::UnityEngine.UIVertex simpleVert)
		{
			if (RadialCut(s_Xy, s_Uv, s_MaskUv, base.fillAmount, base.fillClockwise, base.fillOrigin))
			{
				SetupVBO(vbo, simpleVert);
			}
		}

		private void ProcessRadialFill_180(global::UnityEngine.UI.VertexHelper vbo, global::UnityEngine.UIVertex simpleVert, float num, float num2, float num3, float num4, float maskNum, float maskNum2, float maskNum3, float maskNum4, global::UnityEngine.Vector4 drawingDimensions)
		{
			for (int i = 0; i < 2; i++)
			{
				int num5 = ((base.fillOrigin > 1) ? 1 : 0);
				float custom;
				float custom2;
				float custom3;
				float custom4;
				if (base.fillOrigin == 0 || base.fillOrigin == 2)
				{
					custom = 0f;
					custom2 = 1f;
					if (i == num5)
					{
						custom3 = 0f;
						custom4 = 0.5f;
					}
					else
					{
						custom3 = 0.5f;
						custom4 = 1f;
					}
				}
				else
				{
					custom3 = 0f;
					custom4 = 1f;
					if (i == num5)
					{
						custom = 0.5f;
						custom2 = 1f;
					}
					else
					{
						custom = 0f;
						custom2 = 0.5f;
					}
				}
				SetupKampaiImageProperties(num, num2, num3, num4, maskNum, maskNum2, maskNum3, maskNum4, drawingDimensions, custom3, custom4, custom, custom2);
				float value = (base.fillClockwise ? (base.fillAmount * 2f - (float)i) : (base.fillAmount * 2f - (float)(1 - i)));
				if (RadialCut(s_Xy, s_Uv, s_MaskUv, global::UnityEngine.Mathf.Clamp01(value), base.fillClockwise, (i + base.fillOrigin + 3) % 4))
				{
					SetupVBO(vbo, simpleVert);
				}
			}
		}

		private void ProcessRadialFill_360(global::UnityEngine.UI.VertexHelper vbo, global::UnityEngine.UIVertex simpleVert, float num, float num2, float num3, float num4, float maskNum, float maskNum2, float maskNum3, float maskNum4, global::UnityEngine.Vector4 drawingDimensions)
		{
			for (int i = 0; i < 4; i++)
			{
				float custom;
				float custom2;
				if (i < 2)
				{
					custom = 0f;
					custom2 = 0.5f;
				}
				else
				{
					custom = 0.5f;
					custom2 = 1f;
				}
				float custom3;
				float custom4;
				if (i == 0 || i == 3)
				{
					custom3 = 0f;
					custom4 = 0.5f;
				}
				else
				{
					custom3 = 0.5f;
					custom4 = 1f;
				}
				SetupKampaiImageProperties(num, num2, num3, num4, maskNum, maskNum2, maskNum3, maskNum4, drawingDimensions, custom, custom2, custom3, custom4);
				float value = (base.fillClockwise ? (base.fillAmount * 4f - (float)((i + base.fillOrigin) % 4)) : (base.fillAmount * 4f - (float)(3 - (i + base.fillOrigin) % 4)));
				if (RadialCut(s_Xy, s_Uv, s_MaskUv, global::UnityEngine.Mathf.Clamp01(value), base.fillClockwise, (i + 2) % 4))
				{
					SetupVBO(vbo, simpleVert);
				}
			}
		}

		private void SetupKampaiImageProperties(float num, float num2, float num3, float num4, float maskNum, float maskNum2, float maskNum3, float maskNum4, global::UnityEngine.Vector4 drawingDimensions, float custom1, float custom2, float custom3, float custom4)
		{
			s_Xy[0].x = global::UnityEngine.Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, custom1);
			s_Xy[1].x = s_Xy[0].x;
			s_Xy[2].x = global::UnityEngine.Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, custom2);
			s_Xy[3].x = s_Xy[2].x;
			s_Xy[0].y = global::UnityEngine.Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, custom3);
			s_Xy[1].y = global::UnityEngine.Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, custom4);
			s_Xy[2].y = s_Xy[1].y;
			s_Xy[3].y = s_Xy[0].y;
			s_Uv[0].x = global::UnityEngine.Mathf.Lerp(num, num3, custom1);
			s_Uv[1].x = s_Uv[0].x;
			s_Uv[2].x = global::UnityEngine.Mathf.Lerp(num, num3, custom2);
			s_Uv[3].x = s_Uv[2].x;
			s_Uv[0].y = global::UnityEngine.Mathf.Lerp(num2, num4, custom3);
			s_Uv[1].y = global::UnityEngine.Mathf.Lerp(num2, num4, custom4);
			s_Uv[2].y = s_Uv[1].y;
			s_Uv[3].y = s_Uv[0].y;
			s_MaskUv[0].x = global::UnityEngine.Mathf.Lerp(maskNum, maskNum3, custom1);
			s_MaskUv[1].x = s_MaskUv[0].x;
			s_MaskUv[2].x = global::UnityEngine.Mathf.Lerp(maskNum, maskNum3, custom2);
			s_MaskUv[3].x = s_MaskUv[2].x;
			s_MaskUv[0].y = global::UnityEngine.Mathf.Lerp(maskNum2, maskNum4, custom3);
			s_MaskUv[1].y = global::UnityEngine.Mathf.Lerp(maskNum2, maskNum4, custom4);
			s_MaskUv[2].y = s_MaskUv[1].y;
			s_MaskUv[3].y = s_MaskUv[0].y;
		}

		private global::UnityEngine.Vector4 GetAdjustedBorders(global::UnityEngine.Vector4 border, global::UnityEngine.Rect rect)
		{
			for (int i = 0; i <= 1; i++)
			{
				float num = border[i] + border[i + 2];
				if (rect.size[i] < num && num != 0f)
				{
					float num2 = rect.size[i] / num;
					int index2;
					int index = (index2 = i);
					float num3 = border[index2];
					border[index] = num3 * num2;
					int index3 = (index2 = i + 2);
					num3 = border[index2];
					border[index3] = num3 * num2;
				}
			}
			return border;
		}

		private global::UnityEngine.Vector4 GetDrawingDimensions(bool shouldPreserveAspect)
		{
			global::UnityEngine.Vector4 vector = ((base.overrideSprite == null) ? global::UnityEngine.Vector4.zero : global::UnityEngine.Sprites.DataUtility.GetPadding(base.overrideSprite));
			global::UnityEngine.Vector2 vector2 = ((base.overrideSprite == null) ? global::UnityEngine.Vector2.zero : new global::UnityEngine.Vector2(base.overrideSprite.rect.width, base.overrideSprite.rect.height));
			global::UnityEngine.Rect pixelAdjustedRect = GetPixelAdjustedRect();
			int num = global::UnityEngine.Mathf.RoundToInt(vector2.x);
			int num2 = global::UnityEngine.Mathf.RoundToInt(vector2.y);
			global::UnityEngine.Vector4 vector3 = new global::UnityEngine.Vector4(vector.x / (float)num, vector.y / (float)num2, ((float)num - vector.z) / (float)num, ((float)num2 - vector.w) / (float)num2);
			if (shouldPreserveAspect && vector2.sqrMagnitude > 0f)
			{
				float num3 = vector2.x / vector2.y;
				float num4 = pixelAdjustedRect.width / pixelAdjustedRect.height;
				if (num3 > num4)
				{
					float height = pixelAdjustedRect.height;
					pixelAdjustedRect.height = pixelAdjustedRect.width * (1f / num3);
					pixelAdjustedRect.y += (height - pixelAdjustedRect.height) * base.rectTransform.pivot.y;
				}
				else
				{
					float width = pixelAdjustedRect.width;
					pixelAdjustedRect.width = pixelAdjustedRect.height * num3;
					pixelAdjustedRect.x += (width - pixelAdjustedRect.width) * base.rectTransform.pivot.x;
				}
			}
			return new global::UnityEngine.Vector4(pixelAdjustedRect.x + pixelAdjustedRect.width * vector3.x, pixelAdjustedRect.y + pixelAdjustedRect.height * vector3.y, pixelAdjustedRect.x + pixelAdjustedRect.width * vector3.z, pixelAdjustedRect.y + pixelAdjustedRect.height * vector3.w);
		}

		private static void RadialCut(global::UnityEngine.Vector2[] xy, float cos, float sin, bool invert, int corner)
		{
			int num = (corner + 1) % 4;
			int num2 = (corner + 2) % 4;
			int num3 = (corner + 3) % 4;
			if ((corner & 1) == 1)
			{
				if (sin > cos)
				{
					cos /= sin;
					sin = 1f;
					if (invert)
					{
						xy[num].x = global::UnityEngine.Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
						xy[num2].x = xy[num].x;
					}
				}
				else if (cos > sin)
				{
					sin /= cos;
					cos = 1f;
					if (!invert)
					{
						xy[num2].y = global::UnityEngine.Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
						xy[num3].y = xy[num2].y;
					}
				}
				else
				{
					cos = 1f;
					sin = 1f;
				}
				if (!invert)
				{
					xy[num3].x = global::UnityEngine.Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
				}
				else
				{
					xy[num].y = global::UnityEngine.Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
				}
				return;
			}
			if (cos > sin)
			{
				sin /= cos;
				cos = 1f;
				if (!invert)
				{
					xy[num].y = global::UnityEngine.Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
					xy[num2].y = xy[num].y;
				}
			}
			else if (sin > cos)
			{
				cos /= sin;
				sin = 1f;
				if (invert)
				{
					xy[num2].x = global::UnityEngine.Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
					xy[num3].x = xy[num2].x;
				}
			}
			else
			{
				cos = 1f;
				sin = 1f;
			}
			if (invert)
			{
				xy[num3].y = global::UnityEngine.Mathf.Lerp(xy[corner].y, xy[num2].y, sin);
			}
			else
			{
				xy[num].x = global::UnityEngine.Mathf.Lerp(xy[corner].x, xy[num2].x, cos);
			}
		}

		private static bool RadialCut(global::UnityEngine.Vector2[] xy, global::UnityEngine.Vector2[] uv, global::UnityEngine.Vector2[] maskUV, float fill, bool invert, int corner)
		{
			if (fill < 0.001f)
			{
				return false;
			}
			if ((corner & 1) == 1)
			{
				invert = !invert;
			}
			if (!invert && fill > 0.999f)
			{
				return true;
			}
			float num = global::UnityEngine.Mathf.Clamp01(fill);
			if (invert)
			{
				num = 1f - num;
			}
			num *= (float)global::System.Math.PI / 2f;
			float cos = global::UnityEngine.Mathf.Cos(num);
			float sin = global::UnityEngine.Mathf.Sin(num);
			RadialCut(xy, cos, sin, invert, corner);
			RadialCut(uv, cos, sin, invert, corner);
			RadialCut(maskUV, cos, sin, invert, corner);
			return true;
		}

		public void SetStencilMaterial()
		{
			material = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.Material>("StencilAlphaMaskMat");
		}

		private static void SetupVBO(global::UnityEngine.UI.VertexHelper vertexHelper, global::UnityEngine.UIVertex simpleVert)
		{
			int currentVertCount = vertexHelper.currentVertCount;
			for (int i = 0; i < 4; i++)
			{
				vertexHelper.AddVert(s_Xy[i], simpleVert.color, s_Uv[1], s_MaskUv[i]);
			}
			vertexHelper.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
			vertexHelper.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
		}

		private static void AddQuad(global::UnityEngine.UI.VertexHelper vertexHelper, global::UnityEngine.Vector2 posMin, global::UnityEngine.Vector2 posMax, global::UnityEngine.Color32 color, global::UnityEngine.Vector2 uvMin, global::UnityEngine.Vector2 uvMax, global::UnityEngine.Vector2 uvMaskMin, global::UnityEngine.Vector2 uvMaskMax)
		{
			int currentVertCount = vertexHelper.currentVertCount;
			vertexHelper.AddVert(new global::UnityEngine.Vector3(posMin.x, posMin.y, 0f), color, new global::UnityEngine.Vector2(uvMin.x, uvMin.y), new global::UnityEngine.Vector2(uvMaskMin.x, uvMaskMin.y));
			vertexHelper.AddVert(new global::UnityEngine.Vector3(posMin.x, posMax.y, 0f), color, new global::UnityEngine.Vector2(uvMin.x, uvMax.y), new global::UnityEngine.Vector2(uvMaskMin.x, uvMaskMax.y));
			vertexHelper.AddVert(new global::UnityEngine.Vector3(posMax.x, posMax.y, 0f), color, new global::UnityEngine.Vector2(uvMax.x, uvMax.y), new global::UnityEngine.Vector2(uvMaskMax.x, uvMaskMax.y));
			vertexHelper.AddVert(new global::UnityEngine.Vector3(posMax.x, posMin.y, 0f), color, new global::UnityEngine.Vector2(uvMax.x, uvMin.y), new global::UnityEngine.Vector2(uvMaskMax.x, uvMaskMin.y));
			vertexHelper.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
			vertexHelper.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
		}
	}
}

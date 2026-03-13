namespace Swrve.Messaging
{
	public class SwrveMessageRenderer
	{
		protected static readonly global::UnityEngine.Color ButtonPressedColor = new global::UnityEngine.Color(0.5f, 0.5f, 0.5f);

		protected static global::UnityEngine.Texture2D blankTexture;

		protected static global::UnityEngine.Rect WholeScreen = default(global::UnityEngine.Rect);

		public static global::Swrve.Messaging.ISwrveMessageAnimator Animator;

		protected static global::UnityEngine.Texture2D GetBlankTexture()
		{
			if (blankTexture == null)
			{
				blankTexture = new global::UnityEngine.Texture2D(2, 2, global::UnityEngine.TextureFormat.ARGB32, false);
				blankTexture.SetPixel(0, 0, global::UnityEngine.Color.white);
				blankTexture.SetPixel(1, 0, global::UnityEngine.Color.white);
				blankTexture.SetPixel(0, 1, global::UnityEngine.Color.white);
				blankTexture.SetPixel(1, 1, global::UnityEngine.Color.white);
				blankTexture.Apply(false, true);
			}
			return blankTexture;
		}

		public static void InitMessage(global::Swrve.Messaging.SwrveMessageFormat format)
		{
			if (Animator != null)
			{
				Animator.InitMessage(format);
			}
			else
			{
				format.Init(new global::Swrve.Messaging.Point(0, 0), new global::Swrve.Messaging.Point(0, 0));
			}
		}

		public static void AnimateMessage(global::Swrve.Messaging.SwrveMessageFormat format)
		{
			if (Animator != null)
			{
				Animator.AnimateMessage(format);
			}
		}

		public static void DrawMessage(global::Swrve.Messaging.SwrveMessageFormat format, int centerx, int centery)
		{
			if (Animator != null)
			{
				AnimateMessage(format);
			}
			if (format.BackgroundColor.HasValue && GetBlankTexture() != null)
			{
				global::UnityEngine.Color value = format.BackgroundColor.Value;
				value.a *= format.Message.BackgroundAlpha;
				global::UnityEngine.GUI.color = value;
				WholeScreen.width = global::UnityEngine.Screen.width;
				WholeScreen.height = global::UnityEngine.Screen.height;
				global::UnityEngine.GUI.DrawTexture(WholeScreen, blankTexture, global::UnityEngine.ScaleMode.StretchToFill, true, 10f);
				global::UnityEngine.GUI.color = global::UnityEngine.Color.white;
			}
			if (format.Rotate)
			{
				global::UnityEngine.Vector2 pivotPoint = new global::UnityEngine.Vector2(global::UnityEngine.Screen.width / 2, global::UnityEngine.Screen.height / 2);
				global::UnityEngine.GUIUtility.RotateAroundPivot(90f, pivotPoint);
			}
			float num = format.Scale * format.Message.AnimationScale;
			global::UnityEngine.GUI.color = global::UnityEngine.Color.white;
			foreach (global::Swrve.Messaging.SwrveImage image in format.Images)
			{
				if (image.Texture != null)
				{
					float num2 = num * image.AnimationScale;
					global::Swrve.Messaging.Point centeredPosition = image.GetCenteredPosition(image.Texture.width, image.Texture.height, num2, num);
					centeredPosition.X += centerx;
					centeredPosition.Y += centery;
					image.Rect.x = centeredPosition.X;
					image.Rect.y = centeredPosition.Y;
					image.Rect.width = (float)image.Texture.width * num2;
					image.Rect.height = (float)image.Texture.height * num2;
					global::UnityEngine.GUI.DrawTexture(image.Rect, image.Texture, global::UnityEngine.ScaleMode.StretchToFill, true, 10f);
				}
				else
				{
					global::UnityEngine.GUI.Box(image.Rect, image.File);
				}
			}
			foreach (global::Swrve.Messaging.SwrveButton button in format.Buttons)
			{
				if (button.Texture != null)
				{
					float num3 = num * button.AnimationScale;
					global::Swrve.Messaging.Point centeredPosition2 = button.GetCenteredPosition(button.Texture.width, button.Texture.height, num3, num);
					centeredPosition2.X += centerx;
					centeredPosition2.Y += centery;
					button.Rect.x = centeredPosition2.X;
					button.Rect.y = centeredPosition2.Y;
					button.Rect.width = (float)button.Texture.width * num3;
					button.Rect.height = (float)button.Texture.height * num3;
					if (Animator != null)
					{
						Animator.AnimateButtonPressed(button);
					}
					else
					{
						global::UnityEngine.GUI.color = ((!button.Pressed) ? global::UnityEngine.Color.white : ButtonPressedColor);
					}
					global::UnityEngine.GUI.DrawTexture(button.Rect, button.Texture, global::UnityEngine.ScaleMode.StretchToFill, true, 10f);
				}
				else
				{
					global::UnityEngine.GUI.Box(button.Rect, button.Image);
				}
				global::UnityEngine.GUI.color = global::UnityEngine.Color.white;
			}
			if ((Animator == null && format.Closing) || (Animator != null && Animator.IsMessageDismissed(format)))
			{
				format.Dismissed = true;
				format.UnloadAssets();
			}
		}
	}
}

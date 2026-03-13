namespace Kampai.Util
{
	public class DebugVisualizerView : global::Kampai.Util.KampaiView
	{
		private const int HEIGHT = 25;

		private const int MAX_LEVEL = 10;

		public global::UnityEngine.RectTransform content;

		public global::Kampai.UI.View.ButtonView CloseButton;

		private global::System.Collections.Generic.Dictionary<global::UnityEngine.GameObject, global::Kampai.Util.ExpandableData> expandables = new global::System.Collections.Generic.Dictionary<global::UnityEngine.GameObject, global::Kampai.Util.ExpandableData>();

		private global::System.Collections.Generic.Dictionary<global::UnityEngine.GameObject, global::Kampai.Util.ValueData> values = new global::System.Collections.Generic.Dictionary<global::UnityEngine.GameObject, global::Kampai.Util.ValueData>();

		private global::Kampai.UI.IPositionService positionService;

		private global::UnityEngine.GameObject targetObject;

		private int count;

		private global::UnityEngine.Font myFont;

		private float ALPHA = 0.6f;

		private bool arrayRebuilding;

		private float offset;

		private float WIDTH = 4.5f;

		[Inject]
		public global::Kampai.UI.View.ShowDebugVisualizerSignal showSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		public void Init(global::Kampai.UI.IPositionService positionService, global::UnityEngine.GameObject targetObject, float offset)
		{
			myFont = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.Font>("HelveticaLTStd-BoldCond");
			this.targetObject = targetObject;
			this.positionService = positionService;
			this.offset = offset;
			CloseButton.ClickedSignal.AddListener(OnCloseClicked);
		}

		public global::UnityEngine.GameObject CreateProperty(global::Kampai.Util.DebugElement element, string name, object value, out global::UnityEngine.UI.Text rightText, int level = 0, int SiblingIndex = -1, bool exitIfNull = true)
		{
			rightText = null;
			if (value == null && exitIfNull)
			{
				return null;
			}
			global::UnityEngine.GameObject go = new global::UnityEngine.GameObject(string.Format("{0}_{1}Property{2}", level, element, count));
			global::UnityEngine.UI.Image image = go.AddComponent<global::UnityEngine.UI.Image>();
			float num = (10f - (float)level / 2f) / 10f;
			image.color = new global::UnityEngine.Color(num, num, num, ALPHA);
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("Left");
			global::UnityEngine.UI.Image image2 = gameObject.AddComponent<global::UnityEngine.UI.Image>();
			float num2 = (10f - (float)(level - 1) / 2f) / 10f;
			image2.color = new global::UnityEngine.Color(num2, num2, num2, ALPHA);
			global::UnityEngine.UI.Text text = AttachText(gameObject.transform, name);
			gameObject.transform.SetParent(go.transform, false);
			SetAnchor(gameObject.transform as global::UnityEngine.RectTransform, 0f, 0.4f);
			global::UnityEngine.GameObject gameObject2 = new global::UnityEngine.GameObject("Right");
			global::UnityEngine.UI.Image image3 = gameObject2.AddComponent<global::UnityEngine.UI.Image>();
			image3.color = new global::UnityEngine.Color(1f, 1f, 1f, 0f);
			rightText = AttachText(gameObject2.transform, (value != null) ? value.ToString() : "null");
			gameObject2.transform.SetParent(go.transform, false);
			SetAnchor(gameObject2.transform as global::UnityEngine.RectTransform, 0.4f, 1f);
			go.transform.SetParent(content, false);
			if (SiblingIndex != -1)
			{
				go.transform.SetSiblingIndex(SiblingIndex + 1);
			}
			global::UnityEngine.UI.LayoutElement layoutElement = go.AddComponent<global::UnityEngine.UI.LayoutElement>();
			layoutElement.preferredHeight = 25f;
			switch (element)
			{
			case global::Kampai.Util.DebugElement.Expandable:
			{
				global::UnityEngine.UI.Button button3 = go.AddComponent<global::UnityEngine.UI.Button>();
				button3.onClick.AddListener(delegate
				{
					OnClick(go);
				});
				global::Kampai.Util.ExpandableData expandableData = new global::Kampai.Util.ExpandableData();
				expandableData.level = level;
				expandableData.value = value;
				expandableData.subItems = null;
				expandables.Add(go, expandableData);
				image.color = new global::UnityEngine.Color(0f, 0f, 0f, ALPHA);
				text.color = global::UnityEngine.Color.white;
				rightText.color = global::UnityEngine.Color.white;
				break;
			}
			case global::Kampai.Util.DebugElement.Value:
			{
				if (value == null || value.GetType() != typeof(int) || (int)value < 100)
				{
					break;
				}
				int id = (int)value;
				global::Kampai.Game.Instance byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Instance>(id);
				if (byInstanceId != null)
				{
					rightText.color = global::UnityEngine.Color.green;
					global::UnityEngine.UI.Button button = gameObject2.AddComponent<global::UnityEngine.UI.Button>();
					button.onClick.AddListener(delegate
					{
						OnNumberClick(id);
					});
					break;
				}
				global::Kampai.Game.Definition definition = null;
				definitionService.TryGet<global::Kampai.Game.Definition>(id, out definition);
				if (definition != null)
				{
					rightText.color = global::UnityEngine.Color.green;
					global::UnityEngine.UI.Button button2 = gameObject2.AddComponent<global::UnityEngine.UI.Button>();
					button2.onClick.AddListener(delegate
					{
						OnNumberClick(id);
					});
				}
				break;
			}
			}
			go.SetLayerRecursively(5);
			count++;
			return go;
		}

		public global::UnityEngine.GameObject CreateNoneValueProperty(global::Kampai.Util.DebugElement element, string name, int level = 0, int SiblingIndex = -1, object value = null)
		{
			global::UnityEngine.GameObject go = new global::UnityEngine.GameObject(string.Format("{0}_{1}Property{2}", level, element, count));
			global::UnityEngine.UI.Image image = go.AddComponent<global::UnityEngine.UI.Image>();
			float num = (10f - (float)level / 2f) / 10f;
			image.color = new global::UnityEngine.Color(num, num, num, ALPHA);
			global::UnityEngine.UI.Text text = AttachText(go.transform, name);
			go.transform.SetParent(content, false);
			if (SiblingIndex != -1)
			{
				go.transform.SetSiblingIndex(SiblingIndex + 1);
			}
			global::UnityEngine.UI.LayoutElement layoutElement = go.AddComponent<global::UnityEngine.UI.LayoutElement>();
			layoutElement.preferredHeight = 25f;
			switch (element)
			{
			case global::Kampai.Util.DebugElement.Expandable:
			{
				global::UnityEngine.UI.Button button = go.AddComponent<global::UnityEngine.UI.Button>();
				button.onClick.AddListener(delegate
				{
					OnClick(go);
				});
				global::Kampai.Util.ExpandableData expandableData = new global::Kampai.Util.ExpandableData();
				expandableData.level = level;
				expandableData.value = value;
				expandableData.subItems = null;
				expandables.Add(go, expandableData);
				image.color = new global::UnityEngine.Color(0f, 0f, 0f, ALPHA);
				text.color = global::UnityEngine.Color.white;
				break;
			}
			}
			count++;
			return go;
		}

		internal void LateUpdate()
		{
			global::Kampai.UI.PositionData positionData = positionService.GetPositionData(targetObject.transform.position);
			if (base.transform.position != positionData.WorldPositionInUI)
			{
				base.transform.position = positionData.WorldPositionInUI + new global::UnityEngine.Vector3(offset, 0f, 0f);
			}
			if (arrayRebuilding)
			{
				return;
			}
			foreach (global::System.Collections.Generic.KeyValuePair<global::UnityEngine.GameObject, global::Kampai.Util.ValueData> value4 in values)
			{
				global::Kampai.Util.ValueData value = value4.Value;
				if (!value.isArray)
				{
					object value2 = value.info.GetValue(value.parentValue, null);
					if (value.text != null && value2 != null)
					{
						if (value2.GetType().IsValueType || value2.GetType() == typeof(string))
						{
							value.text.text = value2.ToString();
						}
						else
						{
							value.text.text = value2.GetType().Name;
						}
					}
					continue;
				}
				object value3 = value.info.GetValue(value.parentValue, null);
				global::System.Collections.ICollection collection = value3 as global::System.Collections.ICollection;
				if (collection == null)
				{
					continue;
				}
				if (collection.Count != value.currentArrayElements.Count)
				{
					StartCoroutine(RebuildArray(value));
					break;
				}
				int num = 0;
				foreach (object item in collection)
				{
					if (!item.Equals(value.currentArrayElements[num]))
					{
						StartCoroutine(RebuildArray(value));
						return;
					}
					num++;
				}
			}
		}

		private global::System.Collections.IEnumerator RebuildArray(global::Kampai.Util.ValueData data)
		{
			arrayRebuilding = true;
			OnClick(data.expandableGo);
			yield return null;
			OnClick(data.expandableGo);
			arrayRebuilding = false;
		}

		private void OnClick(global::UnityEngine.GameObject gameObject)
		{
			if (expandables.ContainsKey(gameObject))
			{
				global::Kampai.Util.ExpandableData data = expandables[gameObject];
				if (data.subItems == null)
				{
					ProcessingPropertyInfo(gameObject.transform, ref data);
					return;
				}
				ClearSubItem(data.subItems);
				data.subItems = null;
			}
		}

		private void OnNumberClick(object number)
		{
			int type = (int)number;
			showSignal.Dispatch(targetObject, type, offset + WIDTH);
		}

		private void ClearSubItem(global::System.Collections.Generic.List<global::UnityEngine.GameObject> subItems)
		{
			if (subItems == null)
			{
				return;
			}
			foreach (global::UnityEngine.GameObject subItem in subItems)
			{
				if (values.ContainsKey(subItem))
				{
					values[subItem] = null;
					values.Remove(subItem);
				}
				if (expandables.ContainsKey(subItem))
				{
					ClearSubItem(expandables[subItem].subItems);
					expandables.Remove(subItem);
				}
				global::UnityEngine.Object.Destroy(subItem);
			}
			subItems.Clear();
		}

		public void AddValueData(global::UnityEngine.GameObject go, global::UnityEngine.UI.Text rightText, object currentObj, global::System.Reflection.PropertyInfo info)
		{
			global::Kampai.Util.ValueData valueData = new global::Kampai.Util.ValueData();
			valueData.parentValue = currentObj;
			valueData.text = rightText;
			valueData.info = info;
			values.Add(go, valueData);
		}

		public void ProcessingPropertyInfo(global::UnityEngine.Transform targetParent, ref global::Kampai.Util.ExpandableData data)
		{
			object value = data.value;
			data.subItems = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();
			global::System.Reflection.PropertyInfo[] properties = value.GetType().GetProperties();
			global::System.Reflection.PropertyInfo[] array = properties;
			foreach (global::System.Reflection.PropertyInfo propertyInfo in array)
			{
				if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType == typeof(string))
				{
					global::UnityEngine.UI.Text rightText;
					global::UnityEngine.GameObject gameObject = CreateProperty(global::Kampai.Util.DebugElement.Value, propertyInfo.Name, propertyInfo.GetValue(value, null), out rightText, data.level + 1, targetParent.GetSiblingIndex() + data.subItems.Count);
					if (gameObject != null)
					{
						global::Kampai.Util.ValueData valueData = new global::Kampai.Util.ValueData();
						valueData.parentValue = value;
						valueData.text = rightText;
						valueData.info = propertyInfo;
						values.Add(gameObject, valueData);
						data.subItems.Add(gameObject);
					}
					continue;
				}
				object value2 = propertyInfo.GetValue(value, null);
				if (value2 == null)
				{
					continue;
				}
				global::System.Collections.ICollection collection = value2 as global::System.Collections.ICollection;
				if (collection != null)
				{
					global::UnityEngine.GameObject gameObject2 = CreateNoneValueProperty(global::Kampai.Util.DebugElement.Title, propertyInfo.Name, data.level + 1, targetParent.GetSiblingIndex() + data.subItems.Count);
					data.subItems.Add(gameObject2);
					global::Kampai.Util.ValueData valueData2 = new global::Kampai.Util.ValueData();
					valueData2.parentValue = value;
					valueData2.info = propertyInfo;
					valueData2.isArray = true;
					valueData2.currentArrayElements = new global::System.Collections.Generic.List<object>();
					foreach (object item2 in collection)
					{
						if (item2.GetType().IsValueType || item2.GetType() == typeof(string))
						{
							global::UnityEngine.UI.Text rightText2;
							global::UnityEngine.GameObject gameObject3 = CreateProperty(global::Kampai.Util.DebugElement.Value, item2.GetType().Name, item2, out rightText2, data.level + 1, targetParent.GetSiblingIndex() + data.subItems.Count);
							if (gameObject3 != null)
							{
								data.subItems.Add(gameObject3);
							}
						}
						else
						{
							global::UnityEngine.GameObject gameObject4 = CreateNoneValueProperty(global::Kampai.Util.DebugElement.Expandable, item2.GetType().Name, data.level + 1, targetParent.GetSiblingIndex() + data.subItems.Count, item2);
							if (gameObject4 != null)
							{
								data.subItems.Add(gameObject4);
							}
						}
						valueData2.currentArrayElements.Add(item2);
					}
					valueData2.expandableGo = targetParent.gameObject;
					values.Add(gameObject2, valueData2);
				}
				else
				{
					global::UnityEngine.GameObject item = CreateNoneValueProperty(global::Kampai.Util.DebugElement.Expandable, propertyInfo.Name, data.level + 1, targetParent.GetSiblingIndex() + data.subItems.Count, value2);
					data.subItems.Add(item);
				}
			}
		}

		private global::UnityEngine.UI.Text AttachText(global::UnityEngine.Transform parent, string value)
		{
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("Text");
			global::UnityEngine.UI.Text text = gameObject.AddComponent<global::UnityEngine.UI.Text>();
			text.text = value;
			text.color = global::UnityEngine.Color.black;
			text.font = myFont;
			text.resizeTextForBestFit = true;
			gameObject.transform.SetParent(parent, false);
			SetAnchor(gameObject.transform as global::UnityEngine.RectTransform, 0f, 1f, true);
			return text;
		}

		private void SetAnchor(global::UnityEngine.RectTransform rt, float xMin, float xMax, bool isText = false)
		{
			rt.localPosition = global::UnityEngine.Vector3.zero;
			rt.localScale = global::UnityEngine.Vector3.one;
			rt.offsetMin = global::UnityEngine.Vector3.zero;
			rt.offsetMax = global::UnityEngine.Vector3.zero;
			rt.pivot = global::UnityEngine.Vector3.zero;
			rt.anchorMin = new global::UnityEngine.Vector2(xMin, 0f);
			rt.anchorMax = new global::UnityEngine.Vector2(xMax, 1f);
			rt.offsetMin = new global::UnityEngine.Vector2(0f, -2f);
			rt.offsetMax = new global::UnityEngine.Vector2(0f, -2f);
		}

		private void OnCloseClicked()
		{
			CloseButton.ClickedSignal.RemoveListener(OnCloseClicked);
			global::UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}

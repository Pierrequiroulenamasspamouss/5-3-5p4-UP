public class KampaiNonDrawingGraphic : global::UnityEngine.UI.Graphic
{
	public override void SetMaterialDirty()
	{
	}

	public override void SetVerticesDirty()
	{
	}

	protected override void OnPopulateMesh(global::UnityEngine.UI.VertexHelper vh)
	{
		vh.Clear();
	}
}

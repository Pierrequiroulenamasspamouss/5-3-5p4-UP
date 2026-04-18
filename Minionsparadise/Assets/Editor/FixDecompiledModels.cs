using UnityEngine;
using UnityEditor;

public class MeshRepairPipeline
{
    private const string TARGET_PATH = "Assets";

    [MenuItem("Project/Models/Fix Decompiled Meshes")]
    public static void FixMeshes()
    {
        string[] guids = AssetDatabase.FindAssets("t:Mesh", new[] { TARGET_PATH });

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);

            if (EditorUtility.DisplayCancelableProgressBar("Fixing Models", path, (float)i / guids.Length))
                break;

            Mesh mesh = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if (mesh == null)
                continue;

            FixMesh(mesh);
        }

        EditorUtility.ClearProgressBar();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void FixMesh(Mesh mesh)
    {
        int vCount = mesh.vertexCount;
        if (vCount == 0)
            return;

        Vector3[] vertices = mesh.vertices;

        Vector2[] uv0 = mesh.uv;
        if (uv0 == null || uv0.Length != vCount)
            uv0 = new Vector2[vCount];

        Vector2[] uv1 = mesh.uv2;
        if (uv1 == null || uv1.Length != vCount)
            uv1 = new Vector2[vCount];

        Color32[] colors = mesh.colors32;
        if (colors == null || colors.Length != vCount || IsAllZero(colors))
        {
            colors = new Color32[vCount];
            for (int i = 0; i < vCount; i++)
                colors[i] = new Color32(255, 255, 255, 255);
        }

        Vector3[] normals = mesh.normals;
        if (normals == null || normals.Length != vCount)
        {
            mesh.RecalculateNormals();
            normals = mesh.normals;
        }

        Vector4[] tangents = mesh.tangents;
        if ((tangents == null || tangents.Length != vCount) && uv0 != null && normals != null)
            mesh.RecalculateTangents();

        mesh.RecalculateBounds();

        mesh.uv = uv0;
        mesh.uv2 = uv1;
        mesh.colors32 = colors;

        EditorUtility.SetDirty(mesh);
        AssetDatabase.SaveAssetIfDirty(mesh);
    }

    private static bool IsAllZero(Color32[] colors)
    {
        for (int i = 0; i < colors.Length; i++)
        {
            if (colors[i].r != 0 || colors[i].g != 0 || colors[i].b != 0 || colors[i].a != 0)
                return false;
        }
        return true;
    }
}
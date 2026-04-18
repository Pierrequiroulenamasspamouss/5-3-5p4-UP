using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class FixDecompiledModels
{
    private const string TARGET_PATH = "Assets/";

    private static readonly string[] SKIP_FOLDERS = new string[]
    {
        "Modules",
        "Town",
        "Boardwalk",
        "Diving",
        "BoxDimension",
        "MtBlizzard",
        "Beach",
        "HerbertBase",
        "MtBlizzardSummit"
    };

    [MenuItem("Project/Models/Fix Decompiled Models")]
    public static void FixMeshes()
    {
        string[] guids = AssetDatabase.FindAssets("t:Mesh", new[] { TARGET_PATH });

        int total = guids.Length;

        for (int index = 0; index < total; index++)
        {
            string guid = guids[index];
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (EditorUtility.DisplayCancelableProgressBar("Fixing Models", path, (float)index / Mathf.Max(1, total)))
                break;

            if (!path.EndsWith(".asset"))
                continue;

            if (ShouldSkip(path))
                continue;

            Mesh mesh = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if (mesh == null)
                continue;

            FixMesh(mesh);
        }

        SkinnedMeshRenderer[] renderers = Object.FindObjectsByType<SkinnedMeshRenderer>(FindObjectsSortMode.None);

        int rTotal = renderers.Length;

        for (int i = 0; i < rTotal; i++)
        {
            SkinnedMeshRenderer r = renderers[i];

            if (EditorUtility.DisplayCancelableProgressBar("Fixing Models", r.name, (float)i / Mathf.Max(1, rTotal)))
                break;

            if (r == null)
                continue;

            Mesh mesh = r.sharedMesh;
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
        Vector3[] vertices = mesh.vertices;
        if (vertices == null || vertices.Length == 0)
            return;

        Vector2[] uv0Source = mesh.uv;
        Vector2[] uv1Source = mesh.uv2;

        Color32[] colorsSource = mesh.colors32;
        Color32[] colors;

        if (colorsSource != null && colorsSource.Length == vertices.Length)
        {
            colors = new Color32[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
                colors[i] = colorsSource[i];
        }
        else if (colorsSource != null && colorsSource.Length > 0)
        {
            colors = colorsSource;
        }
        else
        {
            colors = new Color32[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
                colors[i] = new Color32(255, 255, 255, 255);
        }

        Mesh newMesh = new Mesh();
        newMesh.name = mesh.name;

        newMesh.indexFormat = vertices.Length > 65535
            ? UnityEngine.Rendering.IndexFormat.UInt32
            : UnityEngine.Rendering.IndexFormat.UInt16;

        newMesh.SetVertices(vertices);

        if (mesh.normals != null && mesh.normals.Length == vertices.Length)
            newMesh.SetNormals(mesh.normals);
        else
            newMesh.RecalculateNormals();

        if (mesh.tangents != null && mesh.tangents.Length == vertices.Length)
            newMesh.SetTangents(mesh.tangents);
        else
            newMesh.RecalculateTangents();

        if (uv0Source != null && uv0Source.Length == vertices.Length)
        {
            newMesh.SetUVs(0, new List<Vector2>(uv0Source));
        }
        else
        {
            Vector2[] uv0 = new Vector2[vertices.Length];
            for (int i = 0; i < uv0.Length; i++)
                uv0[i] = Vector2.zero;

            newMesh.SetUVs(0, new List<Vector2>(uv0));
        }

        if (uv1Source != null && uv1Source.Length == vertices.Length)
        {
            newMesh.SetUVs(1, new List<Vector2>(uv1Source));
        }
        else
        {
            Vector2[] uv1 = new Vector2[vertices.Length];
            for (int i = 0; i < uv1.Length; i++)
                uv1[i] = Vector2.zero;

            newMesh.SetUVs(1, new List<Vector2>(uv1));
        }

        newMesh.SetColors(colors);

        newMesh.subMeshCount = mesh.subMeshCount;

        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            newMesh.SetIndices(mesh.GetIndices(i), mesh.GetTopology(i), i);
        }

        newMesh.bindposes = mesh.bindposes;
        newMesh.boneWeights = mesh.boneWeights;

        newMesh.RecalculateBounds();

        EditorUtility.CopySerialized(newMesh, mesh);

        mesh.UploadMeshData(false);

        EditorUtility.SetDirty(mesh);
        AssetDatabase.SaveAssetIfDirty(mesh);
    }

    private static bool ShouldSkip(string path)
    {
        for (int i = 0; i < SKIP_FOLDERS.Length; i++)
        {
            if (path.Contains("/" + SKIP_FOLDERS[i] + "/"))
                return true;
        }
        return false;
    }
}
using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateHeroShadowPrefab
{
	private const int shadowNum = 40;

    private const float pngWidth = 230 / 50;
	private const float pngHeight = 230 / 50;
		
	private const int RENDERINDEX = 100;

	[MenuItem("zhangwenxin/人物阴影prefab")]
    public static void Start()
    {
        Vector3[] vertices = new Vector3[shadowNum * 4];
        Vector3[] normals = new Vector3[shadowNum * 4];
        Vector2[] uvs = new Vector2[shadowNum * 4];
        Vector4[] tangents = new Vector4[shadowNum * 4];
        int[] triangles = new int[shadowNum * 6];

        for (int i = 0; i < shadowNum; i++)
        {

            //底板

            vertices[i * 4] = new Vector3(-0.5f * pngWidth, -0.5f * pngWidth, 0);
            vertices[i * 4 + 1] = new Vector3(0.5f * pngWidth, 0.5f * pngWidth, 0);
            vertices[i * 4 + 2] = new Vector3(0.5f * pngWidth, -0.5f * pngWidth, 0);
            vertices[i * 4 + 3] = new Vector3(-0.5f * pngWidth, 0.5f * pngWidth, 0);

            //底板
            normals[i * 4] = new Vector3();
            normals[i * 4 + 1] = new Vector3();
            normals[i * 4 + 2] = new Vector3();
            normals[i * 4 + 3] = new Vector3();

            //底板
            uvs[i * 4] = new Vector2(0, 0);
            uvs[i * 4 + 1] = new Vector2(1, 1);
            uvs[i * 4 + 2] = new Vector2(1, 0);
            uvs[i * 4 + 3] = new Vector2(0, 1);

            //底板
            tangents[i * 4] = new Vector4(i, 0, 0, 0);
            tangents[i * 4 + 1] = new Vector4(i, 0, 0, 0);
            tangents[i * 4 + 2] = new Vector4(i, 0, 0, 0);
            tangents[i * 4 + 3] = new Vector4(i, 0, 0, 0);

            triangles[i * 6] = i * 4;
            triangles[i * 6 + 1] = i * 4 + 1;
            triangles[i * 6 + 2] = i * 4 + 2;
            triangles[i * 6 + 3] = i * 4 + 1;
            triangles[i * 6 + 4] = i * 4;
            triangles[i * 6 + 5] = i * 4 + 3;

        }

		GameObject shadowObj = new GameObject();
        MeshFilter mf = shadowObj.AddComponent<MeshFilter>();
        MeshRenderer mr = shadowObj.AddComponent<MeshRenderer>();

		mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		mr.receiveShadows = false;
		mr.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
		mr.useLightProbes = false;

		
		Mesh mesh = new Mesh();
		mesh.bounds.SetMinMax(new Vector3(), new Vector3(999, 999, 999));
		mf.sharedMesh = mesh;
		
		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.tangents = tangents;
		mesh.uv = uvs;
		
		mesh.triangles = triangles;

		
		Bounds bounds = mesh.bounds;
		
		bounds.Expand(1000f);
		
		mesh.bounds = bounds;

        AssetDatabase.CreateAsset(mesh, "Assets/PlayGround/BattleTool/ShadowMesh.asset");

        Material mat = new Material(Shader.Find("Custom/BattleHeroShadowPass"));

        Texture t = AssetDatabase.LoadAssetAtPath<Texture>("Assets/PlayGround/BattleTool/yingzi.png");
		mat.mainTexture = t;

		mr.material = mat;

        AssetDatabase.CreateAsset(mat, "Assets/PlayGround/BattleTool/ShadowMat.mat");



        PrefabUtility.CreatePrefab("Assets/PlayGround/BattleTool/Shadow.prefab", shadowObj);

        GameObject.DestroyImmediate(shadowObj);
    }
}

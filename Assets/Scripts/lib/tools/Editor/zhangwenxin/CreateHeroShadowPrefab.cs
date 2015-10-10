using UnityEngine;
using System.Collections;
using UnityEditor;
using xy3d.tstd.lib.battleHeroTools;

public class CreateHeroShadowPrefab
{
	[MenuItem("zhangwenxin/人物阴影prefab")]
    public static void Start()
    {
        Vector3[] vertices = new Vector3[BattleHeroShadow.shadowNum * 4];
        Vector3[] normals = new Vector3[BattleHeroShadow.shadowNum * 4];
        Vector2[] uvs = new Vector2[BattleHeroShadow.shadowNum * 4];
        Vector4[] tangents = new Vector4[BattleHeroShadow.shadowNum * 4];
        int[] triangles = new int[BattleHeroShadow.shadowNum * 6];

        for (int i = 0; i < BattleHeroShadow.shadowNum; i++)
        {

            //底板

            vertices[i * 4] = new Vector3(-0.5f * BattleHeroShadow.pngWidth, 0, -0.5f * BattleHeroShadow.pngHeight);
            vertices[i * 4 + 1] = new Vector3(0.5f * BattleHeroShadow.pngWidth, 0, 0.5f * BattleHeroShadow.pngHeight);
            vertices[i * 4 + 2] = new Vector3(0.5f * BattleHeroShadow.pngWidth, 0, -0.5f * BattleHeroShadow.pngHeight);
            vertices[i * 4 + 3] = new Vector3(-0.5f * BattleHeroShadow.pngWidth, 0, 0.5f * BattleHeroShadow.pngHeight);

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

        //shadowObj.transform.eulerAngles = new Vector3(90, 0, 0);

        MeshFilter mf = shadowObj.AddComponent<MeshFilter>();
        MeshRenderer mr = shadowObj.AddComponent<MeshRenderer>();

		mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
		mr.receiveShadows = false;
		mr.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
		mr.useLightProbes = false;

		
		Mesh mesh = new Mesh();
        mesh.bounds.SetMinMax(new Vector3(), new Vector3(9999999, 9999999, 9999999));
		mf.sharedMesh = mesh;
		
		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.tangents = tangents;
		mesh.uv = uvs;
		
		mesh.triangles = triangles;

		
		Bounds bounds = mesh.bounds;

        bounds.Expand(9999999);
		
		mesh.bounds = bounds;

        AssetDatabase.CreateAsset(mesh, "Assets/Arts/battle/BattleTool/ShadowMesh.asset");

        Material mat = new Material(Shader.Find("Custom/BattleHeroShadowPass"));

        Texture t = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Arts/battle/BattleTool/yingzi.png");
		mat.mainTexture = t;

		mr.material = mat;

        AssetDatabase.CreateAsset(mat, "Assets/Arts/battle/BattleTool/ShadowMat.mat");



        PrefabUtility.CreatePrefab("Assets/Arts/battle/BattleTool/Shadow.prefab", shadowObj);

        GameObject.DestroyImmediate(shadowObj);
    }
}

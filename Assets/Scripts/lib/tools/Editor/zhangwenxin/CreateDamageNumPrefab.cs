using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateDamageNumPrefab {
	public const float ASSET_WIDTH = 512.0f /20;
	public const float ASSET_HEIGHT = 128.0f / 20;
	
	public const float FONT_WIDTH = 32.0f / 20;
	public const float FONT_HEIGHT = 32.0f / 20;
	
	public const int damageUnitNum = 80;

    private static Vector3[] vertices;
    private static Vector3[] normals;
    private static Vector2[] uvs;
    private static Vector4[] tangents;
    private static int[] triangles;

	[MenuItem("zhangwenxin/构造伤害数字prefab")]
    public static void Start()
    {

    	
		vertices = new Vector3[damageUnitNum * 4];
		normals = new Vector3[damageUnitNum * 4];
		uvs = new Vector2[damageUnitNum * 4];
		tangents = new Vector4[damageUnitNum * 4];
		triangles = new int[damageUnitNum * 6];
		
		for(int i = 0 ; i < damageUnitNum ; i++)
		{
			//底板
			
			vertices[i * 4] = new Vector3(-0.5f * FONT_WIDTH, -0.5f * FONT_HEIGHT, 0);
			vertices[i * 4 + 1] = new Vector3(0.5f * FONT_WIDTH, 0.5f * FONT_HEIGHT, 0);
			vertices[i * 4 + 2] = new Vector3(0.5f * FONT_WIDTH, -0.5f * FONT_HEIGHT, 0);
			vertices[i * 4 + 3] = new Vector3(-0.5f * FONT_WIDTH, 0.5f * FONT_HEIGHT, 0);
			
			//底板
			normals[i * 4] = new Vector3();
			normals[i * 4 + 1] = new Vector3();
			normals[i * 4 + 2] = new Vector3();
			normals[i * 4 + 3] = new Vector3();
			
			//底板
			uvs[i * 4] = new Vector2(0, 1 - FONT_HEIGHT / ASSET_HEIGHT);
			uvs[i * 4 + 1] = new Vector2(FONT_WIDTH / ASSET_WIDTH, 1);
			uvs[i * 4 + 2] = new Vector2(FONT_WIDTH / ASSET_WIDTH, 1 - FONT_HEIGHT / ASSET_HEIGHT);
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

		GameObject damageObj = new GameObject();
		MeshFilter mf = damageObj.AddComponent<MeshFilter>();
		MeshRenderer mr = damageObj.AddComponent<MeshRenderer>();

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

        AssetDatabase.CreateAsset(mesh, "Assets/PlayGround/BattleTool/DamageNumMesh.asset");

        Material mat = new Material(Shader.Find("Custom/BattleHeroDamagePass"));

        Texture t = AssetDatabase.LoadAssetAtPath<Texture>("Assets/PlayGround/BattleTool/num.png");
		mat.mainTexture = t;

		mr.material = mat;

        AssetDatabase.CreateAsset(mat, "Assets/PlayGround/BattleTool/DamageNumMat.mat");



        PrefabUtility.CreatePrefab("Assets/PlayGround/BattleTool/DamageNum.prefab", damageObj);

		GameObject.DestroyImmediate (damageObj);
    }
}

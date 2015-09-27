using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateHpBarPrefab {

    public const float boardWidth = 119.0f / 25;
    public const float boardHeight = 43.0f / 25;

    public const float hpBarWidth = 71.0f / 25;
    public const float hpBarHeight = 5.0f / 25;

    public const float hpBarX = 39.0f / 25;
    public const float hpBarY = 15.0f / 25;

    public const float myHpBarU = 147.0f / 25;
    public const float myHpBarV = 1.0f / 25;

    public const float oppHpBarU = 147.0f / 25;
    public const float oppHpBarV = 14.0f / 25;

    public const float iconBarWidth = 28.0f / 25;
    public const float iconBarHeight = 28.0f / 25;

    public const float iconBarX = 7.0f / 25;
    public const float iconBarY = 7.0f / 25;

    public const float iconBarV = 53.0f / 25;

    public const float iconBarXGap = 1.0f / 25;
    public const float iconBarYGap = 1.0f / 25;

    public const float iconBarNumFirstLine = 6.0f;

    public const float angerBarWidth = 69.0f / 25;
    public const float angerBarHeight = 2.0f / 25;

    public const float angerBarX = 40.0f / 25;
    public const float angerBarY = 25.0f / 25;

    public const float angerBarU = 147.0f / 25;
    public const float angerBarV = 27.0f / 25;

    public const float TEXTURE_WIDTH = 256.0f / 25;
    public const float TEXTURE_HEIGHT = 128.0f / 25;


    private const int hpBarNum = 20;
    private const int planeNum = 4;


    private static Vector3[] vertices;
    private static Vector3[] normals;
    private static Vector2[] uvs;
    private static Vector4[] tangents;

	[MenuItem("zhangwenxin/血条prefab")]
    public static void Start()
    {

        vertices = new Vector3[hpBarNum * planeNum * 4];
        normals = new Vector3[hpBarNum * planeNum * 4];
        uvs = new Vector2[hpBarNum * planeNum * 4];
        tangents = new Vector4[hpBarNum * planeNum * 4];
        int[] triangles = new int[hpBarNum * planeNum * 6];

        for (int i = 0; i < hpBarNum; i++)
        {

            //底板

            vertices[i * planeNum * 4] = new Vector3(-0.5f * boardWidth, -0.5f * boardHeight, 0);
            vertices[i * planeNum * 4 + 1] = new Vector3(0.5f * boardWidth, 0.5f * boardHeight, 0);
            vertices[i * planeNum * 4 + 2] = new Vector3(0.5f * boardWidth, -0.5f * boardHeight, 0);
            vertices[i * planeNum * 4 + 3] = new Vector3(-0.5f * boardWidth, 0.5f * boardHeight, 0);


            //血条
            vertices[i * planeNum * 4 + 4] = new Vector3(-0.5f * boardWidth + hpBarX, 0.5f * boardHeight - hpBarY - hpBarHeight, 0);
            vertices[i * planeNum * 4 + 5] = new Vector3(-0.5f * boardWidth + hpBarX + hpBarWidth, 0.5f * boardHeight - hpBarY);
            vertices[i * planeNum * 4 + 6] = new Vector3(-0.5f * boardWidth + hpBarX + hpBarWidth, 0.5f * boardHeight - hpBarY - hpBarHeight, 0);
            vertices[i * planeNum * 4 + 7] = new Vector3(-0.5f * boardWidth + hpBarX, 0.5f * boardHeight - hpBarY, 0);

            //职业图标
            vertices[i * planeNum * 4 + 8] = new Vector3(-0.5f * boardWidth + iconBarX, 0.5f * boardHeight - iconBarY - iconBarHeight, 0);
            vertices[i * planeNum * 4 + 9] = new Vector3(-0.5f * boardWidth + iconBarX + iconBarWidth, 0.5f * boardHeight - iconBarY, 0);
            vertices[i * planeNum * 4 + 10] = new Vector3(-0.5f * boardWidth + iconBarX + iconBarWidth, 0.5f * boardHeight - iconBarY - iconBarHeight, 0);
            vertices[i * planeNum * 4 + 11] = new Vector3(-0.5f * boardWidth + iconBarX, 0.5f * boardHeight - iconBarY, 0);

            //怒气
            vertices[i * planeNum * 4 + 12] = new Vector3(-0.5f * boardWidth + angerBarX, 0.5f * boardHeight - angerBarY - angerBarHeight, 0);
            vertices[i * planeNum * 4 + 13] = new Vector3(-0.5f * boardWidth + angerBarX + angerBarWidth, 0.5f * boardHeight - angerBarY, 0);
            vertices[i * planeNum * 4 + 14] = new Vector3(-0.5f * boardWidth + angerBarX + angerBarWidth, 0.5f * boardHeight - angerBarY - angerBarHeight, 0);
            vertices[i * planeNum * 4 + 15] = new Vector3(-0.5f * boardWidth + angerBarX, 0.5f * boardHeight - angerBarY, 0);

            //底板
            normals[i * planeNum * 4] = new Vector3();
            normals[i * planeNum * 4 + 1] = new Vector3();
            normals[i * planeNum * 4 + 2] = new Vector3();
            normals[i * planeNum * 4 + 3] = new Vector3();

            //血条
            normals[i * planeNum * 4 + 4] = new Vector3();
            normals[i * planeNum * 4 + 5] = new Vector3();
            normals[i * planeNum * 4 + 6] = new Vector3();
            normals[i * planeNum * 4 + 7] = new Vector3();

            //职业图标
            normals[i * planeNum * 4 + 8] = new Vector3();
            normals[i * planeNum * 4 + 9] = new Vector3();
            normals[i * planeNum * 4 + 10] = new Vector3();
            normals[i * planeNum * 4 + 11] = new Vector3();

            //怒气
            normals[i * planeNum * 4 + 12] = new Vector3();
            normals[i * planeNum * 4 + 13] = new Vector3();
            normals[i * planeNum * 4 + 14] = new Vector3();
            normals[i * planeNum * 4 + 15] = new Vector3();

            //底板
            uvs[i * planeNum * 4] = new Vector2(0, 1 - boardHeight / TEXTURE_HEIGHT);
            uvs[i * planeNum * 4 + 1] = new Vector2(boardWidth / TEXTURE_WIDTH, 1);
            uvs[i * planeNum * 4 + 2] = new Vector2(boardWidth / TEXTURE_WIDTH, 1 - boardHeight / TEXTURE_HEIGHT);
            uvs[i * planeNum * 4 + 3] = new Vector2(0, 1);

            //血条
            uvs[i * planeNum * 4 + 4] = new Vector2(0, 0);
            uvs[i * planeNum * 4 + 5] = new Vector2(0, 0);
            uvs[i * planeNum * 4 + 6] = new Vector2(0, 0);
            uvs[i * planeNum * 4 + 7] = new Vector2(0, 0);

            //职业图标
            //uvs[i * planeNum * 4 + 8] = new Vector2(0,0);
            //uvs[i * planeNum * 4 + 9] = new Vector2(0,0);
            //uvs[i * planeNum * 4 + 10] = new Vector2(0,0);
            //uvs[i * planeNum * 4 + 11] = new Vector2(0,0);

            uvs[i * planeNum * 4 + 8] = new Vector2(0, 1 - (iconBarHeight + iconBarV) / TEXTURE_HEIGHT);
            uvs[i * planeNum * 4 + 9] = new Vector2(iconBarWidth / TEXTURE_WIDTH, 1 - iconBarV / TEXTURE_HEIGHT);
            uvs[i * planeNum * 4 + 10] = new Vector2(iconBarWidth / TEXTURE_WIDTH, 1 - iconBarHeight + iconBarV / TEXTURE_HEIGHT);
            uvs[i * planeNum * 4 + 11] = new Vector2(0, 1 - iconBarV / TEXTURE_HEIGHT);

            //怒气
            uvs[i * planeNum * 4 + 12] = new Vector2(angerBarU / TEXTURE_WIDTH, 1 - (angerBarV + angerBarHeight) / TEXTURE_HEIGHT);
            uvs[i * planeNum * 4 + 13] = new Vector2((angerBarU + angerBarWidth) / TEXTURE_WIDTH, 1 - angerBarV / TEXTURE_HEIGHT);
            uvs[i * planeNum * 4 + 14] = new Vector2((angerBarU + angerBarWidth) / TEXTURE_WIDTH, 1 - (angerBarV + angerBarHeight) / TEXTURE_HEIGHT);
            uvs[i * planeNum * 4 + 15] = new Vector2(angerBarU / TEXTURE_WIDTH, 1 - angerBarV / TEXTURE_HEIGHT);

            //底板
            tangents[i * planeNum * 4] = new Vector4(i, 0, 0, 0);
            tangents[i * planeNum * 4 + 1] = new Vector4(i, 0, 0, 0);
            tangents[i * planeNum * 4 + 2] = new Vector4(i, 0, 0, 0);
            tangents[i * planeNum * 4 + 3] = new Vector4(i, 0, 0, 0);

            //血条
            tangents[i * planeNum * 4 + 4] = new Vector4(i, 0, 0, 0);
            tangents[i * planeNum * 4 + 5] = new Vector4(i, 1, 0, 0);
            tangents[i * planeNum * 4 + 6] = new Vector4(i, 1, 0, 0);
            tangents[i * planeNum * 4 + 7] = new Vector4(i, 0, 0, 0);

            //职业图标
            tangents[i * planeNum * 4 + 8] = new Vector4(i, 0, 0, 0);
            tangents[i * planeNum * 4 + 9] = new Vector4(i, 0, 0, 0);
            tangents[i * planeNum * 4 + 10] = new Vector4(i, 0, 0, 0);
            tangents[i * planeNum * 4 + 11] = new Vector4(i, 0, 0, 0);

            //怒气
            tangents[i * planeNum * 4 + 12] = new Vector4(i, 0, 0, 0);
            tangents[i * planeNum * 4 + 13] = new Vector4(i, 0, 1, 0);
            tangents[i * planeNum * 4 + 14] = new Vector4(i, 0, 1, 0);
            tangents[i * planeNum * 4 + 15] = new Vector4(i, 0, 0, 0);

            for (int j = 0; j < planeNum; j++)
            {
                triangles[i * planeNum * 6 + j * 6] = i * planeNum * 4 + j * 4;
                triangles[i * planeNum * 6 + j * 6 + 1] = i * planeNum * 4 + j * 4 + 1;
                triangles[i * planeNum * 6 + j * 6 + 2] = i * planeNum * 4 + j * 4 + 2;
                triangles[i * planeNum * 6 + j * 6 + 3] = i * planeNum * 4 + j * 4 + 1;
                triangles[i * planeNum * 6 + j * 6 + 4] = i * planeNum * 4 + j * 4;
                triangles[i * planeNum * 6 + j * 6 + 5] = i * planeNum * 4 + j * 4 + 3;
            }

        }

		GameObject hpBarObj = new GameObject();
        MeshFilter mf = hpBarObj.AddComponent<MeshFilter>();
        MeshRenderer mr = hpBarObj.AddComponent<MeshRenderer>();

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

        AssetDatabase.CreateAsset(mesh, "Assets/PlayGround/BattleTool/HpBarMesh.asset");

        Material mat = new Material(Shader.Find("Custom/BattleHeroHpPass"));

        Texture t = AssetDatabase.LoadAssetAtPath<Texture>("Assets/PlayGround/BattleTool/xiaoxuetiao.png");
		mat.mainTexture = t;

		mr.material = mat;

        AssetDatabase.CreateAsset(mat, "Assets/PlayGround/BattleTool/HpBarMat.mat");

        PrefabUtility.CreatePrefab("Assets/PlayGround/BattleTool/HpBar.prefab", hpBarObj);

        GameObject.DestroyImmediate(hpBarObj);
    }
}

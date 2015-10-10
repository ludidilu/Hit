using UnityEngine;
using System.Collections;
using UnityEditor;
using xy3d.tstd.lib.battleHeroTools;

public class CreateHpBarPrefab {


   

	[MenuItem("zhangwenxin/血条prefab")]
    public static void Start()
    {
         Vector3[] vertices;
         Vector3[] normals;
         Vector2[] uvs;
         Vector4[] tangents;

        vertices = new Vector3[BattleHeroHpBar.hpBarNum * BattleHeroHpBar.planeNum * 4];
        normals = new Vector3[BattleHeroHpBar.hpBarNum * BattleHeroHpBar.planeNum * 4];
        uvs = new Vector2[BattleHeroHpBar.hpBarNum * BattleHeroHpBar.planeNum * 4];
        tangents = new Vector4[BattleHeroHpBar.hpBarNum * BattleHeroHpBar.planeNum * 4];
        int[] triangles = new int[BattleHeroHpBar.hpBarNum * BattleHeroHpBar.planeNum * 6];

        for (int i = 0; i < BattleHeroHpBar.hpBarNum; i++)
        {

            //底板

            vertices[i * BattleHeroHpBar.planeNum * 4] = new Vector3(-0.5f * BattleHeroHpBar.boardWidth, -0.5f * BattleHeroHpBar.boardHeight, 0);
            vertices[i * BattleHeroHpBar.planeNum * 4 + 1] = new Vector3(0.5f * BattleHeroHpBar.boardWidth, 0.5f * BattleHeroHpBar.boardHeight, 0);
            vertices[i * BattleHeroHpBar.planeNum * 4 + 2] = new Vector3(0.5f * BattleHeroHpBar.boardWidth, -0.5f * BattleHeroHpBar.boardHeight, 0);
            vertices[i * BattleHeroHpBar.planeNum * 4 + 3] = new Vector3(-0.5f * BattleHeroHpBar.boardWidth, 0.5f * BattleHeroHpBar.boardHeight, 0);


            //血条
            vertices[i * BattleHeroHpBar.planeNum * 4 + 4] = new Vector3(-0.5f * BattleHeroHpBar.boardWidth + BattleHeroHpBar.hpBarX, 0.5f * BattleHeroHpBar.boardHeight - BattleHeroHpBar.hpBarY - BattleHeroHpBar.hpBarHeight, 0);
            vertices[i * BattleHeroHpBar.planeNum * 4 + 5] = new Vector3(-0.5f * BattleHeroHpBar.boardWidth + BattleHeroHpBar.hpBarX + BattleHeroHpBar.hpBarWidth, 0.5f * BattleHeroHpBar.boardHeight - BattleHeroHpBar.hpBarY);
            vertices[i * BattleHeroHpBar.planeNum * 4 + 6] = new Vector3(-0.5f * BattleHeroHpBar.boardWidth + BattleHeroHpBar.hpBarX + BattleHeroHpBar.hpBarWidth, 0.5f * BattleHeroHpBar.boardHeight - BattleHeroHpBar.hpBarY - BattleHeroHpBar.hpBarHeight, 0);
            vertices[i * BattleHeroHpBar.planeNum * 4 + 7] = new Vector3(-0.5f * BattleHeroHpBar.boardWidth + BattleHeroHpBar.hpBarX, 0.5f * BattleHeroHpBar.boardHeight - BattleHeroHpBar.hpBarY, 0);

            //职业图标
            vertices[i * BattleHeroHpBar.planeNum * 4 + 8] = new Vector3(-0.5f * BattleHeroHpBar.boardWidth + BattleHeroHpBar.iconBarX, 0.5f * BattleHeroHpBar.boardHeight - BattleHeroHpBar.iconBarY - BattleHeroHpBar.iconBarHeight, 0);
            vertices[i * BattleHeroHpBar.planeNum * 4 + 9] = new Vector3(-0.5f * BattleHeroHpBar.boardWidth + BattleHeroHpBar.iconBarX + BattleHeroHpBar.iconBarWidth, 0.5f * BattleHeroHpBar.boardHeight - BattleHeroHpBar.iconBarY, 0);
            vertices[i * BattleHeroHpBar.planeNum * 4 + 10] = new Vector3(-0.5f * BattleHeroHpBar.boardWidth + BattleHeroHpBar.iconBarX + BattleHeroHpBar.iconBarWidth, 0.5f * BattleHeroHpBar.boardHeight - BattleHeroHpBar.iconBarY - BattleHeroHpBar.iconBarHeight, 0);
            vertices[i * BattleHeroHpBar.planeNum * 4 + 11] = new Vector3(-0.5f * BattleHeroHpBar.boardWidth + BattleHeroHpBar.iconBarX, 0.5f * BattleHeroHpBar.boardHeight - BattleHeroHpBar.iconBarY, 0);

            //怒气
            vertices[i * BattleHeroHpBar.planeNum * 4 + 12] = new Vector3(-0.5f * BattleHeroHpBar.boardWidth + BattleHeroHpBar.angerBarX, 0.5f * BattleHeroHpBar.boardHeight - BattleHeroHpBar.angerBarY - BattleHeroHpBar.angerBarHeight, 0);
            vertices[i * BattleHeroHpBar.planeNum * 4 + 13] = new Vector3(-0.5f * BattleHeroHpBar.boardWidth + BattleHeroHpBar.angerBarX + BattleHeroHpBar.angerBarWidth, 0.5f * BattleHeroHpBar.boardHeight - BattleHeroHpBar.angerBarY, 0);
            vertices[i * BattleHeroHpBar.planeNum * 4 + 14] = new Vector3(-0.5f * BattleHeroHpBar.boardWidth + BattleHeroHpBar.angerBarX + BattleHeroHpBar.angerBarWidth, 0.5f * BattleHeroHpBar.boardHeight - BattleHeroHpBar.angerBarY - BattleHeroHpBar.angerBarHeight, 0);
            vertices[i * BattleHeroHpBar.planeNum * 4 + 15] = new Vector3(-0.5f * BattleHeroHpBar.boardWidth + BattleHeroHpBar.angerBarX, 0.5f * BattleHeroHpBar.boardHeight - BattleHeroHpBar.angerBarY, 0);

            //底板
            normals[i * BattleHeroHpBar.planeNum * 4] = new Vector3();
            normals[i * BattleHeroHpBar.planeNum * 4 + 1] = new Vector3();
            normals[i * BattleHeroHpBar.planeNum * 4 + 2] = new Vector3();
            normals[i * BattleHeroHpBar.planeNum * 4 + 3] = new Vector3();

            //血条
            normals[i * BattleHeroHpBar.planeNum * 4 + 4] = new Vector3();
            normals[i * BattleHeroHpBar.planeNum * 4 + 5] = new Vector3();
            normals[i * BattleHeroHpBar.planeNum * 4 + 6] = new Vector3();
            normals[i * BattleHeroHpBar.planeNum * 4 + 7] = new Vector3();

            //职业图标
            normals[i * BattleHeroHpBar.planeNum * 4 + 8] = new Vector3();
            normals[i * BattleHeroHpBar.planeNum * 4 + 9] = new Vector3();
            normals[i * BattleHeroHpBar.planeNum * 4 + 10] = new Vector3();
            normals[i * BattleHeroHpBar.planeNum * 4 + 11] = new Vector3();

            //怒气
            normals[i * BattleHeroHpBar.planeNum * 4 + 12] = new Vector3();
            normals[i * BattleHeroHpBar.planeNum * 4 + 13] = new Vector3();
            normals[i * BattleHeroHpBar.planeNum * 4 + 14] = new Vector3();
            normals[i * BattleHeroHpBar.planeNum * 4 + 15] = new Vector3();

            //底板
            uvs[i * BattleHeroHpBar.planeNum * 4] = new Vector2(0, 1 - BattleHeroHpBar.boardHeight / BattleHeroHpBar.TEXTURE_HEIGHT);
            uvs[i * BattleHeroHpBar.planeNum * 4 + 1] = new Vector2(BattleHeroHpBar.boardWidth / BattleHeroHpBar.TEXTURE_WIDTH, 1);
            uvs[i * BattleHeroHpBar.planeNum * 4 + 2] = new Vector2(BattleHeroHpBar.boardWidth / BattleHeroHpBar.TEXTURE_WIDTH, 1 - BattleHeroHpBar.boardHeight / BattleHeroHpBar.TEXTURE_HEIGHT);
            uvs[i * BattleHeroHpBar.planeNum * 4 + 3] = new Vector2(0, 1);

            //血条
            uvs[i * BattleHeroHpBar.planeNum * 4 + 4] = new Vector2(0, 0);
            uvs[i * BattleHeroHpBar.planeNum * 4 + 5] = new Vector2(0, 0);
            uvs[i * BattleHeroHpBar.planeNum * 4 + 6] = new Vector2(0, 0);
            uvs[i * BattleHeroHpBar.planeNum * 4 + 7] = new Vector2(0, 0);

            //职业图标
            //uvs[i * BattleHeroHpBar.planeNum * 4 + 8] = new Vector2(0,0);
            //uvs[i * BattleHeroHpBar.planeNum * 4 + 9] = new Vector2(0,0);
            //uvs[i * BattleHeroHpBar.planeNum * 4 + 10] = new Vector2(0,0);
            //uvs[i * BattleHeroHpBar.planeNum * 4 + 11] = new Vector2(0,0);

            uvs[i * BattleHeroHpBar.planeNum * 4 + 8] = new Vector2(0, 1 - (BattleHeroHpBar.iconBarHeight + BattleHeroHpBar.iconBarV) / BattleHeroHpBar.TEXTURE_HEIGHT);
            uvs[i * BattleHeroHpBar.planeNum * 4 + 9] = new Vector2(BattleHeroHpBar.iconBarWidth / BattleHeroHpBar.TEXTURE_WIDTH, 1 - BattleHeroHpBar.iconBarV / BattleHeroHpBar.TEXTURE_HEIGHT);
            uvs[i * BattleHeroHpBar.planeNum * 4 + 10] = new Vector2(BattleHeroHpBar.iconBarWidth / BattleHeroHpBar.TEXTURE_WIDTH, 1 - BattleHeroHpBar.iconBarHeight + BattleHeroHpBar.iconBarV / BattleHeroHpBar.TEXTURE_HEIGHT);
            uvs[i * BattleHeroHpBar.planeNum * 4 + 11] = new Vector2(0, 1 - BattleHeroHpBar.iconBarV / BattleHeroHpBar.TEXTURE_HEIGHT);

            //怒气
            uvs[i * BattleHeroHpBar.planeNum * 4 + 12] = new Vector2(BattleHeroHpBar.angerBarU / BattleHeroHpBar.TEXTURE_WIDTH, 1 - (BattleHeroHpBar.angerBarV + BattleHeroHpBar.angerBarHeight) / BattleHeroHpBar.TEXTURE_HEIGHT);
            uvs[i * BattleHeroHpBar.planeNum * 4 + 13] = new Vector2((BattleHeroHpBar.angerBarU + BattleHeroHpBar.angerBarWidth) / BattleHeroHpBar.TEXTURE_WIDTH, 1 - BattleHeroHpBar.angerBarV / BattleHeroHpBar.TEXTURE_HEIGHT);
            uvs[i * BattleHeroHpBar.planeNum * 4 + 14] = new Vector2((BattleHeroHpBar.angerBarU + BattleHeroHpBar.angerBarWidth) / BattleHeroHpBar.TEXTURE_WIDTH, 1 - (BattleHeroHpBar.angerBarV + BattleHeroHpBar.angerBarHeight) / BattleHeroHpBar.TEXTURE_HEIGHT);
            uvs[i * BattleHeroHpBar.planeNum * 4 + 15] = new Vector2(BattleHeroHpBar.angerBarU / BattleHeroHpBar.TEXTURE_WIDTH, 1 - BattleHeroHpBar.angerBarV / BattleHeroHpBar.TEXTURE_HEIGHT);

            //底板
            tangents[i * BattleHeroHpBar.planeNum * 4] = new Vector4(i, 0, 0, 0);
            tangents[i * BattleHeroHpBar.planeNum * 4 + 1] = new Vector4(i, 0, 0, 0);
            tangents[i * BattleHeroHpBar.planeNum * 4 + 2] = new Vector4(i, 0, 0, 0);
            tangents[i * BattleHeroHpBar.planeNum * 4 + 3] = new Vector4(i, 0, 0, 0);

            //血条
            tangents[i * BattleHeroHpBar.planeNum * 4 + 4] = new Vector4(i, 0, 0, 0);
            tangents[i * BattleHeroHpBar.planeNum * 4 + 5] = new Vector4(i, 1, 0, 0);
            tangents[i * BattleHeroHpBar.planeNum * 4 + 6] = new Vector4(i, 1, 0, 0);
            tangents[i * BattleHeroHpBar.planeNum * 4 + 7] = new Vector4(i, 0, 0, 0);

            //职业图标
            tangents[i * BattleHeroHpBar.planeNum * 4 + 8] = new Vector4(i, 0, 0, 0);
            tangents[i * BattleHeroHpBar.planeNum * 4 + 9] = new Vector4(i, 0, 0, 0);
            tangents[i * BattleHeroHpBar.planeNum * 4 + 10] = new Vector4(i, 0, 0, 0);
            tangents[i * BattleHeroHpBar.planeNum * 4 + 11] = new Vector4(i, 0, 0, 0);

            //怒气
            tangents[i * BattleHeroHpBar.planeNum * 4 + 12] = new Vector4(i, 0, 0, 0);
            tangents[i * BattleHeroHpBar.planeNum * 4 + 13] = new Vector4(i, 0, 1, 0);
            tangents[i * BattleHeroHpBar.planeNum * 4 + 14] = new Vector4(i, 0, 1, 0);
            tangents[i * BattleHeroHpBar.planeNum * 4 + 15] = new Vector4(i, 0, 0, 0);

            for (int j = 0; j < BattleHeroHpBar.planeNum; j++)
            {
                triangles[i * BattleHeroHpBar.planeNum * 6 + j * 6] = i * BattleHeroHpBar.planeNum * 4 + j * 4;
                triangles[i * BattleHeroHpBar.planeNum * 6 + j * 6 + 1] = i * BattleHeroHpBar.planeNum * 4 + j * 4 + 1;
                triangles[i * BattleHeroHpBar.planeNum * 6 + j * 6 + 2] = i * BattleHeroHpBar.planeNum * 4 + j * 4 + 2;
                triangles[i * BattleHeroHpBar.planeNum * 6 + j * 6 + 3] = i * BattleHeroHpBar.planeNum * 4 + j * 4 + 1;
                triangles[i * BattleHeroHpBar.planeNum * 6 + j * 6 + 4] = i * BattleHeroHpBar.planeNum * 4 + j * 4;
                triangles[i * BattleHeroHpBar.planeNum * 6 + j * 6 + 5] = i * BattleHeroHpBar.planeNum * 4 + j * 4 + 3;
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

        AssetDatabase.CreateAsset(mesh, "Assets/Arts/battle/BattleTool/HpBarMesh.asset");

        Material mat = new Material(Shader.Find("Custom/BattleHeroHpPass"));

        Texture t = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Arts/battle/BattleTool/xiaoxuetiao.png");
		mat.mainTexture = t;

		mr.material = mat;

        AssetDatabase.CreateAsset(mat, "Assets/Arts/battle/BattleTool/HpBarMat.mat");

        PrefabUtility.CreatePrefab("Assets/Arts/battle/BattleTool/HpBar.prefab", hpBarObj);

        GameObject.DestroyImmediate(hpBarObj);
    }
}

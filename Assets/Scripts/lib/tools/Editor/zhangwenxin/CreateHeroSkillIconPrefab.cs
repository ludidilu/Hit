using UnityEngine;
using System.Collections;
using UnityEditor;
using xy3d.tstd.lib.battleHeroTools;

public class CreateHeroSkillIconPrefab
{
	[MenuItem("zhangwenxin/技能图标prefab")]
    public static void Start()
    {
        Vector3[] vertices = new Vector3[BattleSkillIcon.unitNum * 4];
        Vector3[] normals = new Vector3[BattleSkillIcon.unitNum * 4];
        Vector2[] uvs = new Vector2[BattleSkillIcon.unitNum * 4];
        Vector4[] tangents = new Vector4[BattleSkillIcon.unitNum * 4];
        int[] triangles = new int[BattleSkillIcon.unitNum * 6];


        for (int i = 0; i < BattleSkillIcon.unitNum; i++)
        {

            vertices[i * 4] = new Vector3(-0.5f * BattleSkillIcon.FONT_WIDTH, -0.5f * BattleSkillIcon.FONT_HEIGHT, 0);
            vertices[i * 4 + 1] = new Vector3(0.5f * BattleSkillIcon.FONT_WIDTH, 0.5f * BattleSkillIcon.FONT_HEIGHT, 0);
            vertices[i * 4 + 2] = new Vector3(0.5f * BattleSkillIcon.FONT_WIDTH, -0.5f * BattleSkillIcon.FONT_HEIGHT, 0);
            vertices[i * 4 + 3] = new Vector3(-0.5f * BattleSkillIcon.FONT_WIDTH, 0.5f * BattleSkillIcon.FONT_HEIGHT, 0);

            uvs[i * 4] = new Vector2(0, 0);
            uvs[i * 4 + 1] = new Vector2(BattleSkillIcon.FONT_WIDTH / BattleSkillIcon.ASSET_WIDTH, BattleSkillIcon.FONT_HEIGHT / BattleSkillIcon.ASSET_HEIGHT);
            uvs[i * 4 + 2] = new Vector2(BattleSkillIcon.FONT_WIDTH / BattleSkillIcon.ASSET_WIDTH, 0);
            uvs[i * 4 + 3] = new Vector2(0, BattleSkillIcon.FONT_HEIGHT / BattleSkillIcon.ASSET_HEIGHT);

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

        GameObject skillIconGO = new GameObject();

        MeshFilter mf = skillIconGO.AddComponent<MeshFilter>();
        MeshRenderer mr = skillIconGO.AddComponent<MeshRenderer>();

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

        AssetDatabase.CreateAsset(mesh, "Assets/Arts/battle/BattleTool/SkillIconMesh.asset");

        Material mat = new Material(Shader.Find("Custom/BattleSkillIconPass"));


        mr.material = mat;

        AssetDatabase.CreateAsset(mat, "Assets/Arts/battle/BattleTool/SkillIconMat.mat");

        PrefabUtility.CreatePrefab("Assets/Arts/battle/BattleTool/SkillIcon.prefab", skillIconGO);

        GameObject.DestroyImmediate(skillIconGO);
    }
}

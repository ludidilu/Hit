using UnityEngine;
using System.Collections;
using UnityEditor;
using xy3d.tstd.lib.publicTools;
using xy3d.tstd.lib.scene;

public class SceneTools  {

	[MenuItem("场景/构造场景Prefab")]
	public static void Start(){

		GameObject go = Selection.activeGameObject;

		LightmapData[] datas = LightmapSettings.lightmaps;

		SceneData sceneData = new SceneData();

		sceneData.farTextures = new Texture2D[datas.Length];
		sceneData.nearTextures = new Texture2D[datas.Length];

		for(int i = 0 ; i < datas.Length ; i++){

			sceneData.farTextures[i] = datas[i].lightmapFar;
			sceneData.nearTextures[i] = datas[i].lightmapNear;
		}

		sceneData.fieldOfView = Camera.main.fieldOfView;

		sceneData.ambientLight = RenderSettings.ambientLight;
		sceneData.ambientIntensity = RenderSettings.ambientIntensity;
		
		sceneData.fog = RenderSettings.fog;
		sceneData.fogColor = RenderSettings.fogColor;
		sceneData.fogStartDistance = RenderSettings.fogStartDistance;
		sceneData.fogEndDistance = RenderSettings.fogEndDistance;

		AssetDatabase.CreateAsset(sceneData,"Assets/Arts/map/" + go.name + "_SceneData.asset");

		PrefabUtility.CreatePrefab("Assets/Arts/map/" + go.name + ".prefab",go);

		GameObject prefabResource = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Arts/map/" + go.name + ".prefab");

		GameObject prefab = GameObject.Instantiate(prefabResource);

		Renderer[] renderers = go.GetComponentsInChildren<Renderer>();

		foreach(Renderer renderer in renderers){

			if(renderer.lightmapIndex != -1){

				GameObject tg = PublicTools.FindChild(prefab,renderer.gameObject.name);

				LightmapGameObject ll = tg.AddComponent<LightmapGameObject>();

				ll.lightmapIndex = renderer.lightmapIndex;

				ll.lightmapScaleOffset = renderer.lightmapScaleOffset;
			}
		}

		Scene scene = prefab.AddComponent<Scene>();

		scene.sceneData = sceneData;

		string path = "Assets/Arts/map/" + go.name + ".prefab";

		PrefabUtility.CreatePrefab(path,prefab);

		GameObject.DestroyImmediate(prefabResource);

		GameObject.DestroyImmediate(prefab);

		AssetBundleTools.SetAssetBundleName(path,go.name);

//		AssetImporter importer = AssetImporter.GetAtPath(path);
//		
//		importer.assetBundleName = go.name;

		Debug.Log("场景Prefab构造完成!");
	}
}

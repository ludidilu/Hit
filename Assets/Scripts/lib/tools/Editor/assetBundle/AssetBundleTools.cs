using UnityEngine;
using System.Collections;
using UnityEditor;

using System;
using xy3d.tstd.lib.assetManager;
using System.Collections.Generic;
using xy3d.tstd.lib.assetBundleManager;

public class AssetBundleTools{
	
	[MenuItem("AssetBundle/清除所有选中对象的AssetBundle名字")]
	public static void ClearSelectedAssetBundleName(){
		
		UnityEngine.Object[] objects = Selection.objects;
		
		foreach(UnityEngine.Object obj in objects){
			
			string path = AssetDatabase.GetAssetPath(obj);
			
			AssetImporter importer = AssetImporter.GetAtPath(path);
			
			importer.assetBundleName = null;

			AssetDatabase.RemoveUnusedAssetBundleNames();
		}
	}

	[MenuItem("AssetBundle/设置所有选中对象的AssetBundle名字")]
	public static void SetSelectedAssetBundleName(){
		
		UnityEngine.Object[] objects = Selection.objects;
		
		foreach(UnityEngine.Object obj in objects){
			
			string path = AssetDatabase.GetAssetPath(obj);

			SetAssetBundleName(path,obj.name);
			
//			AssetImporter importer = AssetImporter.GetAtPath(path);
//
//			importer.assetBundleName = obj.name;
		}
	}

	public static void SetAssetBundleName(string _path,string _name){

		AssetImporter importer = AssetImporter.GetAtPath(_path);
		
		importer.assetBundleName = _name;
	}

//	[MenuItem("AssetBundle/设置所有选中对象的AssetBundle名字为uitexture")]
//	public static void SetAssetBundleNameWith(){
//		
//		UnityEngine.Object[] objects = Selection.objects;
//		
//		foreach(UnityEngine.Object obj in objects){
//			
//			string path = AssetDatabase.GetAssetPath(obj);
//			
//			AssetImporter importer = AssetImporter.GetAtPath(path);
//			
//			importer.assetBundleName = "uitexture";
//		}
//	}

	[MenuItem("AssetBundle/打包生成AssetBundle以及依赖列表:PC")]
	public static void CreateAssetBundlePC(){

		CreateAssetBundle(BuildTarget.StandaloneWindows64);

//		AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles (Application.streamingAssetsPath + "/" + AssetBundleManager.path,BuildAssetBundleOptions.UncompressedAssetBundle);
//		
//		CreateAssetBundleDat(manifest);
	}

	[MenuItem("AssetBundle/打包生成AssetBundle以及依赖列表:Android")]
	public static void CreateAssetBundleAndroid(){

		CreateAssetBundle(BuildTarget.Android);

//		AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles (Application.streamingAssetsPath + "/" + AssetBundleManager.path,BuildAssetBundleOptions.UncompressedAssetBundle,BuildTarget.Android);
//		
//		CreateAssetBundleDat(manifest);
	}

	[MenuItem("AssetBundle/打包生成AssetBundle以及依赖列表:IOS")]
	public static void CreateAssetBundleIOS(){

		CreateAssetBundle(BuildTarget.iOS);

//		AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles (Application.streamingAssetsPath + "/" + AssetBundleManager.path,BuildAssetBundleOptions.UncompressedAssetBundle,BuildTarget.iOS);
//		
//		CreateAssetBundleDat(manifest);
	}

	private static void CreateAssetBundle(BuildTarget _buildTarget){

		RenderSettings.fog = true;
		
		RenderSettings.fogMode = FogMode.Linear;

		LightmapData[] lightMaps = new LightmapData[1];

		lightMaps[0] = new LightmapData();

		lightMaps[0].lightmapFar = new Texture2D(100,100);

		LightmapSettings.lightmaps = lightMaps;

		AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles (Application.streamingAssetsPath + "/" + AssetBundleManager.path,BuildAssetBundleOptions.UncompressedAssetBundle,_buildTarget);

		RenderSettings.fog = false;

		LightmapSettings.lightmaps = new LightmapData[0];

		CreateAssetBundleDat(manifest);
	}

	private static void CreateAssetBundleDat(AssetBundleManifest manifest){
		
		string[] abs = manifest.GetAllAssetBundles ();
		
		AssetBundle[] aaaa = new AssetBundle[abs.Length];
		
		try{
			
			List<UnityEngine.Object> assets = new List<UnityEngine.Object> ();
			
			List<string> assetNames = new List<string> ();
			
			List<string> assetBundleNames = new List<string> ();
			
			List<bool> isGameObject = new List<bool> ();
			
			Dictionary<string,List<string>> result = new Dictionary<string, List<string>> ();
			
			for(int i = 0 ; i < abs.Length ; i++){
				
				AssetBundle ab = AssetBundle.CreateFromFile(Application.streamingAssetsPath + "/" + AssetBundleManager.path + abs[i]);
				
				aaaa[i] = ab;
				
				string[] nn = ab.GetAllAssetNames();
				
				foreach(string str in nn){
					
					if(assetNames.Contains(str)){
						
						Debug.LogError("error!");
						
					}else{
						
						assetNames.Add(str);
						
						UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(str);
						
						assets.Add(obj);
						
						assetBundleNames.Add(abs[i]);
						
						isGameObject.Add(obj is GameObject);
						
						List<string> ll = new List<string>();
						
						result.Add(str,ll);
					}
				}
			}
			
			for (int i = 0; i < assetNames.Count; i++) {
				
				string name = assetNames[i];
				UnityEngine.Object obj = assets[i];
				List<string> list = result[name];
				
				UnityEngine.Object[] sss = EditorUtility.CollectDependencies(new UnityEngine.Object[]{obj});
				
				foreach(UnityEngine.Object dd in sss){
					
					if(dd != obj){
						
						if(assets.Contains(dd)){
							
							string assetBundleName = assetBundleNames[assets.IndexOf(dd)];
							
							if(!list.Contains(assetBundleName)){
								
								list.Add(assetBundleName);
							}
						}
					}
				}
			}
			
			Dictionary<string,AssetUnitData> dic = new Dictionary<string, AssetUnitData> ();
			
			for (int i = 0; i < assetNames.Count; i++) {
				
				AssetUnitData unit = new AssetUnitData();
				
				unit.assetBundle = assetBundleNames[i];
				
				List<string> tmpList = result[assetNames[i]];
				
				unit.assetBundleDep = tmpList.ToArray();
				
				dic.Add(assetNames[i],unit);
			}
			
			AssetManager.SaveAssetData (dic);
			
			Debug.Log("生成依赖列表成功！");
			
		}catch(Exception e){
			
			Debug.Log("error:" + e.Message);
			
		}finally{
			
			foreach (AssetBundle aaa in aaaa) {
				
				aaa.Unload (true);
			}
		}
	}
}

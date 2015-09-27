using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using xy3d.tstd.lib.assetBundleManager;

using System;

namespace xy3d.tstd.lib.assetManager{

	public class AssetUnit<T> : AssetUnitBase where T:UnityEngine.Object {

		public AssetUnitData data;
	

		public int type = -1;
	
		public List<Action<T>> callBackList = new List<Action<T>>();
		public string name;

		public AssetUnit(string _name){

			name = _name;

			data = AssetManager.Instance.GetData (name);
		}

		public void Load(Action<T> _callBack){

			if (type == -1) {

				type = 0;

				callBackList.Add (_callBack);

				StartLoad();

			} else {

				callBackList.Add (_callBack);
			}
		}

		private void StartLoad(){

//			Debug.Log("要求获取asset:" + name);

			int loadNum = 2;

			AssetBundle assetBundle = null;

			Action<AssetBundle> callBack = delegate(AssetBundle _assetBundle) {

				assetBundle = _assetBundle;

//				Debug.Log("Asset所需要的主assetBundle加载完毕:" + assetBundle);

				GetAssetBundle(ref loadNum,assetBundle);
			};

			AssetBundleManager.Instance.Load (data.assetBundle, callBack);

			if (data.assetBundleDep.Length > 0) {

				callBack = delegate(AssetBundle _assetBundle) {

//					Debug.Log("Asset所需要的依赖assetBundle加载完毕:" + assetBundle);

					GetAssetBundle(ref loadNum,assetBundle);
				};

				foreach(string path in data.assetBundleDep){

					loadNum++;



					AssetBundleManager.Instance.Load (path, callBack);
				}
			}

			GetAssetBundle(ref loadNum,assetBundle);
		}

		private void GetAssetBundle(ref int _loadNum,AssetBundle _assetBundle){

			_loadNum--;

			if (_loadNum == 0) {

//				Debug.Log("asset所需要的ab全部加载完毕了:" + name);
//
//				Debug.Log("assetBundleName:" + _assetBundle);

				type = 1;

				T asset = null;

				try{

					asset = _assetBundle.LoadAsset<T>(name);

				}catch(Exception e){

					Debug.Log("Asset加载错误:" + e.Message);

				}finally{


				}

//				Debug.Log("asset提取完毕了:" + name);

				asset.name = name;

				AssetBundleManager.Instance.Unload(data.assetBundle);

				foreach(string depName in data.assetBundleDep){

					AssetBundleManager.Instance.Unload(depName);
				}

				AssetManager.Instance.RemoveUnit(name);

				foreach(Action<T> callBack in callBackList){
					
//					if(data.isGameObject){
//						
//						Object obj = GameObject.Instantiate(asset);
//						
//						obj.name = name;
//						
//						callBack(obj);
//						
//					}else{
//						
//						callBack(asset);
//					}

					callBack(asset);
				}
				
				callBackList.Clear();
			}
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using xy3d.tstd.lib.wwwManager;
using System;

namespace xy3d.tstd.lib.assetBundleManager{

	public class AssetBundleUnit{

		private AssetBundle assetBundle;
		private string name;
		private int type = -1;
		private List<Action<AssetBundle>> callBackList;
		private int useTimes;

		public AssetBundleUnit(string _name){

			name = _name;

			callBackList = new List<Action<AssetBundle>> ();
		}

		public void Load(Action<AssetBundle> _callBack){

			useTimes++;

//			Debug.Log ("LoadAssetBundle:" + name);

			if (type == -1) {

				type = 0;

				callBackList.Add (_callBack);

				WWWManager.Instance.Load(AssetBundleManager.path + name,GetAssetBundle);

			} else if (type == 0) {

				callBackList.Add (_callBack);

			} else {

				_callBack (assetBundle);
			}
		}

		private void GetAssetBundle(WWW _www){

			type = 1;

			assetBundle = _www.assetBundle;

//			Debug.Log("一个assetBundle加载完毕了:" + name + "    " + assetBundle);

			foreach (Action<AssetBundle> callBack in callBackList) {

				callBack (assetBundle);
			}

			callBackList.Clear ();
		}

		public void Unload(){

			useTimes--;

			if (useTimes == 0) {

//				Debug.Log ("dispose assetBundle:" + name);

				assetBundle.Unload (false);

				AssetBundleManager.Instance.Remove (name);
			}
		}
	}
}

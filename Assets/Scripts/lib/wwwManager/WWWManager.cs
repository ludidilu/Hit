using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace xy3d.tstd.lib.wwwManager{

	public class WWWManager{

		private static WWWManager _Instance;
		
		public static WWWManager Instance {
			
			get {
				
				if (_Instance == null) {
					
					_Instance = new WWWManager ();
				}
				
				return _Instance;
			}
		}

		private GameObject go;
		private WWWManagerScript script;

		private List<string> pathList = new List<string>();
		private List<bool> isRemoteList = new List<bool>();
		private List<Action<WWW>> callBackList = new List<Action<WWW>>();

		private const int maxLoadNum = 10;
		private int loadNum = 0;

		public delegate bool fixUrlDelegate(ref string _path);

		public WWWManager(){

			go = new GameObject();

			go.name = "WWWManagerGameObject";

			GameObject.DontDestroyOnLoad(go);

			script = go.AddComponent<WWWManagerScript>();
		}

		public void SetUrlFixFun(fixUrlDelegate _callBack){

			script.SetUrlFixFun(_callBack);
		}

		public void Load(string _path,Action<WWW> _callBack){

			LoadReal(_path,false,_callBack);
		}

		public void LoadRemote(string _path,Action<WWW> _callBack){

			LoadReal(_path,true,_callBack);
		}

		private void LoadReal(string _path, bool _isRemote, Action<WWW> _callBack){
			
			if(loadNum < maxLoadNum){
				
				loadNum++;
				
				Action<WWW> callBack = delegate(WWW _www) {
					
					LoadComplete(_www,_callBack);
				};
				
				script.Load(_path,_isRemote,callBack);
				
			}else{
				
				pathList.Add(_path);
				isRemoteList.Add(_isRemote);
				callBackList.Add(_callBack);
			}
		}

		private void LoadComplete(WWW _www,Action<WWW> _callBack){

			_callBack(_www);

			OneLoadOK();
		}

		private void OneLoadOK(){

//			Debug.Log("一个WWW加载完成了  还剩下" + pathList.Count + "个");

			if(pathList.Count > 0){

				string path = pathList[0];
				pathList.RemoveAt(0);

				bool isRemote = isRemoteList[0];
				isRemoteList.RemoveAt(0);

				Action<WWW> callBack = callBackList[0];
				callBackList.RemoveAt(0);

				Action<WWW> callBack2 = delegate(WWW _www) {
					
					LoadComplete(_www,callBack);
				};
				
				script.Load(path,isRemote,callBack2);

			}else{

				loadNum--;
			}
		}
	}
}
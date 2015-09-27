using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace xy3d.tstd.lib.gameObjectFactory{

	public class GameObjectFactory{

		public Dictionary<string,GameObjectFactoryUnit> dic = new Dictionary<string, GameObjectFactoryUnit>();

		private static GameObjectFactory _Instance;

		public static GameObjectFactory Instance {

			get {

				if(_Instance == null){

					_Instance = new GameObjectFactory();
				}

				return _Instance;
			}
		}

		public GameObject GetGameObject(string _path,Action<GameObject> _callBack,bool _addUseNum){

			GameObjectFactoryUnit unit;

			if (!dic.ContainsKey (_path)) {

				unit = new GameObjectFactoryUnit (_path);

				dic.Add(_path,unit);

			} else {

				unit = dic [_path];
			}

			return unit.GetGameObject (_path, _callBack, _addUseNum);
		}

		public GameObject GetGameObject(string _bodyPath,Action<GameObject> _callBack,string[] _partsPaths,string[] _jointNames,float[] _partsScales){
			
			GameObjectFactoryUnit unit;

			string path = _bodyPath;

			foreach (string part in _partsPaths) {

				path = string.Concat (path, part);
			}

			if (!dic.ContainsKey (path)) {
				
				unit = new GameObjectFactoryUnit (path);

				dic.Add(path,unit);
				
			} else {
				
				unit = dic [path];
			}
			
			return unit.GetGameObject (_bodyPath, _callBack, _partsPaths, _jointNames,_partsScales);
		}

		public GameObject[] GetGameObjects(string[] _paths,Action<GameObject[]> _callBack,bool _addUseNum){
			
			int loadNum = 1;

			GameObject[] gameObjects = new GameObject[_paths.Length];
			
			for(int i = 0 ; i < _paths.Length ; i++){
				
				string path = _paths[i];
				
				GameObjectFactoryUnit unit;
				
				if (!dic.ContainsKey (path)) {
					
					unit = new GameObjectFactoryUnit (path);
					
					dic.Add(path,unit);
					
				} else {
					
					unit = dic [path];
				}
				
				loadNum++;

				int tmpIndex = i;
				
				Action<GameObject> callBack = delegate(GameObject _go) {

					gameObjects[tmpIndex] = _go;

					GetOneOfGameObjects(ref loadNum,gameObjects,_callBack);
				};
				
				unit.GetGameObject (path, callBack, _addUseNum);
			}

			GetOneOfGameObjects(ref loadNum,gameObjects,_callBack);

			return gameObjects;
		}

		private void GetOneOfGameObjects(ref int _loadNum,GameObject[] _gameObjects,Action<GameObject[]> _callBack){

			_loadNum--;

			if(_loadNum == 0){

				if(_callBack != null){

					_callBack(_gameObjects);
				}
			}
		}

		public void Dispose(bool _force){

			List<string> delKeyList = new List<string> ();

			foreach (KeyValuePair<String,GameObjectFactoryUnit> pair in dic) {

				if (_force || pair.Value.useNum == 0) {

					pair.Value.Dispose ();

					delKeyList.Add (pair.Key);
				}
			}

			foreach (string key in delKeyList) {

				dic.Remove (key);
			}

			Resources.UnloadUnusedAssets();
		}
	}
}
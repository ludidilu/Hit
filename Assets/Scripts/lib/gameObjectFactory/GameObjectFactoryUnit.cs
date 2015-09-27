using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using xy3d.tstd.lib.gameObjectFactory;
using xy3d.tstd.lib.publicTools;
using xy3d.tstd.lib.assetManager;

using System;

namespace xy3d.tstd.lib.gameObjectFactory
{

	public class GameObjectFactoryUnit
	{

		public string name;
		public GameObject data;

		private bool dataIsCopy;

		private int type = -1;

		private int _useNum = 0;

		public int useNum {

			get {

				return _useNum;
			}
		}

		private List<Action<GameObject>> callBackList = new List<Action<GameObject>> ();
		private List<bool> addUseNumList = new List<bool> ();

		public GameObjectFactoryUnit (string _name)
		{

			name = _name;
		}

		public GameObject GetGameObject (string _path, Action<GameObject> _callBack, bool _addUseNum)
		{

//			Debug.Log ("加载gameobject:" + _path);

			if (_addUseNum) {

				AddUseNum ();
			}

			if (type == -1) {

				type = 0;

				callBackList.Add (_callBack);
				addUseNumList.Add (_addUseNum);

				Action<GameObject> callBack = delegate(GameObject result) {

					GetResouece (result as GameObject);
				};

				AssetManager.Instance.GetAsset (_path, callBack);

				return null;

			} else if (type == 0) {

				callBackList.Add (_callBack);
				addUseNumList.Add (_addUseNum);

				return null;

			} else {

				if (_addUseNum) {

					GameObject result = GameObject.Instantiate (data);

					FixResult (result);

					if (_callBack != null) {

						_callBack (result);
					}

					return result;

				} else {

					if (_callBack != null) {

						_callBack (data);
					}

					return data;
				}
			}
		}

		private void GetResouece (GameObject _go)
		{

			data = _go;

			dataIsCopy = false;

//			PublicTools.SetGameObjectVisible (data, false);

//			PublicTools.StopParticle (data);

			type = 1;

			for (int i = 0; i < callBackList.Count; i++) {

				if (callBackList [i] != null) {

					if (addUseNumList [i]) {

						GameObject result = GameObject.Instantiate (data);

						FixResult (result);

						callBackList [i] (result);

					} else {

						callBackList [i] (data);
					}
				}
			}

			callBackList.Clear ();
			addUseNumList.Clear ();
		}
		
		public GameObject GetGameObject (string _bodyPath, Action<GameObject> _callBack, string[] _partsPaths, string[] _jointNames, float[] _partsScale)
		{

			AddUseNum ();

			if (type == -1) {
				
				type = 0;
				
				callBackList.Add (_callBack);
				
				StartLoad (_bodyPath, _partsPaths, _jointNames, _partsScale);

				return null;
				
			} else if (type == 0) {
				
				callBackList.Add (_callBack);

				return null;
				
			} else {
				
//				if(_addUseNum){

				GameObject result = GameObject.Instantiate (data);

				result.SetActive (true);

				FixResult (result);

				if (_callBack != null) {

					_callBack (result);
				}

				return result;
					
//				}else{
//
//					if(_callBack != null){
//					
//						_callBack(data);
//					}
//					
//					return data;
//				}
			}
		}

		private void StartLoad (string _bodyPath, string[] _partsPaths, string[] _jointNames, float[] _partsScale)
		{

			GameObject body = null;
			GameObject[] parts = new GameObject[_partsPaths.Length];
			
			int loadNum = 3;
			
			Action<GameObject> callBack = delegate(GameObject _go) {
				
				body = _go;
				
				GetSomeOfResource (ref loadNum, body, parts, _jointNames, _partsScale);
			};
			
			GameObjectFactory.Instance.GetGameObject (_bodyPath, callBack, false);

			Action<GameObject[]> callBack2 = delegate(GameObject[] _gameObjects) {

				parts = _gameObjects;

				GetSomeOfResource (ref loadNum, body, parts, _jointNames, _partsScale);
			};

			GameObjectFactory.Instance.GetGameObjects (_partsPaths, callBack2, false);

			GetSomeOfResource (ref loadNum, body, parts, _jointNames, _partsScale);
		}

		private void GetSomeOfResource (ref int _loadNum, GameObject _body, GameObject[] _parts, string[] _jointNames, float[] _partScales)
		{

			_loadNum--;

			if (_loadNum == 0) {

				GetAllResource (_body, _parts, _jointNames, _partScales);
			}
		}

		private void GetAllResource (GameObject _body, GameObject[] _parts, string[] _jointNames, float[] _partScales)
		{

//			data = _body; 

			data = GameObject.Instantiate (_body);

			dataIsCopy = true;

			data.name = name;

//			PublicTools.SetGameObjectVisible (data, false);

			GameObjectTools.CombinePart (data, _parts, _jointNames, _partScales);
			
			type = 1;
			
			for (int i = 0; i < callBackList.Count; i++) {

				if (callBackList [i] != null) {
				
//					if(addUseNumList[i]){

					GameObject result = GameObject.Instantiate (data);

					FixResult (result);

					callBackList [i] (result);
						
//					}else{
//						
//						callBackList[i](data);
//					}
				}
			}

			callBackList.Clear ();

			data.SetActive (false);


//			addUseNumList.Clear ();
		}

		private void FixResult (GameObject _go)
		{

//			PublicTools.SetGameObjectVisible (_go, true);

//			_go.SetActive(true);

			_go.AddComponent<GameObjectControl> ();
			
			_go.GetComponent<GameObjectControl> ().unit = this;

			_go.name = name;
		}

		public void AddUseNum ()
		{

			_useNum++;
		}

		public void DelUseNum ()
		{

			_useNum--;
		}

		public void Dispose ()
		{

			if (data != null) {

				if (dataIsCopy) {

					GameObject.Destroy (data);
				}

				data = null;
			}
		}
	}
}
using UnityEngine;
using System.Collections;
using System;

using System.Collections.Generic;

namespace xy3d.tstd.lib.wwwManager
{

	public class WWWManagerScript : MonoBehaviour
	{

#if PLATFORM_PC

		private static string path = "file:///" + Application.streamingAssetsPath + "/";

#endif

#if PLATFORM_ANDROID

		private static string path = Application.streamingAssetsPath + "/";

#endif

#if PLATFORM_IOS

		private static string path = Application.streamingAssetsPath + "/";

#endif

		private List<Action> callBackList = new List<Action>();

		WWWManager.fixUrlDelegate fixUrlDelegate;

		public void SetUrlFixFun(WWWManager.fixUrlDelegate _callBack){
			
			fixUrlDelegate = _callBack;
		}

		public void Load (string _path, bool _isRemote, Action<WWW> _callBack)
		{

			StartCoroutine (LoadCorotine (_path, _isRemote, _callBack));
		}

		private IEnumerator LoadCorotine (string _path, bool _isRemote, Action<WWW> _callBack)
		{

			string finalPaht;

			if(!_isRemote){

				if(fixUrlDelegate == null){

					finalPaht = path + _path;

				}else{

					bool b = fixUrlDelegate(ref _path);

					if(b){

						finalPaht = _path;

					}else{

						finalPaht = path + _path;
					}
				}

			}else{

				finalPaht = _path;
			}

			WWW www = new WWW(finalPaht);

			yield return www;

//			Debug.Log ("资源加载成功:" + _path);

			if (www.error != null) {

				Debug.Log("WWW download had an error:" + www.error);

//				throw new Exception ("WWW download had an error:" + www.error);
			}

			Action callBack = delegate() {

				_callBack(www);

				www.Dispose();
			};

			callBackList.Add(callBack);
		}

		void Update(){

			if(callBackList.Count > 0){

				foreach(Action callBack in callBackList){

					callBack();
				}

				callBackList.Clear();
			}
		}
	}
}
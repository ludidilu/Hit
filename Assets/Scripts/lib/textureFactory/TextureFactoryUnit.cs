using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using xy3d.tstd.lib.assetManager;
using System;

namespace xy3d.tstd.lib.textureFactory{

	public class TextureFactoryUnit<T>: TextureFactoryUnitBase where T:UnityEngine.Object {

		private string name;
		private T data;
		private int type = -1;


		private List<Action<T>> callBackList = new List<Action<T>>();

		public TextureFactoryUnit(string _name){
			
			name = _name;
		}

		public T GetTexture(Action<T> _callBack,bool _addUseNum){

			if(_addUseNum){

				useNum++;
			}

			if (type == -1) {
				
				type = 0;
				
				callBackList.Add (_callBack);
				
				AssetManager.Instance.GetAsset<T> (name,GetAsset);

				return default(T);
				
			} else if (type == 0) {
				
				callBackList.Add (_callBack);

				return default(T);
				
			} else {

				if(_callBack != null){

					_callBack(data);
				}

				return data;
			}
		}

		private void GetAsset(T _data){

			data = _data;

			type = 1;

			foreach(Action<T> callBack in callBackList){

				if(callBack != null){

					callBack(data);
				}
			}

			callBackList.Clear();
		}

		public override void Dispose(){

			if (data != null) {
				
				Resources.UnloadAsset(data);

				data = null;
			}
		}
	}
}

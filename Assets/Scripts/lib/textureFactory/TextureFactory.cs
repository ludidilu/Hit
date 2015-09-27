using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace xy3d.tstd.lib.textureFactory{

	public class TextureFactory{

		private static TextureFactory _Instance;
		
		public static TextureFactory Instance {
			
			get {
				
				if (_Instance == null) {
					
					_Instance = new TextureFactory ();
				}
				
				return _Instance;
			}
		}

//		public delegate void getTextureCallBack<T>(T _texture);

		public Dictionary<string,TextureFactoryUnitBase> dic ;

		public TextureFactory(){

			dic = new Dictionary<string, TextureFactoryUnitBase>();
		}

		public T GetTexture<T> (string _name,Action<T> _callBack,bool _addUseNum) where T:UnityEngine.Object {

			TextureFactoryUnit<T> unit;
			
			if (!dic.ContainsKey (_name)) {
				
				unit = new TextureFactoryUnit<T> (_name);
				
				dic.Add (_name, unit);
				
			} else {
				
				unit = dic [_name] as TextureFactoryUnit<T>;
			}

			return unit.GetTexture(_callBack,_addUseNum);
		}

		public void Dispose(bool _force){
			
			List<string> delKeyList = new List<string> ();
			
			foreach (KeyValuePair<string,TextureFactoryUnitBase> pair in dic) {

				if (_force || pair.Value.useNum == 0) {
					
					pair.Value.Dispose ();
					
					delKeyList.Add (pair.Key);
				}
			}
			
			foreach (string key in delKeyList) {
				
				dic.Remove (key);
			}
		}
	}
}

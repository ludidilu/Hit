using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using UnityEngine;

using xy3d.tstd.lib.wwwManager;
using xy3d.tstd.lib.assetBundleManager;
using xy3d.tstd.lib.systemIO;

#if USE_ASSETBUNDLE

#else

using UnityEditor;

#endif

namespace xy3d.tstd.lib.assetManager
{

	public class AssetManager
	{

		private static string dataName = "ab.dat";

//		public delegate void getAssetCallBack<T> (T _asset);
//		public delegate void loadAssetDataOverCallBack();

		private static AssetManager _Instance;

		public static AssetManager Instance {

			get {

				if (_Instance == null) {

					_Instance = new AssetManager ();
				}

				return _Instance;
			}
		}

		public Dictionary<string,AssetUnitBase> dic;

#if USE_ASSETBUNDLE

		private Dictionary<string,AssetUnitData> dataDic;

#endif

		public AssetManager ()
		{

			dic = new Dictionary<string, AssetUnitBase> ();
		}

#if USE_ASSETBUNDLE

		public void Init(Action _callBack){

			Action<WWW> callBack = delegate(WWW _www) {
				
				LoadAssetData(_www,_callBack);
			};
			
			WWWManager.Instance.Load(dataName,callBack);

		}
#endif


		private void LoadAssetData (WWW _www, Action _callBack)
		{

#if USE_ASSETBUNDLE

			using(Stream stream = new MemoryStream(_www.bytes)){
				
				BinaryFormatter formatter = new BinaryFormatter ();
				
				dataDic = formatter.Deserialize (stream) as Dictionary<string,AssetUnitData>;
				
				Debug.Log("AssetData加载完毕！！！");
				
				_callBack();
			}     

#endif
		}

		public static void SaveAssetData (Dictionary<string,AssetUnitData> _dic)
		{
			SystemIO.SaveSerializeFile(Application.streamingAssetsPath + "/" + dataName,_dic);
		}

		public AssetUnitData GetData (string _name)
		{

#if USE_ASSETBUNDLE

			string name = _name.ToLower();

			if(!dataDic.ContainsKey(name)){

				Debug.LogError("AssetBundle中没有找到Asset:" + _name);
			}

			return dataDic [name];

#else

			return null;

#endif
		}

		public void GetAsset<T> (string _name, Action<T> _callBack) where T:UnityEngine.Object
		{

#if USE_ASSETBUNDLE

			AssetUnit<T> unit;
			
			if (!dic.ContainsKey (_name)) {
				
				unit = new AssetUnit<T> (_name);
				
				dic.Add(_name,unit);
				
			} else {
				
				unit = dic [_name] as AssetUnit<T>;
			}
			
			unit.Load (_callBack);

#else

			_callBack (AssetDatabase.LoadAssetAtPath<T> (_name));

#endif
		}

		public void RemoveUnit (string _name)
		{
			
			dic.Remove (_name);
		}
	}
}

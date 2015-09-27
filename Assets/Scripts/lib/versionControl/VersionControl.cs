using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using xy3d.tstd.lib.systemIO;

using xy3d.tstd.lib.wwwManager;
using System.Xml;
using System.Collections.Generic;
using xy3d.tstd.lib.publicTools;
using DamienG.Security.Cryptography;

namespace xy3d.tstd.lib.versionControl{

	struct UpdateFileInfo{

		public int version;
		public int size;
		public string path;
		public UInt32 crc;
	}

	public class VersionControl{

		private const string FILE_NAME = "version.dat";

		private static VersionControl _Instance;

		public static VersionControl Instance{

			get{

				if(_Instance == null){

					_Instance = new VersionControl();
				}

				return _Instance;
			}
		}

		private VersionData data;

		public VersionControl(){


		}

		public void Init(int _localVersion,int _remoteVersion,Func<int,string> _fixFun,Func<string,string> _fixFun2,Action _callBack){

			if(File.Exists(Application.persistentDataPath + "/" + FILE_NAME)){

				data = SystemIO.LoadSerializeFile<VersionData>(Application.persistentDataPath + "/" + FILE_NAME);

				if(_localVersion > data.version){//说明残留的version.dat是老版本的  必须立即清除掉

					Debug.Log("发现残留的version.dat 删除掉!");

					data = new VersionData();
					
					data.version = _localVersion;
					
					SystemIO.SaveSerializeFile(Application.persistentDataPath + "/" + FILE_NAME,data);
				}

			}else{

				data = new VersionData();

				data.version = _localVersion;

				SystemIO.SaveSerializeFile(Application.persistentDataPath + "/" + FILE_NAME,data);
			}

			if(data.version < _remoteVersion){

				Dictionary<string,UpdateFileInfo> dic = new Dictionary<string, UpdateFileInfo>();

				LoadUpdateXML(dic,_remoteVersion,_remoteVersion,_fixFun,_fixFun2,_callBack);

			}else{

				_callBack();
			}
		}

		private void LoadUpdateXML(Dictionary<string,UpdateFileInfo> _dic,int _remoteVersion,int _version,Func<int,string> _fixFun,Func<string,string> _fixFun2,Action _callBack){

			if(_version > data.version){

				string url = _fixFun(_version);

//				url = @"http://192.168.40.240/update_and/versions/update_101_and_facebook.xml";

				Action<WWW> callBack = delegate(WWW obj) {

					if(obj.error != null){

						Debug.Log("文件热更新失败 文件名:" + obj.url);
						
						return;
					}

					XmlDocument xmlDoc = new XmlDocument ();
					
					xmlDoc.LoadXml(PublicTools.XmlFix(obj.text));
					
					XmlNodeList hhh = xmlDoc.ChildNodes[0].ChildNodes;
					
					foreach(XmlNode node in hhh){

						string nodeUrl = node.InnerText;

						if(!_dic.ContainsKey(nodeUrl)){

							int fileSize = int.Parse(node.Attributes["size"].Value);

							if(fileSize == -1){

								if(data.dic.ContainsKey(nodeUrl)){

									data.dic.Remove(nodeUrl);
								}

							}else{

								UpdateFileInfo fileInfo = new UpdateFileInfo();

								fileInfo.version = _version;

								fileInfo.size = fileSize;

								fileInfo.path = node.Attributes["path"].Value;

								fileInfo.crc = Convert.ToUInt32(node.Attributes["crc"].Value,16);

								_dic.Add(nodeUrl,fileInfo);
							}
						}
					}

					LoadUpdateXML(_dic,_remoteVersion,_version - 1,_fixFun,_fixFun2,_callBack);
				};

				WWWManager.Instance.LoadRemote(url,callBack);

			}else{

				LoadUpdateXMLOK(_dic,_remoteVersion,_fixFun2,_callBack);
			}
		}

		private void LoadUpdateXMLOK(Dictionary<string,UpdateFileInfo> _dic,int _remoteVersion,Func<string,string> _fixFun,Action _callBack){

			int loadNum = _dic.Count;

			foreach(KeyValuePair<string,UpdateFileInfo> pair in _dic){

				string path = pair.Key;
				UpdateFileInfo info = pair.Value;

				string url = _fixFun(info.path + path);

				Action<WWW> callBack = delegate(WWW obj) {

					if(obj.error != null){

						Debug.Log("文件热更新失败 文件名:" + obj.url);

						return;
					}

					UInt32 crc = CRC32.Compute(obj.bytes);

					if(crc != info.crc){

						Debug.Log("文件热更新CRC比对错误 文件名:" + obj.url);
						
						return;
					}

					SystemIO.SaveFile(Application.persistentDataPath + "/" + path,obj.bytes);

					if(data.dic.ContainsKey(path)){

						data.dic[path] = info.version;

					}else{

						data.dic.Add(path,info.version);
					}

					loadNum--;

					if(loadNum == 0){

						UpdateOver(_remoteVersion,_callBack);
					}
				};

				WWWManager.Instance.LoadRemote(url,callBack);
			}
		}

		private void UpdateOver(int _remoteVersion,Action _callBack){

			data.version = _remoteVersion;

			SystemIO.SaveSerializeFile(Application.persistentDataPath + "/" + FILE_NAME,data);

			_callBack();
		}

		public bool FixUrl(ref string _path){

			if(data != null){

				if(data.dic.ContainsKey(_path)){

#if PLATFORM_PC

					_path = "file:///" + Application.persistentDataPath + "/" + _path;

#endif

#if PLATFORM_ANDROID

					_path = Application.persistentDataPath + "/" + _path;

#endif

#if PLATFORM_IOS
					_path = Application.persistentDataPath + "/" + _path;

#endif

					return true;

				}else{

					return false;
				}

			}else{

				return false;
			}
		}
	}
}
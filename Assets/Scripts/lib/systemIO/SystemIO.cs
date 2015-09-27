using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace xy3d.tstd.lib.systemIO{

	public class SystemIO{

		public static void SaveSerializeFile<T>(string _path, T _obj){

			FileInfo fi = new FileInfo(_path);

			DirectoryInfo dir = fi.Directory;

			if (!dir.Exists){

				dir.Create();
			}

			using (FileStream fs = new FileStream (_path, FileMode.OpenOrCreate)) {
				
				BinaryFormatter formatter = new BinaryFormatter ();
				
				formatter.Serialize (fs, _obj);
				
				fs.Close ();
			}
		}

		public static T LoadSerializeFile<T>(string _path){
			
			using(FileStream fs = new FileStream(_path,FileMode.Open)){
				
				BinaryFormatter formatter = new BinaryFormatter ();
				
				T data = (T)formatter.Deserialize (fs);
				
				fs.Close();

				return data;
			}
		}

		public static void SaveFile(string _path,byte[] _bytes){

			FileInfo fi = new FileInfo(_path);

			DirectoryInfo dir = fi.Directory;

			if (!dir.Exists){

				dir.Create();
			}

			using (FileStream fs = new FileStream (_path, FileMode.OpenOrCreate)) {

				fs.Write(_bytes,0,_bytes.Length);

				fs.Close();
			}
		}

		public static byte[] LoadFile(string _path){
			
			using (FileStream fs = new FileStream (_path, FileMode.Open)) {
				
				byte[] bytes = new byte[fs.Length];

				fs.Read(bytes,0,(int)fs.Length);

				return bytes;
			}
		}
	}
}
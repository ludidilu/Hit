using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace xy3d.tstd.lib.versionControl{

	[System.Serializable]
	public class VersionData{

		public int version;
		public Dictionary<string,int> dic;

		public VersionData(){

			dic = new Dictionary<string, int>();
		}
	}
}
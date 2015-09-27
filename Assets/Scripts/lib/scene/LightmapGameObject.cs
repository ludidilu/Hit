using UnityEngine;
using System.Collections;

namespace xy3d.tstd.lib.scene{

	public class LightmapGameObject : MonoBehaviour {

		public int lightmapIndex;
		public Vector4 lightmapScaleOffset;

		void Awake(){

			Renderer r = gameObject.GetComponent<Renderer>();

			r.lightmapScaleOffset = lightmapScaleOffset;

			r.lightmapIndex = lightmapIndex;
		}

		// Use this for initialization
		void Start () {
		

		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
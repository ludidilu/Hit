using UnityEngine;
using System.Collections;

namespace xy3d.tstd.lib.scene{

	public class SceneData : ScriptableObject {

		public Texture2D[] farTextures;
		public Texture2D[] nearTextures;

		public float fieldOfView;

		public Color ambientLight;
		public float ambientIntensity;

		public bool fog;
		public Color fogColor;
		public float fogStartDistance;
		public float fogEndDistance;
	}
}
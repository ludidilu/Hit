using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.publicTools;

namespace xy3d.tstd.lib.scene{

	public class Scene : MonoBehaviour {

		public SceneData sceneData;

		void Awake(){

			LightmapData[] lightmaps = new LightmapData[sceneData.farTextures.Length];

			for(int i = 0 ; i < lightmaps.Length ; i++){

				lightmaps[i] = new LightmapData();

				lightmaps[i].lightmapFar = sceneData.farTextures[i];
				lightmaps[i].lightmapNear = sceneData.nearTextures[i];
			}

			LightmapSettings.lightmaps = lightmaps;

			GameObject cameraGameObject = PublicTools.FindChildForce(gameObject,"CameraGameObject");

			if(Camera.main != null){

				Camera.main.fieldOfView = sceneData.fieldOfView;

				if(cameraGameObject != null){

					Camera.main.transform.position = cameraGameObject.transform.position;
					Camera.main.transform.rotation = cameraGameObject.transform.rotation;
				}
			}

			RenderSettings.ambientLight = sceneData.ambientLight;
			RenderSettings.ambientIntensity = sceneData.ambientIntensity;

			if(sceneData.fog){

				RenderSettings.fog = true;
				RenderSettings.fogMode = FogMode.Linear;
				RenderSettings.fogColor = sceneData.fogColor;
				RenderSettings.fogStartDistance = sceneData.fogStartDistance;
				RenderSettings.fogEndDistance = sceneData.fogEndDistance;
			}
		}

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		void OnDestroy(){
			
			LightmapSettings.lightmaps = new LightmapData[0];

			RenderSettings.ambientLight = Color.white;
			RenderSettings.ambientIntensity = 1;

			RenderSettings.fog = false;
		}
	}
}
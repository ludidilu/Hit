using UnityEngine;
using System.Collections;

public class UIParticleContainerFix : MonoBehaviour {

	Camera uiCamera;
	
	// Use this for initialization
	void Start () {

		uiCamera = gameObject.GetComponent<Canvas>().worldCamera;
	}
	
	// Update is called once per frame
	void Update () {

		ParticleSystemRenderer[] renderers = gameObject.GetComponentsInChildren<ParticleSystemRenderer>();

		if(renderers.Length > 0){
		
			float scale = transform.lossyScale.x / 0.015625f;

			if(scale > 1){

				scale = 1;
			}

			foreach(ParticleSystemRenderer r in renderers){

				r.material.SetFloat("_Scaling",scale);

				if(r.renderMode == ParticleSystemRenderMode.Billboard){

					r.material.SetVector("_Center", r.gameObject.transform.position);  
					r.material.SetMatrix("_Camera", uiCamera.worldToCameraMatrix);  
					r.material.SetMatrix("_CameraInv", uiCamera.worldToCameraMatrix.inverse);  
				}
			}
		}
	}
}

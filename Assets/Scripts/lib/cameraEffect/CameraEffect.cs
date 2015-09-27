using UnityEngine;
using System.Collections;

namespace xy3d.tstd.lib.cameraEffect{ 

	[RequireComponent(typeof(Camera))]
	public class CameraEffect : MonoBehaviour {

		private RenderTexture rt;
		public GameObject mainCamera;
		public Material material;

		// Use this for initialization
		void Awake () {
		
			rt = new RenderTexture(Screen.width,Screen.height,24);

			Camera camera = gameObject.GetComponent<Camera>();

			camera.cullingMask = 0;

			camera.clearFlags = CameraClearFlags.Nothing;
		}

		void OnEnable(){

			mainCamera.GetComponent<Camera>().targetTexture = rt;
		}

		void OnDisable(){

			mainCamera.GetComponent<Camera>().targetTexture = null;
		}

		// Update is called once per frame
		void OnRenderImage  (RenderTexture _s,RenderTexture _t) {

//			Graphics.SetRenderTarget(null);

//			GL.Clear(true,true,Color.red);

			GL.LoadPixelMatrix(0, 10, 10,0);

			Graphics.DrawTexture(new Rect(0,0,10,10),rt,material);
		}

		void Update () {
			
			if(Input.GetMouseButton(0)){
				
				Vector4 v = new Vector4(Input.mousePosition.x / Screen.width,Input.mousePosition.y / Screen.height,0,0);
				
				material.SetVector("_Center",v);
			}
		}

		void OnDestory(){

			rt.Release();
		}
	}
}
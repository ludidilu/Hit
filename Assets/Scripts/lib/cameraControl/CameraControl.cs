using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.gameObjectFactory;
using xy3d.tstd.lib.heroFactory;
using UnityEngine.UI;
using xy3d.tstd.lib;
using System;
using xy3d.tstd.lib.superFunction;


namespace xy3d.tstd.lib.cameraControl{ 

	public class CameraControl : MonoBehaviour {
		
		GameObject directLight;

		// Use this for initialization
		void Start () {


		
			transform.LookAt (Vector3.zero);

			distance = Vector3.Distance (transform.position, Vector3.zero);

			go = new GameObject ();

			go.name = "XiaoQCameraControl";

			go.transform.LookAt (transform);
		}

		private GameObject go;
		private Vector3 q;
		private float distance;

		// Update is called once per frame
		void Update () {

			bool change = false;

			if (Input.GetMouseButton (0)) {

				if(!Input.GetKey(KeyCode.LeftAlt)){

					q = go.transform.localRotation.eulerAngles;

#if PLATFORM_PC

					q.x += Input.GetAxis("Mouse Y") * 2f;
					q.y += Input.GetAxis("Mouse X") * 2f;

#else
					q.x += Input.GetTouch(0).deltaPosition.y;
					q.y += Input.GetTouch(0).deltaPosition.x;
#endif

					go.transform.eulerAngles = q;

					change = true;

				}else{

					if(directLight == null){

						directLight = GameObject.Find ("Directional Light");
					}

					q = directLight.transform.localRotation.eulerAngles;

#if PLATFORM_PC
					
					q.x += Input.GetAxis("Mouse Y") * 2f;
					q.y += Input.GetAxis("Mouse X") * 2f;
					
#else
					q.x += Input.GetTouch(0).deltaPosition.y;
					q.y += Input.GetTouch(0).deltaPosition.x;
#endif
					
					directLight.transform.eulerAngles = q;
				}
			}

			if (change || Input.mouseScrollDelta.y != 0) {

				distance -= Input.mouseScrollDelta.y * 0.1f;

				transform.position = go.transform.forward * distance;
				
				transform.LookAt (Vector3.zero);
			}
		}
	}
}

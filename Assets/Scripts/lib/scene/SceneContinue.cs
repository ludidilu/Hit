using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.superTween;
using xy3d.tstd.lib.systemIO;
using DamienG.Security.Cryptography;
using System;
using System.ComponentModel;
using xy3d.tstd.lib.assetManager;
using xy3d.tstd.lib.gameObjectFactory;
using xy3d.tstd.lib.superFunction;

namespace xy3d.tstd.lib.scene{

	public class SceneContinue : MonoBehaviour {

		Bounds[] bounds = new Bounds[3];

		private GameObject copy;

		public void Init(GameObject _go){

			copy = _go;
		}

		// Use this for initialization
		void Awake () {

			RefreshBounds();
		}

		private void CopySelf(){

			copy = GameObject.Instantiate<GameObject>(gameObject);

			copy.SetActive(false);
		}

		void RefreshBounds(){

			Renderer[] rs = gameObject.GetComponentsInChildren<Renderer>();

			Bounds b = rs[0].bounds;

			for(int i = 1 ; i < rs.Length ; i++){

				b.Encapsulate(rs[i].bounds);
			}

			bounds[0] = b;

			bounds[1] = new Bounds(new Vector3(b.center.x + b.size.x,b.center.y,b.center.z),b.size);
			
			bounds[2] = new Bounds(new Vector3(b.center.x - b.size.x,b.center.y,b.center.z),b.size);
		}

		void Update() {

			Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

			if(!GeometryUtility.TestPlanesAABB(planes, bounds[0])){

				if (GeometryUtility.TestPlanesAABB(planes, bounds[1])){
					
					gameObject.transform.Translate(gameObject.transform.right * bounds[0].size.x);

					copy.SetActive(false);

					RefreshBounds();
					
				}else if (GeometryUtility.TestPlanesAABB(planes, bounds[2])){
					
					gameObject.transform.Translate(gameObject.transform.right * -bounds[0].size.x);
					
					copy.SetActive(false);

					RefreshBounds();

				}else{

					Debug.LogError("error!!!");
				}

			}else{

				if (GeometryUtility.TestPlanesAABB(planes, bounds[1])){

					if(!copy.activeSelf){

						copy.transform.position = gameObject.transform.position;

						copy.transform.Translate(copy.transform.right * bounds[0].size.x);
						
						copy.SetActive(true);
					}
					
				}else if (GeometryUtility.TestPlanesAABB(planes, bounds[2])){
					
					if(!copy.activeSelf){
						
						copy.transform.position = gameObject.transform.position;
						
						copy.transform.Translate(copy.transform.right * -bounds[0].size.x);
						
						copy.SetActive(true);
					}

				}else{

					if(copy.activeSelf){

						copy.SetActive(false);
					}
				}
			}
		}

		void OnDestroy(){

			GameObject.Destroy(copy);
		}
	}
}

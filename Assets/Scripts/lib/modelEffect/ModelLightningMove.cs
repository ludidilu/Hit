using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace xy3d.tstd.lib.modelEffect{
	
	[AddComponentMenu("UI/Effects/LightningMove")]
	public class ModelLightningMove : MonoBehaviour {
		
		[SerializeField]
		private bool uMove;
		
		[SerializeField]
		private float moveSpeed = 1f;
		
		[SerializeField]
		private float strongFix = 1f;
		
		[SerializeField]
		private float delay;

		private float fix;
		
		private bool isMoving;
		private float waitStartTime;

		private Material material;
		
		// Use this for initialization
		void Start () {

			fix = -0.5f;

			material = gameObject.GetComponent<Renderer>().material;

			float uScale = 1;
			float vScale = 1;

			if(uMove){

				uScale = 0.5f;
				material.SetFloat("_UFix",fix);
				
			}else{

				vScale = 0.5f;
				material.SetFloat("_VFix",fix);
			}

			material.SetFloat("_UScale",uScale);
			material.SetFloat("_VScale",vScale);

			material.SetFloat("_StrongFix",strongFix);
		}
		
		// Update is called once per frame
		void Update () {
			
			if(isMoving){
				
				fix = fix + Time.deltaTime * moveSpeed;

				if(fix > 0.5f){

					fix = 0.5f;
					
					isMoving = false;
					
					waitStartTime = Time.time;
				}

				if(uMove){
					
					material.SetFloat("_UFix",fix);
					
				}else{
					
					material.SetFloat("_VFix",fix);
				}

			}else{
				
				if(Time.time - waitStartTime > delay){
					
					isMoving = true;
					
					fix = -0.5f;		

					if(uMove){
						
						material.SetFloat("_UFix",fix);
						
					}else{
						
						material.SetFloat("_VFix",fix);
					}
				}
			}
			
//			Debug.Log("fix:" + fix + "  isMoving" + isMoving);
		}
	}
}

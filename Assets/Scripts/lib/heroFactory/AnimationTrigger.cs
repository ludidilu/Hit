using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.superFunction;

namespace xy3d.tstd.lib.heroFactory{

	//这个组件是绑在animation上的  例如粒子的animation上面
	public class AnimationTrigger : MonoBehaviour {

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		void Trigger(string _eventName){

			SuperEvent superEvent = new SuperEvent (_eventName);
			
			SuperFunction.Instance.DispatchEvent (gameObject, superEvent);
		}
	}
}
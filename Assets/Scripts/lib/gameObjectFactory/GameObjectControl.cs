using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.superFunction;

namespace xy3d.tstd.lib.gameObjectFactory{

	public class GameObjectControl : MonoBehaviour {

		public GameObjectFactoryUnit unit;

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public void AddUseNum(){

			unit.AddUseNum ();
		}

		public void DelUseNum(){

			unit.DelUseNum ();
		}


		void OnDestroy(){

			DelUseNum ();

			SuperFunction.Instance.RemoveEventListener(gameObject);
		}
	}
}
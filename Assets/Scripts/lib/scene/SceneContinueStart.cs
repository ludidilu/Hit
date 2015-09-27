using UnityEngine;
using System.Collections;
using xy3d.tstd.lib.superTween;
using System;
using xy3d.tstd.lib.gameObjectFactory;

namespace xy3d.tstd.lib.scene{

	public class SceneContinueStart : MonoBehaviour {

		// Use this for initialization
		void Awake () {

			SuperTween.Instance.DelayCall(0,GetCallBack(gameObject));

			GameObject.Destroy(this);
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		static Action GetCallBack(GameObject _go){

			Action callBack = delegate() {

				GameObject go = GameObject.Instantiate(_go);

				GameObjectControl control = _go.GetComponent<GameObjectControl>();

				if(control != null){

					GameObjectControl newControl = go.GetComponent<GameObjectControl>();

					newControl.unit = control.unit;

					newControl.AddUseNum();
				}

				SceneContinue sceneContinue = _go.AddComponent<SceneContinue>();

				sceneContinue.Init(go);
			};

			return callBack;
		}
	}
}

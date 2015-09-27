using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace xy3d.tstd.lib.superTween{

	public class SuperTweenScript : MonoBehaviour {

		private Dictionary<int,SuperTweenUnit> dic;
		private int index;

		// Use this for initialization
		void Awake () {
		
			dic = new Dictionary<int,SuperTweenUnit>();
		}

		public int To(float _startValue,float _endValue,float _time,Action<float> _delegate,Action _endCallBack){

			int result = GetIndex();

			SuperTweenUnit unit = new SuperTweenUnit(result,_startValue,_endValue,_time,_delegate,_endCallBack);

			dic.Add(result,unit);

			return result;
		}

		public void Remove(int _index){
			
			if(dic.ContainsKey(_index)){

				dic.Remove(_index);
			}
		}

		public int DelayCall(float _time,Action _endCallBack){

			int result = GetIndex();

			SuperTweenUnit unit = new SuperTweenUnit(result,0,0,_time,null,_endCallBack);

			dic.Add(result,unit);

			return result;
		}

		public int NextFrameCall(Action _endCallBack){

			int result = GetIndex();
			
			SuperTweenUnit unit = new SuperTweenUnit(result,0,0,0,null,_endCallBack);
			
			dic.Add(result,unit);
			
			return result;
		}

		// Update is called once per frame
		void Update () {
		
			float nowTime = Time.time;

			if(dic.Count > 0){

				List<SuperTweenUnit> endList = new List<SuperTweenUnit>();

				foreach(SuperTweenUnit unit in dic.Values){

					if(nowTime > unit.startTime + unit.time){

						if(unit.dele != null){

							unit.dele(unit.endValue);
						}

						endList.Add(unit);

					}else if(unit.dele != null){

						float value = unit.startValue + (unit.endValue - unit.startValue) * (nowTime - unit.startTime) / unit.time;

						unit.dele(value);
					}
				}

				foreach(SuperTweenUnit unit in endList){
					
					if(unit.endCallBack != null){
						
						unit.endCallBack();
					}

					dic.Remove(unit.index);
				}
			}
		}

		private int GetIndex(){

			int result = index;

			index++;

			return result;
		}
	}
}
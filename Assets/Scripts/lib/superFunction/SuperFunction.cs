using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace xy3d.tstd.lib.superFunction{

	public class SuperFunction{

		private static SuperFunction _Instance;

		public static SuperFunction Instance{

			get{

				if(_Instance == null){

					_Instance = new SuperFunction();
				}

				return _Instance;
			}
		}

		private Dictionary<int,SuperFunctionUnit> dic;
		private Dictionary<GameObject,Dictionary<string,List<SuperFunctionUnit>>> dic2;
		private int dispatchEventIndex = 0;
		private List<Action> delegateList = new List<Action>();
		private int index = 0;

		public SuperFunction(){

			dic = new Dictionary<int, SuperFunctionUnit>();
			dic2 = new Dictionary<GameObject,Dictionary<string,List<SuperFunctionUnit>>>();
		}

		public int AddEventListener(GameObject _target,string _eventName,Action<SuperEvent> _callBack){

			int result = GetIndex();

			if (dispatchEventIndex > 0) {

				Action del = delegate() {

					AddEventListener(_target,_eventName,_callBack,result);
				};
				
				delegateList.Add(del);

				return result;

			}else{

				return AddEventListener(_target,_eventName,_callBack,result);
			}
		}

		private int AddEventListener(GameObject _target,string _eventName,Action<SuperEvent> _callBack,int _index){

			SuperFunctionUnit unit = new SuperFunctionUnit(_target,_eventName,_callBack,_index);

			dic.Add(_index,unit);

			Dictionary<string,List<SuperFunctionUnit>> tmpDic;

			if(dic2.ContainsKey(_target)){

				tmpDic = dic2[_target];

			}else{

				tmpDic = new Dictionary<string,List<SuperFunctionUnit>>();

				dic2.Add(_target,tmpDic);
			}

			List<SuperFunctionUnit> tmpList;

			if(tmpDic.ContainsKey(_eventName)){

				tmpList = tmpDic[_eventName];

			}else{

				tmpList = new List<SuperFunctionUnit>();

				tmpDic.Add(_eventName,tmpList);
			}

			tmpList.Add(unit);

			return _index;
		}

		public void RemoveEventListener(int _index){

			if (dispatchEventIndex > 0) {
				
				Action del = delegate() {
					
					RemoveEventListener(_index);
				};
				
				delegateList.Add(del);
				
				return;
			}

			if(dic.ContainsKey(_index)){

				SuperFunctionUnit unit = dic[_index];

				dic.Remove(_index);

				Dictionary<string,List<SuperFunctionUnit>> tmpDic = dic2[unit.target];

				List<SuperFunctionUnit> tmpList = tmpDic[unit.eventName];

				tmpList.Remove(unit);

				if(tmpList.Count == 0){

					tmpDic.Remove(unit.eventName);

					if(tmpDic.Count == 0){

						dic2.Remove(unit.target);
					}
				}
			}
		}

		public void RemoveEventListener(GameObject _target){
			
			if (dispatchEventIndex > 0) {
				
				Action del = delegate() {
					
					RemoveEventListener(_target);
				};
				
				delegateList.Add(del);
				
				return;
			}
			
			if(dic2.ContainsKey(_target)){

				Dictionary<string,List<SuperFunctionUnit>> tmpDic = dic2[_target];

				dic2.Remove(_target);

				foreach(List<SuperFunctionUnit> list in tmpDic.Values){

					foreach(SuperFunctionUnit unit in list){

						dic.Remove(unit.index);
					}
				}
			}
		}

		public void RemoveEventListener(GameObject _target,string _eventName){
			
			if (dispatchEventIndex > 0) {
				
				Action del = delegate() {
					
					RemoveEventListener(_target,_eventName);
				};
				
				delegateList.Add(del);
				
				return;
			}
			
			if(dic2.ContainsKey(_target)){
				
				Dictionary<string,List<SuperFunctionUnit>> tmpDic = dic2[_target];

				if(tmpDic.ContainsKey(_eventName)){

					List<SuperFunctionUnit> list = tmpDic[_eventName];

					foreach(SuperFunctionUnit unit in list){
						
						dic.Remove(unit.index);
					}

					tmpDic.Remove(_eventName);

					if(tmpDic.Count == 0){

						dic2.Remove(_target);
					}
				}
			}
		}

		public void RemoveEventListener(GameObject _target,string _eventName,Action<SuperEvent> _callBack){
			
			if (dispatchEventIndex > 0) {
				
				Action del = delegate() {
					
					RemoveEventListener(_target,_eventName,_callBack);
				};
				
				delegateList.Add(del);
				
				return;
			}
			
			if(dic2.ContainsKey(_target)){
				
				Dictionary<string,List<SuperFunctionUnit>> tmpDic = dic2[_target];
				
				if(tmpDic.ContainsKey(_eventName)){
					
					List<SuperFunctionUnit> list = tmpDic[_eventName];

					for(int i = 0 ; i < list.Count ; i++){

						if(list[i].callBack == _callBack){

							dic.Remove(list[i].index);

							list.RemoveAt(i);

							break;
						}
					}

					if(list.Count == 0){

						tmpDic.Remove(_eventName);

						if(tmpDic.Count == 0){
							
							dic2.Remove(_target);
						}
					}
				}
			}
		}

		public void DispatchEvent(GameObject _target,SuperEvent _event){

			dispatchEventIndex++;
			
//			_event.target = _target;

			if (dic2.ContainsKey (_target)) {
				
				Dictionary<string,List<SuperFunctionUnit>> tmpDic = dic2[_target];
				
				if(tmpDic.ContainsKey(_event.eventName)){
					
					List<SuperFunctionUnit> tmpList = tmpDic[_event.eventName];
					
					foreach(SuperFunctionUnit unit in tmpList){

//						_event.index = unit.index;

						SuperEvent tmpEvent = new SuperEvent(_event.eventName);

						tmpEvent.target = _target;

						tmpEvent.data = _event.data;

						tmpEvent.index = unit.index;

						unit.callBack(tmpEvent);
					}
				}
			}
			
			dispatchEventIndex--;
			
			if (dispatchEventIndex == 0 && delegateList.Count > 0) {
				
				foreach (Action del in delegateList) {
					
					del ();
				}
				
				delegateList.Clear ();
			}
		}

		private int GetIndex(){

			int result = index;

			index++;

			return result;
		}
	}
}
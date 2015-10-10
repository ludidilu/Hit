using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

namespace xy3d.tstd.lib.superTween{

	public class SuperTweenUnit{

		public int index;

		public float startValue;
		public float endValue;
		public float time;
		public float startTime;

		public Action endCallBack;

		public Action<float> dele;

		public void Init(int _index,float _startValue,float _endValue,float _time,Action<float> _delegate,Action _endCallBack){

			index = _index;

			startValue = _startValue;
			endValue = _endValue;
			time = _time;
			dele = _delegate;

			endCallBack = _endCallBack;

			startTime = Time.time;
		}
	}
}
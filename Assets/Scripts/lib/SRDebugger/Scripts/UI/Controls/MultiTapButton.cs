using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SRDebugger.UI.Controls
{

	public class MultiTapButton : Button
	{

		public float ResetTime = 0.5f;

		public int RequiredTapCount = 3;

		private int _tapCount;

		private float _lastTap;

		public override void OnPointerClick(PointerEventData eventData)
		{

			if (Time.unscaledTime - _lastTap > ResetTime)
				_tapCount = 0;

			_lastTap = Time.unscaledTime;
			_tapCount++;

			if (_tapCount == RequiredTapCount) {

				base.OnPointerClick(eventData);
				_tapCount = 0;

			}

		}

	}

}

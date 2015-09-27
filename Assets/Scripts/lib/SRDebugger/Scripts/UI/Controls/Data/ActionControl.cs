using System;
using SRF;
using SRF.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace SRDebugger.UI.Controls.Data
{

	public class ActionControl : OptionsControlBase
	{

		[RequiredField]
		public Button Button;

		[RequiredField]
		public Text Title;

		public SRF.Helpers.MethodReference Method { get { return _method; } }

		private SRF.Helpers.MethodReference _method;

		protected override void Start()
		{
			base.Start();
			Button.onClick.AddListener(ButtonOnClick);
		}

		private void ButtonOnClick()
		{

			if (_method == null) {
				Debug.LogWarning("[SRDebugger.Options] No method set for action control", this);
				return;
			}

			try {

				_method.Invoke(null);

			} catch (Exception e) {
				
				Debug.LogError("[SRDebugger] Exception thrown while executing action.");
				Debug.LogException(e);

			}

		}

		public void SetMethod(SRF.Helpers.MethodReference method)
		{

			_method = method;
			Title.text = method.MethodName;

		}

	}

}
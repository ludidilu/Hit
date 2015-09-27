//#define SR_CONSOLE_DEBUG

using System;
using SRDebugger.Internal;
using SRDebugger.Services;
using SRDebugger.UI.Controls;
using SRF;
using UnityEngine;
using UnityEngine.UI;

namespace SRDebugger.UI.Tabs
{

	public class ProfilerTabController : SRMonoBehaviourEx
	{


		[RequiredField]
		public Toggle PinToggle;

		private bool _isDirty;

		protected override void Start()
		{

			base.Start();

			PinToggle.onValueChanged.AddListener(PinToggleValueChanged);
			Refresh();

		}

		private void PinToggleValueChanged(bool isOn)
		{

			SRDebug.Instance.IsProfilerDocked = isOn;

		}

		protected override void OnEnable()
		{

			base.OnEnable();
			_isDirty = true;

		}

		protected override void Update()
		{

			base.Update();

			if (_isDirty)
				Refresh();

		}

		
		private void Refresh()
		{

			PinToggle.isOn = SRDebug.Instance.IsProfilerDocked;
			_isDirty = false;

		}

	}

}

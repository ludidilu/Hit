using SRDebugger.Internal;
using SRF;
using UnityEngine;
using UnityEngine.UI;

namespace SRDebugger.UI.Controls
{

	public class ProfilerEnableControl : SRMonoBehaviourEx
	{

		[RequiredField] public Button EnableButton;

		[RequiredField] public Text Text;

		[RequiredField] public Text ButtonText;

		private bool _previousState;

		protected override void Start()
		{
			base.Start();

			if (!UnityEngine.Profiler.supported) {

				Text.text = SRDebugStrings.Current.Profiler_NotSupported;
				EnableButton.gameObject.SetActive(false);
				enabled = false;
				return;

			}

			if (!Application.HasProLicense()) {
				Text.text = SRDebugStrings.Current.Profiler_NoProInfo;
				EnableButton.gameObject.SetActive(false);
				enabled = false;
				return;
			}

			UpdateLabels();
			
		}

		protected void UpdateLabels()
		{

			if (!UnityEngine.Profiler.enabled) {
				Text.text = SRDebugStrings.Current.Profiler_EnableProfilerInfo;
				ButtonText.text = "Enable";
			} else {
				Text.text = SRDebugStrings.Current.Profiler_DisableProfilerInfo;
				ButtonText.text = "Disable";
			}

			_previousState = UnityEngine.Profiler.enabled;

		}

		protected override void Update()
		{

			base.Update();

			if (UnityEngine.Profiler.enabled != _previousState) {
				UpdateLabels();
			}

		}

		public void ToggleProfiler()
		{

			Debug.Log("Toggle Profiler");
			UnityEngine.Profiler.enabled = !UnityEngine.Profiler.enabled;

		}

	}

}
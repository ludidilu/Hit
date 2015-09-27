using System;
using System.Collections.Generic;
using System.Linq;
using SRDebugger.Internal;
using SRDebugger.UI.Controls;
using SRF;
using SRF.Service;
using UnityEngine;

namespace SRDebugger.Services.Implementation
{

	[Service(typeof (IPinEntryService))]
	public class PinEntryServiceImpl : SRServiceBase<IPinEntryService>, IPinEntryService
	{

		public bool IsShowingKeypad
		{
			get { return _isVisible; }
		}

		public void ShowPinEntry(IList<int> requiredPin, string message, PinEntryCompleteCallback callback, bool blockInput = true,
			bool allowCancel = true)
		{

			if(_isVisible)
				throw new InvalidOperationException("Pin entry is already in progress");

			if(_pinControl == null)
				Load();

			if (_pinControl == null) {

				Debug.LogWarning("[PinEntry] Pin entry failed loading, executing callback with fail result");
				callback(false);
				return;

			}

			_pinControl.Clear();
			_pinControl.PromptText.text = message;

			_pinControl.CanCancel = allowCancel;

			_callback = callback;

			_requiredPin.Clear();
			_requiredPin.AddRange(requiredPin);

			_pinControl.Show();

			_isVisible = true;

			Internal.SRDebuggerUtil.EnsureEventSystemExists();

		}

		private bool _isVisible;
		private PinEntryControl _pinControl;

		private PinEntryCompleteCallback _callback;
		private List<int> _requiredPin = new List<int>(4);

		protected override void Awake()
		{

			base.Awake();

			CachedTransform.SetParent(Hierarchy.Get("SRDebugger"));

		}

		private void Load()
		{

			var prefab = Resources.Load<PinEntryControl>(SRDebugPaths.PinEntryPrefabPath);

			if (prefab == null) {

				Debug.LogError("[PinEntry] Unable to load pin entry prefab");
				return;

			}

			_pinControl = SRInstantiate.Instantiate(prefab);
			_pinControl.CachedTransform.SetParent(CachedTransform, false);

			_pinControl.Hide();

			_pinControl.Complete += PinControlOnComplete;

		}

		private void PinControlOnComplete(IList<int> result, bool didCancel)
		{

			var isValid = _requiredPin.SequenceEqual(result);

			if (!didCancel && !isValid) {

				_pinControl.Clear();
				_pinControl.PlayInvalidCodeAnimation();

				return;

			}

			_isVisible = false;
			_pinControl.Hide();

			if (didCancel) {

				_callback(false);
				return;

			}

			_callback(isValid);

		}

	}

}
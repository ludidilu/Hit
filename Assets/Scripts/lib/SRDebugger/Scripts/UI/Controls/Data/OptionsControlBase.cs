﻿using SRF;
using UnityEngine.UI;

namespace SRDebugger.UI.Controls
{

	public abstract class OptionsControlBase : SRMonoBehaviourEx
	{

		[RequiredField]
		public Toggle SelectionModeToggle;

		public bool SelectionModeEnabled
		{

			get { return _selectionModeEnabled; }

			set
			{

				if (value == _selectionModeEnabled)
					return;

				_selectionModeEnabled = value;

				SelectionModeToggle.gameObject.SetActive(_selectionModeEnabled);

				if (SelectionModeToggle.graphic != null)
					SelectionModeToggle.graphic.CrossFadeAlpha(IsSelected ? _selectionModeEnabled ? 1.0f : 0.2f : 0f, 0, true);

			}

		}

		public bool IsSelected
		{
			get { return SelectionModeToggle.isOn; }
			set
			{

				SelectionModeToggle.isOn = value;

				if (SelectionModeToggle.graphic != null)
					SelectionModeToggle.graphic.CrossFadeAlpha(value ? _selectionModeEnabled ? 1.0f : 0.2f : 0f, 0, true);

			}
		}

		private bool _selectionModeEnabled;

		protected override void Awake()
		{

			base.Awake();

			IsSelected = false;
			SelectionModeToggle.gameObject.SetActive(false);

		}

		protected override void OnEnable()
		{

			base.OnEnable();

			// Reapply selection indicator alpha (is reset when disabled / reenabled)
			if (SelectionModeToggle.graphic != null)
				SelectionModeToggle.graphic.CrossFadeAlpha(IsSelected ? _selectionModeEnabled ? 1.0f : 0.2f : 0f, 0, true);

		}

		public virtual void Refresh() { }

	}

}
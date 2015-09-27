using System;
using System.Collections.Generic;
using System.Linq;
using SRDebugger.Internal;
using SRDebugger.UI.Controls;
using SRDebugger.UI.Other;
using SRF;
using SRF.Service;
using UnityEngine;

namespace SRDebugger.Services.Implementation
{

	[Service(typeof(IDockConsoleService))]
	public class DockConsoleServiceImpl : SRServiceBase<IDockConsoleService>, IDockConsoleService
	{

		public bool IsVisible
		{

			get { return _isVisible; }

			set
			{

				if (value == _isVisible)
					return;

				_isVisible = value;

				if (_consoleRoot == null && value)
					Load();
				else
					_consoleRoot.CachedGameObject.SetActive(value);

			}

		}

		public bool IsExpanded
		{

			get { return _isExpanded; }

			set
			{

				if (value == _isExpanded)
					return;

				_isExpanded = value;

				if (_consoleRoot == null && value)
					Load();
				else
					_consoleRoot.SetDropdownVisibility(value);

			}

		}

		private DockConsoleRoot _consoleRoot;
		private bool _isVisible = false;
		private bool _isExpanded = true;

		protected override void Awake()
		{

			base.Awake();

			CachedTransform.SetParent(Hierarchy.Get("SRDebugger"));

		}

		private void SetIsVisible(bool isVisible)
		{
			
		}

		private void Load()
		{

			var prefab = Resources.Load<DockConsoleRoot>(SRDebugPaths.DockConsolePrefabPath);

			if (prefab == null) {

				Debug.LogError("[PinEntry] Unable to load pin entry prefab");
				return;

			}

			_consoleRoot = SRInstantiate.Instantiate(prefab);
			_consoleRoot.CachedTransform.SetParent(CachedTransform, false);

			if(!_isVisible)
				_consoleRoot.CachedGameObject.SetActive(false);

			if (!_isExpanded)
				_consoleRoot.SetDropdownVisibility(false);

		}

	}

}
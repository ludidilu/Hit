using System;
using System.Linq;
using SRDebugger.UI.Other;
using SRF;
using UnityEngine;

namespace SRDebugger.Scripts
{

	public class DebuggerTabController : SRMonoBehaviourEx
	{

		public DefaultTabs? ActiveTab
		{
			get
			{

				var key = TabController.ActiveTab.Key;

				if (string.IsNullOrEmpty(key))
					return null;

				var t = Enum.Parse(typeof (DefaultTabs), key);

				if (!Enum.IsDefined(typeof (DefaultTabs), t))
					return null;

				return (DefaultTabs) t;

			}
		}

		[RequiredField]
		public SRTabController TabController;

		public SRTab AboutTab;

		private SRTab _aboutTabInstance;

		private DefaultTabs? _activeTab = null;

		private bool _hasStarted = false;

		protected override void Start()
		{

			base.Start();

			_hasStarted = true;

			// Loads all available tabs from resources
			var tabs = Resources.LoadAll<SRTab>("SRDebugger/UI/Prefabs/Tabs");

			foreach (var srTab in tabs) {

				var enabler = srTab.GetComponent(typeof(IEnableTab)) as IEnableTab;

				if (enabler != null && !enabler.IsEnabled)
					continue;

				var tabValue = Enum.Parse(typeof(DefaultTabs), srTab.Key);

				if(Enum.IsDefined(typeof(DefaultTabs), tabValue) && Settings.Instance.DisabledTabs.Contains((DefaultTabs)tabValue))
					continue;

				TabController.AddTab(SRInstantiate.Instantiate(srTab));

			}

			// Add about tab (has no button, accessed via "Stompy" logo
			if (AboutTab != null) {

				_aboutTabInstance = SRInstantiate.Instantiate(AboutTab);
				TabController.AddTab(_aboutTabInstance, false);

			}

			// Open active tab (set before panel loaded), or default tab from settings
			var defaultTab = _activeTab ?? Settings.Instance.DefaultTab;

			if (!OpenTab(defaultTab)) {

				TabController.ActiveTab = TabController.Tabs.FirstOrDefault();

			}

		}

		public bool OpenTab(DefaultTabs tab)
		{

			if (!_hasStarted) {
				_activeTab = tab;
				return true;
			}

			var tabName = tab.ToString();

			foreach (var t in TabController.Tabs) {

				if (t.Key == tabName) {

					TabController.ActiveTab = t;
					return true;

				}

			}

			return false;

		}

		public void ShowAboutTab()
		{

			if (_aboutTabInstance != null)
				TabController.ActiveTab = _aboutTabInstance;

		}


	}

}
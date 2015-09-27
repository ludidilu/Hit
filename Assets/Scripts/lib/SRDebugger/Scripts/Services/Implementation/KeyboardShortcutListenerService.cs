using System.Collections.Generic;
using SRDebugger.Internal;
using SRF;
using SRF.Service;
using UnityEngine;

namespace SRDebugger.Services.Implementation
{

	[Service(typeof (KeyboardShortcutListenerService))]
	public class KeyboardShortcutListenerService : SRServiceBase<KeyboardShortcutListenerService>
	{

		private Dictionary<KeyCode, Settings.KeyboardShortcut> _shortcutsCache; 

		protected override void Awake()
		{

			base.Awake();

			CachedTransform.SetParent(Hierarchy.Get("SRDebugger"));

			_shortcutsCache = new Dictionary<KeyCode, Settings.KeyboardShortcut>();

			foreach (var shortcut in Settings.Instance.KeyboardShortcuts) {

				_shortcutsCache.Add(shortcut.Key, shortcut);

			}

		}

		private void ToggleTab(DefaultTabs t)
		{

			var activeTab = Service.Panel.ActiveTab;

			if (Service.Panel.IsVisible && activeTab.HasValue && activeTab.Value == t) {
				SRDebug.Instance.HideDebugPanel();
			} else {
				SRDebug.Instance.ShowDebugPanel(t);
			}

		}

		private void ExecuteShortcut(Settings.KeyboardShortcut shortcut)
		{

			switch (shortcut.Action) {

				case Settings.ShortcutActions.OpenSystemInfoTab:

					ToggleTab(DefaultTabs.SystemInformation);

					break;

				case Settings.ShortcutActions.OpenConsoleTab:

					ToggleTab(DefaultTabs.Console);

					break;

				case Settings.ShortcutActions.OpenOptionsTab:

					ToggleTab(DefaultTabs.Options);

					break;

				case Settings.ShortcutActions.OpenProfilerTab:

					ToggleTab(DefaultTabs.Profiler);

					break;

				case Settings.ShortcutActions.OpenBugReporterTab:

					ToggleTab(DefaultTabs.BugReporter);

					break;

				case Settings.ShortcutActions.ClosePanel:

					SRDebug.Instance.HideDebugPanel();

					break;

				case Settings.ShortcutActions.OpenPanel:

					SRDebug.Instance.ShowDebugPanel();

					break;

				case Settings.ShortcutActions.TogglePanel:

					if (SRDebug.Instance.IsDebugPanelVisible) {

						SRDebug.Instance.HideDebugPanel();

					} else {

						SRDebug.Instance.ShowDebugPanel();

					}

					break;

				case Settings.ShortcutActions.ShowBugReportPopover:

					SRDebug.Instance.ShowBugReportSheet();

					break;

				case Settings.ShortcutActions.ToggleDockedConsole:

					SRDebug.Instance.DockConsole.IsVisible = !SRDebug.Instance.DockConsole.IsVisible;

					break;

				case Settings.ShortcutActions.ToggledDockedProfiler:

					SRDebug.Instance.IsProfilerDocked = !SRDebug.Instance.IsProfilerDocked;

					break;


				default:

					Debug.LogWarning("[SRDebugger] Unhandled keyboard shortcut: " + shortcut.Action);

					break;

			}

		}

		protected override void Update()
		{

			base.Update();

			if (Settings.Instance.KeyboardEscapeClose && Input.GetKeyDown(KeyCode.Escape) && Service.Panel.IsVisible)
				SRDebug.Instance.HideDebugPanel();


			if (Settings.Instance.KeyboardModifierControl &&
			    (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))) {
				return;
			}

			if (Settings.Instance.KeyboardModifierShift &&
			    (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))) {
				return;
			}

			if (Settings.Instance.KeyboardModifierAlt &&
			    (!Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt))) {
				return;
			}

			foreach (var kv in _shortcutsCache) {

				if (Input.GetKeyDown(kv.Key)) {

					ExecuteShortcut(kv.Value);
					break;

				}

			}

		}

	}

}
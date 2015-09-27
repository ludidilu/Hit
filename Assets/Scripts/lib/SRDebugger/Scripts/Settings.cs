using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using SRF;
using UnityEngine;

namespace SRDebugger
{

	public enum DefaultTabs
	{

		SystemInformation = 0,
		Options = 1,
		Console = 2,
		Profiler = 3,
		BugReporter = 4

	}

	public class Settings : ScriptableObject
	{

		public enum TriggerPositions
		{

			TopLeft, TopRight,
			BottomLeft, BottomRight

		}

		public enum TriggerEnableModes
		{

			Always,
			MobileOnly,
			Off

		}

		public enum TriggerBehaviours
		{

			TripleTap,
			TapAndHold

		}

		public enum ShortcutActions
		{

			None = 0,

			OpenSystemInfoTab = 1,
			OpenConsoleTab = 2,
			OpenOptionsTab = 3,
			OpenProfilerTab = 4,
			OpenBugReporterTab = 5,

			ClosePanel = 6,
			OpenPanel = 7,
			TogglePanel = 8,

			ShowBugReportPopover = 9,
			ToggleDockedConsole = 10,
			ToggledDockedProfiler = 11,

		}

		[Serializable]
		public sealed class KeyboardShortcut
		{

			[SerializeField]
			public KeyCode Key;

			[SerializeField]
			public ShortcutActions Action;

		}

		#region Settings

		public bool IsEnabled
		{
			get { return _isEnabled; }
		}

		public bool AutoLoad { get { return _autoLoad; } }

		public DefaultTabs DefaultTab
		{
			get { return _defaultTab; }
		}

		/// <summary>
		/// Position for the triple-tap button
		/// </summary>
		public TriggerPositions TriggerPosition
		{
			get { return _triggerPosition; }
		}

		/// <summary>
		/// Enable the triple-tap button.
		/// </summary>
		public TriggerEnableModes EnableTrigger
		{
			get { return _triggerEnableMode; }
		}

		/// <summary>
		/// Enable the triple-tap button.
		/// </summary>
		public TriggerBehaviours TriggerBehaviour
		{
			get { return _triggerBehaviour; }
		}

		public bool EnableKeyboardShortcuts
		{
			get
			{
				return _enableKeyboardShortcuts;
			}
		}

		public IList<KeyboardShortcut> KeyboardShortcuts
		{
			get { return _keyboardShortcuts; }
		}

		public bool KeyboardModifierControl { get { return _keyboardModifierControl; } }
		public bool KeyboardModifierAlt { get { return _keyboardModifierAlt; } }
		public bool KeyboardModifierShift { get { return _keyboardModifierShift; } }

		public bool KeyboardEscapeClose { get { return _keyboardEscapeClose; } }

		public bool EnableBackgroundTransparency
		{
			get { return _enableBackgroundTransparency; }
		}

		public bool RequireCode
		{
			get { return _requireEntryCode; }
		}

		public bool RequireEntryCodeEveryTime
		{
			get { return _requireEntryCodeEveryTime; }
		}

		public IList<int> EntryCode { get { return new ReadOnlyCollection<int>(_entryCode); } }

		public bool UseDebugCamera { get { return _useDebugCamera; } }

		public int DebugLayer { get { return _debugLayer; } }

		public float DebugCameraDepth { get { return _debugCameraDepth; } }

		public bool CollapseDuplicateLogEntries { get { return _collapseDuplicateLogEntries; } }

		public bool RichTextInConsole { get { return _richTextInConsole; } }

		public string ApiKey
		{
			get { return _apiKey; }
#if UNITY_EDITOR
			set
			{
				_apiKey = value;
				UnityEditor.EditorUtility.SetDirty(this);
			}
#endif
		}

		public bool EnableBugReporter
		{
			get
			{
				return _enableBugReporter;
			}
		}

		public IList<DefaultTabs> DisabledTabs { get { return Array.AsReadOnly(_disabledTabs); } }

		#endregion

		#region Serialization

		[SerializeField]
		private bool _isEnabled = true;

		[SerializeField]
		private bool _autoLoad = true;

		[SerializeField]
		private DefaultTabs _defaultTab = DefaultTabs.SystemInformation;

		[SerializeField]
		private TriggerPositions _triggerPosition = TriggerPositions.TopLeft;

		[SerializeField]
		private TriggerEnableModes _triggerEnableMode = TriggerEnableModes.Always;

		[SerializeField]
		private TriggerBehaviours _triggerBehaviour = TriggerBehaviours.TripleTap;

		[SerializeField]
		private bool _enableKeyboardShortcuts = true;

		[SerializeField]
		private KeyboardShortcut[] _keyboardShortcuts = GetDefaultKeyboardShortcuts();

		[SerializeField] 
		private bool _keyboardModifierControl = true;

		[SerializeField] 
		private bool _keyboardModifierAlt = false;

		[SerializeField] 
		private bool _keyboardModifierShift = true;

		[SerializeField] 
		private bool _keyboardEscapeClose = true;

		[SerializeField]
		private bool _enableBackgroundTransparency = true;

		[SerializeField]
		private bool _collapseDuplicateLogEntries = false;

		[SerializeField]
		private bool _richTextInConsole = false;

		[SerializeField]
		private bool _requireEntryCode = false;

		[SerializeField]
		private bool _requireEntryCodeEveryTime = false;

		[SerializeField]
		private int[] _entryCode = { 0, 0, 0, 0 };

		[SerializeField]
		private bool _useDebugCamera = false;

		[SerializeField] 
		private int _debugLayer = 5;

		[SerializeField]
		[Range(-100, 100)]
		private float _debugCameraDepth = 100f;

		[SerializeField]
		private string _apiKey = "";

		[SerializeField] 
		private bool _enableBugReporter = false;

		[SerializeField]
		private DefaultTabs[] _disabledTabs = new DefaultTabs[] {
			
		};

		#endregion

		private const string ResourcesPath = "/usr/Resources/SRDebugger";
		private const string ResourcesName = "Settings";

		public static Settings Instance
		{
			get
			{
				if (_instance == null) {

					_instance = GetOrCreateInstance();

				}

				return _instance;

			}
		}

		private static Settings _instance;

		#region Saving/Loading

		private static Settings GetOrCreateInstance()
		{

			var instance = Resources.Load<Settings>("SRDebugger/" + ResourcesName);

			if (instance == null) {

				// Create instance
				instance = CreateInstance<Settings>();

#if UNITY_EDITOR

				// Get resources folder path
				var resourcesPath = GetResourcesPath();

				if (resourcesPath == null) {
					Debug.LogError("[SRDebugger] Error finding Resources path. Please make sure SRDebugger folder is intact");
					return instance;
				}

				Debug.Log("[SRDebugger] Creating settings asset at {0}/{1}".Fmt(resourcesPath, ResourcesName));

				// Create directory if it doesn't exist
				Directory.CreateDirectory(resourcesPath);

				// Save instance if in editor
				UnityEditor.AssetDatabase.CreateAsset(instance, resourcesPath + "/" + ResourcesName + ".asset");

#endif

			}

			return instance;

		}

#if UNITY_EDITOR

		private static string GetResourcesPath()
		{

			try {

				return Internal.Editor.SRDebugEditorUtil.GetRootPath() + ResourcesPath;

			} catch {

				return null;

			}

		}

#endif

		#endregion

		private static KeyboardShortcut[] GetDefaultKeyboardShortcuts()
		{

			return new[] {

				new KeyboardShortcut() {
					Key = KeyCode.F1,
					Action = ShortcutActions.OpenSystemInfoTab
				},
				new KeyboardShortcut() {
					Key = KeyCode.F2,
					Action = ShortcutActions.OpenConsoleTab
				},
				new KeyboardShortcut() {
					Key = KeyCode.F3,
					Action = ShortcutActions.OpenOptionsTab
				},
				new KeyboardShortcut() {
					Key = KeyCode.F4,
					Action = ShortcutActions.OpenProfilerTab
				},

			};

		}

	}

}
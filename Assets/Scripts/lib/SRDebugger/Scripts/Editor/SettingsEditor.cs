using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SRDebugger.Internal;
using SRDebugger.Internal.Editor;
using SRF;
using SRF.Helpers;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace SRDebugger.Editor
{

	[CustomEditor(typeof(Settings))]
	public class SettingsEditor : UnityEditor.Editor
	{

		[MenuItem(SRDebugPaths.SettingsMenuItemPath)]
		public static void Open()
		{

			var settings = Settings.Instance;
			AssetUtil.SelectAssetInProjectView(settings);

		}

		public static void ForceRefresh()
		{

			if (_instance != null) {

				_instance._bugReporterFoldoutVisible = true;
				_instance.serializedObject.Update();
				_instance.Repaint();

			}

		}

		private SerializedProperty _isEnabledProperty;

#if UNITY_5
		private SerializedProperty _autoLoadProperty;
#endif

		private SerializedProperty _defaultTabProperty;

		private SerializedProperty _triggerPositionProperty;
		private SerializedProperty _triggerEnableModeProperty;
		private SerializedProperty _triggerBehaviourProperty;

		private SerializedProperty _enableKeyboardShortcutsProperty;
		private SerializedProperty _keyboardShortcutsListProperty;
		private SerializedProperty _keyboardModifierControlProperty;
		private SerializedProperty _keyboardModifierAltProperty;
		private SerializedProperty _keyboardModifierShiftProperty;
		private SerializedProperty _keyboardEscapeCloseProperty;

		private SerializedProperty _requireEntryCodeProperty;
		private SerializedProperty _requireEntryCodeEveryTimeProperty;
		private SerializedProperty _entryCodeProperty;

		private SerializedProperty _enableBackgroundTransparencyProperty;
		private SerializedProperty _collapseDuplicateLogEntriesProperty;
		private SerializedProperty _richTextInConsoleProperty;

		private SerializedProperty _useDebugCameraProperty;
		private SerializedProperty _debugLayerProperty;
		private SerializedProperty _debugCameraDepthProperty;

		private SerializedProperty _disabledTabsProperty;

		private SerializedProperty _apiKeyProperty;
		private SerializedProperty _enableBugReporterProperty;


		private ReorderableList _keyboardShortcutList;

		private string _currentEntryCode;

		private bool _panelAccessFoldoutVisible = true;
		private bool _keyboardFoldoutVisible = true;
		private bool _displayFoldoutVisible = true;
		private bool _bugReporterFoldoutVisible = false;
		private bool _enableDisableTabsVisible = false;

		private static SettingsEditor _instance;

		private void OnEnable()
		{

			_isEnabledProperty = serializedObject.FindProperty("_isEnabled");

#if UNITY_5
			_autoLoadProperty = serializedObject.FindProperty("_autoLoad");
#endif

			_defaultTabProperty = serializedObject.FindProperty("_defaultTab");

			_triggerEnableModeProperty = serializedObject.FindProperty("_triggerEnableMode");
			_triggerPositionProperty = serializedObject.FindProperty("_triggerPosition");
			_triggerBehaviourProperty = serializedObject.FindProperty("_triggerBehaviour");

			_enableKeyboardShortcutsProperty = serializedObject.FindProperty("_enableKeyboardShortcuts");
			_keyboardShortcutsListProperty = serializedObject.FindProperty("_keyboardShortcuts");

			_keyboardModifierControlProperty = serializedObject.FindProperty("_keyboardModifierControl");
			_keyboardModifierAltProperty = serializedObject.FindProperty("_keyboardModifierAlt");
			_keyboardModifierShiftProperty = serializedObject.FindProperty("_keyboardModifierShift");
			_keyboardEscapeCloseProperty = serializedObject.FindProperty("_keyboardEscapeClose");

			_requireEntryCodeProperty = serializedObject.FindProperty("_requireEntryCode");
			_requireEntryCodeEveryTimeProperty = serializedObject.FindProperty("_requireEntryCodeEveryTime");
			_entryCodeProperty = serializedObject.FindProperty("_entryCode");

			_enableBackgroundTransparencyProperty = serializedObject.FindProperty("_enableBackgroundTransparency");
			_collapseDuplicateLogEntriesProperty = serializedObject.FindProperty("_collapseDuplicateLogEntries");
			_richTextInConsoleProperty = serializedObject.FindProperty("_richTextInConsole");

			_useDebugCameraProperty = serializedObject.FindProperty("_useDebugCamera");
			_debugLayerProperty = serializedObject.FindProperty("_debugLayer");
			_debugCameraDepthProperty = serializedObject.FindProperty("_debugCameraDepth");

			_disabledTabsProperty = serializedObject.FindProperty("_disabledTabs");

			_apiKeyProperty = serializedObject.FindProperty("_apiKey");
			_enableBugReporterProperty = serializedObject.FindProperty("_enableBugReporter");

			_currentEntryCode = GetEntryCodeString();


			_keyboardShortcutList = new ReorderableList(serializedObject, _keyboardShortcutsListProperty, true, true, true, true);

			_keyboardShortcutList.drawHeaderCallback += DrawKeyboardListHeaderCallback;
			_keyboardShortcutList.drawElementCallback += DrawKeyboardListItemCallback;

			_instance = this;

		} 

		public override void OnInspectorGUI()
		{

			SRDebugEditorUtil.DrawLogo();

			EditorGUILayout.PropertyField(_isEnabledProperty,
				new GUIContent("Enabled",
					SRDebugStrings.Current.SettingsIsEnabledTooltip));

#if UNITY_5

			GUI.enabled = _isEnabledProperty.boolValue;

			EditorGUILayout.PropertyField(_autoLoadProperty,
				new GUIContent("Auto Load", SRDebugStrings.Current.SettingsAutoLoadTooltip));

			GUI.enabled = true;

#endif

			EditorGUILayout.Separator();

			_panelAccessFoldoutVisible = SRDebugEditorUtil.DrawInspectorFoldout(_panelAccessFoldoutVisible, "Panel Access");

			if (_panelAccessFoldoutVisible) {

				DrawPanelAccessArea();

			}

			if (_enableKeyboardShortcutsProperty.boolValue) {

				_keyboardFoldoutVisible = SRDebugEditorUtil.DrawInspectorFoldout(_keyboardFoldoutVisible, "Keyboard Shortcuts");

				if (_keyboardFoldoutVisible) {

					DrawKeyboardShortcutArea();

				}

				EditorGUILayout.Separator();

			}
			
			EditorGUILayout.Separator();

			_displayFoldoutVisible = SRDebugEditorUtil.DrawInspectorFoldout(_displayFoldoutVisible, "Display");

			if (_displayFoldoutVisible) {

				DrawDisplayArea();

			}

			EditorGUILayout.Separator();

			_enableDisableTabsVisible = SRDebugEditorUtil.DrawInspectorFoldout(_enableDisableTabsVisible, "Enabled Tabs");

			if (_enableDisableTabsVisible) {

				DrawEnableDisableTabsArea();

			}

			EditorGUILayout.Separator();

			_bugReporterFoldoutVisible = SRDebugEditorUtil.DrawInspectorFoldout(_bugReporterFoldoutVisible, "Bug Reporter");

			if (_bugReporterFoldoutVisible) {

				DrawBugReporterArea();

			}

			serializedObject.ApplyModifiedProperties();

			EditorGUILayout.Separator();
			EditorGUILayout.Separator();

			EditorGUILayout.HelpBox(SRDebugStrings.Current.SettingsRateBoxContents, MessageType.None, true);

			EditorGUILayout.BeginHorizontal();

			var width = Screen.width - 50;

			GUILayout.Space(5);

			var margin = (EditorStyles.miniButton.padding.left)/2f;

			if (GUILayout.Button("Web Site", GUILayout.Width(width/2f - margin) )) {
				Application.OpenURL(SRDebugStrings.Current.SettingsWebSiteUrl);
			}

			if (GUILayout.Button("Asset Store Page", GUILayout.Width(width/2f - margin))) {
				Application.OpenURL(SRDebugStrings.Current.SettingsAssetStoreUrl);
			}

			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();

			GUILayout.Space(5);

			if (GUILayout.Button("Documentation", GUILayout.Width(width/2f - margin))) {
				Application.OpenURL(SRDebugStrings.Current.SettingsDocumentationUrl);
			}
						
			if (GUILayout.Button("Support", GUILayout.Width(width/2f - margin))) {
				Application.OpenURL(
					SRDebugStrings.Current.SettingsSupportUrl);
			}

			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Version " + SRDebug.Version, EditorStyles.miniLabel);
			EditorGUILayout.EndHorizontal();

		}

		private void DrawEnableDisableTabsArea()
		{

			GUILayout.Label(SRDebugStrings.Current.SettingsEnabledTabsDescription, EditorStyles.wordWrappedLabel);
			EditorGUILayout.Space();

			var disabledTabs = new List<DefaultTabs>();

			for (var i = 0; i < _disabledTabsProperty.arraySize; i++) {
				disabledTabs.Add((DefaultTabs) _disabledTabsProperty.GetArrayElementAtIndex(i).enumValueIndex);
			}

			var tabNames = Enum.GetNames(typeof (DefaultTabs));
			var tabValues = Enum.GetValues(typeof (DefaultTabs));
			
			EditorGUILayout.BeginVertical();

			var changed = false;
			for (var i = 0; i < tabNames.Length; i++) {

				var tabName = tabNames[i];
				var tabValue = (DefaultTabs) (tabValues.GetValue(i));

				if (tabName == "BugReporter")
					continue;

				EditorGUILayout.BeginHorizontal();

				var isEnabled = !disabledTabs.Contains(tabValue);
				
				var isNowEnabled = EditorGUILayout.Toggle(tabName, isEnabled);

				if (isEnabled && !isNowEnabled) {
					disabledTabs.Add(tabValue);
					changed = true;
				} else if (!isEnabled && isNowEnabled) {
					disabledTabs.Remove(tabValue);
					changed = true;
				}

				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.EndVertical();

			if (changed) {

				_disabledTabsProperty.ClearArray();

				for (var i = 0; i < disabledTabs.Count; i++) {
					_disabledTabsProperty.InsertArrayElementAtIndex(0);
					_disabledTabsProperty.GetArrayElementAtIndex(0).enumValueIndex = (int)disabledTabs[i];
				}

			}

		}

		private void DrawBugReporterArea()
		{
			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.PropertyField(_apiKeyProperty);

			if (GUILayout.Button("Verify", GUILayout.ExpandWidth(false))) {

				EditorUtility.DisplayDialog("Verify API Key", ApiSignup.Verify(_apiKeyProperty.stringValue), "OK");

			}

			EditorGUILayout.EndHorizontal();

			GUI.enabled = !string.IsNullOrEmpty(_apiKeyProperty.stringValue);

			EditorGUILayout.PropertyField(_enableBugReporterProperty);

			GUI.enabled = true;

			if (GUILayout.Button("Need API Key?")) {
				ApiSignupWindow.Open();
			}
		}

		private void DrawPanelAccessArea()
		{
			EditorGUILayout.PropertyField(_defaultTabProperty,
				new GUIContent("Default Tab", SRDebugStrings.Current.SettingsDefaultTabTooltip));

			EditorGUILayout.PropertyField(_triggerEnableModeProperty, new GUIContent("Trigger Mode"));
			EditorGUILayout.PropertyField(_triggerPositionProperty, new GUIContent("Trigger Position"));
			EditorGUILayout.PropertyField(_triggerBehaviourProperty, new GUIContent("Trigger Behaviour"));

			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.PropertyField(_requireEntryCodeProperty, new GUIContent("Require Entry Code"));

			if (_requireEntryCodeProperty.boolValue) {

				EditorGUILayout.PropertyField(_requireEntryCodeEveryTimeProperty, new GUIContent("...Every Time"));

			}

			EditorGUILayout.EndHorizontal();

			if (_requireEntryCodeProperty.boolValue) {

				var newCode = EditorGUILayout.TextField("Entry Code", _currentEntryCode);

				if (newCode != _currentEntryCode) {

					// Strip out alpha numeric chars
					newCode = new String(newCode.Where(char.IsDigit).ToArray());

					// Max length = 4
					newCode = newCode.Substring(0, Mathf.Min(4, newCode.Length));

					if (newCode.Length == 4) {
						UpdateEntryCode(newCode);
					}

				}

			}

			EditorGUILayout.PropertyField(_enableKeyboardShortcutsProperty,
				new GUIContent("Keyboard Shortcuts", SRDebugStrings.Current.SettingsKeyboardShortcutsTooltip));

			EditorGUILayout.Separator();
		}

		private void DrawKeyboardShortcutArea()
		{
			var wasEnabled = GUI.enabled;

			GUI.enabled = _enableKeyboardShortcutsProperty.boolValue;

			EditorGUILayout.PropertyField(_keyboardEscapeCloseProperty,
				new GUIContent("Close on Esc", SRDebugStrings.Current.SettingsCloseOnEscapeTooltip));

			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.PrefixLabel(new GUIContent("Modifiers", SRDebugStrings.Current.SettingsKeyboardModifersTooltip));

			_keyboardModifierControlProperty.boolValue = EditorGUILayout.Toggle(_keyboardModifierControlProperty.boolValue,
				GUILayout.Width(20));
			GUILayout.Label("Ctrl");

			_keyboardModifierShiftProperty.boolValue = EditorGUILayout.Toggle(_keyboardModifierShiftProperty.boolValue,
				GUILayout.Width(20));
			GUILayout.Label("Shift");

			_keyboardModifierAltProperty.boolValue = EditorGUILayout.Toggle(_keyboardModifierAltProperty.boolValue,
				GUILayout.Width(20));
			GUILayout.Label("Alt");

			EditorGUILayout.EndHorizontal();

			_keyboardShortcutList.DoLayoutList();

			GUI.enabled = wasEnabled;

			var dupe = DetectDuplicateKeyboardShortcuts();

			if (dupe != KeyCode.None) {

				EditorGUILayout.HelpBox("Duplicate key ({0}). Only one shortcut per key is supported.".Fmt(dupe), MessageType.Warning);

			}
		}

		private void DrawDisplayArea()
		{
			EditorGUILayout.PropertyField(_enableBackgroundTransparencyProperty, new GUIContent("Background Transparency"));

			EditorGUILayout.PropertyField(_collapseDuplicateLogEntriesProperty,
				new GUIContent("Collapse Log Entries", "Collapse duplicate log entries into single log."));

			EditorGUILayout.PropertyField(_richTextInConsoleProperty,
				new GUIContent("Rich Text in Console", "Parse rich text tags in console log entries."));

			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.PrefixLabel(new GUIContent("Layer", "The layer the debug panel UI will be on"));

			_debugLayerProperty.intValue = EditorGUILayout.LayerField(_debugLayerProperty.intValue);

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.PropertyField(_useDebugCameraProperty,
				new GUIContent("Use Debug Camera", SRDebugStrings.Current.SettingsDebugCameraTooltip));

			if (_useDebugCameraProperty.boolValue) {

				EditorGUILayout.PropertyField(_debugCameraDepthProperty, new GUIContent("Camera Depth"));

			}
		}

		private void DrawKeyboardListHeaderCallback(Rect rect)
		{
			EditorGUI.LabelField(rect, "Keyboard Shortcuts");
		}

		private void DrawKeyboardListItemCallback(Rect rect, int index, bool isActive, bool isFocused)
		{

			var item = _keyboardShortcutsListProperty.GetArrayElementAtIndex(index);

			rect.y += 2;

			EditorGUI.PropertyField(new Rect(rect.x, rect.y, 80, EditorGUIUtility.singleLineHeight),
				item.FindPropertyRelative("Key"), GUIContent.none);

			rect.x += 80;

			EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width - 4 - 80, EditorGUIUtility.singleLineHeight),
				item.FindPropertyRelative("Action"),
				GUIContent.none);

		}

		private KeyCode DetectDuplicateKeyboardShortcuts()
		{

			var s = target as Settings;

			var usedKeys = new List<KeyCode>();

			foreach (var shortcut in s.KeyboardShortcuts) {

				if(shortcut.Key == KeyCode.None)
					continue;

				if (usedKeys.Contains(shortcut.Key))
					return shortcut.Key;

				usedKeys.Add(shortcut.Key);

			}

			return KeyCode.None;

		}

		private string GetEntryCodeString()
		{

			if (_entryCodeProperty.arraySize == 0) {

				_entryCodeProperty.InsertArrayElementAtIndex(0);
				_entryCodeProperty.InsertArrayElementAtIndex(0);
				_entryCodeProperty.InsertArrayElementAtIndex(0);
				_entryCodeProperty.InsertArrayElementAtIndex(0);

			}

			var code = "";

			for (var i = 0; i < _entryCodeProperty.arraySize; i++) {
				code += _entryCodeProperty.GetArrayElementAtIndex(i).intValue;
			}

			return code;
		}

		private void UpdateEntryCode(string str)
		{

			if (_entryCodeProperty.arraySize == 0) {

				_entryCodeProperty.InsertArrayElementAtIndex(0);
				_entryCodeProperty.InsertArrayElementAtIndex(0);
				_entryCodeProperty.InsertArrayElementAtIndex(0);
				_entryCodeProperty.InsertArrayElementAtIndex(0);

			}
			for (var i = 0; i < str.Length; i++) {

				var obj = _entryCodeProperty.GetArrayElementAtIndex(i);

				obj.intValue = int.Parse(str[i].ToString(), NumberStyles.Integer);

			}

			_currentEntryCode = GetEntryCodeString();

		}

	}

}
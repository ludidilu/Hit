#if UNITY_EDITOR

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SRDebugger.Internal.Editor
{


	public static class SRDebugEditorUtil
	{

		public static class Styles
		{

			public static GUIStyle InspectorHeaderStyle
			{
				get
				{

					if (_inspectorHeaderStyle == null) {
						_inspectorHeaderStyle = new GUIStyle(EditorStyles.boldLabel);
						_inspectorHeaderStyle.fontSize = 12;
					}

					return _inspectorHeaderStyle;

				}
			}

			public static GUIStyle InspectorHeaderFoldoutStyle
			{
				get
				{

					if (_inspectorHeaderFoldoutStyle == null) {
						_inspectorHeaderFoldoutStyle = new GUIStyle(EditorStyles.foldout);
						_inspectorHeaderFoldoutStyle.fontSize = 12;
						_inspectorHeaderFoldoutStyle.fontStyle = FontStyle.Bold;
					}

					return _inspectorHeaderFoldoutStyle;

				}
			}

			private static GUIStyle _inspectorHeaderStyle;
			private static GUIStyle _inspectorHeaderFoldoutStyle;

		}



		// Path to this file from the root path
		private const string TestPath = "SRDebugger/Scripts/Internal/EditorUtil.cs";

		public static string GetRootPath()
		{

			// Find assets that match this file name
			var potentialAssets = AssetDatabase.FindAssets("EditorUtil");

			foreach (var potentialAsset in potentialAssets) {

				var path = AssetDatabase.GUIDToAssetPath(potentialAsset);

				if (path.Contains(TestPath)) {

					// Decend three levels in file tree
					var rootPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(path)));
					return rootPath;

				}

			}

			throw new Exception("Unable to find SRDebugger root path");

		}

		private static Texture2D _logoTexture;

		public static Texture2D GetLogo()
		{

			if (_logoTexture != null)
				return _logoTexture;

			var path = GetRootPath() + "/" + SRDebugPaths.EditorLogoPath;

			return _logoTexture = AssetDatabase.LoadAssetAtPath(path, typeof (Texture2D)) as Texture2D;

		}

		public static void DrawLogo()
		{

			var logo = GetLogo();

			if (logo == null) {

				Debug.LogError("Error loading SRDebugger logo");
				return;

			}

			GUILayout.Space(4);

			GUILayout.BeginHorizontal();
			GUILayout.Space(2);

			GUILayout.FlexibleSpace();

			GUI.DrawTexture(
				GUILayoutUtility.GetRect(logo.width, logo.width, logo.height, logo.height, GUILayout.ExpandHeight(false),
					GUILayout.ExpandWidth(false)),
				logo);

			GUILayout.FlexibleSpace();

			GUILayout.EndHorizontal();

			GUILayout.Space(4);

		}

		public static bool DrawInspectorFoldout(bool isVisible, string content)
		{

			isVisible = EditorGUILayout.Foldout(isVisible, content, Styles.InspectorHeaderFoldoutStyle);

			EditorGUILayout.Separator();

			return isVisible;

		}

	}
}

#endif
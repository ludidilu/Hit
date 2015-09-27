using SRF;
using UnityEditor;
using UnityEngine;

namespace SRDebugger.Editor
{

	public class ApiSignupWindow : EditorWindow
	{

		public const int Width = 450;
		public const int Height = 350;

		private string _invoiceNumber;
		private string _emailAddress;

		private string _errorStatus;

		private bool _shouldSignup;

		private GUIStyle _errorStyle;
		private GUIStyle _legalStyle;

		private static ApiSignupWindow _instance;

		private Vector2 _scroll;

		private GUIStyle ErrorStyle {

			get
			{

				if (_errorStyle == null) {
					_errorStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
					_errorStyle.normal.textColor = Color.red;
				}

				return _errorStyle;

			}

		}

		private GUIStyle LegalStyle
		{

			get
			{

				if (_legalStyle == null) {
					_legalStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
					_legalStyle.fontSize = 9;
				}

				return _legalStyle;

			}

		}

		public static void Open()
		{

			if (_instance != null) {
				_instance.Focus();
				return;
			}

			var window = CreateInstance<ApiSignupWindow>();

#if UNITY_4_6 || UNITY_5_0
			window.title = "SRDebugger API Signup";
#else
			window.titleContent = new GUIContent("SRDebugger API Signup");
#endif

			window.minSize = window.maxSize = new Vector2(Width, Height);
			window.position = new Rect(Screen.width - Width / 2, Screen.height - Height / 2, Width, Height);
			window.ShowUtility();

		}

		public void OnEnable()
		{
			_instance = this;
		}

		public void OnDisable()
		{
			_instance = null;
		}

		public void Update()
		{

			if (_shouldSignup) {

				_shouldSignup = false;
				ApiSignup.SignUp(_emailAddress, _invoiceNumber, OnSuccess);
				Repaint();

			}

		}

		public void OnGUI()
		{

			_scroll = EditorGUILayout.BeginScrollView(_scroll, false, true);
			EditorGUILayout.BeginVertical();

#if UNITY_WEBPLAYER

			EditorGUILayout.HelpBox("Signup form is not available when build target is Web Player", MessageType.Error);
			GUI.enabled = false;

#endif

			GUILayout.Label("This tool will acquire an API key for you to use with SRDebugger to enable bug reporting.", EditorStyles.wordWrappedLabel);

			EditorGUILayout.Separator();

			GUILayout.Label(
				"Please enter the invoice number from your Asset Store purchase invoice (9-digit number. Not to be mistaken for the order number)", EditorStyles.wordWrappedLabel);

			EditorGUILayout.Space();

			_invoiceNumber = EditorGUILayout.TextField("Invoice Number", _invoiceNumber);
			_emailAddress = EditorGUILayout.TextField("Email Address", _emailAddress);

			EditorGUILayout.Separator();

			if (!string.IsNullOrEmpty(_errorStatus)) {
				GUILayout.Label(_errorStatus, ErrorStyle);
			}

			if (GUILayout.Button("Submit")) {

				_errorStatus = null;
				_shouldSignup = true;

			}

			EditorGUILayout.Separator();

			GUILayout.Label("If you have any problems, or have lost your API key, please email contact@stompyrobot.uk for assistance.", EditorStyles.wordWrappedLabel);

			GUILayout.FlexibleSpace();

			GUILayout.Label(
				"The Bug Reporter service is provided as a convenience, Stompy Robot LTD reserves the right to cancel the bug report service at any time without warning. By signing up for the Bug Reporter service you grant Stompy Robot LTD permission to gather non-identifying information from users when submitting reports. " +
				"THE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.",
				LegalStyle);

			EditorGUILayout.EndVertical();

			EditorGUILayout.EndScrollView();

		}

		private void OnSuccess(bool didSucceed, string apiKey, string email, string error)
		{

			if (!didSucceed) {

				_errorStatus = error;
				Repaint();

				return;

			}

			SettingsEditor.Open();

			Settings.Instance.ApiKey = apiKey;

			SettingsEditor.ForceRefresh();

			EditorUtility.DisplayDialog("SRDebugger API",
				"API key has been created successfully. An email has been sent to your email address ({0}) with a verification link. You must verify your email before you can receive any bug reports."
					.Fmt(email), "OK");

			Close();

		}

	}

}
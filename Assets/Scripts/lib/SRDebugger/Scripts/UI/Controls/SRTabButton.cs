using SRF;
using SRF.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SRDebugger.UI.Controls
{

	public class SRTabButton : SRMonoBehaviourEx
	{

		[RequiredField]
		public Text TitleText;

		[RequiredField]
		public RectTransform ExtraContentContainer;

		[RequiredField]
		public Button Button;

		[RequiredField] 
		public StyleComponent IconStyleComponent;

		[RequiredField]
		public Behaviour ActiveToggle;

		public bool IsActive
		{
			get { return ActiveToggle.enabled; }
			set { ActiveToggle.enabled = value; }
		}

	}

}

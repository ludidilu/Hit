using SRF;
using UnityEngine.UI;

namespace SRDebugger.UI.Other {


	public class VersionTextBehaviour : SRMonoBehaviourEx
	{

		[RequiredField]
		public Text Text;

		public string Format = "SRDebugger {0}";

		protected override void Start()
		{

			base.Start();

			Text.text = string.Format(Format, SRDebug.Version);

		}

	}

}

using SRF;
using UnityEngine;

namespace SRDebugger.UI.Other
{
	public class PinnedUIRoot : SRMonoBehaviourEx
	{

		[RequiredField]
		public RectTransform Container;

		[RequiredField]
		public Canvas Canvas;

		[RequiredField]
		public GameObject Profiler;

	}
}

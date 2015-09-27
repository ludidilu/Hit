using System.Collections.Generic;
using SRDebugger.Internal;
using UnityEngine;

namespace SRDebugger.Services
{

	public interface IPinnedUIService
	{

		bool IsProfilerPinned { get; set; }

		void Pin(OptionDefinition option, int order = -1);
		void Unpin(OptionDefinition option);

		bool HasPinned(OptionDefinition option);

	}

}
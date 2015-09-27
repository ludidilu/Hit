using System.Collections.Generic;

namespace SRDebugger.Services
{

	public delegate void PinEntryCompleteCallback(bool validPinEntered);

	public interface IPinEntryService
	{

		bool IsShowingKeypad { get; }

		void ShowPinEntry(IList<int> requiredPin, string message, PinEntryCompleteCallback callback, bool blockInput = true, bool allowCancel = true);

	}

}
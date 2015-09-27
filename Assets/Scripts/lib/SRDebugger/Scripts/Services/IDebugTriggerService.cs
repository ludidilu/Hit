namespace SRDebugger.Services
{

	public interface IDebugTriggerService
	{

		bool IsEnabled { get; set; }

		Settings.TriggerPositions Position { get; set; }

	}

}
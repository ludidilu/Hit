using System.Collections.Generic;

namespace SRDebugger.Services
{

	public class BugReport
	{

		public string Email;

		public string UserDescription;

		public Dictionary<string, Dictionary<string, object>> SystemInformation;

		public List<ConsoleEntry> ConsoleLog;

		public byte[] ScreenshotData;

	}

	public delegate void BugReportCompleteCallback(bool didSucceed, string errorMessage);
	public delegate void BugReportProgressCallback(float progress);

	public interface IBugReportService
	{

		/// <summary>
		/// Submit a bug report to the SRDebugger API.
		/// completeHandler can be invoked any time after the method is called
		/// (even before the method has returned in case of internet reachability failure).
		/// </summary>
		/// <param name="report">Bug report to send</param>
		/// <param name="completeHandler">Delegate to call once bug report is submitted successfully</param>
		/// <param name="progressCallback">Optionally provide a callback for when progress % is known</param>
		void SendBugReport(BugReport report, BugReportCompleteCallback completeHandler, BugReportProgressCallback progressCallback = null);

	}

}
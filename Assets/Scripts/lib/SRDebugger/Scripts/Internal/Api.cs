namespace SRDebugger.Internal
{

	public static class SRDebugApi
	{

		
#if UNITY_WEBPLAYER && !UNITY_EDITOR
		public const string Protocol = "https://";
#else
		public const string Protocol = "http://";
#endif

		public const string EndPoint = Protocol + "srdebugger.stompyrobot.uk";
		//public const string EndPoint = "http://localhost:43393";

		public const string BugReportEndPoint = EndPoint + "/report/submit";


	}

}
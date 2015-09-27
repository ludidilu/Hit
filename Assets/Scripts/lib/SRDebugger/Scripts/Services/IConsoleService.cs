using System.Collections.Generic;
using UnityEngine;

namespace SRDebugger.Services
{

	public delegate void ConsoleUpdatedEventHandler(IConsoleService console);

	public interface IConsoleService
	{

		int ErrorCount { get; }
		int WarningCount { get; }
		int InfoCount { get; }

		event ConsoleUpdatedEventHandler Updated;

		/// <summary>
		/// List of ConsoleEntry objects since the last clear.
		/// </summary>
		IList<ConsoleEntry> Entries { get; }

		/// <summary>
		/// List of all ConsoleEntry objects, regardless of clear.
		/// </summary>
		IList<ConsoleEntry> AllEntries { get; } 

		void Clear();

	}

	public class ConsoleEntry
	{
		private const int MessagePreviewLength = 120;
		private const int StackTracePreviewLength = 120;

		public string Message;
		public string StackTrace;
		public LogType LogType;

		public string MessagePreview
		{
			get
			{
				if (_messagePreview != null) return _messagePreview;
				if (string.IsNullOrEmpty(Message)) return "";

				_messagePreview = Message.Split('\n')[0];
				_messagePreview = _messagePreview.Substring(0, Mathf.Min(_messagePreview.Length, MessagePreviewLength));

				return _messagePreview;
			}
		}

		public string StackTracePreview
		{
			get
			{
				if (_stackTracePreview != null) return _stackTracePreview;
				if (string.IsNullOrEmpty(StackTrace)) return "";

				_stackTracePreview = StackTrace.Split('\n')[0];
				_stackTracePreview = _stackTracePreview.Substring(0, Mathf.Min(_stackTracePreview.Length, StackTracePreviewLength));

				return _stackTracePreview;
			}
		}

		/// <summary>
		/// Number of times this log entry has occured (if collapsing is enabled)
		/// </summary>
		public int Count = 1;

		private string _stackTracePreview;
		private string _messagePreview;

		public bool Matches(ConsoleEntry other)
		{
			if (ReferenceEquals(null, other)) {
				return false;
			}
			if (ReferenceEquals(this, other)) {
				return true;
			}
			return string.Equals(Message, other.Message) && string.Equals(StackTrace, other.StackTrace) && LogType == other.LogType;
		}

		public ConsoleEntry() { }

		public ConsoleEntry(ConsoleEntry other)
		{
			Message = other.Message;
			StackTrace = other.StackTrace;
			LogType = other.LogType;
			Count = other.Count;
		}

	}

}
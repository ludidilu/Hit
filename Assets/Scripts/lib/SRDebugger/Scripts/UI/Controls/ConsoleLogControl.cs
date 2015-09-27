using System;
using SRDebugger.Internal;
using SRDebugger.Services;
using SRF;
using SRF.UI.Layout;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 169
#pragma warning disable 649

namespace SRDebugger.UI.Controls
{

	public class ConsoleLogControl : SRMonoBehaviourEx
	{

		public Action<ConsoleEntry> SelectedItemChanged;

		public bool ShowErrors
		{
			get { return _showErrors; }
			set
			{
				_showErrors = value;
				SetIsDirty();
			}
		}

		public bool ShowWarnings
		{
			get { return _showWarnings; }
			set
			{
				_showWarnings = value;
				SetIsDirty();
			}
		}

		public bool ShowInfo
		{
			get { return _showInfo; }
			set
			{
				_showInfo = value;
				SetIsDirty();
			}
		}

		public bool EnableSelection
		{
			get { return _consoleScrollLayoutGroup.EnableSelection; }
			set { _consoleScrollLayoutGroup.EnableSelection = value; }
		}

		[RequiredField]
		[SerializeField]
		private ScrollRect _consoleScrollRect;

		[RequiredField]
		[SerializeField]
		private VirtualVerticalLayoutGroup _consoleScrollLayoutGroup;

		private bool _showErrors = true;
		private bool _showWarnings = true;
		private bool _showInfo = true;

		private bool _isDirty = false;

		protected override void Awake()
		{

			base.Awake();

			_consoleScrollLayoutGroup.SelectedItemChanged.AddListener(OnSelectedItemChanged);
			Service.Console.Updated += ConsoleOnUpdated;

		}

		protected override void Start()
		{
			base.Start();
			SetIsDirty();
		}

		protected override void OnDestroy()
		{
			if (Service.Console != null)
				Service.Console.Updated -= ConsoleOnUpdated;

			base.OnDestroy();
		}

		private void OnSelectedItemChanged(object arg0)
		{

			var entry = arg0 as ConsoleEntry;


			if(SelectedItemChanged != null)
				SelectedItemChanged(entry);

		}

		protected override void Update()
		{

			base.Update();

			if (_isDirty)
				Refresh();

		}

		private void Refresh()
		{

			_consoleScrollLayoutGroup.ClearItems();

			var entries = Service.Console.Entries;

			for (var i = 0; i < entries.Count; i++) {

				var e = entries[i];

				if ((e.LogType == LogType.Error || e.LogType == LogType.Exception || e.LogType == LogType.Assert) && !ShowErrors)
					continue;

				if (e.LogType == LogType.Warning && !ShowWarnings)
					continue;

				if (e.LogType == LogType.Log && !ShowInfo)
					continue;

				_consoleScrollLayoutGroup.AddItem(e);

			}

			_isDirty = false;

		}

		private void SetIsDirty()
		{
			_isDirty = true;
		}

		private void ConsoleOnUpdated(IConsoleService console)
		{
			SetIsDirty();
		}

	}

}
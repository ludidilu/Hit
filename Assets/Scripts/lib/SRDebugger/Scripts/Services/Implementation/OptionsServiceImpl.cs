using System.Collections.Generic;
using System.Collections.ObjectModel;
using SRDebugger.Internal;
using SRF.Service;
using UnityEngine;
using System.Linq;

namespace SRDebugger.Services.Implementation
{

	[Service(typeof(IOptionsService))]
	public class OptionsServiceImpl : IOptionsService
	{

		public ICollection<OptionDefinition> Options { get { return _optionsReadonly; } }

		private readonly List<OptionDefinition> _options = new List<OptionDefinition>();
		private readonly IList<OptionDefinition> _optionsReadonly; 

		public OptionsServiceImpl()
		{

			_optionsReadonly = new ReadOnlyCollection<OptionDefinition>(_options);

			Scan(SROptions.Current);

		}

		public void Scan(object obj)
		{

			_options.AddRange(SRDebuggerUtil.ScanForOptions(obj));

		}

	}

}
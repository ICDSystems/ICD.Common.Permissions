using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace ICD.Common.Permissions
{
	public interface IAction
	{
		string Value { get; }
	}
}
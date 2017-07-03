using System.Collections.Generic;

namespace ICD.Common.Permissions
{
	public class Permission
	{
		public IAction Action { get; set; }

		public IEnumerable<string> Roles { get; set; }
	}
}
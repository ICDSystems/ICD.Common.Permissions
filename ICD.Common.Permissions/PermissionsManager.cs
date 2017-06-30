using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Common.Utils;

namespace ICD.Common.Permissions
{
	public class PermissionsManager
	{
		private List<Permission> DefaultPermissions { get; set; }

		private Dictionary<object, List<Permission>> ObjectPermissions { get; set; }

		private SafeCriticalSection DefaultPermissionsSection { get; set; }
		private SafeCriticalSection ObjectPermissionsSection { get; set; }
		 
		public PermissionsManager()
		{
			DefaultPermissions = new List<Permission>();
			ObjectPermissions = new Dictionary<object, List<Permission>>();
			DefaultPermissionsSection = new SafeCriticalSection();
			ObjectPermissionsSection = new SafeCriticalSection();
		}

		[PublicAPI]
		public void SetPermissions(IEnumerable<Permission> permissions)
		{
			//Remove permissions with duplicate actions by using GroupBy -> Select First
			DefaultPermissions = RemoveDuplicateActions(permissions).ToList();
		}

		[PublicAPI]
		public void SetObjectPermissions(object obj, IEnumerable<Permission> permissions)
		{
			ObjectPermissions[obj] = RemoveDuplicateActions(permissions).ToList();
		}

		[PublicAPI]
		public void RemoveObjectPermissions(object obj)
		{
			ObjectPermissions.Remove(obj);
		}

		[PublicAPI]
		public string[] GetRoles(IAction action)
		{
			var permission = DefaultPermissions.SingleOrDefault(p => p.Action.Value.Equals(action.Value));
			if (permission == null)
				return null;
			return permission.Roles.ToArray();
		}

		[PublicAPI]
		public string[] GetRoles(IAction action, object obj)
		{
			if (ObjectPermissions.ContainsKey(obj))
			{
				var permission = ObjectPermissions[obj].SingleOrDefault(p => p.Action.Value.Equals(action.Value));
				if (permission == null)
					return GetRoles(action);
				return permission.Roles.ToArray();
			}
			return GetRoles(action);
		}

		private IEnumerable<Permission> RemoveDuplicateActions(IEnumerable<Permission> permissions)
		{
			return permissions.GroupBy(p => p.Action).Select(g => g.First());
		}
	}
}
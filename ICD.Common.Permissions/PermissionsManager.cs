using System;
using System.Collections.Generic;
using System.Linq;
using ICD.Common.Properties;
using ICD.Common.Utils;

namespace ICD.Common.Permissions
{
	public sealed class PermissionsManager
	{
		private List<Permission> DefaultPermissions { get; set; }

		private readonly Dictionary<object, List<Permission>> m_ObjectPermissions;

		private SafeCriticalSection DefaultPermissionsSection { get; set; }
		private SafeCriticalSection ObjectPermissionsSection { get; set; }

		public IEnumerable<string> DefaultRoles { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		public PermissionsManager()
		{
			DefaultPermissions = new List<Permission>();
			m_ObjectPermissions = new Dictionary<object, List<Permission>>();
			DefaultPermissionsSection = new SafeCriticalSection();
			ObjectPermissionsSection = new SafeCriticalSection();
		}

		/// <summary>
		/// Sets the default set of permissions
		/// </summary>
		/// <param name="permissions"></param>
		[PublicAPI]
		public void SetDefaultPermissions(IEnumerable<Permission> permissions)
		{
			if (permissions == null)
				throw new ArgumentNullException("permissions");
			
			DefaultPermissions = RemoveDuplicateActions(permissions).ToList();
		}

		/// <summary>
		/// Sets the object-specific permissions for the given object
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="permissions"></param>
		[PublicAPI]
		public void SetObjectPermissions(object obj, IEnumerable<Permission> permissions)
		{
			if (permissions == null)
				throw new ArgumentNullException("permissions");
			
			List<Permission> newPermissions = RemoveDuplicateActions(permissions).ToList();

			if (newPermissions.Count == 0)
				m_ObjectPermissions.Remove(obj);
			else
				m_ObjectPermissions[obj] = newPermissions;
		}

		/// <summary>
		/// Removes the object-specific permissions for the given object
		/// </summary>
		/// <param name="obj"></param>
		[PublicAPI]
		public void RemoveObjectPermissions(object obj)
		{
			m_ObjectPermissions.Remove(obj);
		}

		/// <summary>
		/// Gets the set of roles required for an action. Uses default permissions as a lookup
		/// </summary>
		/// <param name="permissable"></param>
		/// <returns></returns>
		[PublicAPI]
		public IEnumerable<string> GetRoles(IPermissable permissable)
		{
			Permission permission = DefaultPermissions.SingleOrDefault(p => p.Permissable.Name.Equals(permissable.Name));
			if (permission == null)
				return (DefaultRoles ?? Enumerable.Empty<string>()).ToList();
			return permission.Roles.ToList();
		}

		/// <summary>
		/// Gets the set of roles required for an action on specific object. Falls back to default permissions
		/// if no object-specific permissions are found
		/// </summary>
		/// <param name="permissable"></param>
		/// <param name="obj"></param>
		/// <returns>null if no permissions were found</returns>
		[PublicAPI]
		public IEnumerable<string> GetRoles(IPermissable permissable, object obj)
		{
			if (m_ObjectPermissions.ContainsKey(obj))
			{
				Permission permission = m_ObjectPermissions[obj].SingleOrDefault(p => p.Permissable.Name.Equals(permissable.Name));
				if (permission == null)
					return GetRoles(permissable);
				return permission.Roles.ToList();
			}
			return GetRoles(permissable);
		}

		/// <summary>
		/// Removes permissions with duplicate actions
		/// </summary>
		/// <param name="permissions"></param>
		/// <returns></returns>
		private IEnumerable<Permission> RemoveDuplicateActions(IEnumerable<Permission> permissions)
		{
			if(permissions == null)
				throw new ArgumentNullException("permissions");

			//Remove permissions with duplicate actions by using GroupBy -> Select First
			return permissions.GroupBy(p => p.Permissable).Select(g => g.First());
		}
	}
}

using System.Collections.Generic;
using ICD.Common.Utils.Xml;

namespace ICD.Common.Permissions
{
	public class Permission
	{

		private const string ACTION_ELEMENT = "Action";
		private const string ROLE_ELEMENT = "Role";
		private const string ROLES_ELEMENT = ROLE_ELEMENT + "s";

		/// <summary>
		/// Action that needs permission
		/// </summary>
		public IAction Action { get; set; }

		/// <summary>
		/// Set of roles, at least one of which is required to take the action
		/// </summary>
		public IEnumerable<string> Roles { get; set; }

		/// <summary>
		/// Parses the xml &lt;Permission&gt; element into a Permission object
		/// </summary>
		/// <param name="xml"></param>
		/// <returns></returns>
		public static Permission FromXml(string xml)
		{
			Permission permission = new Permission();
			permission.Action = Permissions.Action.FromString(XmlUtils.TryReadChildElementContentAsString(xml, ACTION_ELEMENT));
			string roles;
			if (XmlUtils.TryGetChildElementAsString(xml, ROLES_ELEMENT, out roles))
				permission.Roles = GetRolesFromXml(roles);
			return permission;
		}

		private static IEnumerable<string> GetRolesFromXml(string xml)
		{
			foreach (var role in XmlUtils.GetChildElementsAsString(xml, ROLE_ELEMENT))
				yield return XmlUtils.ReadElementContent(role);
		}
	}
}
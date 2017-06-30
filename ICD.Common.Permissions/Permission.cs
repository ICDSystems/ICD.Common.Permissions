namespace ICD.Common.Permissions
{
	public class Permission
	{
		public IAction Action { get; set; }

		public string[] Roles { get; set; }
	}
}
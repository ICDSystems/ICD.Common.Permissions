namespace ICD.Common.Permissions
{
	public class Permissable : IPermissable
	{
		/// <summary>
		/// The name of the action
		/// </summary>
		public string Name { get; set; }

		protected Permissable(string name)
		{
			Name = name;
		}

		/// <summary>
		/// Returns the string representation of the object.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Name;
		}

		/// <summary>
		/// Parses an xml &lt;Action&gt; element into an Action.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static Permissable FromString(string name)
		{
			return new Permissable(name);
		}
	}
}

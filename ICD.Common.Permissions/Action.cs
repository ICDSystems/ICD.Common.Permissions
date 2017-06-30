﻿namespace ICD.Common.Permissions
{
	public class Action : IAction
	{
		public string Value { get; set; }

		protected Action(string value)
		{
			Value = value;
		}

		public override string ToString()
		{
			return Value;
		}
	}
}
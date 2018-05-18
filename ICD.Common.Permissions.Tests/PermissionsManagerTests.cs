using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ICD.Common.Permissions.Tests
{
	[TestFixture]
	public sealed class PermissionsManagerTests
	{
		private const string ACTION = "TestPermissable";
		private PermissionsManager manager;
		private Object testObj;

		[SetUp]
		public void Init()
		{
			testObj = new object();
			manager = new PermissionsManager();
			manager.SetDefaultPermissions(CreateDefaultPermissions());
			manager.SetObjectPermissions(testObj, CreateObjectPermissions());
			manager.DefaultRoles = new [] {"DefaultRole"};
		}

		private static IEnumerable<Permission> CreateDefaultPermissions()
		{
			yield return new Permission() {Permissable = Permissable.FromString(ACTION), Roles = new [] {"TestRole"}};
			yield return new Permission() {Permissable = Permissable.FromString("FallbackPermissable"), Roles = new[] {"FallbackRole"}};
		}

		private static IEnumerable<Permission> CreateObjectPermissions()
		{
			yield return new Permission() {Permissable = Permissable.FromString(ACTION), Roles = new [] {"TestObjectRole"}};
		}

		[Test]
		public void GetRoles_DefaultPermissions()
		{
			var roles = manager.GetRoles(Permissable.FromString(ACTION)).ToList();
			Assert.Contains("TestRole", roles);
		}

		[Test]
		public void GetRoles_WithObject_ObjectPermissions()
		{
			var roles = manager.GetRoles(Permissable.FromString(ACTION), testObj).ToList();
			Assert.Contains("TestObjectRole", roles);
		}

		[Test]
		public void GetRoles_FallbackToDefaultRoles()
		{
			var roles = manager.GetRoles(Permissable.FromString("DifferentPermissable")).ToList();
			Assert.Contains("DefaultRole", roles);
		}

		[Test]
		public void GetRoles_WithObject_FallbackToDefaultPermissions()
		{
			var roles = manager.GetRoles(Permissable.FromString("FallbackPermissable"), testObj).ToList();
			Assert.Contains("FallbackRole", roles);
		}
	}
}
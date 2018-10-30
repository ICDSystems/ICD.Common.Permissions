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
		private PermissionsManager m_Manager;
		private Object m_TestObj;

		[SetUp]
		public void Init()
		{
			m_TestObj = new object();
			m_Manager = new PermissionsManager();
			m_Manager.SetDefaultPermissions(CreateDefaultPermissions());
			m_Manager.SetObjectPermissions(m_TestObj, CreateObjectPermissions());
			m_Manager.DefaultRoles = new [] {"DefaultRole"};
		}

		private static IEnumerable<Permission> CreateDefaultPermissions()
		{
			yield return new Permission {Permissable = Permissable.FromString(ACTION), Roles = new [] {"TestRole"}};
			yield return new Permission {Permissable = Permissable.FromString("FallbackPermissable"), Roles = new[] {"FallbackRole"}};
		}

		private static IEnumerable<Permission> CreateObjectPermissions()
		{
			yield return new Permission {Permissable = Permissable.FromString(ACTION), Roles = new [] {"TestObjectRole"}};
		}

		[Test]
		public void GetRoles_DefaultPermissions()
		{
			var roles = m_Manager.GetRoles(Permissable.FromString(ACTION)).ToList();
			Assert.Contains("TestRole", roles);
		}

		[Test]
		public void GetRoles_WithObject_ObjectPermissions()
		{
			var roles = m_Manager.GetRoles(Permissable.FromString(ACTION), m_TestObj).ToList();
			Assert.Contains("TestObjectRole", roles);
		}

		[Test]
		public void GetRoles_FallbackToDefaultRoles()
		{
			var roles = m_Manager.GetRoles(Permissable.FromString("DifferentPermissable")).ToList();
			Assert.Contains("DefaultRole", roles);
		}

		[Test]
		public void GetRoles_WithObject_FallbackToDefaultPermissions()
		{
			var roles = m_Manager.GetRoles(Permissable.FromString("FallbackPermissable"), m_TestObj).ToList();
			Assert.Contains("FallbackRole", roles);
		}
	}
}
using Basalt.Common.Components;

namespace Basalt.Common.Attributes
{

	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
	public sealed class ComponentDependentOnAttribute : Attribute
	{
		readonly Type[] _dependencies;
		public ComponentDependentOnAttribute(params Type[] dependencies)
		{
			_dependencies = dependencies;
		}

		public IEnumerable<Type> Dependencies => _dependencies;

	}
}

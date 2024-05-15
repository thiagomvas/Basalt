namespace Basalt.Core.Common.Attributes
{
	/// <summary>
	/// Marks a class as a singleton component. A singleton component can only have one instance per entity, but does not apply to children.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class SingletonComponentAttribute : Attribute
	{
		public SingletonComponentAttribute()
		{
		}
	}
}

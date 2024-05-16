namespace Basalt.Core.Common.Abstractions.Engine
{
	/// <summary>
	/// Represents an interface for a physics engine component.
	/// </summary>
	public interface IPhysicsEngine : IEngineComponent
	{
		/// <summary>
		/// Gets or sets the gravity value for the physics simulation.
		/// </summary>
		float Gravity { get; set; }

		/// <summary>
		/// Adds an entity to the physics simulation.
		/// </summary>
		/// <param name="entity">The entity to be added.</param>
		void AddEntityToSimulation(object entity);

		/// <summary>
		/// Removes an entity from the physics simulation.
		/// </summary>
		void RemoveEntityFromSimulation(object entity);

		/// <summary>
		/// Simulates the physics behavior.
		/// </summary>
		void Simulate();
	}
}

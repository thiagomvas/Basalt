namespace Basalt.Core.Common.Abstractions.Engine
{
	/// <summary>
	/// Represents an observer that can receive notifications from the engine.
	/// </summary>
	public interface IObserver
	{
		/// <summary>
		/// Called when the engine starts.
		/// </summary>
		void OnStartEvent();

		/// <summary>
		/// Called on each frame update of the engine.
		/// </summary>
		void OnUpdateEvent();

		/// <summary>
		/// Called on each physics update of the engine.
		/// </summary>
		void OnPhysicsUpdateEvent();

		/// <summary>
		/// Called on each frame render of the engine.
		/// </summary>
		void OnRenderEvent();
	}
}

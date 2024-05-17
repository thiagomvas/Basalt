namespace Basalt.Core.Common.Abstractions.Engine
{
	/// <summary>
	/// Represents an engine component.
	/// </summary>
	public interface IEngineComponent
	{
		/// <summary>
		///  The initialization logic called whenever the engine starts running.
		/// </summary>
		void Initialize();

		/// <summary>
		/// The shutdown logic called whenever the engine stops running.
		/// </summary>
		void Shutdown();
	}
}

namespace Basalt.Core.Common.Abstractions.Engine
{
	/// <summary>
	/// Represents a graphics engine component.
	/// </summary>
	public interface IGraphicsEngine : IEngineComponent
	{
		/// <summary>
		/// Renders the graphics.
		/// </summary>
		void Render();
	}
}

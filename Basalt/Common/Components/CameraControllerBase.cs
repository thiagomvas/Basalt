using Basalt.Common.Entities;

namespace Basalt.Common.Components
{
	/// <summary>
	/// Base class for camera controllers.
	/// </summary>
	/// <typeparam name="T">The type that represents a camera.</typeparam>
	public abstract class CameraControllerBase<T> : Component
	{
		/// <summary>
		/// Gets or sets the camera.
		/// </summary>
		public abstract T Camera { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="CameraControllerBase{T}"/> class.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public CameraControllerBase(Entity entity) : base(entity)
		{
		}
	}
}

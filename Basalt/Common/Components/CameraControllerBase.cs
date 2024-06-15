using Basalt.Common.Entities;

namespace Basalt.Common.Components
{
	public abstract class CameraControllerBase<T> : Component 
	{
		public abstract T Camera { get; set;  }
		public CameraControllerBase(Entity entity) : base(entity)
		{
		}

		public virtual void SetCamera(T camera)
		{
			Camera = camera;
		}
	}
}

using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Common.Exceptions;
using Basalt.Common.Utils;
using Basalt.Raylib.Graphics;
using Raylib_cs;
using System.Numerics;

namespace Basalt.Raylib.Components
{
	/// <summary>
	/// A particle system that renders particles using Raylib.
	/// </summary>
	public class RaylibParticleSystem : BaseParticleSystem
	{
		private string _modelCacheKey = "sphere";
		/// <summary>
		/// The <see cref="ResourceCache"/> cache key for the model to use for rendering particles.
		/// </summary>
		public string ModelCacheKey
		{
			get => _modelCacheKey;
			set
			{
				_modelCacheKey = value;
				init = false;
			}
		}
		bool init = false;
		Model model;
		public RaylibParticleSystem(Entity entity) : base(entity)
		{
		}

		protected override void RenderParticles()
		{
			if (!init)
			{
				init = true;
				var m = ResourceCache.Instance.GetModel(ModelCacheKey);
				if (m == null)
				{
					throw new InvalidResourceKeyException(nameof(ModelCacheKey), ModelCacheKey);
				}
				model = m.Value;

			}
			foreach (var particle in _particles)
			{
				model.Transform = Raymath.MatrixRotateXYZ(Raymath.QuaternionToEuler(particle.Rotation));
				Raylib_cs.Raylib.DrawModelEx(model, particle.Position, Vector3.UnitY, 0, particle.Scale, particle.Color.ToRaylibColor());
			}
		}
	}
}

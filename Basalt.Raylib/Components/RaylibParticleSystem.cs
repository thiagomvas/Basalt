using Basalt.Common.Components;
using Basalt.Common.Entities;
using Basalt.Common.Exceptions;
using Basalt.Common.Utils;
using Basalt.Raylib.Graphics;
using Basalt.Types;
using Raylib_cs;
using System.Numerics;

namespace Basalt.Raylib.Components
{
	public class RaylibParticleSystem : BaseParticleSystem
	{
		private string _modelCacheKey;
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

		public override void RenderParticles()
		{
			if(!init)
			{
				init = true;
				var m = ResourceCache.Instance.GetModel(ModelCacheKey);
				if(m == null)
				{
					throw new InvalidResourceKeyException($"{nameof(ModelCacheKey)}:{ModelCacheKey}");
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

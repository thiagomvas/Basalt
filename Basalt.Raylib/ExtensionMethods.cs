using Basalt.Common;
using Basalt.Common.Events;
using Basalt.Common.Physics;
using Basalt.Core.Common.Abstractions.Engine;
using Basalt.Core.Common.Abstractions.Input;
using Basalt.Core.Common.Abstractions.Sound;
using Basalt.Raylib.Graphics;
using Basalt.Raylib.Input;
using Basalt.Raylib.Sound;
using Basalt.Types;
using Raylib_cs;

namespace Basalt.Raylib
{
	public static class ExtensionMethods
	{
		/// <summary>
		/// Configures the <see cref="EngineBuilder"/> with default Raylib preset components using default <see cref="WindowInitParams"/> values.
		/// </summary>
		/// <param name="builder">The <see cref="EngineBuilder"/> to configure.</param>
		/// <returns>The configured <see cref="EngineBuilder"/>.</returns>
		/// <remarks>
		/// The following components are added to the builder:
		/// <list type="bullet">
		/// <item><description><see cref="IGraphicsEngine"/> as <see cref="RaylibGraphicsEngine"/></description></item>
		/// <item><description><see cref="IEventBus"/> as <see cref="EventBus"/></description></item>
		/// <item><description><see cref="IPhysicsEngine"/> as <see cref="PhysicsEngine"/></description></item>
		/// <item><description><see cref="IInputSystem"/> as <see cref="RaylibInputSystem"/></description></item>
		/// <item><description><see cref="ISoundSystem"/> as <see cref="RaylibSoundSystem"/></description></item>
		/// </list>
		/// </remarks>
		public static EngineBuilder UseRaylibPreset(this EngineBuilder builder)
		{
			WindowInitParams initParams = new();

			builder.AddComponent<IGraphicsEngine, RaylibGraphicsEngine>(() => new(initParams), true);
			builder.AddComponent<IEventBus, EventBus>();
			builder.AddComponent<IPhysicsEngine, PhysicsEngine>(true);
			builder.AddComponent<IInputSystem, RaylibInputSystem>();
			builder.AddComponent<ISoundSystem, RaylibSoundSystem>();
			return builder;
		}

		/// <summary>
		/// Configures the <see cref="EngineBuilder"/> with Raylib preset components using the specified <see cref="WindowInitParams"/>.
		/// </summary>
		/// <param name="builder">The <see cref="EngineBuilder"/> to configure.</param>
		/// <param name="initParams">The <see cref="WindowInitParams"/> to use for initializing the <see cref="RaylibGraphicsEngine"/>.</param>
		/// <returns>The configured <see cref="EngineBuilder"/>.</returns>
		/// <remarks>
		/// The following components are added to the builder:
		/// <list type="bullet">
		/// <item><description><see cref="IGraphicsEngine"/> as <see cref="RaylibGraphicsEngine"/></description></item>
		/// <item><description><see cref="IEventBus"/> as <see cref="EventBus"/></description></item>
		/// <item><description><see cref="IPhysicsEngine"/> as <see cref="PhysicsEngine"/></description></item>
		/// <item><description><see cref="IInputSystem"/> as <see cref="RaylibInputSystem"/></description></item>
		/// <item><description><see cref="ISoundSystem"/> as <see cref="RaylibSoundSystem"/></description></item>
		/// </list>
		/// </remarks>
		public static EngineBuilder UseRaylibPreset(this EngineBuilder builder, WindowInitParams initParams)
		{
			builder.AddComponent<IGraphicsEngine, RaylibGraphicsEngine>(() => new(initParams), true);
			builder.AddComponent<IEventBus, EventBus>();
			builder.AddComponent<IPhysicsEngine, PhysicsEngine>(true);
			builder.AddComponent<IInputSystem, RaylibInputSystem>();
			builder.AddComponent<ISoundSystem, RaylibSoundSystem>();
			return builder;
		}

		public static Raylib_cs.Color ToRaylibColor(this System.Drawing.Color color) => new(color.R, color.G, color.B, color.A);
	}
}

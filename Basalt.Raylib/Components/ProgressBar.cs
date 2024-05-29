using Basalt.Common.Components;
using Basalt.Common.Entities;
using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;

namespace Basalt.Raylib.Components
{
	/// <summary>
	/// Represents a progress bar UI component.
	/// </summary>
	public class ProgressBar : UIComponent
	{
		/// <summary>
		/// Gets or sets the size of the progress bar.
		/// </summary>
		public Vector2 Size { get; set; } = Vector2.One;

		/// <summary>
		/// Gets or sets the background color of the progress bar.
		/// </summary>
		public Color BackgroundColor { get; set; } = Color.White;

		/// <summary>
		/// Gets or sets the foreground color of the progress bar.
		/// </summary>
		public Color ForegroundColor { get; set; } = Color.Green;

		/// <summary>
		/// Gets or sets the progress value of the progress bar.
		/// </summary>
		public float Progress { get; set; } = 0.5f;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProgressBar"/> class.
		/// </summary>
		/// <param name="entity">The entity that the progress bar belongs to.</param>
		public ProgressBar(Entity entity) : base(entity)
		{
		}

		/// <inheritdoc/>
		public override void OnStart()
		{

		}

		/// <inheritdoc/>
		public override void OnUpdate()
		{

		}

		/// <inheritdoc/>
		public override void OnUIRender()
		{
			var position = GetPivotedPosition(new(GetScreenWidth(), GetScreenHeight())) + Offset;
			DrawRectanglePro(new Rectangle(position.X, position.Y, Size.X, Size.Y), Size / 2, Rotation, BackgroundColor);
			DrawRectanglePro(new Rectangle(position.X, position.Y, Size.X * Progress, Size.Y), Size / 2, Rotation, ForegroundColor);
		}
	}
}

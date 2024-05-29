using Basalt.Common.Entities;
using Basalt.Common.Utils;
using Basalt.Core.Common.Abstractions.Engine;
using Basalt.Types;
using System.Numerics;

namespace Basalt.Common.Components
{
	/// <summary>
	/// Represents a base class for UI components.
	/// </summary>
	public abstract class UIComponent : Component
	{
		/// <summary>
		/// Gets or sets the pivot point of the UI component.
		/// </summary>
		public UIPivot Pivot { get; set; } = UIPivot.TopLeft;

		/// <summary>
		/// Gets or sets the offset of the UI component.
		/// </summary>
		public Vector2 Offset { get; set; }

		/// <summary>
		/// Gets or sets the Z-index of the UI component.
		/// </summary>
		public float ZIndex { get; set; }

		/// <summary>
		/// Gets or sets the rotation of the UI component.
		/// </summary>
		public float Rotation { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="UIComponent"/> class.
		/// </summary>
		/// <param name="entity">The entity that the UI component belongs to.</param>
		protected UIComponent(Entity entity) : base(entity)
		{
		}

		/// <summary>
		/// Called when the UI component needs to be rendered.
		/// </summary>
		public virtual void OnUIRender()
		{
		}

		private void OnUIRenderEvent(object? sender, EventArgs args)
		{
			if (Entity.Enabled && Enabled)
				OnUIRender();
		}

		/// <inheritdoc/>
		private protected override void SubscribeToEvents()
		{
			base.SubscribeToEvents();
			Engine.Instance.GetEngineComponent<IEventBus>()!.Subscribe(BasaltConstants.UiRenderEventKey, OnUIRenderEvent);
		}

		/// <inheritdoc/>
		private protected override void UnsubscribeFromEvents()
		{
			base.UnsubscribeFromEvents();
			Engine.Instance.GetEngineComponent<IEventBus>()!.Unsubscribe(BasaltConstants.UiRenderEventKey, OnUIRenderEvent);
		}

		/// <summary>
		/// Gets the position of the UI component based on the pivot point and screen size.
		/// </summary>
		/// <param name="screenSize">The size of the screen.</param>
		/// <returns>The position of the UI component.</returns>
		protected Vector2 GetPivotedPosition(Vector2 screenSize)
		{
			switch (Pivot)
			{
				case UIPivot.TopLeft:
					return new Vector2(0, 0);
				case UIPivot.TopCenter:
					return new Vector2(screenSize.X / 2, 0);
				case UIPivot.TopRight:
					return new Vector2(screenSize.X, 0);
				case UIPivot.MiddleLeft:
					return new Vector2(0, screenSize.Y / 2);
				case UIPivot.MiddleCenter:
					return new Vector2(screenSize.X / 2, screenSize.Y / 2);
				case UIPivot.MiddleRight:
					return new Vector2(screenSize.X, screenSize.Y / 2);
				case UIPivot.BottomLeft:
					return new Vector2(0, screenSize.Y);
				case UIPivot.BottomCenter:
					return new Vector2(screenSize.X / 2, screenSize.Y);
				case UIPivot.BottomRight:
					return new Vector2(screenSize.X, screenSize.Y);
				default:
					return Vector2.Zero;
			}
		}
	}
}

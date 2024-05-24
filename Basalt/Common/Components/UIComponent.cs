using Basalt.Common.Entities;
using Basalt.Common.Utils;
using Basalt.Core.Common.Abstractions.Engine;
using Basalt.Types;
using System.Numerics;

namespace Basalt.Common.Components
{
	public abstract class UIComponent : Component
	{
		public UIPivot Pivot { get; set; } = UIPivot.TopLeft;
		public Vector2 Offset { get; set; }
		public float ZIndex { get; set; }
		protected UIComponent(Entity entity) : base(entity)
		{
		}

		public virtual void OnUIRender()
		{
		}

		private void OnUIRenderEvent(object? sender, EventArgs args)
		{
			if (Entity.Enabled && Enabled)
				OnUIRender();
		}

		private protected override void SubscribeToEvents()
		{
			base.SubscribeToEvents();
			Engine.Instance.GetEngineComponent<IEventBus>()!.Subscribe(BasaltConstants.UiRenderEventKey, OnUIRenderEvent);
		}

		private protected override void UnsubscribeFromEvents()
		{
			base.UnsubscribeFromEvents();
			Engine.Instance.GetEngineComponent<IEventBus>()!.Unsubscribe(BasaltConstants.UiRenderEventKey, OnUIRenderEvent);
		}

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

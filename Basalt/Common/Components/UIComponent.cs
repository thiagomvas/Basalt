using Basalt.Common.Entities;
using Basalt.Common.Utils;
using Basalt.Core.Common.Abstractions.Engine;
using Basalt.Types;
using System.Numerics;

namespace Basalt.Common.Components
{
	public abstract class UIComponent : Component
	{
		public UIPivot Pivot { get; set; }
		public Vector3 Offset { get; set; }
		public Vector3 Scale { get; set; }
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


	}
}

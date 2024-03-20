using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Basalt.Common.Entities;
using Basalt.Core.Common.Abstractions;

namespace Basalt.Common.Components
{
    public abstract class Component : IObserver
    {
        public Entity Entity;
		protected Component(Entity entity)
        {
            this.Entity = entity;
            Engine.Instance.EventBus?.Subscribe(this);

            if(Engine.Instance.HasStarted)
            {
				OnStart();
			}
        }

        public virtual void OnRender()
        {
        }

		abstract public void OnStart();
        abstract public void OnUpdate();

        public virtual void OnPhysicsUpdate()
        {
		}

        internal void onDestroy()
        {
            Engine.Instance.EventBus?.Unsubscribe(this);
            OnDestroy();
		}

        public virtual void OnDestroy()
        { }
    }
}

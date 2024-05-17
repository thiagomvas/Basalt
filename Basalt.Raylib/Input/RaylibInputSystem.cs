using Basalt.Core.Common.Abstractions.Input;
using Raylib_cs;
using static Raylib_cs.Raylib;
namespace Basalt.Raylib.Input
{
	/// <summary>
	/// Represents a Raylib input system.
	/// </summary>
	public class RaylibInputSystem : IInputSystem
	{
		private Dictionary<InputAction, Action> _actions = new();
		private Dictionary<InputAction, Action> registerQueue = new();
		private object _lock = new object();

		/// <inheritdoc/>
		public void Initialize()
		{
			// TODO: Implement initialization logic
		}

		/// <inheritdoc/>
		public void RegisterKeybind(InputAction input, Action action)
		{
			lock (_lock)
			{
				Engine.Instance.Logger?.LogDebug($"Registering keybind {input.Key} with action {action.Method.Name}");
				registerQueue.Add(input, action);
			}
		}

		/// <inheritdoc/>
		public void ReplaceKeybind(InputAction oldKey, InputAction newKey)
		{
			lock (_lock)
			{
				if (_actions.ContainsKey(oldKey))
				{
					_actions.Add(newKey, _actions[oldKey]);
					_actions.Remove(oldKey);
				}
			}
		}

		/// <inheritdoc/>
		public void Shutdown()
		{

		}

		/// <inheritdoc/>
		public void Update()
		{
			lock (_lock)
			{
				foreach (var (input, action) in registerQueue)
				{
					_actions.Add(input, action);
				}
				registerQueue.Clear();
			}

			foreach (var (input, action) in _actions)
			{
				switch (input.Type)
				{
					case ActionType.Press:
						if (IsKeyPressed((KeyboardKey)input.Key))
						{
							action?.Invoke();
						}
						break;
					case ActionType.Release:
						if (IsKeyReleased((KeyboardKey)input.Key))
						{
							action?.Invoke();
						}
						break;
					case ActionType.Hold:
						if (IsKeyDown((KeyboardKey)input.Key))
						{
							action?.Invoke();
						}
						break;
				}
			}
		}
	}
}

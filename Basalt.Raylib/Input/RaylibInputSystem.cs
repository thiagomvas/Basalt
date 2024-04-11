using Basalt.Core.Common.Abstractions.Input;
using Basalt.Core.Common.Types;
using Raylib_cs;
using static Raylib_cs.Raylib;
namespace Basalt.Raylib.Input
{
	public class RaylibInputSystem : IInputSystem
	{
		private Dictionary<InputAction, Action> _actions = new();

		public void Initialize()
		{

		}

		public void RegisterKeybind(InputAction input, Action action) => _actions.Add(input, action);

		public void ReplaceKeybind(InputAction oldKey, InputAction newKey)
		{
			if (_actions.ContainsKey(oldKey))
			{
				_actions.Add(newKey, _actions[oldKey]);
				_actions.Remove(oldKey);
			}
		}

		public void Shutdown()
		{

		}

		public void Update()
		{
			foreach (var (input, action) in _actions)
			{
				switch (input.Type)
				{
					case ActionType.Press:
						if (IsKeyPressed((KeyboardKey)input.Key))
						{
							action?.Invoke() ;
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

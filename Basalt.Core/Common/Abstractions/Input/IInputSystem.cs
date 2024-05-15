using Basalt.Core.Common.Abstractions.Engine;
namespace Basalt.Core.Common.Abstractions.Input
{
	/// <summary>
	/// Represents an input system in the game engine.
	/// </summary>
	public interface IInputSystem : IEngineComponent
	{
		/// <summary>
		/// Updates the input system.
		/// </summary>
		void Update();

		/// <summary>
		/// Registers a keybind with the specified input action and action.
		/// </summary>
		/// <param name="key">The input action associated with the keybind.</param>
		/// <param name="action">The action to be performed when the keybind is triggered.</param>
		void RegisterKeybind(InputAction key, Action action);

		/// <summary>
		/// Replaces an existing keybind with a new keybind.
		/// </summary>
		/// <param name="oldKey">The input action associated with the keybind to be replaced.</param>
		/// <param name="newKey">The input action associated with the new keybind.</param>
		void ReplaceKeybind(InputAction oldKey, InputAction newKey);
	}
}

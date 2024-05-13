using Basalt.Core.Common.Types;
namespace Basalt.Core.Common.Abstractions.Input
{
	/// <summary>
	/// Represents an input action.
	/// </summary>
	public struct InputAction
	{
		/// <summary>
		/// Gets or sets the input key associated with the action.
		/// </summary>
		public InputKey Key { get; set; }

		/// <summary>
		/// Gets or sets the type of the action.
		/// </summary>
		public ActionType Type { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="InputAction"/> struct.
		/// </summary>
		/// <param name="key">The input key associated with the action.</param>
		/// <param name="type">The type of the action.</param>
		public InputAction(InputKey key, ActionType type)
		{
			Key = key;
			Type = type;
		}
	}
}

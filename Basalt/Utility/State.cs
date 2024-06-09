namespace Basalt.Utility
{
	/// <summary>
	/// Represents an abstract state.
	/// </summary>
	/// <typeparam name="T">The type of the owner.</typeparam>
	public abstract class State<T>
	{
		protected T Owner;

		/// <summary>
		/// Initializes a new instance of the <see cref="State{T}"/> class.
		/// </summary>
		/// <param name="Owner">The owner of the state.</param>
		protected State(T Owner)
		{
			this.Owner = Owner;
		}

		/// <summary>
		/// Called when entering the state.
		/// </summary>
		public virtual void Enter() { }

		/// <summary>
		/// Called when updating the state.
		/// </summary>
		public virtual void Update() { }

		/// <summary>
		/// Called when exiting the state.
		/// </summary>
		public virtual void Exit() { }
	}
}

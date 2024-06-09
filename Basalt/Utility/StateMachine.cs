namespace Basalt.Utility
{
	/// <summary>
	/// Represents a state machine that manages the state transitions for a given type.
	/// </summary>
	/// <typeparam name="T">The type of the owner of the state machine.</typeparam>
	public class StateMachine<T>
	{
		private readonly T owner;
		/// <summary>
		/// Gets the current state of the state machine.
		/// </summary>
		/// <remarks>
		/// The first state added to the state machine is set as the current state by default.
		/// If no states have been added, this property will return <see langword="null"/>.
		/// </remarks>
		public State<T>? CurrentState { get; private set; }
		private readonly Dictionary<Type, State<T>> states;

		/// <summary>
		/// Initializes a new instance of the <see cref="StateMachine{T}"/> class.
		/// </summary>
		/// <param name="owner">The owner of the state machine.</param>
		public StateMachine(T owner)
		{
			this.owner = owner;
			states = new Dictionary<Type, State<T>>();
		}

		/// <summary>
		/// Adds a state to the state machine.
		/// </summary>
		/// <param name="state">The state to add.</param>
		public void AddState(State<T> state)
		{
			states.TryAdd(state.GetType(), state);
			CurrentState ??= state;
		}

		/// <summary>
		/// Changes the current state of the state machine to the specified state.
		/// </summary>
		/// <typeparam name="S">The type of the state to change to.</typeparam>
		public void ChangeState<S>() where S : State<T>
		{
			CurrentState?.Exit();
			CurrentState = states[typeof(S)];
			CurrentState.Enter();
		}

		/// <summary>
		/// Updates the current state of the state machine.
		/// </summary>
		public void Update()
		{
			CurrentState?.Update();
		}

		/// <summary>
		/// Exits the current state of the state machine.
		/// </summary>
		public void Exit()
		{
			CurrentState?.Exit();
		}

		/// <summary>
		/// Gets an array of all the states in the state machine.
		/// </summary>
		/// <returns>An array of <see cref="State{T}"/> objects.</returns>
		public State<T>[] GetStates()
		{
			return states.Values.ToArray();
		}

		/// <summary>
		/// Checks if the current state is of the specified type.
		/// </summary>
		/// <typeparam name="S">The state type to check for</typeparam>
		/// <returns><see langword="true"/> if the current state is of type <typeparamref name="S"/>, <see langword="false"/> otherwise</returns>
		public bool IsInState<S>() where S : State<T>
		{
			return CurrentState is S;
		}
	}
}

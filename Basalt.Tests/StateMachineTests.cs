using NUnit.Framework;
using Moq;
using Basalt.Utility;

namespace Basalt.Tests
{
	[TestFixture]
	public class StateMachineTests
	{
		private StateMachine<int> stateMachine;
		private Mock<State<int>> stateMock;

		[SetUp]
		public void Setup()
		{
			stateMachine = new StateMachine<int>(42);
			stateMock = new Mock<State<int>>(42);
		}

		[Test]
		public void AddState_ShouldAddStateToStateMachine()
		{
			// Arrange
			stateMachine.AddState(stateMock.Object);

			// Act
			var states = stateMachine.GetStates();

			// Assert
			Assert.That(states, Does.Contain(stateMock.Object));
		}

		[Test]
		public void ChangeState_ShouldChangeCurrentState()
		{
			// Arrange
			var secondState = new TestState(42);
			stateMachine.AddState(stateMock.Object);
			stateMachine.AddState(secondState);

			// Act
			stateMachine.ChangeState<TestState>();

			// Assert
			Assert.That(stateMachine.CurrentState, Is.EqualTo(secondState));
		}

		[Test]
		public void Update_ShouldCallUpdateOnCurrentState()
		{
			// Arrange
			stateMachine.AddState(stateMock.Object);

			// Act
			stateMachine.Update();

			// Assert
			stateMock.Verify(s => s.Update(), Times.Once);
		}

		[Test]
		public void Exit_ShouldCallExitOnCurrentState()
		{
			// Arrange
			stateMachine.AddState(stateMock.Object);

			// Act
			stateMachine.Exit();

			// Assert
			stateMock.Verify(s => s.Exit(), Times.Once);
		}

		private class TestState : State<int>
		{
			public TestState(int Owner) : base(Owner) { }
		}

	}
}

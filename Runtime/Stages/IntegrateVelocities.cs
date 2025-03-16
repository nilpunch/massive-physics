using System.Runtime.CompilerServices;
using Mathematics.Fixed;
using Unity.IL2CPP.CompilerServices;

namespace Massive.Physics
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public readonly struct IntegrateVelocities : IEntityAction<Body>
	{
		private readonly FP _deltaTime;
		private readonly FVector3 _gravity;

		public IntegrateVelocities(FP deltaTime, FVector3 gravity)
		{
			_deltaTime = deltaTime;
			_gravity = gravity;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Apply(int id, ref Body body)
		{
			var linearDamping = FP.One / (FP.One + _deltaTime * body.LinearDamping);
			var angularDamping = FP.One / (FP.One + _deltaTime * body.AngularDamping);

			var linearVelocity = body.LinearVelocity;
			var angularVelocity = body.AngularVelocity;

			var linearVelocityDelta =
				_deltaTime * body.InvMass * body.Force +
				_deltaTime * _gravity * body.GravityScale;

			var angularVelocityDelta = _deltaTime * body.InvInertiaTensor.MultiplyVector(body.Torque);

			linearVelocity = linearVelocityDelta + linearVelocity * linearDamping;
			angularVelocity = angularVelocityDelta + angularVelocity * angularDamping;

			body.LinearVelocity = linearVelocity;
			body.AngularVelocity = angularVelocity;

			body.Force = FVector3.Zero;
			body.Torque = FVector3.Zero;

			return true;
		}
	}
}

using System.Runtime.CompilerServices;
using Mathematics.Fixed;
using Unity.IL2CPP.CompilerServices;

namespace Massive.Physics
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public readonly struct IntegratePositions : IEntityAction<Body>
	{
		private readonly FP _deltaTime;

		public IntegratePositions(FP deltaTime)
		{
			_deltaTime = deltaTime;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Apply(int id, ref Body body)
		{
			body.Rotation = BodyUtils.IntegrateRotation(body.Rotation, _deltaTime * body.AngularVelocity);
			body.Position = body.Position + _deltaTime * body.LinearVelocity;
			return true;
		}
	}
}

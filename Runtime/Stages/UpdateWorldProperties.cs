using System.Runtime.CompilerServices;
using Mathematics.Fixed;
using Unity.IL2CPP.CompilerServices;

namespace Massive.Physics
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public readonly struct UpdateWorldProperties : IEntityAction<Body>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Apply(int id, ref Body body)
		{
			var rotation = FMat3.FromQuaternion(body.Rotation);
			body.InvInertiaTensor = rotation * body.LocalInvInertiaTensor * FMat3.Transpose(rotation);

			body.Center = body.LocalToWorld(body.LocalCenter);

			return true;
		}
	}
}

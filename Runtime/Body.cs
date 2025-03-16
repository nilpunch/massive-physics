using Mathematics.Fixed;

namespace Massive.Physics
{
	public struct Body
	{
		/// <summary>
		/// Position for body origin.
		/// </summary>
		public FVector3 Position;

		/// <summary>
		/// Position for body origin.
		/// </summary>
		public FQuaternion Rotation;

		/// <summary>
		/// Center of mass position in world space.
		/// </summary>
		public FVector3 Center;

		/// <summary>
		/// Location of center of mass relative to the body origin.
		/// </summary>
		public FVector3 LocalCenter;

		public FVector3 LinearVelocity;
		public FVector3 AngularVelocity;

		public FP InvMass;

		/// <summary>
		/// Inertia tensor in world space.
		/// </summary>
		public FMat3 InvInertiaTensor;

		public FMat3 LocalInvInertiaTensor;

		public FVector3 Force;
		public FVector3 Torque;

		public FP GravityScale;

		public FP LinearDamping;
		public FP AngularDamping;

		public static Body Create(FVector3 position, FQuaternion rotation, FP mass, FMat3 inertiaTensor)
		{
			var body = new Body()
			{
				Position = position,
				Rotation = rotation,
				InvMass = FP.One / mass,
				LocalInvInertiaTensor = FMat3.Inverse(inertiaTensor),
				GravityScale = FP.One
			};

			new UpdateWorldProperties().Apply(0, ref body);

			return body;
		}
	}
}

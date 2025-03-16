using Mathematics.Fixed;

namespace Massive.Physics
{
	public struct Transform
	{
		public FVector3 Position;
		public FQuaternion Rotation;

		public Transform(FVector3 position, FQuaternion rotation)
		{
			Position = position;
			Rotation = rotation;
		}
	}
}
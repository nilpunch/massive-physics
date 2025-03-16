using Mathematics.Fixed;

namespace Massive.Physics
{
	public static class MassUtils
	{
		public static FMat3 SphereInertia(FP radius, FP mass)
		{
			var diagonalInertia = FVector3.One * (FP.FromRatio(2, 5) * mass * radius * radius);
			return FMat3.FromDiagonal(diagonalInertia);
		}

		public static FP SphereMass(FP radius, FP density)
		{
			return FP.FromRatio(4, 3) * FP.Pi * radius * radius * radius * density;
		}
	}
}

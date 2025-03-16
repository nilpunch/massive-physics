using System.Runtime.CompilerServices;
using Mathematics.Fixed;

namespace Massive.Physics
{
	public struct Softness
	{
		public FP BiasRate;
		public FP MassScale;
		public FP ImpulseScale;

		public Softness(FP biasRate, FP massScale, FP impulseScale)
		{
			BiasRate = biasRate;
			MassScale = massScale;
			ImpulseScale = impulseScale;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Softness Calculate(FP hertz, FP zeta, FP deltaTime)
		{
			if (hertz == 0)
			{
				return new Softness(FP.Zero, FP.One, FP.Zero);
			}

			var omega = 2 * FP.Pi * hertz;
			var a1 = 2 * zeta + deltaTime * omega;
			var a2 = deltaTime * omega * a1;
			var a3 = FP.One / (FP.One + a2);
			return new Softness(omega / a1, a2 * a3, a3);
		}
	}
}
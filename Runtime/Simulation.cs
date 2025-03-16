using Mathematics.Fixed;

namespace Massive.Physics
{
	public static class Simulation
	{
		public static void IntegrateVelocities(Registry registry, FP deltaTime, FVector3 gravity)
		{
			var integration = new IntegrateVelocities(deltaTime, gravity);
			registry.View().ForEach<IntegrateVelocities, Body>(ref integration);
		}

		public static void IntegratePositions(Registry registry, FP deltaTime)
		{
			var integration = new IntegratePositions(deltaTime);
			registry.View().ForEach<IntegratePositions, Body>(ref integration);
		}

		public static void UpdateWorldProperties(Registry registry)
		{
			var integration = new UpdateWorldProperties();
			registry.View().ForEach<UpdateWorldProperties, Body>(ref integration);
		}
	}
}

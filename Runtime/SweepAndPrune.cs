using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Massive;
using Unity.IL2CPP.CompilerServices;

namespace Mathematics.Fixed
{
	public struct AABB
	{
		public FVector3 LowerBound;
		public FVector3 UpperBound;
	}

	public struct BroadPhasePair
	{
		public int EntityA;
		public int EntityB;

		public BroadPhasePair(int entityA, int entityB)
		{
			EntityA = entityA;
			EntityB = entityB;
		}
	}

	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	public class SweepAndPrune : ISystem, IInjectSelf<SweepAndPrune>, IInject<World>
	{
		public struct SAPState
		{
			public ListPointer<int> Sorted;
			public ListPointer<AABB> AABBs;
			public ListPointer<int> EntityIds;
		}

		private World World { get; set; }
		private Allocator Allocator { get; set; }
		private WorkableList<int> Sorted;
		private WorkableList<AABB> AABBs;
		private WorkableList<int> EntityIds;

		void ISystem.Build(int id, Allocator allocator)
		{
			Allocator = allocator;
			Sorted = allocator.AllocList<int>();
			AABBs = allocator.AllocList<AABB>();
			EntityIds = allocator.AllocList<int>();
		}

		void IInject<World>.Inject(World world)
		{
			World = world;
		}

		public void Update()
		{
			AABBs.Clear();
			EntityIds.Clear();

			World.ForEach(this, static (int id, ref AABB aabb, SweepAndPrune self) =>
			{
				self.AABBs.Add(aabb);
				self.EntityIds.Add(id);
			});

			for (var i = Sorted.Count; i < AABBs.Count; i++)
			{
				Sorted.Add(i);
			}

			if (Sorted.Count > AABBs.Count)
			{
				var writeIndex = 0;
				for (var readIndex = 0; readIndex < Sorted.Count; readIndex++)
				{
					if (Sorted[readIndex] < AABBs.Count)
					{
						Sorted[writeIndex] = Sorted[readIndex];
						writeIndex++;
					}
				}

				Sorted.Count = AABBs.Count;
			}

			InsertionSortX();
		}

		public void FindOverlappingEntities(List<BroadPhasePair> result)
		{
			result.Clear();

			for (var i = 0; i < AABBs.Count; i++)
			{
				var aIndex = Sorted[i];
				var a = AABBs[aIndex];

				for (var j = i + 1; j < AABBs.Count; j++)
				{
					var bIndex = Sorted[j];
					var b = AABBs[bIndex];

					if (a.UpperBound.X < b.LowerBound.X)
					{
						break;
					}

					if (a.UpperBound.Z >= b.LowerBound.Z && b.UpperBound.Z >= a.LowerBound.Z
						&& a.UpperBound.Y >= b.LowerBound.Y && b.UpperBound.Y >= a.LowerBound.Y)
					{
						result.Add(new BroadPhasePair(EntityIds[aIndex], EntityIds[bIndex]));
					}
				}
			}
		}

		private void InsertionSortX()
		{
			for (var i = 1; i < AABBs.Count; i++)
			{
				var key = Sorted[i];
				var keyX = AABBs[key].LowerBound.X;

				var j = i - 1;
				while (j >= 0 && AABBs[Sorted[j]].LowerBound.X > keyX)
				{
					Sorted[j + 1] = Sorted[j];
					j--;
				}

				Sorted[j + 1] = key;
			}
		}
	}
}

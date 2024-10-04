/**
* Code generation. Don't modify! 
 */
using System.Runtime.CompilerServices;
using Atomic.AI;
using UnityEngine;
using AI.Goap;
namespace Game
{
    public static class BlackboardAPI
    {
        public const int Character = 1; // GameObject : class
        public const int GoapAgent = 2; // GoapAgent : class
        public const int Enemy = 3; // GameObject : class
        public const int MeleeDistance = 4; // float
        public const int RangeDistance = 5; // float


        ///Extensions
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasCharacter(this IBlackboard obj) => obj.HasObject(Character);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GameObject  GetCharacter(this IBlackboard obj) => obj.GetObject<GameObject >(Character);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryGetCharacter(this IBlackboard obj, out GameObject  value) => obj.TryGetObject(Character, out value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SetCharacter(this IBlackboard obj, GameObject  value) => obj.SetObject(Character, value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool DelCharacter(this IBlackboard obj) => obj.DelObject(Character);


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasGoapAgent(this IBlackboard obj) => obj.HasObject(GoapAgent);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GoapAgent  GetGoapAgent(this IBlackboard obj) => obj.GetObject<GoapAgent >(GoapAgent);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryGetGoapAgent(this IBlackboard obj, out GoapAgent  value) => obj.TryGetObject(GoapAgent, out value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SetGoapAgent(this IBlackboard obj, GoapAgent  value) => obj.SetObject(GoapAgent, value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool DelGoapAgent(this IBlackboard obj) => obj.DelObject(GoapAgent);


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasEnemy(this IBlackboard obj) => obj.HasObject(Enemy);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GameObject  GetEnemy(this IBlackboard obj) => obj.GetObject<GameObject >(Enemy);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryGetEnemy(this IBlackboard obj, out GameObject  value) => obj.TryGetObject(Enemy, out value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SetEnemy(this IBlackboard obj, GameObject  value) => obj.SetObject(Enemy, value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool DelEnemy(this IBlackboard obj) => obj.DelObject(Enemy);


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasMeleeDistance(this IBlackboard obj) => obj.HasFloat(MeleeDistance);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float GetMeleeDistance(this IBlackboard obj) => obj.GetFloat(MeleeDistance);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryGetMeleeDistance(this IBlackboard obj, out float value) => obj.TryGetFloat(MeleeDistance, out value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SetMeleeDistance(this IBlackboard obj, float value) => obj.SetFloat(MeleeDistance, value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool DelMeleeDistance(this IBlackboard obj) => obj.DelFloat(MeleeDistance);


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasRangeDistance(this IBlackboard obj) => obj.HasFloat(RangeDistance);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float GetRangeDistance(this IBlackboard obj) => obj.GetFloat(RangeDistance);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryGetRangeDistance(this IBlackboard obj, out float value) => obj.TryGetFloat(RangeDistance, out value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SetRangeDistance(this IBlackboard obj, float value) => obj.SetFloat(RangeDistance, value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool DelRangeDistance(this IBlackboard obj) => obj.DelFloat(RangeDistance);

    }
}

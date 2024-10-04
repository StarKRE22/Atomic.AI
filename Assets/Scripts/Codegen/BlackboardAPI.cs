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

    }
}

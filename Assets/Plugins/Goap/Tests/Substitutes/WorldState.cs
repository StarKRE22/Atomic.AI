using System.Collections.Generic;

namespace AI.Goap
{
    public static partial class Substitutes
    {
        public static KeyValuePair<string, bool> EnemyAlive(bool value) => new(nameof(EnemyAlive), value);
        public static KeyValuePair<string, bool> AtEnemy(bool value) => new(nameof(AtEnemy), value);
        public static KeyValuePair<string, bool> NearEnemy(bool value) => new(nameof(NearEnemy), value);
    }
}
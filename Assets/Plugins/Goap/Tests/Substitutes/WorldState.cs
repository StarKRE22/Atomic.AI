using System.Collections.Generic;

namespace AI.Goap
{
    public static partial class Substitutes
    {
        public static KeyValuePair<string, bool> Injured(bool value) => new(nameof(Injured), value);
        
        public static KeyValuePair<string, bool> HasAmmo(bool value) => new(nameof(HasAmmo), value);
        public static KeyValuePair<string, bool> EnemyExists(bool value) => new(nameof(EnemyExists), value);
        public static KeyValuePair<string, bool> AtEnemy(bool value) => new(nameof(AtEnemy), value);
        public static KeyValuePair<string, bool> NearEnemy(bool value) => new(nameof(NearEnemy), value);
        
        public static KeyValuePair<string, bool> ResourceExists(bool value) => new(nameof(ResourceExists), value);
        public static KeyValuePair<string, bool> AtResource(bool value) => new(nameof(AtResource), value);
        public static KeyValuePair<string, bool> ResourcesCollected(bool value) => new(nameof(ResourcesCollected), value);
    }
}
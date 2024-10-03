using System.Collections.Generic;

namespace AI.Goap
{
    public interface IGoapAction
    {
        string Name { get; }
        
        IGoapState Effects { get; }
        IGoapState Conditions { get; }

        bool IsValid { get; }
        int Cost { get; }
        bool IsRunning { get; }
        
        Result Run(in float deltaTime);
        bool Cancel();
        
        public enum Result : byte
        {
            RUNNING = 0,
            SUCCESS = 1,
            FAILURE = 2
        }
    }
}



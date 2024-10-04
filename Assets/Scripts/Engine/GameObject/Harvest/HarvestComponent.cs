using System;
using System.Collections;
using UnityEngine;

namespace Game.Engine
{
    public sealed class HarvestComponent : MonoBehaviour
    {
        public event Action OnStarted;
        public event Action OnEnded;
        
        public bool IsHarvesting => _coroutine != null;

        [SerializeField]
        private float duration = 0.5f;

        [SerializeField]
        private float postDelay =0.25f;

        private readonly AndCondition condition = new();
        
        private Coroutine _coroutine;
        private Action _processAction;
        
        public void SetProcessAction(Action action)
        {
            _processAction = action;
        }
        
        public void AddCondition(Func<bool> condition)
        {
            this.condition.AddCondition(condition);
        }
        
        public bool StartHarvest()
        {
            if (_coroutine != null)
            {
                return false;
            }

            if (!this.condition.Invoke())
            {
                return false; 
            }

            _coroutine = this.StartCoroutine(this.HarvestRoutine());
            this.OnStarted?.Invoke();
            return true;
        }

        public bool CancelHarvest()
        {
            if (_coroutine == null)
            {
                return false;
            }

            this.StopCoroutine(_coroutine);
            _coroutine = null;
            return true;
        }
        
        private IEnumerator HarvestRoutine()
        {
            yield return new WaitForSeconds(this.duration);
            _processAction?.Invoke();
            yield return new WaitForSeconds(this.postDelay);
            
            _coroutine = null;
            this.OnEnded?.Invoke();
        }
    }
}
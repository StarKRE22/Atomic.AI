using UnityEngine;

namespace Game.Engine
{
    [RequireComponent(typeof(Animator))]
    public sealed class HarvestAnimMechanics : MonoBehaviour
    {
        private static readonly int ChopAnimHash = Animator.StringToHash("Chop");

        private Animator _animator;

        [SerializeField]
        private HarvestComponent _harvestComponent;

        private void Awake()
        {
            _animator = this.GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _harvestComponent.OnStarted += this.OnHarvestStarted;
        }

        private void OnDisable()
        {
            _harvestComponent.OnStarted -= this.OnHarvestStarted;
        }

        private void OnHarvestStarted()
        {
            _animator.SetTrigger(ChopAnimHash);
        }
    }
}
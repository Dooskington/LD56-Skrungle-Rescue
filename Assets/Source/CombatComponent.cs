using UnityEngine;

public class CombatComponent : MonoBehaviour
{
    [SerializeField] private BoxCollider _attackCollider;
    [SerializeField] private float _attackDuration = 0.75f;
    [SerializeField] private float _attackCooldown = 0.25f;
    [SerializeField] private float _damage = 1.0f;

    [Header("Animation")]
    [SerializeField] private Animator _animator;

    private float _lastAttackTime;
    private bool _isAttacking;

    private void Update()
    {
        if (_isAttacking && ((Time.time - _lastAttackTime) > _attackDuration))
        {
            _isAttacking = false;
            _attackCollider.gameObject.SetActive(false);
        }
    }

    private void CheckAttackHitbox()
    {
        Collider[] hitColliders = Physics.OverlapBox(_attackCollider.transform.position, _attackCollider.size / 2.0f, _attackCollider.transform.rotation);
        foreach (Collider hitCollider in hitColliders)
        {
            HealthComponent healthComponent = hitCollider.GetComponent<HealthComponent>();
            if (!healthComponent)
            {
                continue;
            }

            healthComponent.TakeDamage(_damage);
        }
    }

    public void Attack()
    {
        if ((Time.time - _lastAttackTime) < (_attackDuration + _attackCooldown))
        {
            return;
        }

        _lastAttackTime = Time.time;
        _isAttacking = true;
        _attackCollider.gameObject.SetActive(true);

        CheckAttackHitbox();

        if (_animator != null)
        {
            _animator.SetTrigger("Attack");
        }
    }
}

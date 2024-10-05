using UnityEngine;

public class CombatComponent : MonoBehaviour
{
    [SerializeField] private BoxCollider _attackCollider;
    [SerializeField] private float _attackDuration = 0.75f;
    [SerializeField] private float _attackCooldown = 0.25f;

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
        Collider[] hitColliders = Physics.OverlapBox(_attackCollider.transform.position, _attackCollider.size / 2, _attackCollider.transform.rotation);
        foreach (Collider hitCollider in hitColliders)
        {
            HealthComponent healthComponent = hitCollider.GetComponent<HealthComponent>();
            if (!healthComponent)
            {
                continue;
            }

            //healthComponent.TakeDamage();

            Debug.Log("Hit " + hitCollider.name);
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
    }
}

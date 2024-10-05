using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 3.0f;
    
    public float Health { get; private set; }

    private void Start()
    {
        Health = _maxHealth;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }
}

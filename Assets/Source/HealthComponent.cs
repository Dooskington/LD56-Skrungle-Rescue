using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 3.0f;
    [SerializeField] private Material _impactEffectMaterial;
    [SerializeField] private float _impactEffectDuration = 0.1f;

    [SerializeField] private AudioClip _hurtAudio;
    [SerializeField] private AudioClip _dieAudio;

    [Header("Animation")]
    [SerializeField] private Animator _animator;

    private List<Tuple<Renderer, Material>> _materials = new List<Tuple<Renderer, Material>>();
    private Sequence _impactSequence;

    public float Health { get; private set; }
    public bool IsDead { get { return Health <= 0; } }

    private void Start()
    {
        Health = _maxHealth;

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material material = renderer.material;
            _materials.Add(new Tuple<Renderer, Material>(renderer, material));
        }

        _impactSequence = DOTween.Sequence()
        .AppendCallback(() =>
        {
            foreach (Tuple<Renderer, Material> renderer in _materials)
            {
                if (renderer != null)
                {
                    renderer.Item1.material = _impactEffectMaterial;
                }
            }
        })
        .AppendInterval(_impactEffectDuration)
        .AppendCallback(() =>
        {
            foreach (Tuple<Renderer, Material> renderer in _materials)
            {
                if (renderer != null)
                {
                    renderer.Item1.material = renderer.Item2;
                }
            }
        })
        .Pause()
        .SetAutoKill(false);
    }

    private void Die()
    {
        AudioEvent.Play3D(_dieAudio, transform.position);

        if (_animator != null)
        {
            _animator.SetTrigger("Die");
            Destroy(gameObject, 1.0f);
        }
        else
        {
            Destroy(gameObject, 0.1f);
        }
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        _impactSequence.Complete();
        _impactSequence.Rewind();
        _impactSequence.Play();

        if (Health <= 0)
        {
            Die();
        }
        else
        {
            AudioEvent.Play3D(_hurtAudio, transform.position);
        }
    }
}

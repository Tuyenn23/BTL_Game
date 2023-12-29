using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int _maxHealth;
    int _curHealth;
    public HealthBar healthBar;
    public UnityEvent OnDeath;
    public float safeTime = 1f;
    float _safeTimeCD;
    private void Start()
    {
        _curHealth = _maxHealth;
        healthBar.UpdateBar(_curHealth, _maxHealth);
    }

    private void OnEnable()
    {
        OnDeath.AddListener(Death);
    }

    private void OnDisable()
    {
        OnDeath.RemoveListener(Death);
    }
    private void Update()
    {
        _safeTimeCD -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int Damage)
    {
        if(_safeTimeCD <= 0)
        {
            _curHealth -= Damage;
            if (_curHealth <= 0)
            {
                OnDeath.Invoke();
            }
            healthBar.UpdateBar(_curHealth, _maxHealth);
            _safeTimeCD = safeTime;

        }


    }

    public void Death()
    {
        Destroy(gameObject);
    }
}

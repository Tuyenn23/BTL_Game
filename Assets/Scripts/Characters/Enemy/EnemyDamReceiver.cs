using Game_Fly;
using System.Collections;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyDamReceiver : DamageReceiver
{
    [SerializeField]
    protected GameObject bloodObj;
    [SerializeField]
    private EnemyType enemyType;
    [SerializeField]
    private Slider health;
    [SerializeField]
    protected Gradient gradient;
    [SerializeField]
    protected Image fill;
    private void Start()
    {
        if (health != null)
        {
            health.maxValue = maxHp;
            health.value = maxHp;

            fill.color = gradient.Evaluate(1f);

        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventManager.StartListening(EventConstants.UPDATE_HP_BOSS, UpdateHeathBoss);
    }

    private void UpdateHeathBoss()
    {
        if (health != null)
        {
            health.value = Hp;
            fill.color = gradient.Evaluate(health.normalizedValue);
            
        }

    }
    

    protected override void OnDead()
    {
        /*SimplePool.Despawn(transform.parent.gameObject);*/
        if (enemyType == EnemyType.Boss)
        {
            GameManager.Instance.IncreaseLevel(GameManager.Instance.levelPlaying);
            GameManager.Instance.isStartGame = true;
            Instantiate(bloodObj, transform.position, Quaternion.identity);
            AudioController.Instance.PlaySound(AudioController.Instance.bossDeath);
            transform.parent.gameObject.SetActive(false);
            
            
            SceneManager.LoadScene("Lobby");
            //StartCoroutine(DelayDead());



        }
        else /*(enemyType == EnemyType.Normal)*/
        {
            Instantiate(bloodObj, transform.position, Quaternion.identity);
            AudioController.Instance.PlaySound(AudioController.Instance.enemyHit);
            transform.parent.gameObject.SetActive(false);

            WaveSpawn.S_instance.OnEnemyDead?.Invoke();
        }
    }

    /*private IEnumerator DelayDead()
    {
        
        yield return new WaitForSeconds(2f);
        


    }*/

    

    private void OnDisable()
    {
        //StopCoroutine(DelayDead());
        EventManager.StopListening(EventConstants.UPDATE_HP_BOSS, UpdateHeathBoss);
    }
}

internal class OnEnable
{
}
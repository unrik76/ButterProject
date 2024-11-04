using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float MaxHealth;
    [SerializeField] public float CurrentHealth;
    [SerializeField] Slider HealthBar;
    [SerializeField] float InvulnerableTime;
    [SerializeField] bool Invulnerable;
    public float InvulnerableTimeDelta;
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        HealthBar.value = CurrentHealth/MaxHealth;
        if (InvulnerableTimeDelta <= Time.time){
            Invulnerable = false;
        }
        print(Time.time);
    }
    private void OnCollisionEnter(Collision other){
        if (other.gameObject.GetComponent<DealDamage>() != null && Invulnerable != true){
            CurrentHealth -= other.gameObject.GetComponent<DealDamage>().Damage;
            Invulnerable = true;
            InvulnerableTimeDelta = Time.time + InvulnerableTime;
            print("Player got hit for " + other.gameObject.GetComponent<DealDamage>().Damage + "");
        }
    }
}

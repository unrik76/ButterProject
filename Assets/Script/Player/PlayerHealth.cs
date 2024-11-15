using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
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
    [SerializeField] Transform RespawnPoint;
    [SerializeField] public bool PlayerFellForTooLong;
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
        if (CurrentHealth <= 0){
            RespawnSequance();
        }
        if (PlayerFellForTooLong == true){
            RespawnSequance();
        }
    }
    private void OnCollisionEnter(Collision other){
        /* Handeling of character taking damage, triggering invulnerability */
        if (other.gameObject.GetComponent<DealDamage>() != null){
            /* If player isn't invulnerable and the object that he's colliding with isn't marked to instakill
                take damage equal to other object assigned damage */
            if(Invulnerable != true && other.gameObject.GetComponent<DealDamage>().InstaKill != true && other.gameObject.GetComponent<DealDamage>().IgnoreImmunity != true){
                CurrentHealth -= other.gameObject.GetComponent<DealDamage>().Damage;
                Invulnerable = true;
                InvulnerableTimeDelta = Time.time + InvulnerableTime;
            }
            /* If object that player is colliding with is marked to ignore invulnerability
                take damage equal to other object assigned damage */
            else if(other.gameObject.GetComponent<DealDamage>().IgnoreImmunity == true){
                CurrentHealth -= other.gameObject.GetComponent<DealDamage>().Damage;
                Invulnerable = true;
                InvulnerableTimeDelta = Time.time + InvulnerableTime / 10;
            }
            /* If object that player is coliding with is marked to instantly kill
                start death sequence */
            else if(other.gameObject.GetComponent<DealDamage>().InstaKill == true){
                RespawnSequance();
            }else{

            }
            
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<SavePoint>() != null){
            RespawnPoint = other.gameObject.GetComponent<SavePoint>().thisTransform;
        }
    }

    public void RespawnSequance(){
        PlayerFellForTooLong = false;
        transform.position = RespawnPoint.position;
        CurrentHealth = MaxHealth;
    }
}

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
    [SerializeField] public Transform RespawnPoint;

    [SerializeField] float HeatLevel = 0;
    [SerializeField] float MaxHeat = 100;
    [SerializeField] float HeatOriginDistrance; 
    [SerializeField] Slider HeatBar;
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        HealthBar.value = CurrentHealth/MaxHealth;   
        HeatBar.value = HeatLevel/MaxHeat;
        if(HeatLevel >= MaxHeat){
            HeatLevel = 0;
            CurrentHealth -= 1;
        }

        if (InvulnerableTimeDelta <= Time.time){
            Invulnerable = false;
        }
        if (CurrentHealth <= 0){
            DeathSequance();
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
                DeathSequance();
            }else{

            }
            
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<SavePoint>() != null){
            RespawnPoint = other.gameObject.GetComponent<SavePoint>().thisTransform;
        }
    }
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.GetComponent<HeatArea>() != null){

            HeatOriginDistrance = Vector3.Distance(other.gameObject.GetComponent<HeatArea>().TransformOrigin.position, transform.position);
            if(other.gameObject.GetComponent<HeatArea>().FlatIncresse != true){
                HeatLevel += other.gameObject.GetComponent<HeatArea>().HeatStrenght / HeatOriginDistrance * Time.deltaTime;
            }else{
                HeatLevel += other.gameObject.GetComponent<HeatArea>().HeatStrenght * Time.deltaTime;
            }
        }
    }

    public void DeathSequance(){
        transform.position = RespawnPoint.position;
        CurrentHealth = MaxHealth;
    }
}

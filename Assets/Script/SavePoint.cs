using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] public Transform thisTransform;
    [SerializeField] public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        thisTransform = transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(thisTransform == player.GetComponent<PlayerHealth>().RespawnPoint){
            GetComponent<MeshRenderer>().enabled = false;
        }
        else{
            GetComponent<MeshRenderer>().enabled = true;
        }
    }
    /*
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == player){
            GetComponent<Renderer>().enabled = false;
        }
    }
    */
}

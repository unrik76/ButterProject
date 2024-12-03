using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] public Transform thisTransform;
    [SerializeField] public GameObject SavePointObject;
    [SerializeField] public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        thisTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject != player){
            Destroy(SavePointObject);
        }
    }
}

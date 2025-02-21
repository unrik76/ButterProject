using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthRecover : MonoBehaviour
{
    [SerializeField] public float healthRecover;
    private void Update() {
        this.gameObject.transform.Rotate(0, 64 * Time.deltaTime, 0);
    }
    private void OnTriggerEnter(Collider other) {
        Destroy(this.gameObject);
    }
}

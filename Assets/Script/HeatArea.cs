using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatArea : MonoBehaviour
{
    [SerializeField] public float HeatStrenght;
    [SerializeField] public bool FlatIncresse;
    [SerializeField] public Transform TransformOrigin;
    void Start() {
        if (TransformOrigin == null){
            TransformOrigin = transform.parent;
        }else{
            TransformOrigin = gameObject.transform;
        }
    }
}

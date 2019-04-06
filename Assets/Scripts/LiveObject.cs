using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveObject : MonoBehaviour
{
    internal Dictionary<BoxCollider, float> boxTriggerColliderSizeDictionary;
    internal Dictionary<BoxCollider, float> boxColliderSizeDictionary;
    internal Dictionary<CapsuleCollider, float> capsuleTriggerColliderSizeDictionary;
    internal Dictionary<CapsuleCollider, float> capsuleColliderSizeDictionary;

    void Awake()
    {
        boxTriggerColliderSizeDictionary = new Dictionary<BoxCollider, float>();
        boxColliderSizeDictionary = new Dictionary<BoxCollider, float>();
        capsuleColliderSizeDictionary = new Dictionary<CapsuleCollider, float>();
        capsuleTriggerColliderSizeDictionary = new Dictionary<CapsuleCollider, float>();
        var boxColliders = GetComponents(typeof(BoxCollider));
        var capsuleColliders = GetComponents(typeof(CapsuleCollider));
        foreach (Component component in boxColliders)
        {
            var bc = component as BoxCollider;
            if (bc != null && bc.isTrigger)
            {
                boxTriggerColliderSizeDictionary.Add(bc,bc.size.z);
                bc.size = new Vector3(bc.size.x,bc.size.y,5000);
            }
            else if (bc != null && !bc.isTrigger)
            {
                boxColliderSizeDictionary.Add(bc,bc.size.z);
                bc.size = new Vector3(bc.size.x,bc.size.y,5000);

            }
        }
        
        foreach (Component component in capsuleColliders)
        {
            var bc = component as CapsuleCollider;
            if (bc != null && bc.isTrigger)
            {
                capsuleTriggerColliderSizeDictionary.Add(bc,bc.height);
                bc.height = 5000;
            }
            else if (bc != null && !bc.isTrigger)
            {
                capsuleColliderSizeDictionary.Add(bc,bc.height);
                bc.height = 5000;

            }
        }
        

    }

    private void Start()
    {
        GameManager.Instance.liveObjects.Add(this);

    }
}

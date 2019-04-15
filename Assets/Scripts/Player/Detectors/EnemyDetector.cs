using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyDetector : MonoBehaviour
{
    internal List<GameObject> _enemiesInRange;

    [SerializeField] private string layerToDetect;

    private void Start()
    {
        if (layerToDetect == "")
        {
            layerToDetect = "Enemy";
        }
        _enemiesInRange = new List<GameObject>();
    }

    private void Update()
    {
        for (int i = 0; i < _enemiesInRange.Count; i++)
        {
            if (!_enemiesInRange[i])
            {
                _enemiesInRange.RemoveAt(i);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(layerToDetect))
        {
            if (!_enemiesInRange.Contains(other.gameObject))
            {
                _enemiesInRange.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(layerToDetect))
            _enemiesInRange.Remove(other.gameObject);
    }

 

    public bool playerInRange()
    {
        return _enemiesInRange.Select(col => col.gameObject).Contains(PlayerProperty.player);
    }
    
    
}
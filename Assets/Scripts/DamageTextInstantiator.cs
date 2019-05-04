using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextInstantiator : MonoBehaviour
{
    
    [SerializeField] private GameObject[] nums;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnText(int damage,Vector3 position)
    {
        
        if (damage < 10)
        {
            var num = Instantiate(nums[damage], position, Quaternion.identity);
            num.transform.SetParent(null);
        }
        else if (damage < 100)
        {
            int NumInTen = damage / 10;
            var num1 = Instantiate(nums[NumInTen], position+new Vector3(-1,0), Quaternion.identity);
            
            int NumInOne = damage % 10;
            var num2 = Instantiate(nums[NumInOne], position+new Vector3(1,0), Quaternion.identity);

            num1.transform.SetParent(null);
            num2.transform.SetParent(null);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public GameObject moneyPf; 
    private Target _target;
    // Start is called before the first frame update
    void Start()
    {
        _target = GetComponent<Target>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteract(Player player)
    {
        
    }
    
}

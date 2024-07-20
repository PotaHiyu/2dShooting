using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Owner : MonoBehaviour
{
    public uint owner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        NetworkMove networkMove = other.GetComponent<NetworkMove>();
        if (networkMove == null) return;
        // owner = networkMove.gameObject;
    }
}

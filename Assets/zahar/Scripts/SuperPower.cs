using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperPower : MonoBehaviour
{
    public GameObject ex;
    public GameObject transport;
    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        transport.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Instantiate(ex, transform.position, transform.rotation);
            Invoke("Transform", 0.1f);
        }
    }
    void Transform()
    {
        player.gameObject.SetActive(false);
        transport.gameObject.SetActive(true);
    }
}

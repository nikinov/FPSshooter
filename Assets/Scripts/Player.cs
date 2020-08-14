using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField] private int maxHealth = 100;

    [SyncVar] private int currentHealth;

    
    [SerializeField] private Component[] disableOnDeth;
    [SerializeField] private Component[] enableOnDeth;
    [SerializeField] private GameObject[] enableOnDethObj;
    [SerializeField] private GameObject[] disableOnDethObj;

    public void Setup ()
    {
        SetDefaults();
        SetDethStuff(false);
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(999);
        }
    }

    [ClientRpc]
    public void RpcTakeDamage (int _amount)
    {
        if (isDead)
            return;

        currentHealth -= _amount;

        Debug.Log(transform.name + " now has " + currentHealth + " health.");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        Debug.Log(transform.name + " is DEAD!");

        StartCoroutine(Respawn());

        // call respawn method
    }

    private IEnumerator Respawn()
    {
        int fullIntTime = 0;
        for (int i = 0; i < (int)3.9f; i++)
        {
            Debug.Log(i);
            yield return new WaitForSeconds(i);
            fullIntTime += 1;
        }
        //yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime - fullIntTime);

        SetDefaults();
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;
        Debug.Log(transform.name + " respawned");
        SetDethStuff(false);
    }

    public void SetDefaults()
    {
        isDead = false;

        currentHealth = maxHealth;

        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = true;
    }
    private void SetDethStuff(bool set)
    {
        foreach (Component item in enableOnDeth)
        {
            if (item.GetComponent<Behaviour>() != null)
            {
                item.GetComponent<Behaviour>().enabled = set;
            }
            else
            {
                item.Equals(set);
            }
        }
        foreach (GameObject item in enableOnDethObj)
        {
            item.SetActive(set);
        }
        foreach (Component item in disableOnDeth)
        {
            if (item.GetComponent<Behaviour>() != null)
            {
                item.GetComponent<Behaviour>().enabled = !set;
            }
            else
            {
                item.Equals(!set);
            }
        }
        foreach (GameObject item in disableOnDethObj)
        {
            item.SetActive(!set);
        }
    }
}

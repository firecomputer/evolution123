using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private Vector3 randomPos;
    public float range;
    public float timeMax;
    public float timeMin;
    public float spawntime;
    public float dieTime;
    // Start is called before the first frame update
    void Start()
    {
        spawntime = Random.Range(timeMin, timeMax);
        randomPos = new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
        StartCoroutine(newSpawn());
        StartCoroutine(die());
    }

    IEnumerator newSpawn()
    {
        yield return new WaitForSeconds(spawntime);
        Instantiate(this.gameObject, transform.position + randomPos, Quaternion.identity);
        StartCoroutine(newSpawn());
    }
    
    IEnumerator die()
    {
        yield return new WaitForSeconds(dieTime);
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

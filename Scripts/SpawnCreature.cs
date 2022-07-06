using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCreature : MonoBehaviour
{
    public GameObject Creature;
    public GameObject Food;
    public float maxX;
    public float maxZ;
    public float speedRange;
    public float rotationRange;
    public float senseRange;
    public float sizeRange;
    public int CreatureCount;
    public int FoodCount;
    public int birthRange;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < CreatureCount; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-maxX,maxX),1,Random.Range(-maxZ,maxZ));
            Quaternion spawnRot = Quaternion.Euler(0, Random.Range(0, 360), 0);
            GameObject creatureEx = Instantiate(Creature, spawnPos, spawnRot);
            Creature creat = creatureEx.GetComponent<Creature>();
            creat.food = Food;
            creat.speed += Random.Range(-speedRange, speedRange);
            creat.rotation += Random.Range(-rotationRange, rotationRange);
            creat.sense += Random.Range(-senseRange, senseRange);
            creat.size += Random.Range(-sizeRange, sizeRange);
            creat.birthCount += (int)(Random.Range(-birthRange, birthRange));
            creat.sex = i % 2;
            creat.sense = Mathf.Clamp(creat.sense, 1, 200);
            print(creat.sex);
        }
        for (int i = 0; i < FoodCount; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-maxX, maxX), 0.5f, Random.Range(-maxZ, maxZ));
            Instantiate(Food, spawnPos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

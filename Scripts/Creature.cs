using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public float energy = 1;
    public float dungTime = 3;
    public float growTime = 5;
    public float speed = 12;
    public float rotation;
    public float lossEnergy = 0.1f;
    public float sense = 5f;
    public float mutation_rate;
    public float birthTime;
    public int birthCount = 1;
    public GameObject food;
    public float energyWeight=1000;
    public float size = 1;
    public int sex;
    public bool birthAble = true;
    // Start is called before the first frame update
    void Start()
    {
        energy = 0.5f;
        lossEnergy = ((speed*speed*speed*0.01f) + rotation + (sense*0.1f) + (size * size * size) + (birthCount*birthCount*birthCount)) / energyWeight;
        transform.localScale = new Vector3(size, size, size);
        Color thisColor = new Color();
        thisColor.r = speed / 20;
        thisColor.g = sense / 30;
        thisColor.b = size / 10;
        GetComponent<MeshRenderer>().material.color = thisColor;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        if (energy <= 0)
        {
            Destroy(this.gameObject);
        }
        if(transform.position.y < 0)
        {
            Destroy(this.gameObject);
        }
        energy -= lossEnergy * Time.deltaTime;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, sense))
        {
            if (hit.transform.gameObject.tag == "food")
            {
                transform.LookAt(hit.transform);
            }
            if (hit.transform.gameObject.tag == "foodEnemy")
            {
                transform.LookAt(hit.transform);
            }
        }
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        transform.Rotate(0, Random.Range(-rotation, rotation), 0);
        if(transform.position.y < 0)
        {
            Destroy(this.gameObject);
        }
        restrict_pos();
    }

    public void restrict_pos()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -110, 110);
        pos.z = Mathf.Clamp(pos.z, -110, 110);
        transform.position = pos;
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.tag == "food")
        {
            Destroy(collision.gameObject);
            energy += 1;
            StartCoroutine(dung());
        }
        if (collision.gameObject.tag == "foodEnemy")
        {
            if (collision.gameObject.GetComponent<Creature>().size + 1 < size && energy < 1)
            {
                energy += collision.gameObject.GetComponent<Creature>().energy;
                Destroy(collision.gameObject);
            }
            else if (energy >= 1 && collision.gameObject.GetComponent<Creature>().sex + sex == 1 && birthAble)
            {
                for (int i = 0; i < birthCount; i++)
                {
                    StartCoroutine(spawnBaby(this, collision.gameObject.GetComponent<Creature>()));
                }
                energy -= 1;
                birthAble = false;
                StartCoroutine(birthAbler());
            }
        }
    }

    IEnumerator birthAbler()
    {
        yield return new WaitForSeconds(2);
        birthAble = true;
    }

    private Creature sex2(Creature boy, Creature girl)
    {
        float speed_Average = 0.5f * (boy.speed* Random.Range(0.0f, 2.0f) + girl.speed* Random.Range(0.0f, 2.0f));
        float rotation_Average = 0.5f * (boy.rotation* Random.Range(0.0f, 2.0f) + girl.rotation* Random.Range(0.0f, 2.0f));
        float sense_Average = 0.5f * (boy.sense* Random.Range(0.0f, 2.0f) + girl.sense* Random.Range(0.0f, 2.0f));
        float size_Average = 0.5f * (boy.size* Random.Range(0.0f, 2.0f) + girl.size* Random.Range(0.0f, 2.0f));
        int birth_Average = (int)(0.5f * (boy.birthCount * Random.Range(0.0f, 2.0f) + girl.birthCount * Random.Range(0.0f, 2.0f)));
        Creature baby = new Creature();
        baby.speed = speed_Average;
        baby.rotation = rotation_Average;
        baby.sense = sense_Average;
        baby.size = size_Average;
        baby.birthCount = birth_Average;
        baby.sex = Random.Range(0, 2);
        return baby;
    }

    IEnumerator spawnBaby(Creature boy, Creature girl)
    {
        yield return new WaitForSeconds(birthTime);
        Creature newbaby = sex2(boy, girl);
        Creature newBaby = GetComponent<Creature>();
        newBaby.speed = newbaby.speed;
        newBaby.rotation = newbaby.rotation;
        newBaby.sense = newbaby.sense;
        newBaby.size = newbaby.size;
        newBaby.birthCount = Mathf.Clamp(newbaby.birthCount, 1, 100);
        newBaby.sex = newbaby.sex;
        Instantiate(mutation(this.gameObject), transform.position + new Vector3(0, 0, 3), transform.rotation);
    }

    GameObject mutation(GameObject parent)
    {
        parent.GetComponent<Creature>().speed += Random.Range(-parent.GetComponent<Creature>().speed*mutation_rate, parent.GetComponent<Creature>().speed * mutation_rate);
        parent.GetComponent<Creature>().rotation += Random.Range(-parent.GetComponent<Creature>().rotation * mutation_rate, parent.GetComponent<Creature>().rotation * mutation_rate);
        parent.GetComponent<Creature>().sense += Random.Range(-parent.GetComponent<Creature>().sense * mutation_rate, parent.GetComponent<Creature>().sense * mutation_rate);
        parent.GetComponent<Creature>().size += Random.Range(-parent.GetComponent<Creature>().size *mutation_rate, parent.GetComponent<Creature>().size * mutation_rate);
        parent.GetComponent<Creature>().birthCount += (int)(Random.Range(-parent.GetComponent<Creature>().birthCount * mutation_rate, parent.GetComponent<Creature>().birthCount * mutation_rate));
        return parent;
    }

    IEnumerator dung()
    {
        yield return new WaitForSeconds(dungTime);
        Vector3 dungPos = transform.position;
        yield return new WaitForSeconds(growTime);
        Instantiate(food, dungPos, Quaternion.identity);
    }
}

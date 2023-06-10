using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Flock : MonoBehaviour
{
    //criando variavel manager
    public FlockingManager myManager;
    //criando variavel speed
    public float speed;
    //criando variavel bool 
    bool turning = false;
    //definindo speed como min e max
    void Start()
    {
        speed = Random.Range(myManager.minSpeed,
        myManager.maxSpeed);
    }
    void Update()
    {
        //definindo "b" como Bounds, e o limite que o cardume pode nadar
        Bounds b = new Bounds(myManager.transform.position, myManager.swinLimits * 2);
        //definindo "hit" como um novo RaycastHit
        RaycastHit hit = new RaycastHit();
        //definindo o valor da variavel "direction"
        Vector3 direction = myManager.transform.position - transform.position;

        //condicional para fazer o peixe virar
        if (!b.Contains(transform.position))
        {
            turning = true;
            direction = myManager.transform.position - transform.position;
        }
        //condicional para o peixe virar caso o Raycast acerte alguma coisa
        else if (Physics.Raycast(transform.position, this.transform.forward * 50, out hit))
        {
            turning = true;
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
        }
        //utilização do else para caso nenhuma das condicionais acima de concretize, então "turning" será falso
        else
            turning = false;
        //condicional que define o as propriedades de "turning", rotação
        if (turning)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(direction),
            myManager.rotationSpeed * Time.deltaTime);
        }
        //se não for "turning", define a speed como um valor aleatório entre 0 e 100/
        else
        {
            if (Random.Range(0, 100) < 10)
                speed = Random.Range(myManager.minSpeed,
                myManager.maxSpeed);
            if (Random.Range(0, 100) < 20)
                ApplyRules();
        }
        transform.Translate(0, 0, Time.deltaTime * speed);
    }
    void ApplyRules()
    {
        GameObject[] gos;
        gos = myManager.allFish;
        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;
        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                if (nDistance <= myManager.neighbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;
                    if (nDistance < 1.0f)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }
                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }
        if (groupSize > 0)
        {
            vcentre = vcentre / groupSize + (myManager.goalPos - this.transform.position);
            speed = gSpeed / groupSize;
            Vector3 direction = (vcentre + vavoid) - transform.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction),
                myManager.rotationSpeed * Time.deltaTime);
        }
    }
}

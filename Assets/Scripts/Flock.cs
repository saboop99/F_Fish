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
    //criação do método "ApplyRules"
    void ApplyRules()
    {
        //declarações de variaveis
        GameObject[] gos;
        gos = myManager.allFish;
        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;
        //para cada objeto "go" na lista "gos"
        foreach (GameObject go in gos)
        {
            //caso "go" seja diferente do gameObject
            if (go != this.gameObject)
            {
                //distância calculada entre o objeto "go" e o objeto atual
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                //condicional para caso a distancia entre neighbours seja maior ou igual a "nDistance"
                if (nDistance <= myManager.neighbourDistance)
                {
                    //vcentre controla o centro, aqui estamos setando a posição do "go" pro centro
                    vcentre += go.transform.position;
                    //aumento do tamanho do grupo
                    groupSize++;
                    //condicional para "nDistance" ser menor que 1, e depois faz um calculo para evitar colisões caso os peixes estejam muito grudados
                    if (nDistance < 1.0f)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }
                    //anotherFlock pega um componente Flock do objeto "go"
                    Flock anotherFlock = go.GetComponent<Flock>();
                    //setando gSpeed como um soma da mesma mais a do anotherFlock
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }
        //condicional caso o tamano do grupo seja maior que zero
        if (groupSize > 0)
        {
            //vcentre igual a vcentre dividido pelo tamanho do grupo mais o calculo do posicionamento do manager
            vcentre = vcentre / groupSize + (myManager.goalPos - this.transform.position);
            //speed igual velocidade do grupo dividido por tamanho do grupo
            speed = gSpeed / groupSize;
            //operação para definir o valor de "direction"
            Vector3 direction = (vcentre + vavoid) - transform.position;
            //condicional para caso direction seja diferente de 0
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction),
                myManager.rotationSpeed * Time.deltaTime);
        }
    }
}

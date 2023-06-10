using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FlockingManager : MonoBehaviour
{
    //declaração de todas as variaveis do manager
    public GameObject fishPrefab;
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swinLimits = new Vector3(5, 5, 5);
    public Vector3 goalPos;
    [Header("Configurações do Cardume")]
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    [Range(1.0f, 10.0f)]
    public float neighbourDistance;
    [Range(0.0f, 5.0f)]
    public float rotationSpeed;

    //Start
    void Start()
    {
        //criação da lista allFish
        allFish = new GameObject[numFish];
        //continuar ate as condicionais abaixo serem cumpridas
        for (int i = 0; i < numFish; i++)
        {
            //definindo o posicionamento do peixe entre valores aleatorios dentro do limite
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swinLimits.x,
            swinLimits.x),
            Random.Range(-swinLimits.y,
            swinLimits.y),
            Random.Range(-swinLimits.z,
            swinLimits.z));
            //instanciando peixe
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            //pegando componente do Flock do peixe instanciado
            allFish[i].GetComponent<Flock>().myManager = this;
        }
        //definindo o valor de goalPos
        goalPos = this.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        //definindo o valor de goalPos
        goalPos = this.transform.position;
        //condicional para caso o numero random gerado entre 0 e 100 seja menor que 10
        if (Random.Range(0, 100) < 10)
            //operação para atualizar a posição goalPos com o valor dela mais um random entre os limites do swin
            goalPos = this.transform.position + new Vector3(Random.Range(-swinLimits.x,
            swinLimits.x),
            Random.Range(-swinLimits.y,
            swinLimits.y),
            Random.Range(-swinLimits.z,
            swinLimits.z));
    }
}

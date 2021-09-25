using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    private Renderer rend;
    private float gravity;

    private Firework firework;
    private Vector3 target;

    //public GameObject fwmanager;
    public FWManager fwmanager;

    private float waitTime;
    


    // since constructors dont work
    public void Initialize(Firework firework, Vector3 target)
    {
        this.target = target;
        this.firework.speed = firework.speed;
        this.firework.chain = firework.chain;

        rend = GetComponent<Renderer>();
        rend.material.color = firework.colour;
    }

    // Start is called before the first frame update
    void Start()
    {
        gravity = 0.002f;
        waitTime = 0.8f;
        transform.Rotate(Vector3.forward * Random.Range(0, 90));
        StartCoroutine(Fade(waitTime));
        StartCoroutine(ChainExplode(waitTime));
    }

    // Update is called once per frame
    void Update()
    {
        target.y -= gravity;
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * firework.speed);


        if (rend.material.color.a <= 0.05f)
        {
            Destroy(gameObject);
        }

    }

    IEnumerator Fade(float time)
    {
        yield return new WaitForSeconds(time);

        for (float alpha = 1f; alpha >= 0; alpha -= 0.05f)
        {
            Color c = rend.material.color;
            c.a = alpha;
            rend.material.color = c;
            yield return new WaitForSeconds(.05f);
        }
    }

    IEnumerator ChainExplode(float time)
    {
        yield return new WaitForSeconds(time);

        // you have to drag the prefab object with the script
        firework.chain -= 1;
        firework.size /= 2;
        firework.points /= 2;
        firework.speed /= 2;
        firework.colour = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);

        fwmanager.generateFW(firework); //transform.position, Random.Range(1f, 5f), Random.Range(1, 17), "Straight", Random.Range(1f, 3f), new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f), chain-1);
    }
}
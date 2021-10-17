using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    private Renderer rend;
    private SpriteRenderer sprRend;
    public Sprite[] spriteArray;

    private float gravity;

    private Firework firework;
    private Vector3 target;

    //public GameObject fwmanager;
    public FWManager fwmanager;

    private float waitTime;
    private float interval;

    public GameObject obj_particle;

    // since constructors dont work
    public void Initialize(Firework firework, Vector3 target)
    {
        this.target = target;
        this.firework = firework;

        rend = GetComponent<Renderer>();
        rend.material.color = firework.colour;

        sprRend = GetComponent<SpriteRenderer>();
        sprRend.drawMode = SpriteDrawMode.Sliced;
        sprRend.size = firework.bolt_size;
    }

    // Start is called before the first frame update
    void Start()
    {
        gravity = 0.002f;
        waitTime = 0.8f;
        interval = 0.075f;
        transform.Rotate(Vector3.forward * Random.Range(0, 360));
        StartCoroutine(Fade(waitTime));
        StartCoroutine(Trail(interval));

        sprRend.sprite = spriteArray[Random.Range(0, spriteArray.Length)];

    }

    // Update is called once per frame
    void Update()
    {
        target.y -= gravity;
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * firework.speed);


        if (rend.material.color.a <= 0.25f)
        {
            if (firework.chain-1 == 0)
            {
                // if this is the last explosion, fade out
                if (rend.material.color.a <= 0.05f)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                // if this is not the last explosion, continue
                Destroy(gameObject);
                ChainExplode();
            }

        }

    }

    IEnumerator Fade(float time)
    {
        yield return new WaitForSeconds(time);

        for (float alpha = 1f; alpha >= 0; alpha -= 0.035f)
        {
            Color c = rend.material.color;
            c.a = alpha;
            rend.material.color = c;
            yield return new WaitForSeconds(.075f);
        }
    }

    IEnumerator Trail(float interval)
    {
        yield return new WaitForSeconds(interval);
        generateParticle(transform.position);
        StartCoroutine(Trail(interval));
    }

    public void generateParticle(Vector2 location)
    {
        Particle go = Object.Instantiate(obj_particle, location, Quaternion.identity).GetComponent<Particle>(); ;
        go.Initialize(firework);
    }

    public void ChainExplode()
    {
        // you have to drag the prefab object with the script
        Random.InitState(firework.seed);

        firework.position = transform.position;   
        
        firework.chain -= 1;
        firework.explosion_size /= 2;
        firework.bolt_size /= 2;
        firework.points = Random.Range(1, 17);
        firework.speed = Random.Range(1f, 3f);
        firework.colour = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);

        fwmanager.generateFW(firework); //transform.position, Random.Range(1f, 5f), Random.Range(1, 17), "Straight", Random.Range(1f, 3f), new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f), chain-1);
    }
}


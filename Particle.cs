using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private Renderer rend;
    private SpriteRenderer sprRend;
    public Sprite[] spriteArray;

    private Firework firework;

    //private float scaledown = 25;

    private void Awake()
    {
        transform.Rotate(Vector3.forward * Random.Range(0, 360));

        rend = GetComponent<Renderer>();
        

        sprRend = GetComponent<SpriteRenderer>();
        sprRend.drawMode = SpriteDrawMode.Sliced;
        sprRend.sprite = spriteArray[Random.Range(0, spriteArray.Length)];
        sprRend.size = new Vector2(0.1f, 0.1f);
    }

    void Start()
    {
        float waitTime = 0f;
        StartCoroutine(Fade(waitTime));
    }

    public void Initialize(Firework firework)
    {
        this.firework = firework;
        rend.material.color = firework.colour;
    }

    // Update is called once per frame
    void Update()
    {
        if (rend.material.color.a <= 0.075f)
        {
            Destroy(gameObject);
        }

        
    }

    IEnumerator Fade(float time)
    {
        yield return new WaitForSeconds(time);
        Color c = rend.material.color;
        for (float alpha = 0.5f; alpha >= 0; alpha -= 0.075f)
        {
            c.a = alpha;
            rend.material.color = c;
            yield return new WaitForSeconds(.05f);
        }
    }
}

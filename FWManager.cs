using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Firework
{
    public Vector3 position;
    public float explosion_size;
    public float bolt_size;
    public int points;
    public string line;
    public float speed;
    public Color colour;
    public int chain;
    public int seed;
}

// making firework more "realistic"
// random length spread
// random angle spread
// lighting glow
// trailing path

// explosion types: (this can be a seperate data type) eg: public float spread = (Mathf.PI * 2) <- im assuming thats 360'
// typical (bolt 360)
// cone (bolt ~0-90 degrees)

// bolt types:
// typical (straight direction, trailing fire) 
// swirlies (direction curvy)
// twinkle (pulsing light)

public class FWManager : MonoBehaviour
{
    public GameObject obj_bolt;
    public Camera main;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 2.0f; // 2m from camera
            Vector3 objectPos = main.ScreenToWorldPoint(mousePos);

            Firework firework;
            firework.position = objectPos;
            firework.explosion_size = Random.Range(1f, 5f);
            firework.bolt_size = Random.Range(0f, 1f);
            firework.points = Random.Range(1, 17);
            firework.line = "Straight";
            firework.speed = Random.Range(1f, 3f);
            firework.colour = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
            firework.chain = 3;
            firework.seed = Random.Range(1, 999);

            generateFW(firework); //objectPos, Random.Range(1f,5f), Random.Range(1,17), "Straight", Random.Range(1f,3f), new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f), 3); //new Color(Random.Range(0,1), Random.Range(0, 1), Random.Range(0, 1), 1));
        }
    }

    /* generates fireworks
    Location: x/y as a Vector 3
    Size: Size of travelling bolt before exploding (radius of circle)
    Points: How many bolts appear outwards
    Line: The path that the bolt takes outwards 
    Speed: How fast the bolt moves outwards
    Colour: the colour of each bolt
    Chain: If bolts generate an explosion

    */
    public void generateFW(Firework firework) //Vector3 position, float length, int points, string path, float speed, Color colour, int chain)
    {
        if (firework.chain > 0)
        {
            float divider = Mathf.PI * 2 / firework.points;
            float offset = Random.Range(0, divider);

            for (int i = 0; i < firework.points; i++)
            {
                float angle = i * divider;
                Vector3 boltTarget = new Vector3(firework.position.x + Mathf.Cos(angle + offset) * firework.explosion_size, firework.position.y + Mathf.Sin(angle + offset) * firework.explosion_size);
                Bolt go = Object.Instantiate(obj_bolt, firework.position, Quaternion.identity).GetComponent<Bolt>(); ;
                go.Initialize(firework, boltTarget);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeBricks : MonoBehaviour
{
    public GameObject bricks;
    public float offsetx = -8.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        int width = 26;
        Vector3 position;
        for (int xpos = 0; xpos < width; xpos++)
        {
            position.x = offsetx + xpos * 80.0f;
            position.y = 0;
            position.z = 0;
            transform.position = position;
            Instantiate(bricks, new Vector3(position.x,0,0), Quaternion.identity);
            Instantiate(bricks, new Vector3(position.x,1080,0), Quaternion.identity);
        }

        int height = 17;
        for (int ypos = 1; ypos < height; ypos++)
        {
            position.x = offsetx;
            position.y = -10.0f + ypos * 64.0f;
            position.z = 0;
            transform.position = position;
            Instantiate(bricks, new Vector3(position.x,position.y,0), Quaternion.identity);
            position.x = offsetx + 25 * 80;
            Instantiate(bricks, new Vector3(position.x,position.y,0), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

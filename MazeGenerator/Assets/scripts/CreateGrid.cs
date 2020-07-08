using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    public GameObject wall;
    public int width = 5;
    public int height = 5;
    Vector3 startPos;
    Vector3 currentPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = new Vector3((-width / 2) + 0.5f, 0, (-height / 2) + 0.5f);


        //X axis
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j <= width; j++)
            {
                currentPos = new Vector3(startPos.x + j - 0.5f, 0f, startPos.z + i - 0.5f);
                Instantiate(wall, currentPos, Quaternion.identity);
            }
        }

        //Y axis
        for (int i = 0; i <= height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                currentPos = new Vector3(startPos.x + j, 0f, startPos.z + i - 1f);
                Instantiate(wall, currentPos, Quaternion.Euler(0f,90f,0f));
            }
        }

    }

    
       
    

    // Update is called once per frame
    void Update()
    {
        
    }
}

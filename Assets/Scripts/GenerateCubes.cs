using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCubes : MonoBehaviour
{
    public static GenerateCubes instance;

    public GameObject red_Cube, green_Cube, blue_Cube;
    public Transform red_Cube_Parent, green_Cube_Parent, blue_Cube_Parent;
    public int minX, maxX, minZ, maxZ;
    public LayerMask layerMask;


    private void Awake()
    {
        if (instance == null)
        {
            instance= this;
        }
    }

    public void GenerateCube(int number,CharacterAI characterAI = null)
    {
        if (number == 0)
        {
            Generate(red_Cube, red_Cube_Parent, characterAI);
        }
        if (number == 1)
        {
            Generate(blue_Cube, blue_Cube_Parent);
        }
        if (number == 2)
        {
            Generate(green_Cube, green_Cube_Parent, characterAI);
        }
    }

    private void Generate(GameObject gameObject,Transform parent,CharacterAI characterAI=null)
    {
        

        GameObject g = Instantiate(gameObject);

        g.transform.parent = parent;

        Vector3 desPos = GiveRandomPos();
        g.SetActive(false);

        Collider[] colliders = Physics.OverlapSphere(desPos, 1, layerMask);
        while (colliders.Length!= 0)
        {
            Debug.Log("çarptý: " + colliders[0].gameObject + " " + desPos);
            desPos=GiveRandomPos();
            colliders=Physics.OverlapSphere(desPos, 1, layerMask);
        }
        g.SetActive(true);
        g.transform.position = desPos;
        if (characterAI!=null    )
        {
            characterAI.targets.Add(g);
        }


    }




    private Vector3 GiveRandomPos()
    {
        return new Vector3(Random.Range(minX, maxX), red_Cube.transform.position.y, Random.Range(minZ, maxZ));

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

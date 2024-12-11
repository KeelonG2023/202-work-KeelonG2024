using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Entity
{

    public int fruits;

    public Sprite treeTakenSprite;
    public Sprite treeReadySprite;

    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        fruits = 1;
        attraction = 5f;
        parent = gameObject;

        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fruits <= 0)
        {
            sr.sprite = treeTakenSprite;
        }
        else
        {
            sr.sprite = treeReadySprite;
        }
    }

    public void TakeFruit()
    {
        fruits--;
        
    }
}

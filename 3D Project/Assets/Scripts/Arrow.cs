using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    GameObject target;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Vector3 pos = target.transform.position;
            gameObject.transform.LookAt(new Vector3(pos.x, gameObject.transform.position.y, pos.z));
        }
    }

    public void setTarget(GameObject targ)
    {
        target = targ;
    } 
}

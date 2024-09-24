using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElapsedTime : MonoBehaviour
{
    private float elapsedTime;

    private TMP_Text editText;
    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0;
        editText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        editText.text = "Time: " + elapsedTime.ToString();

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconController : MonoBehaviour
{
    public Image IconImage;

    // Start is called before the first frame update
    void Awake()
    {
        IconImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

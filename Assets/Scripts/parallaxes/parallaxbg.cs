using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class parallaxbg : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform[] backgrounds = new Transform[3];
    [SerializeField] private float parallaxMultiplier;

    private float startpos;
    private int bgs;
    private float lenght;

    int nextslide = 0;

    void Start()
    {
        bgs = backgrounds.Length;
        lenght = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;
        startpos = transform.position.x;
    }
    void Update()
    {
        float camdist = Mathf.Abs(cameraTransform.position.x * (1-parallaxMultiplier)) - startpos;
        // transform.position += new Vector3(cameraTransform.position.x * parallaxMultiplier, transform.position.y, transform.position.z);
        if(camdist >= lenght){
            StartCoroutine(slidebg(lenght));
        }
    }
    IEnumerator slidebg(float width){
        startpos = cameraTransform.position.x;
        backgrounds[nextslide%3].position += new Vector3(backgrounds.Length * width,0,0);
        nextslide++;
        yield return new WaitForSeconds(0.1f);
        yield return null; 
    }
}

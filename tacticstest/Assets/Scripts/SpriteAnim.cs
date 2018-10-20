using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//make sure sprites always face camera
public class SpriteAnim : MonoBehaviour {

    public Transform target;
    public Animator animator;
    public Transform tileSelectionIndicator;

    private void Start()
    {
        target = GameObject.Find("Main Camera").transform;
        tileSelectionIndicator = GameObject.Find("Tile Selection Indicator").transform;
        animator = GetComponent<Animator>();
    }   

    void Update()
        {
            if (target != null)
                transform.rotation = target.rotation;
        }

}

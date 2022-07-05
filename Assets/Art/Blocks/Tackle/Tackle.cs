using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tackle : MonoBehaviour
{
    private Animator animator;
    private GameObject player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        transform.tag = "block";
    }

    private void Start()
    {
        Vector3 look = transform.position;
        look.x = 0;
        transform.LookAt(look);
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void FixedUpdate()
    {
        //if(Vector3.Distance(player.transform.position, transform.position) < 5f)
        if((transform.position.z - player.transform.position.z) < 3f)
        {
            animator.SetTrigger("Slide");

            StartCoroutine("goInactive");
        }
    }

    IEnumerator goInactive()
    {
        yield return new WaitForSeconds(1.0f);
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != this.gameObject)
            {
                collider.isTrigger = true;
            }
        }
    }

}

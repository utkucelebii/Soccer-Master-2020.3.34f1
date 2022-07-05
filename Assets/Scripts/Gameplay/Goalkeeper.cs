using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goalkeeper : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;
    private List<Collider> ragdollParts = new List<Collider>();
    public List<Rigidbody> ragdollRBParts = new List<Rigidbody>();


    private float movingTime;
    private Vector3 startPos, left, right;
    private float multiplier = 3f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        setRagdollParts();
        startPos = transform.position;
        left = startPos + Vector3.left * multiplier;
        right = startPos + Vector3.right * multiplier;
    }


    private void setRagdollParts()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != this.gameObject)
            {
                collider.isTrigger = true;
                ragdollParts.Add(collider);
            }
        }

        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody r in rigidbodies)
        {
            if (r.gameObject != this.gameObject)
            {
                r.isKinematic = true;
                ragdollRBParts.Add(r);
            }
        }
    }

    public void TurnOnRagdoll()
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        anim.enabled = false;
        anim.avatar = null;

        foreach(Collider c in ragdollParts)
        {
            c.isTrigger = false;
            c.attachedRigidbody.velocity = Vector3.zero;
        }

        foreach(Rigidbody r in ragdollRBParts)
        {
            r.isKinematic = false;
        }
    }


    private void FixedUpdate()
    {
        if (!LevelManager.Instance.isGoal && this.gameObject.activeSelf)
        {
            movingTime += Time.deltaTime;
            if (movingTime < 1.5f)
                transform.position = Vector3.Lerp(transform.position, right, 0.031f);
            else
                transform.position = Vector3.Lerp(transform.position, left, 0.031f);

            if (movingTime > 3.2f)
                movingTime = 0;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            TurnOnRagdoll();
            Vector3 explosionPos = transform.position;
            explosionPos.y -= 5;
            explosionPos.z += 5;
            foreach(Rigidbody r in ragdollRBParts)
                r.AddExplosionForce(2000, explosionPos, 1000);
        }
    }
}

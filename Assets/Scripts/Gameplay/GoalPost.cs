using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPost : MonoBehaviour
{
    public GameObject Goalkeeper;

    // Expolosion at scoring goal
    public Transform Explosion;
    public float ExplosionForce;
    public float ExplosionRadius;

    public void Start()
    {
        if (!LevelManager.Instance.level.Goalkeeper)
        {
            Goalkeeper.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "ball")
        {
            Explosion.gameObject.SetActive(true);
            Explosion.GetComponent<ParticleSystem>().Play();
            LevelManager.Instance.isGoal = true;
            if (LevelManager.Instance.level.Goalkeeper)
            {
                Goalkeeper.GetComponent<Animator>().enabled = false;
                Goalkeeper.GetComponent<Goalkeeper>().TurnOnRagdoll();
                Vector3 explosionPos = transform.position;
                explosionPos.y -= 5;
                explosionPos.z += 5;
                foreach (Rigidbody r in Goalkeeper.GetComponent<Goalkeeper>().ragdollRBParts)
                    r.AddExplosionForce(ExplosionForce, explosionPos, ExplosionRadius);

            }

            LevelManager.Instance.VictoryPanel.SetActive(true);
            LevelManager.Instance.isGameActive = false;
        }
    }
}

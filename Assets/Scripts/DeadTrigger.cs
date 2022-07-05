using UnityEngine;

public class DeadTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if ((other.transform.tag == "ball" || other.transform.tag == "player") && LevelManager.Instance.isGameActive)
		{
			LevelManager.Instance.DeathPanel.SetActive(true);
			LevelManager.Instance.isGameActive = false;
		}
	}
}

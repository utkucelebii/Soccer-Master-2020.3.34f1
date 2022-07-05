using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Image CircleImg;
    [SerializeField] private Text progressTxt;

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        transform.gameObject.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            CircleImg.fillAmount = progress;
            progressTxt.text = (progress * 100).ToString();
            yield return null;
        }
    }

    private void Start()
    {
        LoadLevel(1);
    }
}

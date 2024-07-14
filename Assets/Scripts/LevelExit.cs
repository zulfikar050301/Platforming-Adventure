using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f; //declare delay speed
    void OnTriggerEnter2D(Collider2D other) //fungsi untuk karakter jika bersentuhan dengan object exit
    {
        if(other.tag == "Player")
        {
            StartCoroutine(LoadNextLevel());
        }
    }
    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; //untuk mendapatkan nomer scene yang sedang berlangsung
        int nextSceneIndex = currentSceneIndex + 1; //declare next scene


        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings) //probabilitas jika player sudah sampai di scene terakhir
        {
            nextSceneIndex = 0; //maka kembali ke scene awal
        }
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(nextSceneIndex); //untuk meload scene selanjutnya
    }
}

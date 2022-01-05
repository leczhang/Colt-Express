using UnityEngine;
using UnityEngine.SceneManagement;
 
public class Control : MonoBehaviour
{
    public void NextScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChangerManager : MonoBehaviour {

    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);


    }
	
}

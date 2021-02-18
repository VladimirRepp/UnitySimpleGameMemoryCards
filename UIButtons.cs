using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    public void Restart()
    {
        //При загрузки сцены нужно указать
        //название или индекс сцены
        SceneManager.LoadScene("Scene");
    }
}

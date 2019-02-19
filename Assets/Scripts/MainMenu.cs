using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public bool isStart;
	void Start () {
        Cursor.visible = true;
	}

    private void OnMouseUp()
    {
        if (isStart)
        {
            SceneManager.LoadScene(1);
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}

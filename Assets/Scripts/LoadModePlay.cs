using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadModePlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnevsOne()
    {
        SceneManager.LoadScene("1vs1");
    }
    public void OnevsMany()
    {
        SceneManager.LoadScene("1vsMany");
    }
    public void ManyvsMany()
    {
        SceneManager.LoadScene("ManyvsMany");
    }

}

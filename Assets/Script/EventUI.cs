using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EventUI : MonoBehaviour
{
    public List<GameObject> listaInstrucciones;
    public int currentIndex = 0;
    public List<string> cadenasInstrucciones;
    public TextMeshProUGUI textMeshProUGUI;
    private void Awake() 
    {
        DontDestroyOnLoad(this.gameObject);    
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateVisibility();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateVisibility() 
    {
        for (int i = 0; i < listaInstrucciones.Count; i++) 
        {
            listaInstrucciones[i].SetActive(i == currentIndex);
        }
    }

    public void ChangeSceneByIndex(int sceneIndex)
    {
    SceneManager.LoadScene(sceneIndex);
    }

    public void ChangeSceneByName(string sceneName) 
    {
        SceneManager.LoadScene(sceneName);
    }

    public void CycleObject(int direccion) 
    {
        currentIndex = (currentIndex + direccion + listaInstrucciones.Count) % listaInstrucciones.Count;

        UpdateVisibility();
    }

    private void UpdateText() 
    {
        if (cadenasInstrucciones.Count > 0 && textMeshProUGUI != null)
        {
            textMeshProUGUI.text = cadenasInstrucciones[currentIndex];
        }
    } 

    public void CycleText(int direction) 
    {
        currentIndex = (currentIndex + direction + cadenasInstrucciones.Count) % cadenasInstrucciones.Count;
        UpdateText();
    }

    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void ExitGame()
    {
        Debug.Log("Va a salir");
        Application.Quit();
        Debug.Log("Ya salió");
    }
}

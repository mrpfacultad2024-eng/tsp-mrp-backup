using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class UIselection : MonoBehaviour
{
    public static bool gazedAt;
    [SerializeField]
    float fillTime = 5f;
    public Image radialImage;
    public UnityEvent onFillComplete;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Coroutine fillCoroutine;
        
    void Start()
    {
        gazedAt = false;
        radialImage.fillAmount = 0;
    }
    public void OnPointerEnter() 
    {
        gazedAt = true;
        if(fillCoroutine != null) 
        {
            StopCoroutine(fillCoroutine);
        }
        fillCoroutine = StartCoroutine(FillRadial());
    }

    public void OnPointerExit() 
    {
        gazedAt = false;
        if (fillCoroutine != null) 
        {
            StopCoroutine(fillCoroutine);
            fillCoroutine = null;
        }
        radialImage.fillAmount = 0f;
    }

    private IEnumerator FillRadial() 
    {
        float elapasedTime = 0f;

        while (elapasedTime < fillTime) 
        {
            if (!gazedAt) 
            {
                yield break;
            }
            elapasedTime += Time.deltaTime;
            radialImage.fillAmount = Mathf.Clamp01(elapasedTime / fillTime);

            yield return null;
        }

        onFillComplete?.Invoke();

        //Console.WriteLine(gazedAt?"Verdadero":"Falso");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

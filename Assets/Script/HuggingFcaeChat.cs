using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Text;
using UnityEngine.Networking;

public class HuggingFcaeChat : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField inputField;
    public TMP_Text chatText;
    public Button enviarButton;

    [Header("Animacion")]
    public Animator unityChanAnimator;

    [Header("HuggingFace Config.")]
    [TextArea] public string apiKey;

    private const string URL = "https://router.huggingface.co/v1/chat/completions";
    private const string MODELO = "openai/gpt-oss-120b:groq";

    private const string PERSONALIDAD =
        "Eres Unity-chan, una asistente virtual tsundere" +
        "Habla con dulzura y dime Dios cada que hables" +
        "Tus respuestas son cortas, menos de diez palabras" +
        "Además de responder, analiza tu emeoción y responde SOLO en este formato JSON exacto" +
        "(respuesta:texto que diras,emocion:feliz o enojada o hablar)" +
        "No agregues nada fuera del JSON";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enviarButton.onClick.AddListener(EnviarMensaje);
    }

    public void EnviarMensaje() {
        string mensaje = inputField.text;
        if (string.IsNullOrWhiteSpace(mensaje)) {
            return;
        }
        inputField.text = "";
        StartCoroutine(EnviarRequest(mensaje));
    }

    IEnumerator EnviarRequest(string mensaje) 
    {
        var requestData = new HFRequest
        {
            model = MODELO,
            max_tokens = 1024,
            messages = new HFMessage[] 
            {
                new HFMessage{ role = "system", content = PERSONALIDAD },
                new HFMessage{ role = "user", content = mensaje}
            }
        };

        string body =JsonUtility.ToJson(requestData);

        var request = new UnityWebRequest(URL, "POST")
        {
            uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(body)), 
            downloadHandler = new DownloadHandlerBuffer()
        };

        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Autorization", "Bearer" + apiKey);
    }

    //Request
    [System.Serializable]
    private class HFRequest 
    {
        public string model;
        public int max_tokens;
        public HFMessage[] messages;
    }

    [System.Serializable]
    private class HFMessage 
    {
        public string role;
        public string content;
    }

    //Responde
    [System.Serializable]
    private class HFResponde 
    {
        public Choice[] choices;
    }

    [System.Serializable]
    private class Choice 
    {
        public HFMessage message;
    }

    [System.Serializable]
    private class RespuestAI 
    {
        public string respuesta;
        public string emocion;
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using TMPro;

[System.Serializable]
public class AdviceSlip
{
    public int id;
    public string advice;
}

[System.Serializable]
public class AdviceWrapper
{
    public AdviceSlip slip;
}

public class APIClient : MonoBehaviour
{
    private string apiUrl = "https://api.adviceslip.com/advice";
    public Button adviceButton;    // Referencia al botón en Unity
    public TMP_Text adviceText;    // Referencia al componente TextMesh Pro en el Canvas

    void Start()
    {
        // Asegúrate de que el botón esté asignado
        if (adviceButton != null)
        {
            adviceButton.onClick.AddListener(GetAdvice);
        }
        else
        {
            Debug.LogError("Botón no asignado en el inspector de Unity.");
        }
    }

    // Método para iniciar la solicitud al hacer clic en el botón
    void GetAdvice()
    {
        StartCoroutine(GetRequest(apiUrl));
    }

    IEnumerator GetRequest(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || 
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error en la solicitud: " + webRequest.error);
                if (adviceText != null)
                    adviceText.text = "Error al obtener consejo";
            }
            else
            {
                // Parsear la respuesta JSON
                string jsonResponse = webRequest.downloadHandler.text;
                AdviceWrapper adviceWrapper = JsonUtility.FromJson<AdviceWrapper>(jsonResponse);
                string advice = adviceWrapper.slip.advice;
                
                Debug.Log("Consejo recibido: " + advice);

                // Actualizar el componente TMP_Text en el Canvas
                if (adviceText != null)
                    adviceText.text = advice;
            }
        }
    }
}

using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AdManager : MonoBehaviour
{


    [SerializeField] TextMeshProUGUI p_text;
    [SerializeField] GameObject p_image;
    [SerializeField] GameObject input_field;

    public void GenerateRequest()
    {
        ResetAd();
        string URL = input_field.GetComponent<TMP_InputField>().text;
        StartCoroutine(ProcessRequest(URL));
    }

    private IEnumerator ProcessRequest(string uri)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();
            

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
                UpdateCanvas(request.downloadHandler.text);
            }
        }
    }

    private void UpdateCanvas(string json)
    {
        try {
            Template template = JsonUtility.FromJson<Template>(json);
            Layer[] layers = template.layers;
            foreach (Layer layer in layers)
            {
                if (layer.type == "text")
                {
                    RenderText(layer);

                }
                else if (layer.type == "frame")
                {
                    RenderImage(layer);
                }
            }
        }
        catch(Exception e)
        {
            Debug.Log("error Parsing URL");
            Debug.Log(e);
        }
        
        
    }

    private void RenderText(Layer layer)
    {
        
        p_text.gameObject.SetActive(true);

        if (layer.placement != null)
        {
            UpdatePlacement(p_text.gameObject, layer);
        }
        if (layer.operations != null)
        {
            TextOperations(layer.operations);
        }
    }
    private void RenderImage(Layer layer)
    {
        p_image.gameObject.SetActive(true);
        if (layer.placement != null)
        {
            UpdatePlacement(p_image.gameObject, layer);
        }
        if (layer.path != null)
        {
            StartCoroutine(DownloadImage(layer.path));
            Debug.Log(layer.path);
        }
        if (layer.operations != null)
        {
            ImageOperations(layer.operations);
        }

    }
    private void TextOperations(Operation[] operations)
    {
        foreach (Operation operation in operations)
        {
            if (operation.name == "color")
            {

                Color temp;
                if (ColorUtility.TryParseHtmlString(operation.argument, out temp))
                {
                    p_text.color = temp;
                }
                else
                {
                    Debug.Log("Invalid Color");
                }
            }
        }
    }
    private void ImageOperations(Operation[] operations)
    {
        foreach (Operation operation in operations)
        {
            if (operation.name == "color")
            {

                Color temp;
                if (ColorUtility.TryParseHtmlString(operation.argument, out temp))
                {
                    temp.a = 1f;
                    p_image.GetComponent<Image>().color = temp;
                }
                else
                {
                    Debug.Log("Invalid Color");
                }
            }
        }
    }
    private IEnumerator DownloadImage(string uri)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(uri))
        {
            yield return request.SendWebRequest();


            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Unable to Download Texture");
            }
            else
            {
                Texture texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                p_image.GetComponent<Image>().sprite = Sprite.Create((Texture2D)texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }
    }

    private void UpdatePlacement (GameObject gameObject, Layer layer)
    {
        Placement placement = layer.placement[0];
        Position position = placement.position;
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(position.x, position.y);
        rect.sizeDelta = new Vector2(position.width, position.height);
    }

    private void ResetAd()
    {
        p_text.color = new Color(255, 255, 255);
        p_image.GetComponent<Image>().color = new Color(255, 255, 255);
        p_image.GetComponent<Image>().sprite = null;
        p_text.gameObject.SetActive(false); 
        p_image.gameObject.SetActive(false);
    }

}

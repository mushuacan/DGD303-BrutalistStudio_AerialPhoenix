using UnityEngine;
using UnityEngine.UI;
using TMPro;  
using DG.Tweening;
using System.Collections;


public class WriteText : MonoBehaviour
{
    public TextMeshProUGUI TextBox;
    public ShowSpeakersFace ShowFace;
    public float characterDelay = 0.05f; // Her bir karakter arasýnda bekleme süresi
    public float cursorBlinkSpeed = 0.5f; // Yanýp sönme hýzý


    private void Start()
    {
        AnimateText("Komando", "Evet,");
        //AnimateText("Komando", "Aslýnda ben iyi bir insaným sadece savaþta yaptýklarýmdan ötürü insanlar beni kötü zannediyor. Ben ülkemi savunuyorum ve masumlarý öldürerek deðil. Ben masumlarý öldüren gözü dönmüþ cani insanýmsý yaratýklarý öldürüyorum ki masum insanlar rahatça yaþasýnlar");
        
    }

    public void AnimateText(string speakerName, string Text, float speakingSpeed = 0.05f)
    {
        TextBox.text = "";
        ShowFace.ShowImage(speakerName);

        StartCoroutine(TypeText(Text));
    }

    
    private IEnumerator TypeText(string fullText)
    {
        string emptyText = "";

        // fullText'in her bir karakteri için döngü
        foreach (char letter in fullText)
        {
            emptyText += letter;
            TextBox.text = emptyText + "_"; // Her karakteri ekle

            // Karakterler arasýnda bekleme süresi
            yield return new WaitForSeconds(characterDelay);
        }

        StartCoroutine(CursorBlink(fullText));
    }


    // Cursor (alt çizgi) yanýp sönmesini saðlayan coroutine
    private IEnumerator CursorBlink(string fullText)
    {
        string cursor = "_"; // Yanýp sönmesi gereken cursor karakteri
        while (true) // Sonsuz döngüde yanýp sönmesini saðlýyoruz
        {
            // Cursor karakterini ekliyoruz
            TextBox.text = fullText + cursor;

            // 1.5 saniye bekliyoruz
            yield return new WaitForSeconds(cursorBlinkSpeed);

            // Cursor'u silip tekrar yazýyoruz
            TextBox.text = fullText;

            // 1.5 saniye daha bekliyoruz
            yield return new WaitForSeconds(cursorBlinkSpeed);
        }
    }

}

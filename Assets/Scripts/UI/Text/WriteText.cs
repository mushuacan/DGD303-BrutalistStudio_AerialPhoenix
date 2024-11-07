using UnityEngine;
using UnityEngine.UI;
using TMPro;  
using DG.Tweening;
using System.Collections;


public class WriteText : MonoBehaviour
{
    public TextMeshProUGUI TextBox;
    public ShowSpeakersFace ShowFace;
    public float characterDelay = 0.05f; // Her bir karakter aras�nda bekleme s�resi
    public float cursorBlinkSpeed = 0.5f; // Yan�p s�nme h�z�


    private void Start()
    {
        AnimateText("Komando", "Evet,");
        //AnimateText("Komando", "Asl�nda ben iyi bir insan�m sadece sava�ta yapt�klar�mdan �t�r� insanlar beni k�t� zannediyor. Ben �lkemi savunuyorum ve masumlar� �ld�rerek de�il. Ben masumlar� �ld�ren g�z� d�nm�� cani insan�ms� yarat�klar� �ld�r�yorum ki masum insanlar rahat�a ya�as�nlar");
        
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

        // fullText'in her bir karakteri i�in d�ng�
        foreach (char letter in fullText)
        {
            emptyText += letter;
            TextBox.text = emptyText + "_"; // Her karakteri ekle

            // Karakterler aras�nda bekleme s�resi
            yield return new WaitForSeconds(characterDelay);
        }

        StartCoroutine(CursorBlink(fullText));
    }


    // Cursor (alt �izgi) yan�p s�nmesini sa�layan coroutine
    private IEnumerator CursorBlink(string fullText)
    {
        string cursor = "_"; // Yan�p s�nmesi gereken cursor karakteri
        while (true) // Sonsuz d�ng�de yan�p s�nmesini sa�l�yoruz
        {
            // Cursor karakterini ekliyoruz
            TextBox.text = fullText + cursor;

            // 1.5 saniye bekliyoruz
            yield return new WaitForSeconds(cursorBlinkSpeed);

            // Cursor'u silip tekrar yaz�yoruz
            TextBox.text = fullText;

            // 1.5 saniye daha bekliyoruz
            yield return new WaitForSeconds(cursorBlinkSpeed);
        }
    }

}

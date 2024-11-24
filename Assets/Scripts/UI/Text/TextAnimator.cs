using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

[System.Serializable]
public class ChapterData
{
    public int ChapterNumber;               // Bölüm numarasý
    public List<MessageData> Messages;      // Bölümdeki mesajlar
}

[System.Serializable]
public class MessageData
{
    public string SpeakerName;              // Konuþmacý ismi
    [TextArea] public string Text;          // Mesaj metni
    public bool Continue;                   // Önceki mesaja devam mý?
}

public class TextAnimator : MonoBehaviour
{
    public TextMeshProUGUI TextBox;         // Metin alaný
    public ShowSpeakersFace ShowFace;       // Konuþmacý yüzü
    public List<ChapterData> Chapters;      // Tüm bölümleri içeren liste
    public float characterDelay = 0.05f;    // Harf yazma hýzý
    public float cursorBlinkSpeed = 0.5f;   // Yanýp sönen `_` hýzý

    private int currentChapterIndex = 0;    // Mevcut bölüm
    private int currentMessageIndex = 0;    // Mevcut mesaj
    private string currentText = "";        // Þu ana kadar yazýlan metin
    private Tween cursorBlinkTween;         // Yanýp sönen `_` için Tween
    private Tween typingTween;              // Yazý yazma için animasyon
    private bool isTyping = false;          // Yazma iþlemi sürüyor mu?
    private bool waitingForNextMessage = false; // Bir sonraki mesaj için bekleniyor mu?

    private void Start()
    {
        if (Chapters.Count > 0 && Chapters[0].Messages.Count > 0)
        {
            ShowMessage(0, 0); // Ýlk mesajý baþlat
        }
        else
        {
            Debug.LogWarning("Bölüm veya mesaj tanýmlanmamýþ.");
        }
    }

    private void Update()
    {
        // SPACE veya sol fare týklamasý ile kontrol
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            if (isTyping)
            {
                // Yazma iþlemi devam ediyorsa hemen tamamla
                typingTween.Kill(true);
                TextBox.text = currentText + "_";
                isTyping = false;
                waitingForNextMessage = true; // Bir sonraki mesaja geçiþ bekleniyor
            }
            else if (waitingForNextMessage)
            {
                // Yazma bittiyse bir sonraki mesaja geç
                waitingForNextMessage = false;
                ShowNextMessage();
            }
        }
    }

    public void ShowMessage(int chapterIndex, int messageIndex)
    {


        if (chapterIndex >= Chapters.Count || messageIndex >= Chapters[chapterIndex].Messages.Count)
        {
            Debug.LogWarning("Geçersiz bölüm veya mesaj.");
            return;
        }


        MessageData message = Chapters[chapterIndex].Messages[messageIndex];

        ShowFace.ShowImage(message.SpeakerName);

        if (!message.Continue)
        {
            currentText = "";
        }

        currentText += message.Text;
        StartTyping(currentText);
    }

    private void StartTyping(string fullText)
    {
        isTyping = true;
        waitingForNextMessage = false;

        // Yanýp sönme animasyonunu durdur
        if (cursorBlinkTween != null)
        {
            cursorBlinkTween.Kill();
        }

        // TextBox'ý temizle ve yazma iþlemine baþla
        string visibleText = ""; // Animasyon için kullanýlan geçici string
        TextBox.text = currentText + "_"; // Mevcut metin üzerine yazýyoruz

        // DOTween animasyonu baþlat
        typingTween = DOTween.To(() => visibleText, x =>
        {
            visibleText = x;
            TextBox.text = visibleText + "_"; // Her harf eklendiðinde sonuna `_` eklenir
        },
        fullText, fullText.Length * characterDelay) // Animasyon süresi harf sayýsýna göre belirlenir
        .SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            // Yazma iþlemi tamamlandýðýnda
            isTyping = false;
            StartCursorBlink(); // Yanýp sönme animasyonunu baþlat
            waitingForNextMessage = true; // Sonraki mesaja geçiþ bekleniyor
        });
    }


    private void StartCursorBlink()
    {
        // Yanýp sönme animasyonu
        cursorBlinkTween = DOTween.To(() => "_", x => TextBox.text = currentText + x, "", cursorBlinkSpeed)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void ShowNextMessage()
    {
        currentMessageIndex++;

        if (currentMessageIndex >= Chapters[currentChapterIndex].Messages.Count)
        {
            Debug.Log("Bu bölüm bitti");
        //    currentChapterIndex++;
        //    currentMessageIndex = 0;
        }

        if (currentChapterIndex < Chapters.Count && currentMessageIndex < Chapters[currentChapterIndex].Messages.Count)
        {
            ShowMessage(currentChapterIndex, currentMessageIndex);
        }
        else
        {
            Debug.Log("Tüm bölümler tamamlandý.");
        }
    }
}

using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

[System.Serializable]
public class ChapterData
{
    public int ChapterNumber;               // B�l�m numaras�
    public List<MessageData> Messages;      // B�l�mdeki mesajlar
}

[System.Serializable]
public class MessageData
{
    public string SpeakerName;              // Konu�mac� ismi
    [TextArea] public string Text;          // Mesaj metni
    public bool Continue;                   // �nceki mesaja devam m�?
}

public class TextAnimator : MonoBehaviour
{
    public TextMeshProUGUI TextBox;         // Metin alan�
    public ShowSpeakersFace ShowFace;       // Konu�mac� y�z�
    public List<ChapterData> Chapters;      // T�m b�l�mleri i�eren liste
    public float characterDelay = 0.05f;    // Harf yazma h�z�
    public float cursorBlinkSpeed = 0.5f;   // Yan�p s�nen `_` h�z�

    private int currentChapterIndex = 0;    // Mevcut b�l�m
    private int currentMessageIndex = 0;    // Mevcut mesaj
    private string currentText = "";        // �u ana kadar yaz�lan metin
    private Tween cursorBlinkTween;         // Yan�p s�nen `_` i�in Tween
    private Tween typingTween;              // Yaz� yazma i�in animasyon
    private bool isTyping = false;          // Yazma i�lemi s�r�yor mu?
    private bool waitingForNextMessage = false; // Bir sonraki mesaj i�in bekleniyor mu?

    private void Start()
    {
        if (Chapters.Count > 0 && Chapters[0].Messages.Count > 0)
        {
            ShowMessage(0, 0); // �lk mesaj� ba�lat
        }
        else
        {
            Debug.LogWarning("B�l�m veya mesaj tan�mlanmam��.");
        }
    }

    private void Update()
    {
        // SPACE veya sol fare t�klamas� ile kontrol
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            if (isTyping)
            {
                // Yazma i�lemi devam ediyorsa hemen tamamla
                typingTween.Kill(true);
                TextBox.text = currentText + "_";
                isTyping = false;
                waitingForNextMessage = true; // Bir sonraki mesaja ge�i� bekleniyor
            }
            else if (waitingForNextMessage)
            {
                // Yazma bittiyse bir sonraki mesaja ge�
                waitingForNextMessage = false;
                ShowNextMessage();
            }
        }
    }

    public void ShowMessage(int chapterIndex, int messageIndex)
    {


        if (chapterIndex >= Chapters.Count || messageIndex >= Chapters[chapterIndex].Messages.Count)
        {
            Debug.LogWarning("Ge�ersiz b�l�m veya mesaj.");
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

        // Yan�p s�nme animasyonunu durdur
        if (cursorBlinkTween != null)
        {
            cursorBlinkTween.Kill();
        }

        // TextBox'� temizle ve yazma i�lemine ba�la
        string visibleText = ""; // Animasyon i�in kullan�lan ge�ici string
        TextBox.text = currentText + "_"; // Mevcut metin �zerine yaz�yoruz

        // DOTween animasyonu ba�lat
        typingTween = DOTween.To(() => visibleText, x =>
        {
            visibleText = x;
            TextBox.text = visibleText + "_"; // Her harf eklendi�inde sonuna `_` eklenir
        },
        fullText, fullText.Length * characterDelay) // Animasyon s�resi harf say�s�na g�re belirlenir
        .SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            // Yazma i�lemi tamamland���nda
            isTyping = false;
            StartCursorBlink(); // Yan�p s�nme animasyonunu ba�lat
            waitingForNextMessage = true; // Sonraki mesaja ge�i� bekleniyor
        });
    }


    private void StartCursorBlink()
    {
        // Yan�p s�nme animasyonu
        cursorBlinkTween = DOTween.To(() => "_", x => TextBox.text = currentText + x, "", cursorBlinkSpeed)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void ShowNextMessage()
    {
        currentMessageIndex++;

        if (currentMessageIndex >= Chapters[currentChapterIndex].Messages.Count)
        {
            Debug.Log("Bu b�l�m bitti");
        //    currentChapterIndex++;
        //    currentMessageIndex = 0;
        }

        if (currentChapterIndex < Chapters.Count && currentMessageIndex < Chapters[currentChapterIndex].Messages.Count)
        {
            ShowMessage(currentChapterIndex, currentMessageIndex);
        }
        else
        {
            Debug.Log("T�m b�l�mler tamamland�.");
        }
    }
}

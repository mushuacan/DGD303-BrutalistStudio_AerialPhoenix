using System.Collections;
using UnityEngine;
using TMPro;  // TextMeshPro için gerekli kütüphane

public class TextAnimator : MonoBehaviour
{
    public TextMeshProUGUI TextBox;      // TextMeshProUGUI bileþeni
    public ShowSpeakersFace ShowFace;    // Yüz gösterimi bileþeni
    public float characterDelay = 0.05f; // Her bir karakter arasýnda bekleme süresi
    public float cursorBlinkSpeed = 0.5f; // Cursor yanýp sönme hýzý

    private bool isBlinking = false;
    private bool isMouseClicked = false;  // Fare týklanýp týklanmadýðýný kontrol eden bool
    private Coroutine typingCoroutine = null;  // Yazý yazma coroutine referansý
    private Coroutine clickCoroutine = null;   // Fare týklama coroutine referansý
    private Coroutine cursorBlinkCoroutine = null;  // Cursor blink coroutine referansý

    private string fullTextForCurrentAnimation = ""; // Þu anki metni saklamak için deðiþken

    private void Start()
    {
        // Ýlk metin animasyonunu baþlatýyoruz
        StartCoroutine(PlayTextAnimations());
    }

    private IEnumerator PlayTextAnimations()
    {
        // Ýlk animasyonu baþlatýyoruz ve tamamlanmasýný bekliyoruz
        yield return StartCoroutine(AnimateText("Komando", "Evet,£p3,0* aslýnda ben iyi bir insaným sadece savaþta yaptýklarýmdan ötürü insanlar beni kötü zannediyor.£p2,5*"));

        // Fare týklamasýný bekliyoruz
        yield return StartCoroutine(WaitForMouseClick());

        // Ýkinci animasyonu baþlatýyoruz
        yield return StartCoroutine(AnimateText("Komando", "Asker,£p1,5* senin görevin masumlarý kurtarmak."));
    }

    private IEnumerator WaitForMouseClick()
    {
        // Fare týklanmasýný bekliyoruz
        while (!Input.GetMouseButtonDown(0))  // Mouse0 => sol fare tuþu
        {
            yield return null;  // Bir sonraki frame'e geç
        }
        isMouseClicked = true;  // Fare týklamasý algýlandýðýnda bool deðerini true yapýyoruz
    }

    private IEnumerator AnimateText(string speakerName, string fullText, float speakingSpeed = 0.05f)
    {
        TextBox.text = "";  // TextBox'ý boþaltýyoruz
        ShowFace.ShowImage(speakerName);  // Konuþan kiþinin yüzünü gösteriyoruz

        fullTextForCurrentAnimation = fullText; // Mevcut metni saklýyoruz

        // Yazý animasyonunu baþlatýyoruz
        typingCoroutine = StartCoroutine(TypeText(fullText, speakingSpeed));  // Coroutine referansýný sakla

        // Fare týklama coroutine'ini baþlatýyoruz
        clickCoroutine = StartCoroutine(WaitForMouseClick());

        // Animasyonun tamamlanmasýný bekliyoruz
        yield return typingCoroutine;

        // Fare týklama coroutine'ini durduruyoruz
        if (clickCoroutine != null)
        {
            StopCoroutine(clickCoroutine);
        }

        // Cursor blink coroutine'ini baþlatýyoruz
        cursorBlinkCoroutine = StartCoroutine(CursorBlink(TextBox.text));
    }

    private IEnumerator TypeText(string fullText, float speakingSpeed)
    {
        string emptyText = "";
        int index = 0;

        while (index < fullText.Length)
        {
            char letter = fullText[index];

            // Komutlarý kontrol et
            if (letter == '£' && index + 1 < fullText.Length)
            {
                int commandEndIndex = fullText.IndexOf('*', index);
                if (commandEndIndex != -1)
                {
                    string command = fullText.Substring(index, commandEndIndex - index + 1); // Komutu al
                    index = commandEndIndex + 1; // Komutun bitiþinden sonrasýna geç

                    // Komutlarý iþle
                    yield return ProcessCommand(command, emptyText);
                    continue;  // Komut iþlendi, normal yazý yazmaya devam et
                }
            }

            // Eðer komut deðilse, karakteri ekleyelim
            emptyText += letter;
            TextBox.text = emptyText + "_";  // "_" cursor'u ekleyelim

            // Karakterler arasýnda bekleme süresi
            yield return new WaitForSeconds(speakingSpeed);

            index++;
        }

        // Yazý bitince cursor blink iþlemine geçelim
        StartCoroutine(CursorBlink(emptyText));
    }

    private IEnumerator ProcessCommand(string command, string textUntilNow)
    {
        if (command.StartsWith("£p"))
        {
            // £p komutu: Duraklatma komutu (pause)
            string pauseDurationStr = command.Substring(2, command.Length - 3); // "3.0f" kýsmýný al

            if (float.TryParse(pauseDurationStr, out float pauseDuration))
            {
                // Duraklatma süresi boyunca yazýyý duraklat
                StartCoroutine(CursorBlink(textUntilNow));  // Hemen cursor yanýp sönmeye baþlasýn
                yield return new WaitForSeconds(pauseDuration); // Duraklatma süresi
                isBlinking = false;
                StopCoroutine(CursorBlink(""));  // Cursor yanýp sönmesini durdur
            }
            else
            {
                Debug.LogError("Geçersiz duraklatma süresi: " + pauseDurationStr);
            }
        }
        else
        {
            Debug.LogError("Desteklenmeyen komut: " + command);
        }
    }

    private IEnumerator CursorBlink(string fullText)
    {
        string cursor = "_"; // Yanýp sönmesi gereken cursor karakteri
        isBlinking = true;

        while (isBlinking)  // Cursor'un yanýp sönmesini saðlýyoruz
        {
            TextBox.text = fullText + cursor;  // Cursor'u ekliyoruz
            yield return new WaitForSeconds(cursorBlinkSpeed);  // Yanýp sönme süresi
            TextBox.text = fullText;  // Cursor'u kaldýrýyoruz
            yield return new WaitForSeconds(cursorBlinkSpeed);  // Yanýp sönme süresi
        }
    }

    // Komutlarý yazýdan temizleyen fonksiyon
    private string RemoveCommands(string text)
    {
        string cleanedText = "";
        int i = 0;

        while (i < text.Length)
        {
            if (text[i] == '£')
            {
                // Komut karakteri baþlýyor, komutu atla
                int commandEnd = text.IndexOf('*', i);
                if (commandEnd != -1)
                {
                    // Komutun sonuna kadar git
                    i = commandEnd + 1;
                }
                else
                {
                    // Geçersiz komut bitiþi varsa, komutu atla
                    i++;
                }
            }
            else
            {
                // Komut deðilse, normal yazýyý ekle
                cleanedText += text[i];
                i++;
            }
        }

        return cleanedText;
    }
}

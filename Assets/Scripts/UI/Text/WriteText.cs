using System.Collections;
using UnityEngine;
using TMPro;  // TextMeshPro i�in gerekli k�t�phane

public class TextAnimator : MonoBehaviour
{
    public TextMeshProUGUI TextBox;      // TextMeshProUGUI bile�eni
    public ShowSpeakersFace ShowFace;    // Y�z g�sterimi bile�eni
    public float characterDelay = 0.05f; // Her bir karakter aras�nda bekleme s�resi
    public float cursorBlinkSpeed = 0.5f; // Cursor yan�p s�nme h�z�

    private bool isBlinking = false;
    private bool isMouseClicked = false;  // Fare t�klan�p t�klanmad���n� kontrol eden bool
    private Coroutine typingCoroutine = null;  // Yaz� yazma coroutine referans�
    private Coroutine clickCoroutine = null;   // Fare t�klama coroutine referans�
    private Coroutine cursorBlinkCoroutine = null;  // Cursor blink coroutine referans�

    private string fullTextForCurrentAnimation = ""; // �u anki metni saklamak i�in de�i�ken

    private void Start()
    {
        // �lk metin animasyonunu ba�lat�yoruz
        StartCoroutine(PlayTextAnimations());
    }

    private IEnumerator PlayTextAnimations()
    {
        // �lk animasyonu ba�lat�yoruz ve tamamlanmas�n� bekliyoruz
        yield return StartCoroutine(AnimateText("Komando", "Evet,�p3,0* asl�nda ben iyi bir insan�m sadece sava�ta yapt�klar�mdan �t�r� insanlar beni k�t� zannediyor.�p2,5*"));

        // Fare t�klamas�n� bekliyoruz
        yield return StartCoroutine(WaitForMouseClick());

        // �kinci animasyonu ba�lat�yoruz
        yield return StartCoroutine(AnimateText("Komando", "Asker,�p1,5* senin g�revin masumlar� kurtarmak."));
    }

    private IEnumerator WaitForMouseClick()
    {
        // Fare t�klanmas�n� bekliyoruz
        while (!Input.GetMouseButtonDown(0))  // Mouse0 => sol fare tu�u
        {
            yield return null;  // Bir sonraki frame'e ge�
        }
        isMouseClicked = true;  // Fare t�klamas� alg�land���nda bool de�erini true yap�yoruz
    }

    private IEnumerator AnimateText(string speakerName, string fullText, float speakingSpeed = 0.05f)
    {
        TextBox.text = "";  // TextBox'� bo�alt�yoruz
        ShowFace.ShowImage(speakerName);  // Konu�an ki�inin y�z�n� g�steriyoruz

        fullTextForCurrentAnimation = fullText; // Mevcut metni sakl�yoruz

        // Yaz� animasyonunu ba�lat�yoruz
        typingCoroutine = StartCoroutine(TypeText(fullText, speakingSpeed));  // Coroutine referans�n� sakla

        // Fare t�klama coroutine'ini ba�lat�yoruz
        clickCoroutine = StartCoroutine(WaitForMouseClick());

        // Animasyonun tamamlanmas�n� bekliyoruz
        yield return typingCoroutine;

        // Fare t�klama coroutine'ini durduruyoruz
        if (clickCoroutine != null)
        {
            StopCoroutine(clickCoroutine);
        }

        // Cursor blink coroutine'ini ba�lat�yoruz
        cursorBlinkCoroutine = StartCoroutine(CursorBlink(TextBox.text));
    }

    private IEnumerator TypeText(string fullText, float speakingSpeed)
    {
        string emptyText = "";
        int index = 0;

        while (index < fullText.Length)
        {
            char letter = fullText[index];

            // Komutlar� kontrol et
            if (letter == '�' && index + 1 < fullText.Length)
            {
                int commandEndIndex = fullText.IndexOf('*', index);
                if (commandEndIndex != -1)
                {
                    string command = fullText.Substring(index, commandEndIndex - index + 1); // Komutu al
                    index = commandEndIndex + 1; // Komutun biti�inden sonras�na ge�

                    // Komutlar� i�le
                    yield return ProcessCommand(command, emptyText);
                    continue;  // Komut i�lendi, normal yaz� yazmaya devam et
                }
            }

            // E�er komut de�ilse, karakteri ekleyelim
            emptyText += letter;
            TextBox.text = emptyText + "_";  // "_" cursor'u ekleyelim

            // Karakterler aras�nda bekleme s�resi
            yield return new WaitForSeconds(speakingSpeed);

            index++;
        }

        // Yaz� bitince cursor blink i�lemine ge�elim
        StartCoroutine(CursorBlink(emptyText));
    }

    private IEnumerator ProcessCommand(string command, string textUntilNow)
    {
        if (command.StartsWith("�p"))
        {
            // �p komutu: Duraklatma komutu (pause)
            string pauseDurationStr = command.Substring(2, command.Length - 3); // "3.0f" k�sm�n� al

            if (float.TryParse(pauseDurationStr, out float pauseDuration))
            {
                // Duraklatma s�resi boyunca yaz�y� duraklat
                StartCoroutine(CursorBlink(textUntilNow));  // Hemen cursor yan�p s�nmeye ba�las�n
                yield return new WaitForSeconds(pauseDuration); // Duraklatma s�resi
                isBlinking = false;
                StopCoroutine(CursorBlink(""));  // Cursor yan�p s�nmesini durdur
            }
            else
            {
                Debug.LogError("Ge�ersiz duraklatma s�resi: " + pauseDurationStr);
            }
        }
        else
        {
            Debug.LogError("Desteklenmeyen komut: " + command);
        }
    }

    private IEnumerator CursorBlink(string fullText)
    {
        string cursor = "_"; // Yan�p s�nmesi gereken cursor karakteri
        isBlinking = true;

        while (isBlinking)  // Cursor'un yan�p s�nmesini sa�l�yoruz
        {
            TextBox.text = fullText + cursor;  // Cursor'u ekliyoruz
            yield return new WaitForSeconds(cursorBlinkSpeed);  // Yan�p s�nme s�resi
            TextBox.text = fullText;  // Cursor'u kald�r�yoruz
            yield return new WaitForSeconds(cursorBlinkSpeed);  // Yan�p s�nme s�resi
        }
    }

    // Komutlar� yaz�dan temizleyen fonksiyon
    private string RemoveCommands(string text)
    {
        string cleanedText = "";
        int i = 0;

        while (i < text.Length)
        {
            if (text[i] == '�')
            {
                // Komut karakteri ba�l�yor, komutu atla
                int commandEnd = text.IndexOf('*', i);
                if (commandEnd != -1)
                {
                    // Komutun sonuna kadar git
                    i = commandEnd + 1;
                }
                else
                {
                    // Ge�ersiz komut biti�i varsa, komutu atla
                    i++;
                }
            }
            else
            {
                // Komut de�ilse, normal yaz�y� ekle
                cleanedText += text[i];
                i++;
            }
        }

        return cleanedText;
    }
}

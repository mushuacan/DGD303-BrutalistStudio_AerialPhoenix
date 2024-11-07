using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowSpeakersFace : MonoBehaviour
{
    public RawImage displayImage; // UI Image bile�enini temsil eden bir referans (Unity'de atanacak)

    // Belirli bir resim ismini UI'da g�stermek i�in
    public void ShowImage(string imageName)
    {
        string imagePath = "UI/TextFaces/" + imageName; // "Images" klas�r�nde oldu�unu varsay�yoruz

        // Resources klas�r�nden resmi y�kle
        Texture2D texture = Resources.Load<Texture2D>(imagePath);

        if (texture != null)
        {
            displayImage.texture = texture; // Resmi Image bile�enine ata
        }
        else
        {
            Debug.LogError("Resim y�klenemedi: " + imagePath);
        }
    }
}

using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowSpeakersFace : MonoBehaviour
{
    public RawImage displayImage; // UI Image bileþenini temsil eden bir referans (Unity'de atanacak)

    // Belirli bir resim ismini UI'da göstermek için
    public void ShowImage(string imageName)
    {
        string imagePath = "UI/TextFaces/" + imageName; // "Images" klasöründe olduðunu varsayýyoruz

        // Resources klasöründen resmi yükle
        Texture2D texture = Resources.Load<Texture2D>(imagePath);

        if (texture != null)
        {
            displayImage.texture = texture; // Resmi Image bileþenine ata
        }
        else
        {
            Debug.LogError("Resim yüklenemedi: " + imagePath);
        }
    }
}

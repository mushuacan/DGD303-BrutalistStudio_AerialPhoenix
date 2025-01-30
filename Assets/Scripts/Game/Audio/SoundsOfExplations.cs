using UnityEngine;

public class SoundOfExplosions : MonoBehaviour
{
    public static SoundOfExplosions instance;  // Singleton instance
    public AudioSource audioSource;  // Sesin �alaca�� AudioSource
    public AudioClip explosionSFX1;  // Patlama ses efekti
    public AudioClip explosionSFX2;  // Patlama ses efekti
    public AudioClip explosionSFX3;  // Patlama ses efekti
    

    // Patlama sesini �alan fonksiyon
    public void PlayExplosionSFX(int number)
    {
        if (number == 1)
            audioSource.PlayOneShot(explosionSFX1); // Patlama sesini �al
        if (number == 2)
            audioSource.PlayOneShot(explosionSFX2); // Patlama sesini �al
        if (number == 3)
            audioSource.PlayOneShot(explosionSFX3); // Patlama sesini �al
    }
}

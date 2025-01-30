using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    public AudioSource audioSource; // Müzik çalan AudioSource
    public AudioClip[] müzikListesi; // Müziklerin bulunduðu dizi
    public AudioClip victoryMusic; // Müziklerin bulunduðu dizi
    public AudioClip loseMusic; // Müziklerin bulunduðu dizi

    public SoundOfExplosions deathSound;
    private bool m_isPlayable;

    private void Start()
    {
        m_isPlayable = true;
        // Baþlangýçta rastgele bir müzik çal
        PlayMusicByRandom();
    }

    public void PlayMusicByRandom()
    {
        int rastgeleIndex = Random.Range(0, müzikListesi.Length); // Müziklerden rastgele birini seç
        audioSource.clip = müzikListesi[rastgeleIndex]; // Seçilen müziði AudioSource'a ata
        audioSource.Play(); // Müziði çalmaya baþla
    }
    public void PlayVictoryMusic()
    {
        audioSource.clip = victoryMusic;  // 'victoryMusic' müziðini AudioSource'a ata
        audioSource.Play();               // Müzikleri çalmaya baþla
        m_isPlayable = false;
    }
    public void PlayLoseMusic()
    {
        audioSource.clip = loseMusic;  // 'victoryMusic' müziðini AudioSource'a ata
        audioSource.Play();               // Müzikleri çalmaya baþla
        m_isPlayable = false;
        deathSound.PlayExplosionSFX(1);
    }


    // Update içerisinde müzik bitince yeni müzik çalmayý kontrol edebiliriz
    private void Update()
    {
        if (!audioSource.isPlaying && m_isPlayable) // Eðer müzik çalmýyorsa
        {
            PlayMusicByRandom(); // Yeni bir müzik çal
        }
    }
}

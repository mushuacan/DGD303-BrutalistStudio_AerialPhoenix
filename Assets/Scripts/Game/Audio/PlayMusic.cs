using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    public AudioSource audioSource; // M�zik �alan AudioSource
    public AudioClip[] m�zikListesi; // M�ziklerin bulundu�u dizi
    public AudioClip victoryMusic; // M�ziklerin bulundu�u dizi
    public AudioClip loseMusic; // M�ziklerin bulundu�u dizi

    public SoundOfExplosions deathSound;
    private bool m_isPlayable;

    private void Start()
    {
        m_isPlayable = true;
        // Ba�lang��ta rastgele bir m�zik �al
        PlayMusicByRandom();
    }

    public void PlayMusicByRandom()
    {
        int rastgeleIndex = Random.Range(0, m�zikListesi.Length); // M�ziklerden rastgele birini se�
        audioSource.clip = m�zikListesi[rastgeleIndex]; // Se�ilen m�zi�i AudioSource'a ata
        audioSource.Play(); // M�zi�i �almaya ba�la
    }
    public void PlayVictoryMusic()
    {
        audioSource.clip = victoryMusic;  // 'victoryMusic' m�zi�ini AudioSource'a ata
        audioSource.Play();               // M�zikleri �almaya ba�la
        m_isPlayable = false;
    }
    public void PlayLoseMusic()
    {
        audioSource.clip = loseMusic;  // 'victoryMusic' m�zi�ini AudioSource'a ata
        audioSource.Play();               // M�zikleri �almaya ba�la
        m_isPlayable = false;
        deathSound.PlayExplosionSFX(1);
    }


    // Update i�erisinde m�zik bitince yeni m�zik �almay� kontrol edebiliriz
    private void Update()
    {
        if (!audioSource.isPlaying && m_isPlayable) // E�er m�zik �alm�yorsa
        {
            PlayMusicByRandom(); // Yeni bir m�zik �al
        }
    }
}

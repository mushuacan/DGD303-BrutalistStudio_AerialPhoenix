using UnityEngine;

public class creditmenu : MonoBehaviour
{
    public GameObject mainMenu;  // Referans olarak Main Menu GameObject'i
    public GameObject credits;  // Referans olarak Credits GameObject'i

    // Credits butonuna atanacak fonksiyon
    public void ShowCredits()
    {
        mainMenu.SetActive(false); // Main Menu'yu kapat
        credits.SetActive(true);  // Credits'i aç
    }

    // Back butonuna atanacak fonksiyon
    public void ShowMainMenu()
    {
        credits.SetActive(false); // Credits'i kapat
        mainMenu.SetActive(true);  // Main Menu'yu aç
    }
}
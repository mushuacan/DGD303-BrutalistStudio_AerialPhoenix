using UnityEngine;
using System.Collections.Generic;

public class ActiveObjectCounter : MonoBehaviour
{
    public Transform parentObject; // Alt objeleri kontrol edeceðimiz parent
    public string[] excludedNamesArray; // Hariç tutulacak objelerin isimleri

    private HashSet<string> excludedNamesSet;

    private void Awake()
    {
        // Hariç tutulacak isimleri bir HashSet'e dönüþtürüyoruz
        excludedNamesSet = new HashSet<string>(excludedNamesArray);
    }

    public int GetActiveObjectCount()
    {
        int activeCount = 0;

        // Parent altýndaki tüm çocuklarý döngüyle geziyoruz
        foreach (Transform child in parentObject)
        {
            // Pasif objeleri ve hariç tutulan isimleri atlýyoruz
            if (child.gameObject.activeSelf && !excludedNamesSet.Contains(child.name))
            {
                activeCount++;
            }
        }

        return activeCount;
    }
}

using UnityEngine;
using System.Collections.Generic;

public class ActiveObjectCounter : MonoBehaviour
{
    public Transform parentObject; // Alt objeleri kontrol edece�imiz parent
    public string[] excludedNamesArray; // Hari� tutulacak objelerin isimleri

    private HashSet<string> excludedNamesSet;

    private void Awake()
    {
        // Hari� tutulacak isimleri bir HashSet'e d�n��t�r�yoruz
        excludedNamesSet = new HashSet<string>(excludedNamesArray);
    }

    public int GetActiveObjectCount()
    {
        int activeCount = 0;

        // Parent alt�ndaki t�m �ocuklar� d�ng�yle geziyoruz
        foreach (Transform child in parentObject)
        {
            // Pasif objeleri ve hari� tutulan isimleri atl�yoruz
            if (child.gameObject.activeSelf && !excludedNamesSet.Contains(child.name))
            {
                activeCount++;
            }
        }

        return activeCount;
    }
}

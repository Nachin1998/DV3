using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameVersion : MonoBehaviour
{
    public TextMeshProUGUI versionText;
    void Start()
    {
        versionText.text = "v" + Application.version;
    }
}

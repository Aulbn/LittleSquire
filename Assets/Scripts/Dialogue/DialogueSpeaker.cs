using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue Speaker", menuName = "ScriptableObjects/Dialogue Speaker", order = 1)]
public class DialogueSpeaker : ScriptableObject
{
    public string nameText;
    public Color color = Color.white;
}

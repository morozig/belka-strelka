using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JokerType {
    BonusRed,
    BonusGreen,
    BonusYellow,
    BonusMulti,
    Bomb,
}

public class JokerShot : MonoBehaviour
{
    public JokerType type;

    // Start is called before the first frame update
    void Start()
    {
        var typeIndex = Random.Range(0, 5);
        type = (JokerType) typeIndex;
    }
}

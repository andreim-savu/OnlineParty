using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public abstract class GameCanvas : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI codeText;
    public abstract Task InitCanvas();
}

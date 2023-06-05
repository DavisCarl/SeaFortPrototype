using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ItemTab : MonoBehaviour
{
    public string ID;
    public string humanName;
    public int count;
    public TMP_Text idText;
    public TMP_Text countText;
    public UnityEngine.UI.Image highlightImage;
    public void Render(bool highlight)
    {
        idText.text = humanName;
        countText.text = count.ToString();
        highlightImage.enabled = highlight;
        transform.localRotation = Quaternion.identity;
    }
}

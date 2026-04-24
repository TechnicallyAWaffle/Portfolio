using UnityEngine;

[CreateAssetMenu]
public class SOProjectText : ScriptableObject
{
    [TextArea]
    public string title;
    [TextArea]
    public string status;
    [TextArea]
    public string role;
    [TextArea]
    public string summary;
}

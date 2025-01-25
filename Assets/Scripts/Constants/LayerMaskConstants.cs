using UnityEngine;

public class LayerMaskConstants
{
    public static int Default = -1;
    public int Wall = LayerMask.NameToLayer("Walls");
    public int Enemy = LayerMask.NameToLayer("Enemy");
}
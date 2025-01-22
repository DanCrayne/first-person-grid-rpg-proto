using UnityEngine;

public class LayerMaskConstants
{
    public static int Default = -1;
    public static int Wall = LayerMask.GetMask("Walls");
    public static int Enemy = LayerMask.GetMask("Enemy");
    //public static int Player = LayerMask.GetMask("Player");
}
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Level")]
public class Level : ScriptableObject
{
    public int thickness;
    public bool isBonus;
    public bool Goalkeeper;
    public GameObject map;
    public Vector3[] points;
    public Material material;
    public Material barrierMaterial;
    public Blocks[] blocks;
}

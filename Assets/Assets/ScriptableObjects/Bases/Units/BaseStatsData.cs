using UnityEngine;

[CreateAssetMenu(menuName = "Strategy/Stats/Base Stats")]
public class BaseStatsData : ScriptableObject
{
    [Header("Movement")]
    public int movementPoints = 5;
    public float moveSpeed = 8f;

    [Header("Attributes")]
    public int strength = 5;
    public int dexterity = 5;
    public int reflexes = 5;
    public int vitality = 5;
}
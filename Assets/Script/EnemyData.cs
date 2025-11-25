using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int maxHP = 100;
    public TextAsset storyFile;
    public Sprite enemySprite;
    public Sprite backgroundSprite;
}



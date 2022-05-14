using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;
    public static GameAssets Instance
    {
        get
        {
            if (instance == null)
                instance = Resources.Load<GameAssets>("GameAssets");
            
            return instance;
        }
    }

    public Transform pfEnemy;
    public Transform pfEnemyDieParticles;
    public Transform pfArrowProjectile;
    public Transform pfBuildingConstruction;
    public Transform pfBuildingDestroyedParticles;
    public Transform pfBuildingPlacedParticles;
}

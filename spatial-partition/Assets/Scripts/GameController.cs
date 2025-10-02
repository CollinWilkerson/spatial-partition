using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TextMeshProUGUI timeText;

    [Header("Soldier Setup")]
    public GameObject friendlyObj;
    public GameObject enemyObj;

    public Material enemyMaterial;
    public Material closestEnemyMaterial;

    public Transform enemyParent;
    public Transform friendlyParent;

    List<Soldier> enemySoldiers = new List<Soldier>();
    List<Soldier> friendlySoldiers = new List<Soldier>();

    List<Soldier> closestEnemies = new List<Soldier>();

    private bool slow = false;

    [SerializeField] int numberOfSoldiers = 100;

    [Header("Grid Data")]
    [SerializeField] float mapWidth = 50f;
    [SerializeField] int cellSize = 10;

    Grid grid;

    private void Start()
    {
        grid = new Grid((int)mapWidth, cellSize);

        for(int i = 0; i < numberOfSoldiers; i++)
        {
            //i just have to be interesting
            Vector3 randomPos = Random.insideUnitSphere * mapWidth / 2;
            randomPos += Vector3.one * mapWidth / 2;
            randomPos.y = 0;

            GameObject newEnemy = Instantiate(enemyObj, randomPos, Quaternion.identity);

            enemySoldiers.Add(new Enemy(newEnemy, mapWidth, grid));

            newEnemy.transform.parent = enemyParent;

            randomPos = Random.insideUnitSphere * mapWidth / 2;
            randomPos += Vector3.one * mapWidth / 2;
            randomPos.y = 0;

            GameObject newFriendly = Instantiate(friendlyObj, randomPos, Quaternion.identity);

            friendlySoldiers.Add(new Friendly(newFriendly, mapWidth));

            newFriendly.transform.parent = friendlyParent;
        }
    }

    private void Update()
    {
        timeText.text = "Frame time: " + Time.deltaTime;
        //move enemies
        for (int i = 0; i < enemySoldiers.Count; i++)
        {
            enemySoldiers[i].Move();
        }

        //reset enemies
        for (int i = 0; i < closestEnemies.Count; i++)
        {
            closestEnemies[i].soldierMeshRenderer.material = enemyMaterial;
        }

        closestEnemies.Clear();

        for (int i = 0; i < friendlySoldiers.Count; i++)
        {
            Soldier closestEnemy;
            if (slow)
            {
                closestEnemy = FindClosestEnemySlow(friendlySoldiers[i]);
            }
            else
            {
                closestEnemy = grid.FindClosestEnemy(friendlySoldiers[i]);
            }

            if (closestEnemy != null)
            {
                closestEnemy.soldierMeshRenderer.material = closestEnemyMaterial;

                closestEnemies.Add(closestEnemy);

                friendlySoldiers[i].Move(closestEnemy); //aproach target
            }
        }
    }

    Soldier FindClosestEnemySlow(Soldier soldier)
    {
        Soldier closestEnemy = null;

        float bestDistSqr = Mathf.Infinity;

        //Loop thorugh all enemies
        for (int i = 0; i < enemySoldiers.Count; i++)
        {
            //The distance sqr between the soldier and this enemy
            float distSqr = (soldier.soldierTrans.position - enemySoldiers[i].soldierTrans.position).sqrMagnitude;

            //If this distance is better than the previous best distance, then we have found an enemy that's closer
            if (distSqr < bestDistSqr)
            {
                bestDistSqr = distSqr;

                closestEnemy = enemySoldiers[i];
            }
        }

        return closestEnemy;
    }

    public void SlowSwitch()
    {
        slow = !slow;
    }
}

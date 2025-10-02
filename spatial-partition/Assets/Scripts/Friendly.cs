using UnityEngine;

//friendlies have a lot less implementation because they simply follow soldiers
//most of the freindly load is handled by the spatial partition
public class Friendly : Soldier
{
    //init friendly
    public Friendly(GameObject soldierObj, float mapWidth)
    {
        this.soldierTrans = soldierObj.transform;

        this.walkSpeed = 2f;
    }


    //Move towards the closest enemy - will always move within its grid
    public override void Move(Soldier closestEnemy)
    {
        //Rotate towards the closest enemy
        soldierTrans.rotation = Quaternion.LookRotation(closestEnemy.soldierTrans.position - soldierTrans.position);
        //Move towards the closest enemy
        soldierTrans.Translate(Vector3.forward * Time.deltaTime * walkSpeed);
    }
}

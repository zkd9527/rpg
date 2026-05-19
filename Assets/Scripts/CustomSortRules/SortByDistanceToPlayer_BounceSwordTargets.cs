using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortByDistanceToPlayer_BounceSwordTargets : IComparer<Transform>
{
    public int Compare(Transform x, Transform y)
    {
        if (x == null || y == null)
            throw new System.NotImplementedException();


        Transform player = PlayerManager.instance.player.transform;

        float distanceToSword_x = Vector2.Distance(x.position, player.position);
        float distanceToSword_y = Vector2.Distance(y.position, player.position);

        return distanceToSword_x.CompareTo(distanceToSword_y);

    }

}

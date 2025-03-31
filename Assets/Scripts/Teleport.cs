using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform nextPoint;

    public void GoThroughCar()
    {
        player.position = nextPoint.position;
    }
}

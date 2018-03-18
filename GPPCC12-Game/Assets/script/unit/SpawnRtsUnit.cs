using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnRtsUnit : MonoBehaviour {

    public Spawner spawner;

    public void SpawnUnit(string UnitName)
    {
        switch (UnitName)
        {
            case "HealingUnit":
                spawner.CmdSpawnUnit(1);
                Debug.Log("Created HealingUnit");
                break;
            case "TankUnit":
                spawner.CmdSpawnUnit(2);
                Debug.Log("Created TankUnit");
                break;
            case "DpsUnit":
                spawner.CmdSpawnUnit(3);
                Debug.Log("Created DpsUnit");
                break;
        }
    }
}

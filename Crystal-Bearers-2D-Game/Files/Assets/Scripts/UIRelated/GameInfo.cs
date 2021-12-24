using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class GameInfo : ScriptableObject
{
    // holds if new Game or Not.
    public bool newGame = false;

    public string respawnLocation = "BasicTown";

    public string fleeLocation = "BasicTown";

    public bool NewGame { get => newGame; set => newGame = value; }

    public string RespawnLocation { get => respawnLocation; set => respawnLocation = value; }

    public string FleeLocation { get => fleeLocation; set => fleeLocation = value; }
}

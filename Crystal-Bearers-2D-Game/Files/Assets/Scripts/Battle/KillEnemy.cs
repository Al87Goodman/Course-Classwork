/*****************************************************************************************
** Script to handle destruction of enemy unit when killed.
**
** Based on code from RPG tutorial at:
** gamedevacademy.org/how-to-create-an-rpg-game-in-unity-comprehensive-guide
****************************************************************************************/

using UnityEngine;

public class KillEnemy : MonoBehaviour
{
	public GameObject menuItem;

	//run when unit destroyed
	void OnDestory()
	{
		Destroy(this.menuItem);
	}
}
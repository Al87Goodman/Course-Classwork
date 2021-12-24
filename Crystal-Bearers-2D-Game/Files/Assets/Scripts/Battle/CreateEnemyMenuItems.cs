/****************************************************************************************
 * This script is built using help from the tutorial at:
 * https://gamedevacademy.org/how-to-create-an-rpg-game-in-unity-comprehensive-guide/
 ***************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*****************************************************************************************
** Script to handle creation of enemy units and battlefield when spawned.
** Creates unit and provides its functionality.
****************************************************************************************/
public class CreateEnemyMenuItems : MonoBehaviour
{
	[SerializeField]
	private GameObject targetEnemyUnitPrefab;

	[SerializeField]
	private Sprite menuItemSprite;

	[SerializeField]
	private Vector2 initialPosition;

	[SerializeField]
	private Vector2 itemDimensions;

	//this unit's kills script to handle its death
	[SerializeField]
	private KillEnemy killEnemyScript;

	void Awake()
	{
		//find and store the EnemyUnitsMenu object in game
		GameObject enemyUnitsMenu = GameObject.Find("EnemyUnitsMenu");

		//create array of all appropriate enemy units based on tag
		GameObject[] enemyUnits = GameObject.FindGameObjectsWithTag("TargetEnemyUnit");
        //set next position to draw unit in relation to previous ones
        initialPosition.y = -150;
		Vector2 nextPosition = new Vector2(this.initialPosition.x + (enemyUnits.Length * this.itemDimensions.x), this.initialPosition.y);

		/************************
		* Set Up Enemy on Field
		************************/
		GameObject targetEnemyUnit = Instantiate(this.targetEnemyUnitPrefab, enemyUnitsMenu.transform) as GameObject;
		//enemy name
		targetEnemyUnit.name = "Target" + this.gameObject.name;
		//enemy position on field
		targetEnemyUnit.transform.localPosition = nextPosition;
		//enemy sprite scale
		targetEnemyUnit.transform.localScale = new Vector2(0.7f, 0.7f);
		//set click enemy functionality
		//runs selectEnemyTarget on click of enemy
		targetEnemyUnit.GetComponent<Button>().onClick.AddListener(() => selectEnemyTarget());
		//get enemy sprite
		targetEnemyUnit.GetComponent<Image>().sprite = this.menuItemSprite;
		//set kill script to this enemy
		killEnemyScript.menuItem = targetEnemyUnit;
	}

	/***************************
	** Method to Attack Enemy
	***************************/
	public void selectEnemyTarget()
	{
        GameObject partyData = GameObject.Find("Player Party");
        partyData.GetComponent<SelectUnit>().attackEnemyTarget(this.gameObject);
	}
}
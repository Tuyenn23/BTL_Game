/* Written by Kaz Crowe */
/* UltimateJoystickScreenSizeUpdater.cs */
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UltimateJoystickScreenSizeUpdater : UIBehaviour
{
	protected override void OnRectTransformDimensionsChange ()
	{
		StartCoroutine( "YieldPositioning" );
	}

	IEnumerator YieldPositioning ()
	{
		yield return new WaitForEndOfFrame();

		UltimateJoystick[] allJoysticks = FindObjectsOfType( typeof( UltimateJoystick ) ) as UltimateJoystick[];

		for( int i = 0; i < allJoysticks.Length; i++ )
			allJoysticks[ i ].UpdatePositioning();
	}
}
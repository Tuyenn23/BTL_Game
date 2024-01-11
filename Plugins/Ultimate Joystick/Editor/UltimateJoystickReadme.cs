/* Written by Kaz Crowe */
/* UltimateJoystickReadme.cs */
using UnityEngine;
//[CreateAssetMenu( fileName = "README", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1 )]
public class UltimateJoystickReadme : ScriptableObject
{
	public string Version// ALWAYS UDPATE
	{
		get
		{
			return "2.6.0";
		}
	}
	public Texture2D icon;
	public Texture2D scriptReference;
}
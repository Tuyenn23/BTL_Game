/* Written by Kaz Crowe */
/* UltimateJoystickEditor.cs */
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[CanEditMultipleObjects]
[CustomEditor( typeof( UltimateJoystick ) )]
public class UltimateJoystickEditor : Editor
{
	UltimateJoystick targ;

	/* -----< ASSIGNED VARIABLES >----- */
	SerializedProperty joystick, joystickSizeFolder;
	SerializedProperty highlightBase, highlightJoystick;
	SerializedProperty tensionAccentUp, tensionAccentDown;
	SerializedProperty tensionAccentLeft, tensionAccentRight;
	SerializedProperty joystickBase;
	
	/* -----< SIZE AND PLACEMENT >----- */
	SerializedProperty scalingAxis, anchor, joystickTouchSize;
	SerializedProperty customTouchSize_X, customTouchSize_Y;
	SerializedProperty customTouchSizePos_X, customTouchSizePos_Y;
	SerializedProperty dynamicPositioning;
	SerializedProperty joystickSize, radiusModifier;
	SerializedProperty customSpacing_X, customSpacing_Y;

	/* -----< JOYSTICK FUNCTIONALITY >----- */
	SerializedProperty gravity;
	SerializedProperty axis, boundary, deadZone;
	SerializedProperty tapCountOption, tapCountDuration;
	SerializedProperty targetTapCount;

	/* -----< VISUAL OPTIONS >----- */
	SerializedProperty disableVisuals, extendRadius;
	SerializedProperty showHighlight, showTension;
	SerializedProperty highlightColor, tensionColorNone, tensionColorFull;
	SerializedProperty useAnimation, useFade;
	SerializedProperty fadeUntouched, fadeTouched;
	SerializedProperty fadeInDuration, fadeOutDuration;

	/* ------< SCRIPT REFERENCE >------ */
	SerializedProperty joystickName;

	// ----->>> EXAMPLE CODE //
	class ExampleCode
	{
		public string optionName = "";
		public string optionDescription = "";
		public string basicCode = "";
	}
	ExampleCode[] exampleCodes = new ExampleCode[]
	{
		new ExampleCode() { optionName = "GetHorizontalAxis ( string joystickName )", optionDescription = "Returns the horizontal axis value of the targeted Ultimate Joystick.", basicCode = "float h = UltimateJoystick.GetHorizontalAxis( \"{0}\" );" },
		new ExampleCode() { optionName = "GetVerticalAxis ( string joystickName )", optionDescription = "Returns the vertical axis value of the targeted Ultimate Joystick.", basicCode = "float v = UltimateJoystick.GetVerticalAxis( \"{0}\" );" },
		new ExampleCode() { optionName = "GetHorizontalAxisRaw ( string joystickName )", optionDescription = "Returns the raw horizontal axis value of the targeted Ultimate Joystick.", basicCode = "float h = UltimateJoystick.GetHorizontalAxisRaw( \"{0}\" );" },
		new ExampleCode() { optionName = "GetVerticalAxisRaw ( string joystickName )", optionDescription = "Returns the raw vertical axis value of the targeted Ultimate Joystick.", basicCode = "float v = UltimateJoystick.GetVerticalAxisRaw( \"{0}\" );" },
		new ExampleCode() { optionName = "GetDistance ( string joystickName )", optionDescription = "Returns the distance of the joystick image from the center of the targeted Ultimate Joystick.", basicCode = "float distance = UltimateJoystick.GetDistance( \"{0}\" );" },
		new ExampleCode() { optionName = "GetJoystickState ( string joystickName )", optionDescription = "Returns the bool value of the current state of interaction of the targeted Ultimate Joystick.", basicCode = "if( UltimateJoystick.GetJoystickState( \"{0}\" ) )" },
		new ExampleCode() { optionName = "GetTapCount ( string joystickName )", optionDescription = "Returns the bool value of the current state of taps of the targeted Ultimate Joystick.", basicCode = "if( UltimateJoystick.GetTapCount( \"{0}\" ) )" },
		new ExampleCode() { optionName = "DisableJoystick ( string joystickName )", optionDescription = "Disables the targeted Ultimate Joystick.", basicCode = "UltimateJoystick.DisableJoystick( \"{0}\" );" },
		new ExampleCode() { optionName = "EnableJoystick ( string joystickName )", optionDescription = "Enables the targeted Ultimate Joystick.", basicCode = "UltimateJoystick.EnableJoystick( \"{0}\" );" },
		new ExampleCode() { optionName = "GetUltimateJoystick ( string joystickName )", optionDescription = "Returns the Ultimate Joystick component that has been registered with the targeted name.", basicCode = "UltimateJoystick movementJoystick = UltimateJoystick.GetUltimateJoystick( \"{0}\" );" },
	};
	List<string> exampleCodeOptions = new List<string>();
	int exampleCodeIndex = 0;

	Canvas parentCanvas;
	
	public static bool isSelecting = false;
	
	
	void OnEnable ()
	{
		// Store the references to all variables.
		StoreReferences();
		
		// Register the UndoRedoCallback function to be called when an undo/redo is performed.
		Undo.undoRedoPerformed += UndoRedoCallback;
		
		parentCanvas = GetParentCanvas();

		isSelecting = true;
	}

	void OnDisable ()
	{
		// Remove the UndoRedoCallback function from the Undo event.
		Undo.undoRedoPerformed -= UndoRedoCallback;

		isSelecting = false;
	}

	Canvas GetParentCanvas ()
	{
		if( Selection.activeGameObject == null )
			return null;

		// Store the current parent.
		Transform parent = Selection.activeGameObject.transform.parent;

		// Loop through parents as long as there is one.
		while( parent != null )
		{
			// If there is a Canvas component, return the component.
			if( parent.transform.GetComponent<Canvas>() && parent.transform.GetComponent<Canvas>().enabled == true )
				return parent.transform.GetComponent<Canvas>();
			
			// Else, shift to the next parent.
			parent = parent.transform.parent;
		}
		if( parent == null && !AssetDatabase.Contains( Selection.activeGameObject ) )
			UltimateJoystickCreator.RequestCanvas( Selection.activeGameObject );

		return null;
	}

	// Function called for Undo/Redo operations.
	void UndoRedoCallback ()
	{
		// Re-reference all variables on undo/redo.
		StoreReferences();
	}

	// Function called to display an interactive header.
	void DisplayHeader ( string headerName, string editorPref )
	{
		EditorGUILayout.BeginVertical( "Toolbar" );
		GUILayout.BeginHorizontal();

		EditorGUILayout.LabelField( headerName, EditorStyles.boldLabel );
		if( GUILayout.Button( EditorPrefs.GetBool( editorPref ) == true ? "Hide" : "Show", EditorStyles.miniButton, GUILayout.Width( 50 ), GUILayout.Height( 14f ) ) )
			EditorPrefs.SetBool( editorPref, EditorPrefs.GetBool( editorPref ) == true ? false : true );

		GUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
	}

	bool CanvasErrors ()
	{
		// If the selection is currently empty, then return false.
		if( Selection.activeGameObject == null )
			return false;

		// If the selection is actually the prefab within the Project window, then return no errors.
		if( AssetDatabase.Contains( Selection.activeGameObject ) )
			return false;

		// If parentCanvas is unassigned, then get a new canvas and return no errors.
		if( parentCanvas == null )
		{
			parentCanvas = GetParentCanvas();
			return false;
		}

		// If the parentCanvas is not enabled, then return true for errors.
		if( parentCanvas.enabled == false )
			return true;

		// If the canvas' renderMode is not the needed one, then return true for errors.
		if( parentCanvas.renderMode != RenderMode.ScreenSpaceOverlay )
			return true;

		// If the canvas has a CanvasScaler component and it is not the correct option.
		if( parentCanvas.GetComponent<CanvasScaler>() && parentCanvas.GetComponent<CanvasScaler>().uiScaleMode != CanvasScaler.ScaleMode.ConstantPixelSize )
			return true;

		return false;
	}
	
	/*
	For more information on the OnInspectorGUI and adding your own variables
	in the UltimateJoystick.cs script and displaying them in this script,
	see the EditorGUILayout section in the Unity Documentation to help out.
	*/
	public override void OnInspectorGUI ()
	{
		serializedObject.Update();
		
		EditorGUILayout.Space();

		#region ERROR CHECK
		if( CanvasErrors() == true )
		{
			if( parentCanvas.renderMode != RenderMode.ScreenSpaceOverlay )
			{
				EditorGUILayout.BeginVertical( "Box" );
				EditorGUILayout.HelpBox( "The parent Canvas needs to be set to 'Screen Space - Overlay' in order for the Ultimate Joystick to function correctly.", MessageType.Error );
				EditorGUILayout.BeginHorizontal();
				if( GUILayout.Button( "Update Canvas", EditorStyles.miniButtonLeft ) )
				{
					parentCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
					parentCanvas = GetParentCanvas();
				}
				if( GUILayout.Button( "Update Joystick", EditorStyles.miniButtonRight ) )
				{
					UltimateJoystickCreator.RequestCanvas( Selection.activeGameObject );
					parentCanvas = GetParentCanvas();
				}
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
			}
			if( parentCanvas.GetComponent<CanvasScaler>() )
			{
				if( parentCanvas.GetComponent<CanvasScaler>().uiScaleMode != CanvasScaler.ScaleMode.ConstantPixelSize )
				{
					EditorGUILayout.BeginVertical( "Box" );
					EditorGUILayout.HelpBox( "The Canvas Scaler component located on the parent Canvas needs to be set to 'Constant Pixel Size' in order for the Ultimate Joystick to function correctly.", MessageType.Error );
					EditorGUILayout.BeginHorizontal();
					if( GUILayout.Button( "Update Canvas", EditorStyles.miniButtonLeft ) )
					{
						parentCanvas.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
						parentCanvas = GetParentCanvas();
						UltimateJoystick joystick = ( UltimateJoystick )target;
						joystick.UpdatePositioning();
					}
					if( GUILayout.Button( "Update Joystick", EditorStyles.miniButtonRight ) )
					{
						UltimateJoystickCreator.RequestCanvas( Selection.activeGameObject );
						parentCanvas = GetParentCanvas();
					}
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.EndVertical();
				}
			}
			return;
		}
		#endregion
		
		#region SIZE AND PLACEMENT
		/* ----------------------------------------< ** SIZE AND PLACEMENT ** >---------------------------------------- */
		DisplayHeader( "Size and Placement", "UUI_SizeAndPlacement" );
		if( EditorPrefs.GetBool( "UUI_SizeAndPlacement" ) )
		{
			EditorGUILayout.Space();
			EditorGUI.BeginChangeCheck();

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( joystick, new GUIContent( "Joystick" ) );
			EditorGUILayout.PropertyField( joystickSizeFolder, new GUIContent( "Size Folder" ) );
			EditorGUILayout.PropertyField( joystickBase, new GUIContent( "Joystick Base" ) );
			if( EditorGUI.EndChangeCheck() )
				serializedObject.ApplyModifiedProperties();

			EditorGUILayout.PropertyField( scalingAxis, new GUIContent( "Scaling Axis", "The axis to scale the Ultimate Joystick from." ) );
			EditorGUILayout.PropertyField( anchor, new GUIContent( "Anchor", "The side of the screen that the\njoystick will be anchored to." ) );
			EditorGUILayout.PropertyField( joystickTouchSize, new GUIContent( "Touch Size", "The size of the area in which\nthe touch can be initiated." ) );
			if( EditorGUI.EndChangeCheck() )
				serializedObject.ApplyModifiedProperties();

			if( targ.joystickTouchSize == UltimateJoystick.JoystickTouchSize.Custom )
			{
				EditorGUILayout.BeginVertical( "Box" );
				EditorGUILayout.LabelField( "Touch Size Customization" );
				EditorGUI.indentLevel = 1;
				EditorGUI.BeginChangeCheck();
				{
					EditorGUILayout.Slider( customTouchSize_X, 0.0f, 100.0f, new GUIContent( "Width", "The width of the Joystick Touch Area." ) );
					EditorGUILayout.Slider( customTouchSize_Y, 0.0f, 100.0f, new GUIContent( "Height", "The height of the Joystick Touch Area." ) );
					EditorGUILayout.Slider( customTouchSizePos_X, 0.0f, 100.0f, new GUIContent( "X Position", "The x position of the Joystick Touch Area." ) );
					EditorGUILayout.Slider( customTouchSizePos_Y, 0.0f, 100.0f, new GUIContent( "Y Position", "The y position of the Joystick Touch Area." ) );
				}
				if( EditorGUI.EndChangeCheck() )
					serializedObject.ApplyModifiedProperties();
				EditorGUILayout.EndVertical();
				EditorGUI.indentLevel = 0;
				EditorGUILayout.Space();
			}

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( dynamicPositioning, new GUIContent( "Dynamic Positioning", "Moves the joystick to the position of the initial touch." ) );
			EditorGUILayout.Slider( joystickSize, 1.0f, 4.0f, new GUIContent( "Joystick Size", "The overall size of the joystick." ) );
			EditorGUILayout.Slider( radiusModifier, 2.0f, 7.0f, new GUIContent( "Radius", "Determines how far the joystick can\nmove visually from the center." ) );
			EditorGUILayout.BeginVertical( "Box" );
			EditorGUILayout.LabelField( "Joystick Position" );
			EditorGUI.indentLevel = 1;
			EditorGUILayout.Slider( customSpacing_X, 0.0f, 50.0f, new GUIContent( "X Position:", "The horizontal position of the joystick on the screen." ) );
			EditorGUILayout.Slider( customSpacing_Y, 0.0f, 100.0f, new GUIContent( "Y Position:", "The vertical position of the joystick on the screen." ) );
			EditorGUI.indentLevel = 0;
			GUILayout.Space( 1 );
			EditorGUILayout.EndVertical();
			if( EditorGUI.EndChangeCheck() )
				serializedObject.ApplyModifiedProperties();
		}
		/* --------------------------------------< ** END SIZE AND PLACEMENT ** >-------------------------------------- */
		#endregion
		
		EditorGUILayout.Space();

		#region JOYSTICK FUNCTIONALITY
		DisplayHeader( "Joystick Functionality", "UUI_Functionality" );
		if( EditorPrefs.GetBool( "UUI_Functionality" ) )
		{
			EditorGUILayout.Space();
			
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( gravity, new GUIContent( "Gravity", "The speed to apply to the joystick when returning to center." ) );
			if( EditorGUI.EndChangeCheck() )
			{
				gravity.floatValue = Mathf.Clamp( gravity.floatValue, 0.0f, 60.0f );
				serializedObject.ApplyModifiedProperties();
			}

			// --------------------------< EXTEND RADIUS, AXIS, DEAD ZONE >-------------------------- //
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( extendRadius, new GUIContent( "Extend Radius", "Drags the joystick to follow the touch if it is farther than the radius." ) );
			EditorGUILayout.PropertyField( axis, new GUIContent( "Axis", "Constrains the joystick to a certain axis." ) );
			EditorGUILayout.PropertyField( boundary, new GUIContent( "Boundary", "Determines how the joystick's position is clamped." ) );
			EditorGUILayout.Slider( deadZone, 0.0f, 1.0f, new GUIContent( "Dead Zone", "Size of the dead zone. All values within this range map to neutral." ) );
			if( EditorGUI.EndChangeCheck() )
				serializedObject.ApplyModifiedProperties();

			if( targ.extendRadius == true && targ.boundary == UltimateJoystick.Boundary.Square )
				EditorGUILayout.HelpBox( "Extend Radius option will force the boundary to being circular. Please use a circular boundary when using the Extend Radius option.", MessageType.Warning );
			// ------------------------< END EXTEND RADIUS, AXIS, DEAD ZONE >------------------------ //

			// TAP COUNT //
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( tapCountOption, new GUIContent( "Tap Count", "Allows the joystick to calculate double taps and a touch and release within a certain time window." ) );
			if( EditorGUI.EndChangeCheck() )
				serializedObject.ApplyModifiedProperties();

			if( targ.tapCountOption != UltimateJoystick.TapCountOption.NoCount )
			{
				EditorGUI.indentLevel = 1;
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.Slider( tapCountDuration, 0.0f, 1.0f, new GUIContent( "Tap Time Window", "Time in seconds that the joystick can receive taps." ) );
				if( targ.tapCountOption == UltimateJoystick.TapCountOption.Accumulate )
					EditorGUILayout.IntSlider( targetTapCount, 1, 5, new GUIContent( "Target Tap Count", "How many taps to activate the Tap Count Event?" ) );

				if( EditorGUI.EndChangeCheck() )
					serializedObject.ApplyModifiedProperties();

				EditorGUI.indentLevel = 0;
			}
		}
		#endregion

		EditorGUILayout.Space();

		#region VISUAL OPTIONS
		/* ----------------------------------------< ** VISUAL OPTIONS ** >----------------------------------------- */
		DisplayHeader( "Visual Options", "UUI_VisualOptions" );
		if( EditorPrefs.GetBool( "UUI_VisualOptions" ) )
		{
			EditorGUILayout.Space();
			
			// -----------------------< DISABLE VISUALS >---------------------- //
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( disableVisuals, new GUIContent( "Disable Visuals", "Disables the visuals of the joystick." ) );
			if( EditorGUI.EndChangeCheck() )
			{
				serializedObject.ApplyModifiedProperties();
				if( targ.disableVisuals == true )
				{
					showHighlight.boolValue = false;
					showTension.boolValue = false;
					useFade.boolValue = false;
					useAnimation.boolValue = false;
					serializedObject.ApplyModifiedProperties();
				}

				SetDisableVisuals( targ );
				SetHighlight( targ );
				SetTensionAccent( targ );
				SetAnimation( targ );
			}
			
			if( targ.disableVisuals == true && targ.joystickBase == null )
				EditorGUILayout.HelpBox( "Joystick Base needs to be assigned in the Assigned Variables section.", MessageType.Error );
			// ---------------------< END DISABLE VISUALS >-------------------- //

			EditorGUI.BeginDisabledGroup( targ.disableVisuals == true );// This is the start of the disabled fields if the user is using the disableVisuals option.

			// --------------------------< USE FADE >-------------------------- //
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( useFade, new GUIContent( "Use Fade", "Fades the joystick visuals when interacted with." ) );
			if( EditorGUI.EndChangeCheck() )
			{
				serializedObject.ApplyModifiedProperties();
				if( targ.useFade == true )
					targ.gameObject.GetComponent<CanvasGroup>().alpha = targ.fadeUntouched;
				else
					targ.gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
			}
			if( targ.useFade )
			{
				EditorGUI.indentLevel = 1;
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.Slider( fadeUntouched, 0.0f, 1.0f, new GUIContent( "Fade Untouched", "The alpha of the joystick when it is NOT receiving input." ) );
				EditorGUILayout.Slider( fadeTouched, 0.0f, 1.0f, new GUIContent( "Fade Touched", "The alpha of the joystick when receiving input." ) );
				EditorGUILayout.PropertyField( fadeInDuration );
				EditorGUILayout.PropertyField( fadeOutDuration );
				if( EditorGUI.EndChangeCheck() )
				{
					serializedObject.ApplyModifiedProperties();
					targ.gameObject.GetComponent<CanvasGroup>().alpha = targ.fadeUntouched;
				}
				
				EditorGUI.indentLevel = 0;
				EditorGUILayout.Space();
			}
			// ------------------------< END USE FADE >------------------------ //

			// -----------------------< USE ANIMATION >------------------------ //
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( useAnimation, new GUIContent( "Use Animation", "Plays animations in reaction to input." ) );
			if( EditorGUI.EndChangeCheck() )
			{
				serializedObject.ApplyModifiedProperties();
				SetAnimation( targ );
			}
			if( targ.useAnimation && !targ.GetComponent<Animator>() )
				EditorGUILayout.HelpBox( "There is no Animator component attached to this GameObject.", MessageType.Error );
			// ----------------------< END USE ANIMATION >---------------------- //
			
			// --------------------------< HIGHLIGHT >-------------------------- //
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( showHighlight, new GUIContent( "Show Highlight", "Displays the highlight images with the Highlight Color variable." ) );
			if( EditorGUI.EndChangeCheck() )
			{
				serializedObject.ApplyModifiedProperties();
				SetHighlight( targ );
			}
			
			if( targ.showHighlight )
			{
				EditorGUI.indentLevel = 1;
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField( highlightBase, new GUIContent( "Base Highlight" ) );
				EditorGUILayout.PropertyField( highlightJoystick, new GUIContent( "Joystick Highlight" ) );
				if( EditorGUI.EndChangeCheck() )
					serializedObject.ApplyModifiedProperties();
				
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField( highlightColor );
				if( EditorGUI.EndChangeCheck() )
				{
					serializedObject.ApplyModifiedProperties();
					targ.UpdateHighlightColor( targ.highlightColor );

					// For every highlight image that is assigned, set the object to dirty so the properties will be applied. This is needed for prefab instances.
					if( targ.highlightBase != null )
						EditorUtility.SetDirty( targ.highlightBase );
					if( targ.highlightJoystick != null )
						EditorUtility.SetDirty( targ.highlightJoystick );
				}
				
				if( targ.highlightBase == null && targ.highlightJoystick == null )
					EditorGUILayout.HelpBox( "No highlight images have been assigned. Please assign some highlight images before continuing.", MessageType.Error );
				
				EditorGUI.indentLevel = 0;
				EditorGUILayout.Space();
			}
			// ------------------------< END HIGHLIGHT >------------------------ //

			// ---------------------------< TENSION >--------------------------- //
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( showTension, new GUIContent( "Show Tension", "Displays the visual direction of the joystick using the tension color options." ) );
			if( EditorGUI.EndChangeCheck() )
			{
				serializedObject.ApplyModifiedProperties();
				SetTensionAccent( targ );
			}
			
			if( targ.showTension )
			{
				EditorGUI.indentLevel = 1;
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField( tensionAccentUp, new GUIContent( "Tension Up" ) );
				EditorGUILayout.PropertyField( tensionAccentDown, new GUIContent( "Tension Down" ) );
				EditorGUILayout.PropertyField( tensionAccentLeft, new GUIContent( "Tension Left" ) );
				EditorGUILayout.PropertyField( tensionAccentRight, new GUIContent( "Tension Right" ) );
				if( EditorGUI.EndChangeCheck() )
					serializedObject.ApplyModifiedProperties();
				
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField( tensionColorNone, new GUIContent( "Tension None", "The color displayed when the joystick\nis closest to center." ) );
				EditorGUILayout.PropertyField( tensionColorFull, new GUIContent( "Tension Full", "The color displayed when the joystick\nis at the furthest distance." ) );
				if( EditorGUI.EndChangeCheck() )
				{
					serializedObject.ApplyModifiedProperties();
					TensionAccentReset( targ );

					// For every tension accent that is assigned, set it dirty to apply the color properties. This is needed for applying properties to prefab instances.
					if( targ.tensionAccentUp != null )
						EditorUtility.SetDirty( targ.tensionAccentUp );
					if( targ.tensionAccentDown != null )
						EditorUtility.SetDirty( targ.tensionAccentDown );
					if( targ.tensionAccentLeft != null )
						EditorUtility.SetDirty( targ.tensionAccentLeft );
					if( targ.tensionAccentRight != null )
						EditorUtility.SetDirty( targ.tensionAccentRight );
				}

				if( targ.tensionAccentUp == null && targ.tensionAccentDown == null && targ.tensionAccentLeft == null && targ.tensionAccentRight == null )
					EditorGUILayout.HelpBox( "No tension accent images have been assigned. Please assign some images before continuing.", MessageType.Error );

				EditorGUI.indentLevel = 0;
			}
			// -------------------------< END TENSION >------------------------- //

			EditorGUI.EndDisabledGroup();// This is the end for the Touch Pad option.
		}
		/* ----------------------------------------< ** END VISUAL OPTIONS ** >--------------------------------------- */
		#endregion

		EditorGUILayout.Space();
		
		#region SCRIPT REFERENCE
		/* ------------------------------------------< ** SCRIPT REFERENCE ** >------------------------------------------- */
		DisplayHeader( "Script Reference", "UUI_ScriptReference" );
		if( EditorPrefs.GetBool( "UUI_ScriptReference" ) )
		{
			EditorGUILayout.Space();
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField( joystickName, new GUIContent( "Joystick Name", "The name of the targeted joystick used for static referencing." ) );
			if( EditorGUI.EndChangeCheck() )
				serializedObject.ApplyModifiedProperties();

			if( targ.joystickName == string.Empty )
				EditorGUILayout.HelpBox( "Please assign a Joystick Name in order to be able to get this joystick's position dynamically.", MessageType.Warning );
			else
			{
				EditorGUILayout.BeginVertical( "Box" );
				GUILayout.Space( 1 );
				EditorGUILayout.LabelField( "Example Code Generator", EditorStyles.boldLabel );

				exampleCodeIndex = EditorGUILayout.Popup( "Function", exampleCodeIndex, exampleCodeOptions.ToArray() );

				EditorGUILayout.LabelField( "Function Description", EditorStyles.boldLabel );
				GUIStyle wordWrappedLabel = new GUIStyle( GUI.skin.label ) { wordWrap = true };
				EditorGUILayout.LabelField( exampleCodes[ exampleCodeIndex ].optionDescription, wordWrappedLabel );

				EditorGUILayout.LabelField( "Example Code", EditorStyles.boldLabel );
				GUIStyle wordWrappedTextArea = new GUIStyle( GUI.skin.textArea ) { wordWrap = true };
				EditorGUILayout.TextArea( string.Format( exampleCodes[ exampleCodeIndex ].basicCode, joystickName.stringValue ), wordWrappedTextArea );

				GUILayout.Space( 1 );
				EditorGUILayout.EndVertical();
			}

			if( GUILayout.Button( "Open Documentation" ) )
				UltimateJoystickReadmeEditor.OpenReadmeDocumentation();

			if( Selection.activeGameObject != null && !AssetDatabase.Contains( Selection.activeGameObject ) && Application.isPlaying )
			{
				EditorGUILayout.BeginVertical( "Box" );
				EditorGUILayout.LabelField( "Current Position:", EditorStyles.boldLabel );
				EditorGUILayout.LabelField( "Horizontal Axis: " + targ.HorizontalAxis.ToString( "F2" ) );
				EditorGUILayout.LabelField( "Vertical Axis: " + targ.VerticalAxis.ToString( "F2" ) );
				EditorGUILayout.EndVertical();
			}
		}
		/* -----------------------------------------< ** END SCRIPT REFERENCE ** >---------------------------------------- */
		#endregion
		
		EditorGUILayout.Space();
		
		Repaint();
	}
	
	// This function stores the references to the variables of the target.
	void StoreReferences ()
	{
		targ = ( UltimateJoystick )target;

		/* -----< ASSIGNED VARIABLES >----- */
		joystick = serializedObject.FindProperty( "joystick" );
		joystickSizeFolder = serializedObject.FindProperty( "joystickSizeFolder" );
		joystickBase = serializedObject.FindProperty( "joystickBase" );
		highlightBase = serializedObject.FindProperty( "highlightBase" );
		highlightJoystick = serializedObject.FindProperty( "highlightJoystick" );
		tensionAccentUp = serializedObject.FindProperty( "tensionAccentUp" );
		tensionAccentDown = serializedObject.FindProperty( "tensionAccentDown" );
		tensionAccentLeft = serializedObject.FindProperty( "tensionAccentLeft" );
		tensionAccentRight = serializedObject.FindProperty( "tensionAccentRight" );
		
		/* -----< SIZE AND PLACEMENT >----- */
		scalingAxis = serializedObject.FindProperty( "scalingAxis" );
		anchor = serializedObject.FindProperty( "anchor" );
		joystickTouchSize = serializedObject.FindProperty( "joystickTouchSize" );
		customTouchSize_X = serializedObject.FindProperty( "customTouchSize_X" );
		customTouchSize_Y = serializedObject.FindProperty( "customTouchSize_Y" );
		customTouchSizePos_X = serializedObject.FindProperty( "customTouchSizePos_X" );
		customTouchSizePos_Y = serializedObject.FindProperty( "customTouchSizePos_Y" );
		dynamicPositioning = serializedObject.FindProperty( "dynamicPositioning" );
		joystickSize = serializedObject.FindProperty( "joystickSize" );
		radiusModifier = serializedObject.FindProperty( "radiusModifier" );
		customSpacing_X = serializedObject.FindProperty( "customSpacing_X" );
		customSpacing_Y = serializedObject.FindProperty( "customSpacing_Y" );

		/* -----< JOYSTICK FUNCTIONALITY >----- */
		gravity = serializedObject.FindProperty( "gravity" );
		extendRadius = serializedObject.FindProperty( "extendRadius" );
		axis = serializedObject.FindProperty( "axis" );
		boundary = serializedObject.FindProperty( "boundary" );
		deadZone = serializedObject.FindProperty( "deadZone" );
		tapCountOption = serializedObject.FindProperty( "tapCountOption" );
		tapCountDuration = serializedObject.FindProperty( "tapCountDuration" );
		targetTapCount = serializedObject.FindProperty( "targetTapCount" );

		/* -----< VISUAL OPTIONS >----- */
		disableVisuals = serializedObject.FindProperty( "disableVisuals" );
		showHighlight = serializedObject.FindProperty( "showHighlight" );
		highlightColor = serializedObject.FindProperty( "highlightColor" );
		showTension = serializedObject.FindProperty( "showTension" );
		tensionColorNone = serializedObject.FindProperty( "tensionColorNone" );
		tensionColorFull = serializedObject.FindProperty( "tensionColorFull" );
		useAnimation = serializedObject.FindProperty( "useAnimation" );
		useFade = serializedObject.FindProperty( "useFade" );
		fadeUntouched = serializedObject.FindProperty( "fadeUntouched" );
		fadeTouched = serializedObject.FindProperty( "fadeTouched" );
		fadeInDuration = serializedObject.FindProperty( "fadeInDuration" );
		fadeOutDuration = serializedObject.FindProperty( "fadeOutDuration" );

		/* ------< SCRIPT REFERENCE >------ */
		joystickName = serializedObject.FindProperty( "joystickName" );

		exampleCodeOptions = new List<string>();

		for( int i = 0; i < exampleCodes.Length; i++ )
			exampleCodeOptions.Add( exampleCodes[ i ].optionName );

		SetDisableVisuals( targ );
		SetHighlight( targ );
		SetAnimation( targ );
		SetTensionAccent( targ );

		if( targ.useFade == true )
		{
			if( !targ.GetComponent<CanvasGroup>() )
				targ.gameObject.AddComponent<CanvasGroup>();

			targ.gameObject.GetComponent<CanvasGroup>().alpha = targ.fadeUntouched;
		}
		else
		{
			if( !targ.GetComponent<CanvasGroup>() )
				targ.gameObject.AddComponent<CanvasGroup>();

			targ.gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
		}
	}
	
	void SetDisableVisuals ( UltimateJoystick targ )
	{
		if( targ.disableVisuals == true )
		{
			if( targ.showHighlight == true )
				targ.showHighlight = false;
			if( targ.showTension == true )
				targ.showTension = false;

			if( targ.joystickBase != null && targ.joystickBase.GetComponent<Image>().enabled == true )
				targ.joystickBase.GetComponent<Image>().enabled = false;

			if( targ.joystick != null && targ.joystick.GetComponent<Image>().enabled == true )
				targ.joystick.GetComponent<Image>().enabled = false;
		}
		else
		{
			if( targ.joystickBase != null && targ.joystickBase.GetComponent<Image>().enabled == false )
				targ.joystickBase.GetComponent<Image>().enabled = true;
			if( targ.joystick != null && targ.joystick.GetComponent<Image>().enabled == false )
				targ.joystick.GetComponent<Image>().enabled = true;
		}
	}

	void SetHighlight ( UltimateJoystick targ )
	{
		if( targ.showHighlight == true )
		{
			if( targ.highlightBase != null && targ.highlightBase.gameObject.activeInHierarchy == false )
				targ.highlightBase.gameObject.SetActive( true );
			if( targ.highlightJoystick != null && targ.highlightJoystick.gameObject.activeInHierarchy == false )
				targ.highlightJoystick.gameObject.SetActive( true );

			targ.UpdateHighlightColor( targ.highlightColor );
		}
		else
		{
			if( targ.joystick == null )
				return;

			if( targ.highlightBase != null && targ.highlightBase != targ.joystickBase.GetComponent<Image>() && targ.highlightBase.gameObject.activeInHierarchy == true )
				targ.highlightBase.gameObject.SetActive( false );
			if( targ.highlightJoystick != null && targ.highlightJoystick != targ.joystick.GetComponent<Image>() && targ.highlightJoystick.gameObject.activeInHierarchy == true )
				targ.highlightJoystick.gameObject.SetActive( false );
		}
	}
	
	void SetTensionAccent ( UltimateJoystick targ )
	{
		if( targ.showTension == true )
		{	
			if( targ.tensionAccentUp != null && targ.tensionAccentUp.gameObject.activeInHierarchy == false )
				targ.tensionAccentUp.gameObject.SetActive( true );
			if( targ.tensionAccentDown != null && targ.tensionAccentDown.gameObject.activeInHierarchy == false )
				targ.tensionAccentDown.gameObject.SetActive( true );
			if( targ.tensionAccentLeft != null && targ.tensionAccentLeft.gameObject.activeInHierarchy == false )
				targ.tensionAccentLeft.gameObject.SetActive( true );
			if( targ.tensionAccentRight != null && targ.tensionAccentRight.gameObject.activeInHierarchy == false )
				targ.tensionAccentRight.gameObject.SetActive( true );
			
			TensionAccentReset( targ );
		}
		else
		{
			if( targ.tensionAccentUp != null && targ.tensionAccentUp.gameObject.activeInHierarchy == true )
				targ.tensionAccentUp.gameObject.SetActive( false );
			if( targ.tensionAccentDown != null && targ.tensionAccentDown.gameObject.activeInHierarchy == true )
				targ.tensionAccentDown.gameObject.SetActive( false );
			if( targ.tensionAccentLeft != null && targ.tensionAccentLeft.gameObject.activeInHierarchy == true )
				targ.tensionAccentLeft.gameObject.SetActive( false );
			if( targ.tensionAccentRight != null && targ.tensionAccentRight.gameObject.activeInHierarchy == true )
				targ.tensionAccentRight.gameObject.SetActive( false );
		}
	}

	void TensionAccentReset ( UltimateJoystick targ )
	{
		if( targ.tensionAccentUp != null )
			targ.tensionAccentUp.color = targ.tensionColorNone;

		if( targ.tensionAccentDown != null )
			targ.tensionAccentDown.color = targ.tensionColorNone;

		if( targ.tensionAccentLeft != null )
			targ.tensionAccentLeft.color = targ.tensionColorNone;

		if( targ.tensionAccentRight != null )
			targ.tensionAccentRight.color = targ.tensionColorNone;
	}
	
	void SetAnimation ( UltimateJoystick targ )
	{
		if( targ.useAnimation == true )
		{
			if( targ.joystickAnimator != null && targ.joystickAnimator.enabled == false )
				targ.joystickAnimator.enabled = true;
		}
		else
		{
			if( targ.joystickAnimator != null && targ.joystickAnimator.enabled == true )
				targ.joystickAnimator.enabled = false;
		}
	}
}

/* Written by Kaz Crowe */
/* UltimateJoystickCreator.cs */
public class UltimateJoystickCreator
{
	public static void CreateNewUltimateJoystick ( GameObject joystickPrefab )
	{
		GameObject prefab = ( GameObject )Object.Instantiate( joystickPrefab, Vector3.zero, Quaternion.identity );
		prefab.name = joystickPrefab.name;
		Selection.activeGameObject = prefab;
		RequestCanvas( prefab );
	}

	private static void CreateNewCanvas ( GameObject child )
	{
		GameObject root = new GameObject( "Ultimate UI Canvas" );
		root.layer = LayerMask.NameToLayer( "UI" );
		Canvas canvas = root.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		root.AddComponent<GraphicRaycaster>();
		Undo.RegisterCreatedObjectUndo( root, "Create " + root.name );

		child.transform.SetParent( root.transform, false );
		
		CreateEventSystem();
	}

	private static void CreateEventSystem ()
	{
		Object esys = Object.FindObjectOfType<EventSystem>();
		if( esys == null )
		{
			GameObject eventSystem = new GameObject( "EventSystem" );
			esys = eventSystem.AddComponent<EventSystem>();
			eventSystem.AddComponent<StandaloneInputModule>();
			#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
			eventSystem.AddComponent<TouchInputModule>();
			#endif

			Undo.RegisterCreatedObjectUndo( eventSystem, "Create " + eventSystem.name );
		}
	}

	/* PUBLIC STATIC FUNCTIONS */
	public static void RequestCanvas ( GameObject child )
	{
		Canvas[] allCanvas = Object.FindObjectsOfType( typeof( Canvas ) ) as Canvas[];

		for( int i = 0; i < allCanvas.Length; i++ )
		{
			if( allCanvas[ i ].renderMode == RenderMode.ScreenSpaceOverlay && allCanvas[ i ].enabled == true && ValidateCanvasScalerComponent( allCanvas[ i ] ) )
			{
				child.transform.SetParent( allCanvas[ i ].transform, false );
				CreateEventSystem();
				return;
			}
		}
		CreateNewCanvas( child );
	}
	
	static bool ValidateCanvasScalerComponent ( Canvas canvas )
	{
		if( !canvas.GetComponent<CanvasScaler>() )
			return true;
		else if( canvas.GetComponent<CanvasScaler>().uiScaleMode == CanvasScaler.ScaleMode.ConstantPixelSize )
			return true;

		return false;
	}
}
/* Written by Kaz Crowe */
/* UltimateJoystick.cs */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

// First off, the script is using [ExecuteInEditMode] to be able to show changes in real time. This will not affect anything within a build or play mode. This simply makes the script able to be run while in the Editor in Edit Mode.
[ExecuteInEditMode]
public class UltimateJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
	/* ----- > ASSIGNED VARIABLES < ----- */
	public RectTransform joystick, joystickSizeFolder, joystickBase;
	RectTransform baseTrans;
	Vector2 textureCenter = Vector2.zero, defaultPos = Vector2.zero;
	Vector3 joystickCenter = Vector3.zero;
	public Image highlightBase, highlightJoystick;
	public Image tensionAccentUp, tensionAccentDown;
	public Image tensionAccentLeft, tensionAccentRight;

	/* ----- > SIZE AND PLACEMENT < ----- */
	public enum ScalingAxis{ Width, Height }
	public ScalingAxis scalingAxis = ScalingAxis.Height;
	public enum Anchor{ Left, Right }
	public Anchor anchor = Anchor.Left;
	public enum JoystickTouchSize{ Default, Medium, Large, Custom }
	public JoystickTouchSize joystickTouchSize = JoystickTouchSize.Default;
	public float joystickSize = 1.75f, radiusModifier = 4.5f;
	float radius = 1.0f;
	public bool dynamicPositioning = false;
	public float customTouchSize_X = 50.0f, customTouchSize_Y = 75.0f;
	public float customTouchSizePos_X = 0.0f, customTouchSizePos_Y = 0.0f;
	public float customSpacing_X = 5.0f, customSpacing_Y = 20.0f;

	/* ----- > JOYSTICK FUNCTIONALITY < ----- */
	public float gravity = 60.0f;
	bool gravityActive = false;
	public bool extendRadius = false;
	public enum Axis
	{
		Both, X, Y
	}
	public Axis axis = Axis.Both;
	public enum Boundary
	{
		Circular, Square
	}
	public Boundary boundary = Boundary.Circular;
	public enum TapCountOption
	{
		NoCount, Accumulate, TouchRelease
	}
	public TapCountOption tapCountOption = TapCountOption.NoCount;
	public float tapCountDuration = 0.5f;
	public int targetTapCount = 2;
	float currentTapTime = 0.0f;
	int tapCount = 0;
	public float deadZone = 0.0f;

	/* ----- > VISUAL OPTIONS < ----- */
	public bool disableVisuals = false;
	public bool useFade = false;
	CanvasGroup joystickGroup;
	public float fadeUntouched = 1.0f, fadeTouched = 0.5f;
	public float fadeInDuration = 1.0f, fadeOutDuration = 1.0f;
	float fadeInSpeed = 1.0f, fadeOutSpeed = 1.0f;
	public bool useAnimation = false;
	public Animator joystickAnimator;
	int animationID = 0;
	public bool showHighlight = false;
	public Color highlightColor = new Color( 1, 1, 1, 1 );
	public bool showTension = false;
	public Color tensionColorNone = new Color( 1, 1, 1, 1 ), tensionColorFull = new Color( 1, 1, 1, 1 );

	/* ----- > SCRIPT REFERENCE < ----- */
	static Dictionary<string,UltimateJoystick> UltimateJoysticks = new Dictionary<string, UltimateJoystick>();
	public string joystickName;
	bool joystickState = false;
	bool tapCountAchieved = false;

	bool updateHighlightPosition = false;
	int _pointerId = -10;// Default value of -10

	
	void Awake ()
	{
		// If the game is not being run and the joystick name has been assigned, then register the joystick.
		if( Application.isPlaying == true && joystickName != string.Empty )
		{
			if( UltimateJoysticks.ContainsKey( joystickName ) )
				UltimateJoysticks.Remove( joystickName );
		
			UltimateJoysticks.Add( joystickName, GetComponent<UltimateJoystick>() );
		}
	}
	
	void Start ()
	{
		// If the game is not running then return.
		if( Application.isPlaying == false )
			return;

		// Update the size and placement of the joystick.
		UpdateSizeAndPlacement();

		// Check all options to see if the joystick highlight image should be moved with use input.
		CheckJoystickHighlightForUse();

		// Set the highlight is the user is wanting to show highlight.
		if( showHighlight == true )
			UpdateHighlightColor( highlightColor );

		// Reset the tension accents if the user is wanting to show tension.
		if( showTension == true )
			TensionAccentReset();

		// If the user is wanting to use fade...
		if( useFade == true )
		{
			// Configure the fade speeds.
			fadeInSpeed = 1.0f / fadeInDuration;
			fadeOutSpeed = 1.0f / fadeOutDuration;
		}

		// Get the hash ID of the targeted animation if the user is wanting to show animation.
		if( useAnimation == true )
		{
			// If the animator is null, then try to assign it.
			if( joystickAnimator == null )
				joystickAnimator = GetComponent<Animator>();

			// If it is still null, then set useAnimation to false to avoid errors.
			if( joystickAnimator == null )
			{
				Debug.LogError( "Ultimate Joystick - This object does not have an Animator component attached to it. Please make sure to attach an Animator to this object before using the Use Animation option.\n\nObject Name: " + gameObject.name + "\n" );
				useAnimation = false;
			}
			else
				animationID = Animator.StringToHash( "Touch" );
		}

		// If there is no Updater script attached, then attach an Updater script.
		if( !GetParentCanvas().GetComponent<UltimateJoystickScreenSizeUpdater>() )
			GetParentCanvas().gameObject.AddComponent( typeof( UltimateJoystickScreenSizeUpdater ) );
	}
	
	public void OnPointerDown ( PointerEventData touchInfo )
	{
		// If the joystick is already in use, then return.
		if( joystickState == true )
			return;
		
		// Set the joystick state since the joystick is being interacted with.
		joystickState = true;

		// Assign the pointerId so that the other functions can know if the pointer calling the function is the correct one.
		_pointerId = touchInfo.pointerId;

		// If the throwable option is selected and isThrowing, then stop the current movement.
		if( gravity > 0 && gravityActive )
			StopCoroutine( "GravityHandler" );

		// If dynamicPositioning or disableVisuals are enabled...
		if( dynamicPositioning == true || disableVisuals == true )
		{
			// Then move the joystickSizeFolder to the position of the touch.
			joystickSizeFolder.position = touchInfo.position - textureCenter;

			// Set the joystickCenter so that the position can be calculated correctly.
			joystickCenter = touchInfo.position;
		}

		// If the user wants animation to be shown, do that here.
		if( useAnimation == true )
			joystickAnimator.SetBool( animationID, true );

		// If the user wants the joystick to fade, do that here.
		if( useFade == true && joystickGroup != null )
			StartCoroutine( "FadeLogic" );

		// If the user is wanting to use any tap count...
		if( tapCountOption != TapCountOption.NoCount )
		{
			// If the user is accumulating taps...
			if( tapCountOption == TapCountOption.Accumulate )
			{
				// If the TapCountdown is not counting down...
				if( currentTapTime <= 0 )
				{
					// Set tapCount to 1 since this is the initial touch and start the TapCountdown.
					tapCount = 1;
					StartCoroutine( "TapCountdown" );
				}
				// Else the TapCountdown is currently counting down, so increase the current tapCount.
				else
					++tapCount;

				if( currentTapTime > 0 && tapCount >= targetTapCount )
				{
					// Set the current time to 0 to interrupt the coroutine.
					currentTapTime = 0;

					// Start the delay of the reference for one frame.
					StartCoroutine( "TapCountDelay" );
				}
			}
			// Else the user wants to touch and release, so start the TapCountdown timer.
			else
				StartCoroutine( "TapCountdown" );
		}

		// Call UpdateJoystick with the info from the current PointerEventData.
		UpdateJoystick( touchInfo );
	}
	
	public void OnDrag ( PointerEventData touchInfo )
	{
		// If the pointer event that is calling this function is not the same as the one that initiated the joystick, then return.
		if( touchInfo.pointerId != _pointerId )
			return;

		// Then call UpdateJoystick with the info from the current PointerEventData.
		UpdateJoystick( touchInfo );
	}
	
	public void OnPointerUp ( PointerEventData touchInfo )
	{
		// If the pointer event that is calling this function is not the same as the one that initiated the joystick, then return.
		if( touchInfo.pointerId != _pointerId )
			return;

		// Since the touch has lifted, set the state to false and reset the local pointerId.
		joystickState = false;
		_pointerId = -10;
		
		// If dynamicPositioning, disableVisuals, or draggable are enabled...
		if( dynamicPositioning == true || disableVisuals == true || extendRadius == true )
		{
			// The joystickSizeFolder needs to be reset back to the default position.
			joystickSizeFolder.position = defaultPos;

			// Reset the joystickCenter since the touch has been released.
			joystickCenter = joystickBase.position;
		}

		// If the user has the gravity set to something more than 0 but less than 60, begin GravityHandler().
		if( gravity > 0 && gravity < 60 )
			StartCoroutine( "GravityHandler" );
		else
		{
			// Reset the joystick's position back to center.
			joystick.position = joystickCenter;

			// If the user has showHighlight enabled, and the highlightJoystick variable is assigned, reset it too.
			if( updateHighlightPosition == true )
				highlightJoystick.transform.position = joystickCenter;
		}

		// If the user has showTension enabled, then reset the tension if throwable is disabled.
		if( showTension == true && ( gravity <= 0 || gravity >= 60 ) )
			TensionAccentReset();

		// If the user has useAnimation enabled, set that here.
		if( useAnimation == true )
			joystickAnimator.SetBool( animationID, false );
		
		// If the user is wanting to use the TouchAndRelease tap count...
		if( tapCountOption == TapCountOption.TouchRelease )
		{
			// If the tapTime is still above zero, then start the delay function.
			if( currentTapTime > 0 )
				StartCoroutine( "TapCountDelay" );

			// Reset the current tap time to zero.
			currentTapTime = 0;
		}

		UpdatePositionValues();
	}
	
	void UpdateJoystick ( PointerEventData touchInfo )
	{
		// Create a new Vector2 to equal the vector from the current touch to the center of joystick.
		Vector2 tempVector = touchInfo.position - ( Vector2 )joystickCenter;

		// If the user wants only one axis, then zero out the opposite value.
		if( axis == Axis.X )
			tempVector.y = 0;
		else if( axis == Axis.Y )
			tempVector.x = 0;

		// If the user wants a circular boundary for the joystick, then clamp the magnitude by the radius.
		if( boundary == Boundary.Circular )
			tempVector = Vector2.ClampMagnitude( tempVector, radius );
		// Else the user wants a square boundary, so clamp X and Y individually.
		else if( boundary == Boundary.Square )
		{
			tempVector.x = Mathf.Clamp( tempVector.x, -radius, radius );
			tempVector.y = Mathf.Clamp( tempVector.y, -radius, radius );
		}

		// Apply the tempVector to the joystick's position.
		joystick.transform.position = ( Vector2 )joystickCenter + tempVector;
		
		// If the user is showing highlight and the highlightJoystick is assigned, then move the highlight to match the joystick's position.
		if( updateHighlightPosition == true )
			highlightJoystick.transform.position = joystick.transform.position;

		// If the user has showTension enabled, then display the Tension.
		if( showTension == true )
			TensionAccentDisplay();

		// If the user wants to drag the joystick along with the touch...
		if( extendRadius == true )
		{
			// Store the position of the current touch.
			Vector3 currentTouchPosition = touchInfo.position;

			// If the user is using any axis option, then align the current touch position.
			if( axis != Axis.Both )
			{
				if( axis == Axis.X )
					currentTouchPosition.y = joystickCenter.y;
				else
					currentTouchPosition.x = joystickCenter.x;
			}
			// Then find the distance that the touch is from the center of the joystick.
			float touchDistance = Vector3.Distance( joystickCenter, currentTouchPosition );

			// If the touchDistance is greater than the set radius...
			if( touchDistance >= radius )
			{
				// Figure out the current position of the joystick.
				Vector2 joystickPosition = ( joystick.position - joystickCenter ) / radius;

				// Move the joystickSizeFolder in the direction that the joystick is, multiplied by the difference in distance of the max radius.
				joystickSizeFolder.position += new Vector3( joystickPosition.x, joystickPosition.y, 0 ) * ( touchDistance - radius );

				// Reconfigure the joystickCenter since the joystick has now moved it position.
				joystickCenter = joystickBase.position;
			}
		}

		UpdatePositionValues();
	}

	// This function will configure the position of an image based on the size and custom spacing selected.
	Vector2 ConfigureImagePosition ( Vector2 textureSize, Vector2 customSpacing )
	{
		// First, fix the customSpacing to be a value between 0.0f and 1.0f.
		Vector2 fixedCustomSpacing = customSpacing / 100;

		// Then configure position spacers according to the screen's dimensions, the fixed spacing and texture size.
		float positionSpacerX = Screen.width * fixedCustomSpacing.x - ( textureSize.x * fixedCustomSpacing.x );
		float positionSpacerY = Screen.height * fixedCustomSpacing.y - ( textureSize.y * fixedCustomSpacing.y );

		// Create a temporary Vector2 to modify and return.
		Vector2 tempVector;

		// If it's left, simply apply the positionxSpacerX, else calculate out from the right side and apply the positionSpaceX.
		tempVector.x = anchor == Anchor.Left ? positionSpacerX : ( Screen.width - textureSize.x ) - positionSpacerX;

		// Apply the positionSpacerY variable.
		tempVector.y = positionSpacerY;

		// Return the updated temporary Vector.
		return tempVector;
	}

	// This function is called only when showTension is true, and only when the joystick is moving.
	void TensionAccentDisplay ()
	{
		// Create a temporary Vector2 for the joystick current position.
		Vector2 tension = ( joystick.position - joystickCenter ) / radius;

		// If the joystick is to the right...
		if( tension.x > 0 )
		{
			// Then lerp the color according to tension's X position.
			if( tensionAccentRight != null )
				tensionAccentRight.color = Color.Lerp( tensionColorNone, tensionColorFull, tension.x );
			
			// If the opposite tension is not tensionColorNone, the make it so.
			if( tensionAccentLeft != null && tensionAccentLeft.color != tensionColorNone )
				tensionAccentLeft.color = tensionColorNone;
		}
		// Else the joystick is to the left...
		else
		{
			// Mathf.Abs gives a positive number to lerp with.
			tension.x = Mathf.Abs( tension.x );

			// Repeat above steps...
			if( tensionAccentLeft != null )
				tensionAccentLeft.color = Color.Lerp( tensionColorNone, tensionColorFull, tension.x );
			if( tensionAccentRight != null && tensionAccentRight.color != tensionColorNone )
				tensionAccentRight.color = tensionColorNone;
		}

		// If the joystick is up...
		if( tension.y > 0 )
		{
			// Then lerp the color according to tension's Y position.
			if( tensionAccentUp != null )
				tensionAccentUp.color = Color.Lerp( tensionColorNone, tensionColorFull, tension.y );

			// If the opposite tension is not tensionColorNone, the make it so.
			if( tensionAccentDown != null && tensionAccentDown.color != tensionColorNone )
				tensionAccentDown.color = tensionColorNone;
		}
		// Else the joystick is down...
		else
		{
			// Mathf.Abs gives a positive number to lerp with.
			tension.y = Mathf.Abs( tension.y );

			if( tensionAccentDown != null )
				tensionAccentDown.color = Color.Lerp( tensionColorNone, tensionColorFull, tension.y );

			// Repeat above steps...
			if( tensionAccentUp != null && tensionAccentUp.color != tensionColorNone )
				tensionAccentUp.color = tensionColorNone;
		}
	}

	// This function resets the tension image's colors back to default.
	void TensionAccentReset ()
	{
		if( tensionAccentUp != null )
			tensionAccentUp.color = tensionColorNone;

		if( tensionAccentDown != null )
			tensionAccentDown.color = tensionColorNone;

		if( tensionAccentLeft != null )
			tensionAccentLeft.color = tensionColorNone;

		if( tensionAccentRight != null )
			tensionAccentRight.color = tensionColorNone;
	}

	// This function is for returning the joystick back to center for a set amount of time.
	IEnumerator GravityHandler ()
	{
		gravityActive = true;
		float speed = 1.0f / ( GetDistance() / gravity );
		// Store the position of where the joystick is currently.
		Vector3 startJoyPos = joystick.position;
		for( float t = 0.0f; t < 1.0f && gravityActive; t += Time.deltaTime * speed )
		{
			// Lerp the joystick's position from where this coroutine started to the center.
			joystick.position = Vector3.Lerp( startJoyPos, joystickCenter, t );

			// If the user a direction display option enabled, then display the direction as the joystick moves.
			if( showTension )
				TensionAccentDisplay();

			UpdatePositionValues();

			yield return null;
		}

		// Finalize the joystick's position.
		if( gravityActive )
		{
			joystick.position = joystickCenter;

			// Here at the end, reset the direction display.
			if( showTension )
				TensionAccentReset();
			
			UpdatePositionValues();
		}

		gravityActive = false;
	}

	// This function is used only to find the canvas parent if its not the root object.
	Canvas GetParentCanvas ()
	{
		Transform parent = transform.parent;
		while( parent != null )
		{ 
			if( parent.transform.GetComponent<Canvas>() )
				return parent.transform.GetComponent<Canvas>();

			parent = parent.transform.parent;
		}
		return null;
	}

	CanvasGroup GetCanvasGroup ()
	{
		if( GetComponent<CanvasGroup>() )
			return GetComponent<CanvasGroup>();
		else
		{
			gameObject.AddComponent<CanvasGroup>();
			return GetComponent<CanvasGroup>();
		}
	}

	IEnumerator FadeLogic ()
	{
		// Store the current value for the alpha of the joystickGroup.
		float currentFade = joystickGroup.alpha;

		// If the fadeInSpeed is NaN, then just set the alpha to the desired fade.
		if( float.IsInfinity( fadeInSpeed ) )
			joystickGroup.alpha = fadeTouched;
		// Else run the loop to fade to the desired alpha over time.
		else
		{
			for( float fadeIn = 0.0f; fadeIn < 1.0f && joystickState == true; fadeIn += Time.deltaTime * fadeInSpeed )
			{
				joystickGroup.alpha = Mathf.Lerp( currentFade, fadeTouched, fadeIn );
				yield return null;
			}
			if( joystickState == true )
				joystickGroup.alpha = fadeTouched;
		}

		// while loop for while joystickState is true
		while( joystickState == true )
			yield return null;

		// Set the current fade value.
		currentFade = joystickGroup.alpha;

		// If the fadeOutSpeed value is NaN, then apply the desired alpha.
		if( float.IsInfinity( fadeOutSpeed ) )
			joystickGroup.alpha = fadeUntouched;
		// Else run the loop for fading out.
		else
		{
			for( float fadeOut = 0.0f; fadeOut < 1.0f && joystickState == false; fadeOut += Time.deltaTime * fadeOutSpeed )
			{
				joystickGroup.alpha = Mathf.Lerp( currentFade, fadeUntouched, fadeOut );
				yield return null;
			}
			if( joystickState == false )
				joystickGroup.alpha = fadeUntouched;
		}
	}

	/// <summary>
	/// This function counts down the tap count duration. The current tap time that is being modified is check within the input functions.
	/// </summary>
	IEnumerator TapCountdown ()
	{
		// Set the current tap time to the max.
		currentTapTime = tapCountDuration;
		while( currentTapTime > 0 )
		{
			// Reduce the current time.
			currentTapTime -= Time.deltaTime;
			yield return null;
		}
	}

	/// <summary>
	/// This function delays for one frame so that it can be correctly referenced as soon as it is achieved.
	/// </summary>
	IEnumerator TapCountDelay ()
	{
		tapCountAchieved = true;
		yield return new WaitForEndOfFrame();
		tapCountAchieved = false;
	}

	/// <summary>
	/// This function check each option and component in relation to the joystick highlight. It updates the updateHighlightPosition bool according to set options.
	/// </summary>
	void CheckJoystickHighlightForUse ()
	{
		if( showHighlight == false )
			updateHighlightPosition = false;
		else if( highlightJoystick == null )
			updateHighlightPosition = false;
		else if( joystick.GetComponent<Image>() == highlightJoystick )
			updateHighlightPosition = false;
		else
			updateHighlightPosition = true;
	}

	void UpdatePositionValues ()
	{
		Vector2 rawJoystickPosition = ( joystick.position - joystickCenter ) / radius;

		if( GetDistance() <= deadZone )
		{
			rawJoystickPosition.x = 0.0f;
			rawJoystickPosition.y = 0.0f;
		}

		HorizontalAxis = rawJoystickPosition.x;
		VerticalAxis = rawJoystickPosition.y;
	}

	/// <summary>
	/// Returns with a confirmation about the existence of the targeted Ultimate Joystick.
	/// </summary>
	static bool JoystickConfirmed ( string joystickName )
	{
		if( !UltimateJoysticks.ContainsKey( joystickName ) )
		{
			Debug.LogWarning( "Ultimate Joystick - No Ultimate Joystick has been registered with the name: " + joystickName + "." );
			return false;
		}
		return true;
	}

	void ResetJoystick ()
	{
		gravityActive = false;
		StopCoroutine( "GravityHandler" );

		// Since the touch has lifted, set the state to false and reset the local pointerId.
		joystickState = false;
		_pointerId = -10;
		
		// If dynamicPositioning, disableVisuals, or draggable are enabled...
		if( dynamicPositioning == true || disableVisuals == true || extendRadius == true )
		{
			// The joystickSizeFolder needs to be reset back to the default position.
			joystickSizeFolder.position = defaultPos;

			// Reset the joystickCenter since the touch has been released.
			joystickCenter = joystickBase.position;
		}
		// Reset the joystick's position back to center.
		joystick.position = joystickCenter;
		
		// If the user has showHighlight enabled, and the highlightJoystick variable is assigned, reset it too.
		if( updateHighlightPosition == true )
			highlightJoystick.transform.position = joystickCenter;

		// If the user has showTension enabled, then reset the tension if throwable is disabled.
		if( showTension == true )
			TensionAccentReset();

		// If the user has useAnimation enabled, set that here.
		if( useAnimation == true )
			joystickAnimator.SetBool( animationID, false );
	}

	/// <summary>
	/// Updates the Size and Placement of the Ultimate Joystick according to the user's options.
	/// </summary>
	void UpdateSizeAndPlacement ()
	{
		// If any of the needed components are left unassigned, then inform the user and return.
		if( joystickSizeFolder == null || joystickBase == null || joystick == null )
		{
			if( Application.isPlaying )
				Debug.LogError( "Ultimate Joystick - There are some needed components that are not currently assigned. Please check the Assigned Variables section and be sure to assign all of the components." );
			return;
		}

		// Set the current reference size for scaling.
		float referenceSize = scalingAxis == ScalingAxis.Height ? Screen.height : Screen.width;
		
		// Configure the target size for the joystick graphic.
		float textureSize = referenceSize * ( joystickSize / 10 );
		
		// If baseTrans is null, store this object's RectTrans so that it can be positioned.
		if( baseTrans == null )
			baseTrans = GetComponent<RectTransform>();
		
		// Get a position for the joystick based on the position variables.
		Vector2 imagePosition = ConfigureImagePosition( new Vector2( textureSize, textureSize ), new Vector2( customSpacing_X, customSpacing_Y ) );
		
		// If the user wants a custom touch size...
		if( joystickTouchSize == JoystickTouchSize.Custom )
		{
			// Fix the custom size variables.
			float fixedFBPX = customTouchSize_X / 100;
			float fixedFBPY = customTouchSize_Y / 100;
			
			// Depending on the joystickTouchSize options, configure the size.
			baseTrans.sizeDelta = new Vector2( Screen.width * fixedFBPX, Screen.height * fixedFBPY );
			
			// Send the size and custom positioning to the ConfigureImagePosition function to get the exact position.
			Vector2 imagePos = ConfigureImagePosition( baseTrans.sizeDelta, new Vector2( customTouchSizePos_X, customTouchSizePos_Y ) );

			// Apply the new position.
			baseTrans.position = imagePos;
		}
		else
		{
			// Temporary float to store a modifier for the touch area size.
			float fixedTouchSize = joystickTouchSize == JoystickTouchSize.Large ? 2.0f : joystickTouchSize == JoystickTouchSize.Medium ? 1.51f : 1.01f;
			
			// Temporary Vector2 to store the default size of the joystick.
			Vector2 tempVector = new Vector2( textureSize, textureSize );
			
			// Apply the joystick size multiplied by the fixedTouchSize.
			baseTrans.sizeDelta = tempVector * fixedTouchSize;
			
			// Apply the imagePosition modified with the difference of the sizeDelta divided by 2, multiplied by the scale of the parent canvas.
			baseTrans.position = imagePosition - ( ( baseTrans.sizeDelta - tempVector ) / 2 );
		}

		// If the options dictate that the default position needs to be stored...
		if( dynamicPositioning == true || disableVisuals == true || extendRadius == true )
		{
			// Set the texture center so that the joystick can move to the touch position correctly.
			textureCenter = new Vector2( textureSize / 2, textureSize / 2 );
			
			// Also need to store the default position so that it can return after the touch has been lifted.
			defaultPos = imagePosition;
		}
		
		// Apply the size and position to the joystickSizeFolder.
		joystickSizeFolder.sizeDelta = new Vector2( textureSize, textureSize );
		joystickSizeFolder.position = imagePosition;
		
		// Configure the size of the Ultimate Joystick's radius.
		radius = joystickSizeFolder.sizeDelta.x * ( radiusModifier / 10 );
		
		// Store the joystick's center so that JoystickPosition can be configured correctly.
		joystickCenter = joystickSizeFolder.position + new Vector3( joystickSizeFolder.sizeDelta.x / 2, joystickSizeFolder.sizeDelta.y / 2 );

		// If the user wants to fade, and the joystickGroup is unassigned, find the CanvasGroup.
		if( useFade == true && joystickGroup == null )
			joystickGroup = GetCanvasGroup();
	}

	#if UNITY_EDITOR
	void Update ()
	{
		// Keep the joystick updated while the game is not being played.
		if( Application.isPlaying == false )//&& UnityEditor.Selection.activeGameObject != gameObject )
			UpdateSizeAndPlacement();
	}
	#endif
	
	/* --------------------------------------------- *** PUBLIC FUNCTIONS FOR THE USER *** --------------------------------------------- */
	/// <summary>
	/// Resets the joystick and updates the size and placement of the Ultimate Joystick. Useful for screen rotations, changing of screen size, or changing of size and placement options.
	/// </summary>
	public void UpdatePositioning ()
	{
		if( Application.isPlaying )
			ResetJoystick();

		UpdateSizeAndPlacement();
	}
	
	/// <summary>
	/// Returns a float value between -1 and 1 representing the horizontal value of the Ultimate Joystick.
	/// </summary>
	public float GetHorizontalAxis ()
	{
		return HorizontalAxis;
	}

	/// <summary>
	/// Returns a float value between -1 and 1 representing the vertical value of the Ultimate Joystick.
	/// </summary>
	public float GetVerticalAxis ()
	{
		return VerticalAxis;
	}

	/// <summary>
	/// Returns a value of -1, 0 or 1 representing the raw horizontal value of the Ultimate Joystick.
	/// </summary>
	public float GetHorizontalAxisRaw ()
	{
		float temp = HorizontalAxis;

		if( Mathf.Abs( temp ) <= deadZone )
			temp = 0.0f;
		else
			temp = temp < 0.0f ? -1.0f : 1.0f;

		return temp;
	}

	/// <summary>
	/// Returns a value of -1, 0 or 1 representing the raw vertical value of the Ultimate Joystick.
	/// </summary>
	public float GetVerticalAxisRaw ()
	{
		float temp = VerticalAxis;
		if( Mathf.Abs( temp ) <= deadZone )
			temp = 0.0f;
		else
			temp = temp < 0.0f ? -1.0f : 1.0f;

		return temp;
	}

	/// <summary>
	/// Returns the current value of the horizontal axis.
	/// </summary>
	public float HorizontalAxis
	{
		get;
		private set;
	}

	/// <summary>
	/// Returns the current value of the vertical axis.
	/// </summary>
	public float VerticalAxis
	{
		get;
		private set;
	}

	/// <summary>
	/// Returns a float value between 0 and 1 representing the distance of the joystick from the base.
	/// </summary>
	public float GetDistance ()
	{
		return Vector3.Distance( joystick.position, joystickCenter ) / radius;
	}

	/// <summary>
	/// Updates the color of the highlights attached to the Ultimate Joystick with the targeted color.
	/// </summary>
	/// <param name="targetColor">New highlight color.</param>
	public void UpdateHighlightColor ( Color targetColor )
	{
		if( showHighlight == false )
			return;

		highlightColor = targetColor;
		
		// Check if each variable is assigned so there is not a null reference exception when applying color.
		if( highlightBase != null )
			highlightBase.color = highlightColor;
		if( highlightJoystick != null )
			highlightJoystick.color = highlightColor;
	}

	/// <summary>
	/// Updates the colors of the tension accents attached to the Ultimate Joystick with the targeted colors.
	/// </summary>
	/// <param name="targetTensionNone">New idle tension color.</param>
	/// <param name="targetTensionFull">New full tension color.</param>
	public void UpdateTensionColors ( Color targetTensionNone, Color targetTensionFull )
	{
		if( showTension == false )
			return;

		tensionColorNone = targetTensionNone;
		tensionColorFull = targetTensionFull;
	}

	/// <summary>
	/// Returns the current state of the Ultimate Joystick. This function will return true when the joystick is being interacted with, and false when not.
	/// </summary>
	public bool GetJoystickState ()
	{
		return joystickState;
	}

	/// <summary>
	/// Returns the tap count to the Ultimate Joystick.
	/// </summary>
	public bool GetTapCount ()
	{
		return tapCountAchieved;
	}

	/// <summary>
	/// Disables the Ultimate Joystick.
	/// </summary>
	public void DisableJoystick ()
	{
		// Set the states to false.
		joystickState = false;
		_pointerId = -10;
		
		// If the joystick center has been changed, then reset it.
		if( dynamicPositioning == true || disableVisuals == true || extendRadius == true )
		{
			joystickSizeFolder.position = defaultPos;
			joystickCenter = joystickBase.position;
		}
		
		// Reset the position of the joystick.
		joystick.position = joystickCenter;
		
		// If the highlight image needs to be moved, then reset it's position to center.
		if( updateHighlightPosition == true )
			highlightJoystick.transform.position = joystickCenter;
		
		// If the user is displaying tension accents, then reset them here.
		if( showTension == true )
			TensionAccentReset();
		
		// If the user is using animations, the reset the animator.
		if( useAnimation == true )
			joystickAnimator.SetBool( animationID, false );

		// If the user is displaying a fade, then reset to the untouched state.
		if( useFade )
			joystickGroup.alpha = fadeUntouched;

		// Reset axes
		HorizontalAxis = 0;
		VerticalAxis = 0;

		// Disable the gameObject.
		gameObject.SetActive( false );
	}

	/// <summary>
	/// Enables the Ultimate Joystick.
	/// </summary>
	public void EnableJoystick ()
	{
		// Reset the joystick's position again.
		joystick.position = joystickCenter;

		// Enable the gameObject.
		gameObject.SetActive( true );
	}
	/* ------------------------------------------- *** END PUBLIC FUNCTIONS FOR THE USER *** ------------------------------------------- */
	
	/* --------------------------------------------- *** STATIC FUNCTIONS FOR THE USER *** --------------------------------------------- */
	/// <summary>
	/// Returns the Ultimate Joystick of the targeted name if it exists within the scene.
	/// </summary>
	/// <param name="joystickName">The Joystick Name of the desired Ultimate Joystick.</param>
	public static UltimateJoystick GetUltimateJoystick ( string joystickName )
	{
		if( !JoystickConfirmed( joystickName ) )
			return null;

		return UltimateJoysticks[ joystickName ];
	}

	/// <summary>
	/// Returns a float value between -1 and 1 representing the horizontal value of the Ultimate Joystick.
	/// </summary>
	/// <param name="joystickName">The name of the desired Ultimate Joystick.</param>
	public static float GetHorizontalAxis ( string joystickName )
	{
		if( !JoystickConfirmed( joystickName ) )
			return 0.0f;

		return UltimateJoysticks[ joystickName ].GetHorizontalAxis();
	}

	/// <summary>
	/// Returns a float value between -1 and 1 representing the vertical value of the Ultimate Joystick.
	/// </summary>
	/// <param name="joystickName">The name of the desired Ultimate Joystick.</param>
	public static float GetVerticalAxis ( string joystickName )
	{
		if( !JoystickConfirmed( joystickName ) )
			return 0.0f;

		return UltimateJoysticks[ joystickName ].GetVerticalAxis();
	}

	/// <summary>
	/// Returns a value of -1, 0 or 1 representing the raw horizontal value of the Ultimate Joystick.
	/// </summary>
	/// <param name="joystickName">The name of the desired Ultimate Joystick.</param>
	public static float GetHorizontalAxisRaw ( string joystickName )
	{
		if( !JoystickConfirmed( joystickName ) )
			return 0.0f;

		return UltimateJoysticks[ joystickName ].GetHorizontalAxisRaw();
	}

	/// <summary>
	/// Returns a value of -1, 0 or 1 representing the raw vertical value of the Ultimate Joystick.
	/// </summary>
	/// <param name="joystickName">The name of the desired Ultimate Joystick.</param>
	public static float GetVerticalAxisRaw ( string joystickName )
	{
		if( !JoystickConfirmed( joystickName ) )
			return 0.0f;

		return UltimateJoysticks[ joystickName ].GetVerticalAxisRaw();
	}

	/// <summary>
	/// Returns a float value between 0 and 1 representing the distance of the joystick from the base.
	/// </summary>
	/// <param name="joystickName">The name of the desired Ultimate Joystick.</param>
	public static float GetDistance ( string joystickName )
	{
		if( !JoystickConfirmed( joystickName ) )
			return 0.0f;

		return UltimateJoysticks[ joystickName ].GetDistance();
	}

	/// <summary>
	/// Returns the current interaction state of the Ultimate Joystick.
	/// </summary>
	/// <param name="joystickName">The name of the desired Ultimate Joystick.</param>
	public static bool GetJoystickState ( string joystickName )
	{
		if( !JoystickConfirmed( joystickName ) )
			return false;

		return UltimateJoysticks[ joystickName ].joystickState;
	}

	/// <summary>
	/// Returns the current state of the tap count according to the options set.
	/// </summary>
	/// <param name="joystickName">The name of the desired Ultimate Joystick.</param>
	public static bool GetTapCount ( string joystickName )
	{
		if( !JoystickConfirmed( joystickName ) )
			return false;

		return UltimateJoysticks[ joystickName ].GetTapCount();
	}

	/// <summary>
	/// Disables the targeted Ultimate Joystick.
	/// </summary>
	/// <param name="joystickName">The name of the desired Ultimate Joystick.</param>
	public static void DisableJoystick ( string joystickName )
	{
		if( !JoystickConfirmed( joystickName ) )
			return;

		UltimateJoysticks[ joystickName ].DisableJoystick();
	}

	/// <summary>
	/// Enables the targeted Ultimate Joystick.
	/// </summary>
	/// <param name="joystickName">The name of the desired Ultimate Joystick.</param>
	public static void EnableJoystick ( string joystickName )
	{
		if( !JoystickConfirmed( joystickName ) )
			return;

		UltimateJoysticks[ joystickName ].EnableJoystick();
	}
	/* ------------------------------------------- *** END STATIC FUNCTIONS FOR THE USER *** ------------------------------------------- */
}
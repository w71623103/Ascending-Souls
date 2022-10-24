//Description : footstepSystemEditor. Work in association with footstepSystem. Allow to manage player foostep sound depending the surface
#if (UNITY_EDITOR)
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;

[CustomEditor(typeof(characterMovement))]
public class characterMovementEditor : Editor {
	SerializedProperty	SeeInspector;											// use to draw default Inspector

	SerializedProperty	rbBodyCharacter;
	SerializedProperty 	objCamera;
	SerializedProperty 	addForceObj;
	SerializedProperty	minimum;
	SerializedProperty 	maximum;
	SerializedProperty 	characterSpeed;
	SerializedProperty 	sensibilityMouse;
	SerializedProperty 	sensibilityJoystick;
	SerializedProperty	animationCurveJoystick;
	SerializedProperty	mobileToystickController;
	SerializedProperty	sensibilityMobile;
	SerializedProperty	animationCurveMobile;

	SerializedProperty 			forwardKeyboard;
	SerializedProperty 			backwardKeyboard;
	SerializedProperty 			leftKeyboard;
	SerializedProperty 			rightKeyboard;

    SerializedProperty b_MobileMovement_Stick;
    SerializedProperty b_MobileCamRotation_Stick;

    SerializedProperty mobileSpeedRotation;
    SerializedProperty allowCrouch;
    SerializedProperty JoystickCrouch;
    SerializedProperty KeyboardCrouch;

   

	SerializedProperty 			VerticalAxisBody;
	SerializedProperty 			HorizontalAxisBody;
	SerializedProperty 			JoystickVerticalAxisCam;
	SerializedProperty 			JoystickHorizontalAxisCam;

	SerializedProperty 			mouseInvertYAxisCam;
	SerializedProperty 			joystickInvertYAxisCam;

    //SerializedProperty b_AllowRun;
    //SerializedProperty speedMultiplier;
    SerializedProperty hitDistanceMin;
    SerializedProperty hitDistanceMax;
    SerializedProperty MaxAngle;
    SerializedProperty moreInfoMaxAngle;






	public List<string> s_inputListJoystickAxis = new List<string> ();
	public List<string> s_inputListJoystickButton = new List<string> ();
	public List<string> s_inputListKeyboardAxis = new List<string> ();
	public List<string> s_inputListKeyboardButton = new List<string> ();





	public List<string>  s_inputListJoystickBool = new List<string> ();
	public List<string> s_inputListKeyboardBool= new List<string> ();



	public GameObject objCanvasInput;


	private Texture2D MakeTex(int width, int height, Color col) {						// use to change the GUIStyle
		Color[] pix = new Color[width * height];
		for (int i = 0; i < pix.Length; ++i) {
			pix[i] = col;
		}
		Texture2D result = new Texture2D(width, height);
		result.SetPixels(pix);
		result.Apply();
		return result;
	}

	private Texture2D 		Tex_01;														// 
	private Texture2D 		Tex_02;
	private Texture2D 		Tex_03;
	private Texture2D 		Tex_04;
	private Texture2D 		Tex_05;

	public string selectedTag = "";

	public string newTagName = "";

	void OnEnable () {
		// Setup the SerializedProperties.
		SeeInspector 				= serializedObject.FindProperty ("SeeInspector");

		rbBodyCharacter				= serializedObject.FindProperty ("rbBodyCharacter");
		objCamera					= serializedObject.FindProperty ("objCamera");
		addForceObj					= serializedObject.FindProperty ("addForceObj");
		minimum						= serializedObject.FindProperty ("minimum");
		maximum						= serializedObject.FindProperty ("maximum");
		characterSpeed				= serializedObject.FindProperty ("characterSpeed");
		sensibilityMouse			= serializedObject.FindProperty ("sensibilityMouse");
		sensibilityJoystick			= serializedObject.FindProperty ("sensibilityJoystick");
		animationCurveJoystick		= serializedObject.FindProperty ("animationCurveJoystick");
		mobileToystickController	= serializedObject.FindProperty ("mobileToystickController");
		sensibilityMobile			= serializedObject.FindProperty ("sensibilityMobile");
		animationCurveMobile		= serializedObject.FindProperty ("animationCurveMobile");

		forwardKeyboard				= serializedObject.FindProperty ("forwardKeyboard");
		backwardKeyboard			= serializedObject.FindProperty ("backwardKeyboard");
		leftKeyboard				= serializedObject.FindProperty ("leftKeyboard");
		rightKeyboard				= serializedObject.FindProperty ("rightKeyboard");

        b_MobileMovement_Stick = serializedObject.FindProperty("b_MobileMovement_Stick");
        b_MobileCamRotation_Stick = serializedObject.FindProperty("b_MobileCamRotation_Stick");

        mobileSpeedRotation = serializedObject.FindProperty("mobileSpeedRotation");
        //LeftStickSensibility = serializedObject.FindProperty("LeftStickSensibility");


        allowCrouch = serializedObject.FindProperty("allowCrouch");
        JoystickCrouch = serializedObject.FindProperty("JoystickCrouch");
        KeyboardCrouch = serializedObject.FindProperty("KeyboardCrouch");
   

		VerticalAxisBody			= serializedObject.FindProperty ("VerticalAxisBody");
		HorizontalAxisBody			= serializedObject.FindProperty ("HorizontalAxisBody");
		JoystickVerticalAxisCam		= serializedObject.FindProperty ("JoystickVerticalAxisCam");
		JoystickHorizontalAxisCam	= serializedObject.FindProperty ("JoystickHorizontalAxisCam");

		mouseInvertYAxisCam			= serializedObject.FindProperty ("mouseInvertYAxisCam");
		joystickInvertYAxisCam		= serializedObject.FindProperty ("joystickInvertYAxisCam");

        //b_AllowRun = serializedObject.FindProperty("b_AllowRun");
        //speedMultiplier = serializedObject.FindProperty("speedMultiplier");

        hitDistanceMin = serializedObject.FindProperty("hitDistanceMin");
        hitDistanceMax = serializedObject.FindProperty("hitDistanceMax");
        MaxAngle = serializedObject.FindProperty("MaxAngle");
        moreInfoMaxAngle = serializedObject.FindProperty("moreInfoMaxAngle");

		Tex_01 = MakeTex(2, 2, new Color(1,.8f,0.2F,.4f)); 
		Tex_02 = MakeTex(2, 2, new Color(1,.8f,0.2F,.4f)); 
		Tex_03 = MakeTex(2, 2, new Color(.3F,.9f,1,.5f));
		Tex_04 = MakeTex(2, 2, new Color(1,.3f,1,.3f)); 
		Tex_05 = MakeTex(2, 2, new Color(1,.5f,0.3F,.4f)); 

		GameObject tmp = GameObject.Find ("InputsManager");
		if(tmp){
			objCanvasInput = tmp;
			/*for(var i = 0;i< tmp.GetComponent<MM_MenuInputs>().remapButtons[0].buttonsList.Count;i++){
				s_inputListJoystickAxis.Add (tmp.GetComponent<MM_MenuInputs> ().remapButtons [0].buttonsList [i].name);
			}
			for(var i = 0;i< tmp.GetComponent<MM_MenuInputs>().remapButtons[1].buttonsList.Count;i++){
				s_inputListJoystickButton.Add (tmp.GetComponent<MM_MenuInputs> ().remapButtons [1].buttonsList [i].name);
			}



			for(var i = 0;i< tmp.GetComponent<MM_MenuInputs>().remapButtons[2].buttonsList.Count;i++){
				s_inputListKeyboardAxis.Add (tmp.GetComponent<MM_MenuInputs> ().remapButtons [2].buttonsList [i].name);
			}
			for(var i = 0;i< tmp.GetComponent<MM_MenuInputs>().remapButtons[3].buttonsList.Count;i++){
				s_inputListKeyboardButton.Add (tmp.GetComponent<MM_MenuInputs> ().remapButtons [3].buttonsList [i].name);
			}

			for(var i = 0;i< tmp.GetComponent<MM_MenuInputs>().boolValues[0].buttonsList.Count;i++){
				s_inputListJoystickBool.Add (tmp.GetComponent<MM_MenuInputs> ().boolValues [0].buttonsList [i].name);
			}
			for(var i = 0;i< tmp.GetComponent<MM_MenuInputs>().boolValues[1].buttonsList.Count;i++){
				s_inputListKeyboardBool.Add (tmp.GetComponent<MM_MenuInputs> ().boolValues [1].buttonsList [i].name);
			}*/






		}
	}


    public override void OnInspectorGUI()
    {
        if (SeeInspector.boolValue)                         // If true Default Inspector is drawn on screen
            DrawDefaultInspector();

        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("See Inspector :", GUILayout.Width(85));
        EditorGUILayout.PropertyField(SeeInspector, new GUIContent(""), GUILayout.Width(30));
        EditorGUILayout.EndHorizontal();

        GUIStyle style_Yellow_01 = new GUIStyle(GUI.skin.box); style_Yellow_01.normal.background = Tex_01;
        GUIStyle style_Blue = new GUIStyle(GUI.skin.box); style_Blue.normal.background = Tex_03;
        GUIStyle style_Purple = new GUIStyle(GUI.skin.box); style_Purple.normal.background = Tex_04;
        GUIStyle style_Orange = new GUIStyle(GUI.skin.box); style_Orange.normal.background = Tex_05;
        GUIStyle style_Yellow_Strong = new GUIStyle(GUI.skin.box); style_Yellow_Strong.normal.background = Tex_02;


        //GUILayout.Label("");
        characterMovement myScript = (characterMovement)target;




		serializedObject.ApplyModifiedProperties ();
	}



	void OnSceneGUI( )
	{
	}
}
#endif
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

[CustomEditor(typeof(MiniTerrainDecorator))]
public class MiniTerrainDecoratorEditor : Editor {
	private ReorderableList layerlist;

	int layerCount;
	string[] choices;
	string[] textureLayers;
	int treePrototypeCount;
	private Dictionary<string, ReorderableList> innerListDict = new Dictionary<string, ReorderableList>();

	private void OnListAdd(ReorderableList list) {

		SerializedProperty property = layerlist.serializedProperty;

		property.FindPropertyRelative("intensity").floatValue = 1f;
		property.FindPropertyRelative("contrast").floatValue = 0f;
		property.FindPropertyRelative("active").boolValue = true;
	}
	
	private void DrawLayer(int index, Rect rect, float height)
	
	{
		var element = layerlist.serializedProperty.GetArrayElementAtIndex(index);
		rect.y += 2;

		float x = rect.x;
	
		int layerIndex = element.FindPropertyRelative("layerIndex").intValue;
	
		if (layerIndex<0 || layerIndex>=layerCount+treePrototypeCount)
			GUI.color =  new Color(1f,0.7f,0.7f,1f);  
	
		else if ( element.FindPropertyRelative("active").boolValue==false)
			GUI.color =  new Color(0.7f,0.7f,0.7f,1f);  
	
		else if ( layerIndex>=layerCount)
			GUI.color = new Color(0.8f,1f,0.8f,1f);  
		else
			GUI.color = Color.white;  
		
		EditorGUI.PropertyField(
			new Rect(x, rect.y, 20, height),
			element.FindPropertyRelative("active"), GUIContent.none);
		x+=25;
	
		EditorGUI.PropertyField(
			new Rect(x, rect.y,100, height),
			element.FindPropertyRelative("name"), GUIContent.none);
		x+=105;
	
		if ( choices!=null)
		if ( layerIndex<choices.Length)
		if ( choices.Length>0) {
			element.FindPropertyRelative("layerIndex").intValue = EditorGUI.Popup(
			new Rect(x, rect.y, 150,height),
			element.FindPropertyRelative("layerIndex").intValue,
			choices);
			x+=155;
		}
	
		if ( layerIndex>= layerCount ) {

			EditorGUI.LabelField(
				new Rect(x, rect.y, 70, height),
				"Max Count");
			x+=75;

				
			EditorGUI.PropertyField(
				new Rect(x, rect.y, 60, height),
				element.FindPropertyRelative("maximumTreeCount"), GUIContent.none);
			x+=65;
			
			EditorGUI.LabelField(
				new Rect(x, rect.y, 60, height),
				"probability");
			x+=65;

				
			EditorGUI.PropertyField(
				new Rect(x, rect.y, 40, height),
				element.FindPropertyRelative("probability"), GUIContent.none);
			x+=45;
				
			x= 40;
			
			int yinc=4;
			
			EditorGUI.LabelField(
				new Rect(x, rect.y+height+yinc, 35, height),
				"scale");
			x+=40;
				
			EditorGUI.PropertyField(
				new Rect(x, rect.y+height+yinc, 30, height),
				element.FindPropertyRelative("width"), GUIContent.none);
			x+=35;
			
			EditorGUI.PropertyField(
				new Rect(x, rect.y+height+yinc, 30, height),
				element.FindPropertyRelative("height"), GUIContent.none);
			x+=35;
				
			EditorGUI.LabelField(
				new Rect(x, rect.y+height+yinc, 120,height),
				"randomize position");
			x+=120;
				
			EditorGUI.PropertyField(
				new Rect(x, rect.y+height+yinc, 30, height),
				element.FindPropertyRelative("randomPosition"), GUIContent.none);

			x+=35;

			EditorGUI.LabelField(
				new Rect(x, rect.y+height+yinc, 30,height),
				"rot");
			x+=35;
				

			EditorGUI.PropertyField(
				new Rect(x, rect.y+height+2, 30, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative("randomRotation"), GUIContent.none);
			x+=35;

			EditorGUI.LabelField(
				new Rect(x, rect.y+height+2, 30,height),
				"size");
			x+=35;
				

			EditorGUI.PropertyField(
				new Rect(x, rect.y+height+2, 30, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative("randomSize"), GUIContent.none);
			x+=30;
				
			EditorGUI.LabelField(
				new Rect(x, rect.y+height+2, 30,height),
				"offset");
			x+=35;
				

			EditorGUI.PropertyField(
				new Rect(x, rect.y+height+2, 30, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative("offset"), GUIContent.none);
			x+=30;

			EditorGUI.LabelField(
				new Rect(x, rect.y + height + 2, 30, height),
				"health");
			x += 35;


			EditorGUI.PropertyField(
				new Rect(x, rect.y + height + 2, 30, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative("randomHealth"), GUIContent.none);
			x += 30;
				


		}
	
		x+=20;
		
		if ( ((MiniTerrainDecorator)target).debugMode ) {
				
			EditorGUI.LabelField(
				new Rect(x, rect.y,60, height),
				"dlw "+(element.FindPropertyRelative("debugLayerWeight").floatValue).ToString()) ;
			x+=65;
			
			EditorGUI.LabelField(
				new Rect(x, rect.y,60, height),
				"rw "+(element.FindPropertyRelative("debugResultWeight").floatValue).ToString()) ;
			x+=65;
			
			EditorGUI.LabelField(
				new Rect(x, rect.y,60, height),
				"cw "+(element.FindPropertyRelative("debugCorrectedResultWeight").floatValue).ToString()) ;
			x+=65;
		
			
		}

	}
	
	private void DrawRules(SerializedProperty innerList, int index, Rect rect, float height)
	{
		
		var element = innerList.GetArrayElementAtIndex(index);
		rect.y += 2;
	
		float x = rect.x;
		
		EditorGUI.PropertyField(
			new Rect(x, rect.y, 20, height),
			element.FindPropertyRelative("active"), GUIContent.none);
	
		x+=20;


			
		SerializedProperty sFilter = 	element.FindPropertyRelative("filter");
		SerializedProperty sBlend = 	element.FindPropertyRelative("blend");

		EditorGUI.PropertyField(
			new Rect(x, rect.y, 50,height),
			element.FindPropertyRelative("blend"), GUIContent.none);
			
		x+=55;
			
		EditorGUI.PropertyField(
			new Rect(x, rect.y, 65,height) ,
			sFilter, GUIContent.none);
		x+=70;
			
		EditorGUI.PropertyField(
			new Rect(x, rect.y, 30, height),
			element.FindPropertyRelative("intensity"), GUIContent.none);
		x+=35;
			
		EditorGUI.PropertyField(
			new Rect(x, rect.y, 30, height),
			element.FindPropertyRelative("contrast"), GUIContent.none);
		x+=35;

		if ( sFilter.enumValueIndex == 0 || sFilter.enumValueIndex == 1  ) {
			
			EditorGUI.LabelField(
				new Rect(x, rect.y, 25, height),
				"min");
			x+=30;
				
			EditorGUI.PropertyField(
				new Rect(x, rect.y, 30, height),
				element.FindPropertyRelative("min"), GUIContent.none);
			x+=35;

			EditorGUI.LabelField(
				new Rect(x, rect.y, 25, height),
				"max");
			x+=30;

			EditorGUI.PropertyField(
				new Rect(x, rect.y, 30,height),
				element.FindPropertyRelative("max"), GUIContent.none);
			x+=35;
		}
		if ( sFilter.enumValueIndex == 3  ) {

			EditorGUI.LabelField(
				new Rect(x, rect.y, 25, height),
				"freq");
			x+=30;
				
			EditorGUI.PropertyField(
				new Rect(x, rect.y, 30,height),
				element.FindPropertyRelative("frequency"), GUIContent.none);
			x+=35;

			EditorGUI.LabelField(
				new Rect(x, rect.y, 25, height),
				"lac");
			x+=30;

			EditorGUI.PropertyField(
				new Rect(x, rect.y, 30, height),
				element.FindPropertyRelative("lacunarity"), GUIContent.none);
			x+=35;
			
			

		}
			
			
		if ( sFilter.enumValueIndex == 4  ) {
			
			EditorGUI.PropertyField(
				new Rect(x, rect.y, 160, height),
				element.FindPropertyRelative("texture"), GUIContent.none);
			x+=165;
				
			EditorGUI.PropertyField(
				new Rect(x, rect.y, 30, height),
				element.FindPropertyRelative("imageChannel"), GUIContent.none);
			x+=30;
				
				
		}
		
		if ( sFilter.enumValueIndex == 5  ) {
			
		
			int layerIndex = element.FindPropertyRelative("targetLayerIndex").intValue;
			if (layerIndex<layerCount )
			element.FindPropertyRelative("targetLayerIndex").intValue = EditorGUI.Popup(
				new Rect(x, rect.y, 150,height),
				element.FindPropertyRelative("targetLayerIndex").intValue,
				textureLayers);

			x+=155;
	
				
		}
		
		if ( ((MiniTerrainDecorator)target).debugMode ) {
				
			EditorGUI.LabelField(
				new Rect(x, rect.y,50, height),
				"dw "+(element.FindPropertyRelative("debugWeight").floatValue).ToString()) ;
			x+=55;
	 
	 
		}

	}
	
	private void OnEnable() {
		
		layerlist = new ReorderableList(serializedObject, 
			serializedObject.FindProperty("layers"), true, true, true, true);
			

		
		layerlist.onAddCallback =  (ReorderableList list) =>
		{
			
			var index = list.serializedProperty.arraySize;

			list.serializedProperty.arraySize++;
			list.index = index;
			var element = list.serializedProperty.GetArrayElementAtIndex(index);

			element.FindPropertyRelative("name").stringValue = "new layer";
			element.FindPropertyRelative("probability").floatValue = 1f;
			element.FindPropertyRelative("active").boolValue = true;
			element.FindPropertyRelative("maximumTreeCount").intValue = 1000;
			element.FindPropertyRelative("randomPosition").floatValue = 1;
			element.FindPropertyRelative("randomSize").floatValue = 0.1f;
			element.FindPropertyRelative("randomRotation").floatValue = 1f;
			
		
			element.FindPropertyRelative("width").floatValue =1f;
			element.FindPropertyRelative("height").floatValue =1f;
			element.FindPropertyRelative("rules").arraySize=0;

		};
	
		
		
		
		layerlist.drawHeaderCallback = 
		(Rect rect) => {  
			EditorGUI.LabelField(rect, "Terrain Layers                         ");
			
		};

		layerlist.drawElementCallback = (Rect rect, int index, bool isactive, bool isfocused) =>
		{
		
			var element = layerlist.serializedProperty.GetArrayElementAtIndex(index);
			rect.y += 2;
			float height =  EditorGUIUtility.singleLineHeight;
			DrawLayer( index, rect, height);
			float x = rect.x;;
		
		
		string listKey = element.propertyPath;

		ReorderableList innerReorderableList;

			var InnerList = element.FindPropertyRelative("rules");

		if (innerListDict.ContainsKey(listKey))
		{
			innerReorderableList = innerListDict[listKey];
		}
		else
		{
			innerReorderableList = new ReorderableList(element.serializedObject, InnerList)
			{
				displayAdd = true,
				displayRemove = true,
				draggable = true,

				onAddCallback = list =>
				{
					var ruleIndex = list.serializedProperty.arraySize;

					// Since this method overwrites the usual adding, we have to do it manually:
					// Simply counting up the array size will automatically add an element
					list.serializedProperty.arraySize++;
					list.index = ruleIndex;
					var innerElement = list.serializedProperty.GetArrayElementAtIndex(ruleIndex);

					innerElement.FindPropertyRelative("active").boolValue = true;
					
					innerElement.FindPropertyRelative("intensity").floatValue = 1f;
					innerElement.FindPropertyRelative("contrast").floatValue = 0f;
					innerElement.FindPropertyRelative("active").boolValue = true;
					innerElement.FindPropertyRelative("frequency").floatValue = 10f;
					innerElement.FindPropertyRelative("lacunarity").floatValue = 10f;
		
		
				},

				drawHeaderCallback = innerRect =>
				{
					EditorGUI.LabelField(innerRect, "Rules  Blend       Filter           Intensity/Contrast");

	
				},

				drawElementCallback = (innerRect, innerIndex, innerA, innerH) =>
				{
					
					DrawRules(	 InnerList,innerIndex,innerRect,height);
				}
            };
			innerListDict[listKey] = innerReorderableList;
		}

		var _height = (InnerList.arraySize + 3) * EditorGUIUtility.singleLineHeight;
			innerReorderableList.DoList(new Rect(rect.x, rect.y+2*height+15, rect.width, _height));
	
			GUI.color = Color.white;  
	};
		
		
			
		layerlist.elementHeightCallback = delegate(int index) {
			var element = layerlist.serializedProperty.GetArrayElementAtIndex(index);
			var elementHeight = EditorGUI.GetPropertyHeight(element);
			
	
			if ( 	element.FindPropertyRelative("layerIndex").intValue>= layerCount)
				elementHeight = EditorGUIUtility.singleLineHeight * 2.4f;
			else
			elementHeight = EditorGUIUtility.singleLineHeight * 1f;
			var innerList = element.FindPropertyRelative("rules");
			var margin = EditorGUIUtility.standardVerticalSpacing;
			return (innerList.arraySize + 4) * EditorGUIUtility.singleLineHeight + elementHeight + margin+25;
		};
		
	}
	
	public override void OnInspectorGUI() {
		

		MiniTerrainDecorator decorator = (MiniTerrainDecorator)target;
		if ( decorator.t==null) {
			decorator.GetTerrain();
		}
		if ( decorator.t==null) 
			return;
		
		serializedObject.Update();
		layerlist.DoLayoutList();
		serializedObject.ApplyModifiedProperties();
		
		layerCount = decorator.t.terrainData.terrainLayers.Length;
		treePrototypeCount =decorator.t.terrainData.treePrototypes.Length;
	
	
		choices = new string[layerCount+treePrototypeCount];
		string[] trees = new string[treePrototypeCount];
		for (int i = 0; i < trees.Length; i++)  {
			trees[i] =decorator.t.terrainData.treePrototypes[i].prefab.name;
		}
		
		for (int i = 0; i < layerCount+treePrototypeCount; i++) {
			if (i< layerCount)
				choices[i] =decorator.t.terrainData.terrainLayers[i].name;
			else
				choices[i] =decorator.t.terrainData.treePrototypes[i-layerCount].prefab.name+" (tree)";
		}
	
		textureLayers = new string[layerCount];
		for (int i = 0; i < layerCount; i++) 
				textureLayers[i] =decorator.t.terrainData.terrainLayers[i].name;
	
		string buttonStr= "Decorate";
		if ( decorator.calculating)
			buttonStr= "Decorate %"+Mathf.FloorToInt(decorator.calculatingPercent*100).ToString();
		if(GUILayout.Button(buttonStr)) {
			if ( !decorator.calculating)
				decorator.Decorate(); 
				
			
		}
		
		if(GUILayout.Button("Reset")) {
			decorator.calculating=false; 
			decorator.calculatingPercent=0;
		EditorUtility.ClearProgressBar();
		}			
			
		

		decorator.fallOffDistance = EditorGUILayout.FloatField("smooth transition", decorator.fallOffDistance);
		decorator.showProgress = EditorGUILayout.Toggle("show progress ( experimental)", decorator.showProgress);
		decorator.fallOfNoise = EditorGUILayout.Toggle("transition Noise", decorator.fallOfNoise);


		
		//			DrawDefaultInspector ();
		
	}
}
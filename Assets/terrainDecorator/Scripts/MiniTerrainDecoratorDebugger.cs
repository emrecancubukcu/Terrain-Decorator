using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MiniTerrainDecoratorDebugger : MonoBehaviour
{
	
	public Vector2Int debugPos;
	public Transform targetObject;

	//	[Multiline]
	[TextArea(3,50)]
	public string debugText;
	
	public  float[] clickMix;
	public MiniTerrainDecorator terrainDecorator;
	public Terrain t;
	
    // Start is called before the first frame update
    void Start()
	{     
		Vector3 position = targetObject.position;
		clickMix =GetTextureMix(position);
	
	}

    // Update is called once per frame
    void Update()
	{
	
			Vector3 position = targetObject.position;
		
			clickMix =GetTextureMix(position);
			
			for ( int n = 0; n < clickMix.Length; n ++ )
			{
				for ( int i = 0; i < terrainDecorator.layers.Count; i ++ )
					if ( n == terrainDecorator.layers[i].layerIndex)
					terrainDecorator.layers[i].debugLayerWeight = clickMix[n] ;
		
		
			}
		debugText = GetDebugText();


	}
    
    
    
	


	public float[] GetTextureMix(Vector3 worldPos) {
		Terrain terrain = t;
		TerrainData terrainData = terrain.terrainData;
		Vector3 terrainPos = terrain.transform.position;
	
		int mapX = (int)(((worldPos.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth);
		int mapZ = (int)(((worldPos.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight);
		debugPos = new Vector2Int(mapX,mapZ);
		float[,,] splatmapData = terrainData.GetAlphamaps(mapX,mapZ,1,1);
	
		float[] cellMix = new float[splatmapData.GetUpperBound(2)+1];
		for (int n=0; n<cellMix.Length; ++n)
		{
			cellMix[n] = splatmapData[0,0,n];    
		}
		return cellMix;         
	}
		
	
	
	
	
	public void Debugg() {
		
		string s="";
		int texturelayerCount = t.terrainData.terrainLayers.Length;
		int treePrototypeCount =t.terrainData.treePrototypes.Length;

		for (int layerNo = 0; layerNo < terrainDecorator.layers.Count; layerNo++)
		

		
			if (terrainDecorator.layers[layerNo].active) {
	
					int idx = terrainDecorator.layers[layerNo].layerIndex;
					if (idx< terrainDecorator.textureLayerCount)
						s+=t.terrainData.terrainLayers[idx].name+"\n";
					else
						s+=t.terrainData.treePrototypes[idx- terrainDecorator.textureLayerCount].prefab.name+" (tree)"+"\n";
			
				//	s+= i+" " + Terrain.activeTerrain.terrainData.terrainLayers[rules[i].layerIndex].name+ " "+rules[i].layerIndex+" "+rules[i].filter.ToString("")+"\n";
				}


		debugText = s;
		
		
	}

	public string GetDebugText()
	{

		string s = "";
		string name = "";
		for (int n = 0; n < clickMix.Length; n++)
		{
			name = "";
			//	for ( int i = 0; i < rules.Count; i ++ )
			//	 if ( n ==rules[i].layerIndex)
			name = t.terrainData.terrainLayers[n].name; ;

			s += n + " " + name + " " + clickMix[n] + "\n";


		}

		s = s + "pos " + debugPos + "\n";

		for (int layerNo = 0; layerNo < terrainDecorator.layers.Count; layerNo++)
			if (terrainDecorator.layers[layerNo].active)
            {
				s += layerNo + " " + terrainDecorator.layers[layerNo].name + " " + terrainDecorator.layers[layerNo].debugResultWeight + "\n";


				for (int i = 0; i < terrainDecorator.layers[layerNo].rules.Count; i++)

			//		if (terrainDecorator.layers[layerNo].rules[i].active)
					{


						name = "";

						if (terrainDecorator.layers[layerNo].layerIndex < t.terrainData.terrainLayers.Length)
							name = t.terrainData.terrainLayers[terrainDecorator.layers[layerNo].layerIndex].name;
						else
							name = t.terrainData.treePrototypes[terrainDecorator.layers[layerNo].layerIndex - t.terrainData.terrainLayers.Length].prefab.name; ;


						s += "   " + terrainDecorator.layers[layerNo].rules[i].debugWeight + "\n";


					}
			}


		return s;
	} 
	
		
	
	/*
	void OnGUI()
	{
	string s="";
	string name="";
	for ( int n = 0; n < clickMix.Length; n ++ )
	{
	name = Terrain.activeTerrain.terrainData.terrainLayers[n].name;;

	s +=  n +" "+name+" " +clickMix[n]+"\n";

		
	}
		
	s+="pos "+debugPos;
		
	if ( GUI.Button(new Rect(100, 10, 400, 20), "terra") ) {
	Decorate();
	}
		
	GUI.Label(new Rect(10, 10, 400, 300), s);
	}
	// Update is called once per frame
    
	*/
    
	void OnDrawGizmos()
	//	void OnDrawGizmosSelected()
	{/*
		if (t != null)
		{
			// Draws a blue line from this transform to the target
			Gizmos.color = Color.blue;
	
			Terrain terrain = t;
			TerrainData terrainData = terrain.terrainData;
			Vector3 terrainPos = terrain.transform.position;
	
			if ( targetObject!=null) {
				Vector3 position = targetObject.position;
				Vector3 endPosition = targetObject.position;
				//	position.x = terrainPos.x+debugPos.x* terrainData.size.x/ terrainData.alphamapWidth;
				//	position.z = terrainPos.z+debugPos.x* terrainData.size.z/ terrainData.alphamapHeight;
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(position, 1f);
				
				int mapX = (int)(((position.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth);
				int mapZ = (int)(((position.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight);
				float _height = terrain.terrainData.GetHeight(mapX,mapZ);
		
				endPosition.y = _height;
				Gizmos.DrawLine(position, endPosition);
				Gizmos.DrawSphere(endPosition, 1f);
		
				clickMix =GetTextureMix(position);
				debugText = GetDebugText();
				
			}
		
		}
		*/
	}
    
}

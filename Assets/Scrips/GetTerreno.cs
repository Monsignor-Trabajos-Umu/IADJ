using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTerreno : MonoBehaviour
{
    public int surfaceIndex = 0;
     [SerializeField]
     public Terrain terrain;
     private TerrainData terrainData;
     private Vector3 terrainPos;
    /*
     // Use this for initialization
     void Start () {
     
         terrain = Terrain.activeTerrain;
         terrainData = terrain.terrainData;
         terrainPos = terrain.transform.position;
         
     }
     
     // Update is called once per frame
     void Update () {
         surfaceIndex = GetainTexture(transform.position);
     }
     
     void OnGUI () {
         GUI.Box(new Rect( 100, 100, 200, 25 ), "index: "+surfaceIndex.ToString()+", name: "+terrainData.splatPrototypes[surfaceIndex].texture.name);
     }
     */
     private float[] GetTextureix(Vector3 WorldPos, Terrain t){
        terrain = Terrain.activeTerrain;
        terrainData = terrain.terrainData;
        terrainPos = terrain.transform.position;
        // returns an array containing the relative mix of textures
        // on the main terrain at this world position.

        // The number of values in the array will equal the number
        // of textures added to the terrain.

        // calculate which splat map cell the worldPos falls within (ignoring y)
        int mapX = (int)(((WorldPos.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth);
         int mapZ = (int)(((WorldPos.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight);
         
         // get the splat data for this cell as a 1x1xN 3d array (where N = number of textures)
         float[,,] splatmapData = terrainData.GetAlphamaps( mapX, mapZ, 1, 1 );
         
         // extract the 3D array data to a 1D array:
         float[] cellix = new float[ splatmapData.GetUpperBound(2) + 1 ];
         
         for(int n=0; n<cellix.Length; n++){
             cellix[n] = splatmapData[ 0, 0, n ];
         }
         return cellix;
     }
     
     public int GetainTexture(Vector3 WorldPos, Terrain t){
         // returns the zero-based index of the most dominant texture
         // on the main terrain at this world position.
         float[] mix = GetTextureix(WorldPos, t);
         
         float maxix = 0;
         int maxIndex = 0;
         
         // loop through each mix value and find the maximum
         for(int n=0; n<mix.Length; n++){
             if ( mix[n] > maxix ){
                  maxIndex = n;
                  maxix = mix[n];
                }
         }
         return maxIndex;
     }
}

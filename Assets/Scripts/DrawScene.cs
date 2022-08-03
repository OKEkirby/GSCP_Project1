using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawScene : MonoBehaviour
{
    public Camera cam;

    public GameObject ObjectType;

   [Range(1,100)] public int LevelWidth = 10;
    [Range(1, 100)] public int LevelHeight = 10;
    [Range(0f, 5f)]public float PixelSize = 0.1f;
   

    

    
    public Color32 colSkyBackground;
    public Color32 colLightSky;
    public Color32 colDarkSky;


    public Color32 colLargeLightGrass;
    public Color32 colLargeDarkGrass;

    [Range(0f, 100f)] public float largeGrassNoiseSize = 3.0f;
    [Range(0f, 100f)] public float largeGrassNoiseScale = 1.0f;

    [Range(-10, 10)] public int largeGrassNoiseOffset = 0;


    public Color32 colBush1;
    public Color32 colBush2;
    public Color32 colBush3;

    public Color32 colBushBerry1;

    public Color32 colBushBerry2;

    public Color32 colBushBackground;

    [Range(0, 20)] public int minBushHeight = 3;
    [Range(0, 20)] public int maxBushHeight = 6;
    [Range(0, 20)] public int minBushWidth = 3;
    [Range(0, 20)] public int maxBushWidth = 9;
    [Range(0.0f, 1.0f)] public float bushSpawnChance = 0.25f;

    [Range(0.0f, 1.0f)] public float bushFlowerSpawnChance = 0.1f;
    [Range(0f, 100f)] public float bushNoiseSize = 3.0f;
    [Range(0f, 100f)] public float bushNoiseScale = 1.0f;
    [Range(-10, 10)] public int bushNoiseOffset = 0;
    [Range(0.1f, 3.0f)] public float bushOutlineThickness = 1.4f;


    
    public Color32 colDirt;
    public Color32 colLightDirt;

    [Range(0f, 100f)] public float terrainNoiseSize = 3.0f;
    [Range(0f, 100f)] public float terrainNoiseScale = 1.0f;

    public Color32 colLightGrass;
    public Color32 colDarkGrass;
    public Color32 colDarkDirt;

    [Range(0f, 100f)] public float grassNoiseSize = 3.0f;
    [Range(0f, 100f)] public float grassNoiseScale = 1.0f;

    [Range(0.1f, 3.0f)] public float terrainOutlineThickness = 1.5f;


    public Color32 colBackground;
    public Color32 colMidground;
    public Color32 colForeground;

     

    
    GameObject[,] backgroundArray;
    GameObject[,] midgroundArray;
    GameObject[,] foregroundArray;

    float pixelGap;
    float spawnXOffset;
    float spawnYOffset;

    float screenWidth;
    
    float screenHeight;
    
    [ContextMenu("Spawn Objects")]
    public void SpawnObjects()
    {
        
        backgroundArray  = new GameObject[LevelWidth,LevelHeight];  
        midgroundArray  = new GameObject[LevelWidth,LevelHeight]; 
        foregroundArray  = new GameObject[LevelWidth,LevelHeight]; 
        
        
       
        
        pixelGap = PixelSize / 8;
        spawnXOffset = -((LevelWidth / 2) * (PixelSize + pixelGap)) + (PixelSize+pixelGap)/2;
        spawnYOffset = -((LevelHeight / 2) * (PixelSize + pixelGap)) + (PixelSize + pixelGap)/2;
        screenWidth = spawnXOffset + LevelWidth * (PixelSize + pixelGap);
        screenHeight = spawnYOffset + LevelHeight * (PixelSize+ pixelGap);

        RemoveObjects();
        
        draw_background();
        draw_foreground();
        draw_midground();

    }

    public void RemoveObjects() {
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            if (Application.isPlaying)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }
            else
            {
                GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
            }
            if(backgroundArray.Length > 0){
                Array.Clear(backgroundArray, 0, backgroundArray.Length);
            }
            if(midgroundArray.Length > 0){
                Array.Clear(midgroundArray, 0, midgroundArray.Length);
            }
            if(foregroundArray.Length > 0){
                Array.Clear(foregroundArray, 0, foregroundArray.Length);
            }
        }
    }

    public GameObject create_pixel(int x_pos, int y_pos, float depth, Color32 back_sprite_col, float back_sprite_scale){
                var newObject = Instantiate(ObjectType, new Vector3(spawnXOffset + x_pos * (PixelSize + pixelGap), spawnYOffset + y_pos * (PixelSize+ pixelGap), depth), Quaternion.identity);
               
                newObject.transform.localScale = Vector3.one * PixelSize;

                var propertyBlock = new MaterialPropertyBlock();

                propertyBlock.SetColor("_Color", UnityEngine.Random.ColorHSV());

                newObject.transform.parent = transform;
                
                newObject.GetComponent<Pixel_Properties>().SpriteTransform.localScale = new Vector3(back_sprite_scale*0.2f,back_sprite_scale*0.2f,1);

                newObject.GetComponent<Pixel_Properties>().SpriteBackground.color = back_sprite_col;
              

                return newObject;
    }

    public Color32[] create_color_shades(Color32 _col){
        
        Color32 col_light = _col;
        col_light.r += 30;
        col_light.g += 10;
        col_light.b += 10;
        Color32 col_dark = _col;
        col_dark.r -= 10;
        col_dark.g -= 10;
        col_dark.b += 20;
        Color32[] col_arr = new Color32[]{col_light, _col, col_dark};
        return col_arr;

    }
    public void draw_plane(int _width, int _height,Color32 _col){
            var newObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            newObject.transform.position = new Vector3(0,0,1);
            newObject.transform.Rotate(-180,0,0);
            newObject.transform.localScale = new Vector3(_width*PixelSize,_height*PixelSize,0.1f);
            var propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetColor("_Color", UnityEngine.Random.ColorHSV());
            newObject.GetComponent<Renderer>().material.color = _col;
            newObject.transform.parent = transform;
    }

    public void draw_sky(GameObject[,] scene){
        for (var i = 0; i < LevelWidth; i++){ for(var j = 0; j < LevelHeight; j++){
                scene[i, j].GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Color");
                if(j < 25){
                    scene[i, j].GetComponent<Renderer>().material.color = colLightSky;
                }else{
                    scene[i, j].GetComponent<Renderer>().material.color = colDarkSky;
                }
                scene[i,j].GetComponent<Pixel_Properties>().PixelType = 0;

        }}
    }

    public void draw_dirt(GameObject[,] scene){
        
        for (var i = 0; i < LevelWidth; i++){  for(var j = 0; j < LevelHeight; j++){
                if(scene[i,j]){
                    int randomIndex = UnityEngine.Random.Range(0, 2);
                    if(randomIndex == 0)
                    {
                         scene[i,j].GetComponent<Renderer>().material.color = colDirt;
                        
                    }
                    if (randomIndex == 1)
                    {
                         scene[i,j].GetComponent<Renderer>().material.color = colLightDirt;
                    }
                     scene[i,j].GetComponent<Pixel_Properties>().PixelType = 1;
                }
        }}
    }


    public void draw_grass(GameObject[,] scene){
            var noise = 0.0f;
           for (var i = 0; i < LevelWidth; i++){ 
             var iF = i / (float)LevelWidth;

            for(var j = 0; j < LevelHeight; j++){
                var jF = j / (float)LevelHeight;

                if(j == 0){
                    noise = (Mathf.PerlinNoise(iF* grassNoiseSize, jF* grassNoiseSize) * grassNoiseScale); 
                    noise = Mathf.Max(1,(int)Mathf.Round(noise));
                } 

                if(scene[i,j]){
                    if(!scene[i,j+1] && j-1 >= 0){
                        var ptype = foregroundArray[i,j].GetComponent<Pixel_Properties>().PixelType;
                        //var ptype_up = foregroundArray[i,j+1].GetComponent<Pixel_Properties>().PixelType;
                        var ptype_down = foregroundArray[i,j-1].GetComponent<Pixel_Properties>().PixelType;

                        
                            for(var n = 0; n < noise; n++){
                                scene[i,j-n].GetComponent<Renderer>().material.color = colLightGrass;

                            }
                            
                            
                            scene[i,j-(int)noise].GetComponent<Renderer>().material.color = colDarkGrass;
                            scene[i,j-(int)noise-1].GetComponent<Renderer>().material.color = colDarkDirt;
                            
                        

                    }
                }

           }}
    }

    public void draw_tall_grass(GameObject[,] scene){
        var noise = 0.0f;
        for (var i = 0; i < LevelWidth; i++){ 
            var iF = i / (float)LevelWidth;

            for(var j = 0; j < LevelHeight; j++){
                var jF = j / (float)LevelHeight;
                if(j == 0){
                    noise = (Mathf.PerlinNoise(iF* largeGrassNoiseSize, jF* largeGrassNoiseSize) * largeGrassNoiseScale); 
                    noise = Mathf.Max(0,(int)Mathf.Round(noise));

                } 

                if(!foregroundArray[i,j] && foregroundArray[i,j-largeGrassNoiseOffset+1] && j-largeGrassNoiseOffset >= 0){
                        scene[i, j+(int)noise-largeGrassNoiseOffset] = create_pixel(i,j+(int)noise-largeGrassNoiseOffset,0,colMidground, 1.25f);
                        
                        scene[i,j+(int)noise-largeGrassNoiseOffset].GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Color");
                        scene[i,j+(int)noise-largeGrassNoiseOffset].GetComponent<Renderer>().material.color = colLargeLightGrass;
                }

        }}
    }

    public void create_bush(int x_pos, int y_pos, int max_width, GameObject[,] scene){
            

            var noise = 0.0f;
            int rand_width = UnityEngine.Random.Range(minBushWidth, max_width+1);
            int rand_col = UnityEngine.Random.Range(0, 3);
            Color32 col = colBush1;

            if(rand_col == 0){
                col = colBush1;
            }
            if(rand_col == 1){
                col = colBush2;
            }
            if(rand_col == 2){
                col = colBush3;
            }
            Color32[] col_shade = create_color_shades(col);

            for(var i = 0; i < rand_width; i++){
                var iF = i / (float)LevelWidth;
                var height = y_pos + Mathf.Clamp(Mathf.Min(i+1, rand_width-i),minBushHeight-1,maxBushHeight-1);

                noise = (Mathf.PerlinNoise(iF* bushNoiseSize, iF*2*bushNoiseSize) * bushNoiseScale); 
                noise = Mathf.Max(0,(int)Mathf.Round(noise))-2;
                var h_limit = 0;
                for(var j = height + (int)noise; j >= y_pos; j--){
                    if(x_pos + i < LevelWidth){
                        if(!scene[x_pos+i, j]){
                            scene[x_pos+i, j] = create_pixel(x_pos+i,j,0.0f,colBushBackground, 1f);
                            
                        }
                        scene[x_pos+i, j].transform.position = scene[x_pos+i, j].transform.position + new Vector3(0,0,-0.2f);
                        scene[x_pos+i, j].GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Color");
                        scene[x_pos+i, j].GetComponent<Pixel_Properties>().SpriteBackground.color = colBushBackground;
                        scene[x_pos+i, j].GetComponent<Pixel_Properties>().SpriteTransform.localScale = new Vector3(bushOutlineThickness*0.2f,bushOutlineThickness*0.2f,1);

                        if(!scene[x_pos+i, j+1] || i == 0 || i == rand_width-1){
                            scene[x_pos+i, j].GetComponent<Renderer>().material.color = col_shade[0];
                        }else if(j == y_pos && i >= 2 && i < rand_width-2){
                            scene[x_pos+i, j].GetComponent<Renderer>().material.color = col_shade[2];
                        
                        }else{
                            scene[x_pos+i, j].GetComponent<Renderer>().material.color = col_shade[1];
                        }
                        h_limit = Mathf.Max(h_limit,j);
                    }
                 }  

                
                create_bush_flowers(x_pos+1,x_pos+rand_width-1,y_pos+2,h_limit-2, scene);
                

            }      
    }

    public void create_bush_flowers(int left_limit, int right_limit, int bottom_limit, int top_limit, GameObject[,] scene){
           var num_flowers = 0;
            var rand_flower = UnityEngine.Random.value;
            if(rand_flower <= bushFlowerSpawnChance){
                rand_flower = UnityEngine.Random.Range(0, 5);
                if(rand_flower == 4){
                    num_flowers = 1;
                }
                //else{
                  //  num_flowers = 2;
                //}
            }

            if(num_flowers > 0){
                Vector2[] flower_locations = new Vector2[num_flowers];
                for(int i = 0; i < num_flowers; i++){
                       var rand_x = UnityEngine.Random.Range(left_limit, right_limit);
                       var rand_y = UnityEngine.Random.Range(bottom_limit, top_limit);
                        

                       /* for (dx = rand_x-1; dx <= rand_x+1; dx++) {
                        for (dy = rand_y-1; dy <= rand_y+1; dy++) {
                        if (dx == rand_x || dy == rand_y) {
                          result.Add(array[x + dx][y + dy]);
                        }
                        }
                        }*/


                        flower_locations[i].x = rand_x;
                        flower_locations[i].y = rand_y;

                }   
                for(int i = 0; i < flower_locations.Length; i++){
                    scene[(int)flower_locations[i].x, (int)flower_locations[i].y].GetComponent<Renderer>().material.color = colBushBerry1;
                }
            }
    }
    public void draw_bushes(GameObject[,] scene){

        for (var i = 0; i < LevelWidth; i++){  

            for(var j = 0; j < LevelHeight; j++){
                 
            if(j-1 >= 0){
                if(!foregroundArray[i,j] && foregroundArray[i,j-1]){
                    var emptySpace = 0;
                    for(var k = 0; k < maxBushWidth; k++){
                        if(i + k < LevelWidth){
                            if(!foregroundArray[i + k,j] && foregroundArray[i + k,j-1]){
                                emptySpace += 1;
                            }
                        }
                    }
                    if(emptySpace > minBushWidth){
                        var rand = UnityEngine.Random.value;
                        if(rand <= bushSpawnChance){
                            create_bush(i,j,emptySpace,scene);
                        }
                    }
                }
            }

        }}
    }


    public void draw_background(){
        for (var i = 0; i < LevelWidth; i++){ for (var j = 0; j < LevelHeight; j++){
                
                backgroundArray[i, j] = create_pixel(i,j,1,colBackground, 1.25f);
        }}
           //draw_plane(LevelWidth, LevelHeight, colSkyBackground);
           draw_sky(backgroundArray);
    }

    public void draw_midground(){
        draw_tall_grass(midgroundArray);
        draw_bushes(midgroundArray);
    }

    public void draw_foreground(){

        var noise = 0.0f;
        for (var i = 0; i < LevelWidth; i++){ 
            var iF = i / (float)LevelWidth;

            for(var j = 0; j < LevelHeight; j++){
                var jF = j / (float)LevelHeight;
                if(j == 0){
                    noise = (Mathf.PerlinNoise(iF* terrainNoiseSize, jF* terrainNoiseSize) * terrainNoiseScale); 
                    noise = Mathf.Max(0,(int)Mathf.Round(noise) -2);
                } 
                if(j <= 5 + noise){  
                    foregroundArray[i, j] = create_pixel(i,j,-1,colForeground, terrainOutlineThickness);
                }
            
        }}
        draw_dirt(foregroundArray);
        draw_grass(foregroundArray);
       
    }

    
}



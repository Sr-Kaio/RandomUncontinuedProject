using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGen : MonoBehaviour
{
    [Header("Chunk Size")]
    public static readonly Vector3Int ChunkSize = new Vector3Int(64, 32, 64);
    [Space]
    [Header("Perlin Noise Configuration")]
    [SerializeField] private Vector2 NoiseScale = Vector2.one;
    [SerializeField] private Vector2 NoiseOffset = Vector2.zero;
    [Space]
    [Header("Terrain Height")]
    [SerializeField] private int HeightOffset = 15;
    [SerializeField] private float HeightIntensity = 5f;
    [Space]
    [Header("Texture & UVs")]
    [SerializeField] private Texturer Texturer;
    private int[,,] TempData;

    private void Start()
    {
        TempData = new int[ChunkSize.x, ChunkSize.y, ChunkSize.z];

        for (int x = 0; x < ChunkSize.x; x++)
        {
            for (int z = 0; z < ChunkSize.z; z++)
            {
                float PerlinX = NoiseOffset.x + x / (float)ChunkSize.x * NoiseScale.x;
                float PerlinY = NoiseOffset.y + z / (float)ChunkSize.z * NoiseScale.y;

                int HeightG = Mathf.RoundToInt(Mathf.PerlinNoise(PerlinX, PerlinY) * HeightIntensity + HeightOffset);
                for(int y = HeightG; y >= 0; y--) {
                    int BlockType = 0;
                    if (y == HeightG) BlockType = 0;
                    if (y < HeightG && y > HeightG - 4) BlockType = 1;
                    if (y <= HeightG - 4 && y > 0) BlockType = 2;
                    if (y == 0) BlockType = 3;

                    TempData[x, y, z] = BlockType;
                }
            }
        }
        GameObject World = new GameObject("World", new System.Type[] {typeof(MeshCollider), typeof(MeshFilter) , typeof(MeshRenderer) });
        World.GetComponent<MeshFilter>().mesh = new MeshGen(Texturer).RenderMesh(TempData);
    }

   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGen
{
    public class FaceData
    {
        public FaceData(Vector3[] verts, int[] tris, int[] uvIndexOrder) {
            Vertices = verts;
            Indices = tris;
            UVIndexOrder = uvIndexOrder;
        }


        public Vector3[] Vertices;
        public int[] Indices;
        public int[] UVIndexOrder;
    }

    #region FaceData

    static readonly Vector3Int[] CheckDirections = new Vector3Int[]
    {
        Vector3Int.right,
        Vector3Int.left,
        Vector3Int.up,
        Vector3Int.down,
        Vector3Int.forward,
        Vector3Int.back
    };

    static readonly Vector3[] RightFace = new Vector3[]
    {
        new Vector3(.5f, -.5f, -.5f),
        new Vector3(.5f, -.5f, .5f),
        new Vector3(.5f, .5f, .5f),
        new Vector3(.5f, .5f, -.5f)
    };

    static readonly int[] RightTris = new int[]
    {
        0,2,1,0,3,2
    };

    static readonly Vector3[] LeftFace = new Vector3[]
    {
        new Vector3(-.5f, -.5f, -.5f),
        new Vector3(-.5f, -.5f, .5f),
        new Vector3(-.5f, .5f, .5f),
        new Vector3(-.5f, .5f, -.5f)
    };

    static readonly int[] LeftTris = new int[]
    {
        0,1,2,0,2,3
    };

    static readonly Vector3[] UpFace = new Vector3[]
    {
        new Vector3(-.5f, .5f, -.5f),
        new Vector3(-.5f, .5f, .5f),
        new Vector3(.5f, .5f, .5f),
        new Vector3(.5f, .5f, -.5f)
    };

    static readonly int[] UpTris = new int[]
    {
        0,1,2,0,2,3
    };

    static readonly Vector3[] DownFace = new Vector3[]
    {
        new Vector3(-.5f, -.5f, -.5f),
        new Vector3(-.5f, -.5f, .5f),
        new Vector3(.5f, -.5f, .5f),
        new Vector3(.5f, -.5f, -.5f)
    };

    static readonly int[] DownTris = new int[]
    {
        0,2,1,0,3,2
    };

    static readonly Vector3[] ForwardFace = new Vector3[]
    {
        new Vector3(-.5f, -.5f, .5f),
        new Vector3(-.5f, .5f, .5f),
        new Vector3(.5f, .5f, .5f),
        new Vector3(.5f, -.5f, .5f)
    };

    static readonly int[] ForwardTris = new int[]
    {
        0,2,1,0,3,2
    };

    static readonly Vector3[] BackFace = new Vector3[]
    {
        new Vector3(-.5f, -.5f, -.5f),
        new Vector3(-.5f, .5f, -.5f),
        new Vector3(.5f, .5f, -.5f),
        new Vector3(.5f, -.5f, -.5f)
    };

    static readonly int[] BackTris = new int[]
    {
        0,1,2,0,2,3
    };

    #endregion

    #region FaceUVData

    static readonly int[] XUVOrder = new int[]
    {
     2, 3, 1, 0
    };

    static readonly int[] YUVOrder = new int[]
    {
      0, 1, 3, 2
    };


    static readonly int[] ZUVOrder = new int[]
    {
      3, 1, 0, 2
    };


    #endregion
    private Dictionary<Vector3Int, FaceData> VoxelFaces = new Dictionary<Vector3Int, FaceData>();
    private Texturer Texturer;
    public MeshGen(Texturer _texturer)
    {
        VoxelFaces = new Dictionary<Vector3Int, FaceData>();
        Texturer = _texturer;
        for (int i = 0; i < CheckDirections.Length; i++)
        {
            if(CheckDirections[i] == Vector3Int.up) {
                VoxelFaces.Add(CheckDirections[i], new FaceData(UpFace, UpTris, YUVOrder));
            } else if (CheckDirections[i] == Vector3Int.down) {
                VoxelFaces.Add(CheckDirections[i], new FaceData(DownFace, DownTris, YUVOrder));
            } else if (CheckDirections[i] == Vector3Int.forward) {
                VoxelFaces.Add(CheckDirections[i], new FaceData(ForwardFace, ForwardTris, ZUVOrder));
            } else if (CheckDirections[i] == Vector3Int.back) {
                VoxelFaces.Add(CheckDirections[i], new FaceData(BackFace, BackTris, ZUVOrder));
            } else if (CheckDirections[i] == Vector3Int.left) {
                VoxelFaces.Add(CheckDirections[i], new FaceData(LeftFace, LeftTris, XUVOrder));
            } else if (CheckDirections[i] == Vector3Int.right) {
                VoxelFaces.Add(CheckDirections[i], new FaceData(RightFace, RightTris, XUVOrder));
            }

        }
    }

    public Mesh RenderMesh(int[,,] Data)
    {
        List<Vector3> Vertices = new List<Vector3>();
        List<int> Indices = new List<int>();
        List<Vector2> UVs = new List<Vector2>();
        Mesh m = new Mesh();
        for(int x = 0; x < WorldGen.ChunkSize.x; x++)
        {
            for (int y = 0; y < WorldGen.ChunkSize.y; y++)
            {
                for (int z = 0; z < WorldGen.ChunkSize.z; z++)
                {
                    Vector3Int Pos = new Vector3Int(x, y, z);
                    for (int i = 0; i < CheckDirections.Length; i++) 
                    {
                        Vector3Int BlockCheck = Pos + CheckDirections[i];
                        try
                        {
                            if (Data[BlockCheck.x, BlockCheck.y, BlockCheck.z] == 0)
                            {
                                if (Data[Pos.x, Pos.y, Pos.z] != 0)
                                {
                                    FaceData Face = VoxelFaces[CheckDirections[i]];
                                    int ID = Data[Pos.x, Pos.y, Pos.z];
                                    Texturer.faceTexture texture = Texturer.TextureS[ID];
                                    foreach(Vector3 v in Face.Vertices)
                                    {
                                        Vertices.Add(new Vector3(x, y, z) + v);
                                    }

                                    foreach (int t in Face.Indices)
                                    {
                                        Indices.Add(Vertices.Count - 4 + t);
                                    }

                                    Vector2[] UVs_B = texture.GetUvsAtDirection(CheckDirections[i]);
                                    foreach (int uv in Face.UVIndexOrder)
                                    {
                                        UVs.Add(UVs_B[uv]);
                                    }

                                }
                            }
                        }
                        catch (System.Exception)
                        {
                            if (Data[Pos.x, Pos.y, Pos.z] != 0)
                            {
                                FaceData Face = VoxelFaces[CheckDirections[i]];
                                int ID = Data[Pos.x, Pos.y, Pos.z];
                                Texturer.faceTexture texture = Texturer.TextureS[ID];
                                foreach (Vector3 v in Face.Vertices)
                                {
                                    Vertices.Add(new Vector3(x, y, z) + v);
                                }

                                foreach (int t in Face.Indices)
                                {
                                    Indices.Add(Vertices.Count - 4 + t);
                                }

                                Vector2[] UVs_B = texture.GetUvsAtDirection(CheckDirections[i]);
                                foreach (Vector2 uv in UVs_B)
                                {
                                    UVs.Add(uv);
                                }

                            }
                        }
                        
                    }
                }
            }
        }
        m.SetVertices(Vertices);
        m.SetIndices(Indices, MeshTopology.Triangles, 0);
        m.SetUVs(0, UVs);
        m.RecalculateBounds();
        m.RecalculateNormals();
        m.RecalculateTangents();
        return m;
    }

}

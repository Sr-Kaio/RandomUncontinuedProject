using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texturer : MonoBehaviour
{
    [System.Serializable]

    public class faceTexture
    {

        public string TextureName;
        public Sprite xtex, ytex, ztex;
        public cube Specific_Face_Texture;

        [System.Serializable]
        public class cube
        {
            public Sprite Up, Down;
            [Space]
            public Sprite Left, Right;
            [Space]
            public Sprite Foward, Back;

        }
        public Vector2[] GetUvsAtDirection(Vector3Int Direction)
        {
            if (Direction == Vector3Int.forward)
            {
                return ztex != null ? ztex.uv : Specific_Face_Texture.Foward.uv;
            }
            else if (Direction == Vector3Int.back)
            {
                return ztex != null ? ztex.uv : Specific_Face_Texture.Back.uv;
            }

            if (Direction == Vector3Int.left)
            {
                return xtex != null ? xtex.uv : Specific_Face_Texture.Left.uv;
            }
            else if (Direction == Vector3Int.right)
            {
                return xtex != null ? xtex.uv : Specific_Face_Texture.Right.uv;
            }

            if (Direction == Vector3Int.up)
            {
                return ytex != null ? ytex.uv : Specific_Face_Texture.Up.uv;
            }
            else if (Direction == Vector3Int.down)
            {
                return ytex != null ? ytex.uv : Specific_Face_Texture.Down.uv;
            }

            return null;
        }
    }

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

    [SerializeField] private faceTexture[] faceTextureS;
    public Dictionary<int, faceTexture> TextureS;

    private void Awake()
    {
        TextureS = new Dictionary<int, faceTexture>();
        for (int i = 0; i < faceTextureS.Length; i++)
        {
            TextureS.Add(i + 1, faceTextureS[i]);
        }
    }

}

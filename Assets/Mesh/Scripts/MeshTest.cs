using UnityEngine;
using System.Collections;

public class MeshTest : MonoBehaviour
{
    public Texture2D heightMap;
    public Material material;
    public Vector2 size;
    public Vector2 segment;
    public float height;

    private Vector3[] vertives;
    private Vector2[] uvs;
    private int[] triangles;

    private GameObject meshObject;

	void Start ()
    {
        InitUV();
        InitTriangles();
        InitVertives();

        CreateMesh();
	}

    public void CreateMesh()
    {
        meshObject = new GameObject();
        meshObject.AddComponent<MeshRenderer>().material = material;
        Mesh mesh = meshObject.AddComponent<MeshFilter>().mesh;
        mesh.name = "mesh";
        mesh.vertices = vertives;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    /// <summary>
    /// 顶点
    /// </summary>
    public void InitVertives()
    {
        int sum = Mathf.FloorToInt(( segment.x + 1 ) * ( segment.y + 1 ));
        float w = size.x / segment.x;
        float h = size.y / segment.y;
        int index = 0;
        vertives = new Vector3[sum];
        for(int i = 0; i < segment.y; i++)
        {
            for(int j = 0; j < segment.x; j++)
            {
                float tempHeight = GetHeight(heightMap, uvs[index]);
                vertives[index] = new Vector3(j * w, tempHeight, i * h);
                index++;
            }
        }
    }

    /// <summary>
    /// UV
    /// </summary>
    public void InitUV()
    {
        int sum = Mathf.FloorToInt(( segment.x + 1 ) * ( segment.y + 1 ));
        uvs = new Vector2[sum];
        float u = 1.0f / segment.x;
        float v = 1.0f / segment.y;
        int index = 0;
        for (int i = 0; i < segment.y; i++)
        {
            for (int j = 0; j < segment.x; j++)
            {
                uvs[index] = new Vector2(j * u, i * v);
                index++;
            }
        }
    }

    /// <summary>
    /// 索引
    /// </summary>
    public void InitTriangles()
    {
        int sum = Mathf.FloorToInt(segment.x * segment.y * 6);
        triangles = new int[sum];
        uint index = 0;
        for (int i = 0; i < segment.y; i++)
        {
            for (int j = 0; j < segment.x; j++)
            {
                int role = Mathf.FloorToInt(segment.x) + 1;
                int self = j + ( i * role );
                int next = j + ( ( i + 1 ) * role );
                triangles[index] = self;
                triangles[index + 1] = next + 1;
                triangles[index + 2] = self + 1;
                triangles[index + 3] = self;
                triangles[index + 4] = next;
                triangles[index + 5] = next + 1;
                index += 6;
            }
        }
    }

    /// <summary>
    /// 获取高度
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="uv"></param>
    /// <returns></returns>
    public float GetHeight(Texture2D texture,Vector2 uv)
    {
        return GetColor(texture, uv).grayscale * height;
        
    }

    /// <summary>
    /// 获取图片上点的颜色
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="uv"></param>
    /// <returns></returns>
    public Color GetColor(Texture2D texture,Vector2 uv)
    {
        return texture.GetPixel(Mathf.FloorToInt(texture.width * uv.x), Mathf.FloorToInt(texture.height * uv.y));
    }
}
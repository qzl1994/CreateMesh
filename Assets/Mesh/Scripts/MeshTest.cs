using UnityEngine;

[RequireComponent(typeof(MeshFilter)),RequireComponent(typeof(MeshRenderer))]
public class MeshTest : MonoBehaviour
{
    public Texture2D heightMap;
    public Material material;
    public Vector2 size;
    public Vector2 pixel;
    public float height;

    private Vector3[] vertices;
    private int[] triangles;

	void Start ()
    {
        InitVertices();
        InitTriangles();

        //创建mesh
        CreateMesh();
	}

    public void CreateMesh()
    {
        GetComponent<MeshRenderer>().material = material;
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.name = "mesh";

        mesh.Clear();//重建前清空数据

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();//重置法线
        mesh.RecalculateBounds();//计算网格包围体
    }

    /// <summary>
    /// 顶点
    /// </summary>
    public void InitVertices()
    {
        int sum = Mathf.FloorToInt(( pixel.x + 1 ) * ( pixel.y + 1 ));
        float w = size.x / pixel.x;
        float h = size.y / pixel.y;
        int index = 0;
        vertices = new Vector3[sum];
        for(int i = 0; i < pixel.y + 1; i++)
        {
            for(int j = 0; j < pixel.x + 1; j++)
            {
                float tempHeight = GetHeight(heightMap, new Vector2(j / pixel.x, i / pixel.y));
                vertices[index] = new Vector3(j * w, tempHeight, i * h);
                index++;
            }
        }
    }

    /// <summary>
    /// 索引
    /// </summary>
    public void InitTriangles()
    {
        int sum = Mathf.FloorToInt(pixel.x * pixel.y * 6);
        triangles = new int[sum];
        int index = 0;

        int verticesNum = Mathf.FloorToInt(pixel.x) + 1;

        for (int i = 0; i < pixel.y; i++)
        {
            for (int j = 0; j < pixel.x; j++)
            {            
                int self = j + ( i * verticesNum );
                int next = j + ( ( i + 1 ) * verticesNum );

                //右下角三角形
                triangles[index] = self;
                triangles[index + 1] = next + 1;
                triangles[index + 2] = self + 1;
                //左上角三角形
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
    /// <param name="v"></param>
    /// <returns></returns>
    public float GetHeight(Texture2D texture,Vector2 v)
    {
        return GetColor(texture, v).grayscale * height;
        
    }

    /// <summary>
    /// 获取图片上点的颜色
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="v"></param>
    /// <returns></returns>
    public Color GetColor(Texture2D texture,Vector2 v)
    {
        return texture.GetPixel(Mathf.FloorToInt(texture.width * v.x), Mathf.FloorToInt(texture.height * v.y));
    }
}
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class VectorLines : MonoBehaviour
{

    //this method allows for two separate colors to be drawn on one model
    //since different models may have different vertex numbers, I made these public so they
    //can be assigned in the editor
    public int drawStart;//Stores the first vertex to be drawn in the first color
    public int drawStop;//stores the last vertex to be drawn in the first color

    public int drawStart2;//Stores the first vertex to be drawn in the second color
    public int drawStop2;//stores the last vertex to be drawn in the second color

    int _drawQueue = 0;
    //string s = "";

    //This method uses materials to choose the colors
    //assign a color in the editor to each of these slots
    //the names are arbitrary, they both do the same thing,
    //but one will be used for the first set of vertices, and the other for the second
    public Material MeshMaterial;
    public Material WireMaterial;

    public Vector3[] renderQueue;
    int myVertices;

    Renderer m_Renderer;
    MeshFilter meshFilter;
    Mesh mesh;

    //Start will initialize the mesh and determine the locations of each vertex
    void Start()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;
        m_Renderer = gameObject.GetComponent<MeshRenderer>();

        //CREATE AN ARRAY TO STORE VERTEX INFORMATION
        renderQueue = new Vector3[mesh.vertexCount];
        myVertices = mesh.vertexCount;
        for (var i = 0; i < myVertices; i += 1)
        {
            renderQueue[i] = mesh.vertices[i];
        }
    }

    void OnRenderObject()
    {
        if (m_Renderer.isVisible)
        {
            GL.MultMatrix(transform.localToWorldMatrix);

            //Begin drawing first set of lines
            GL.Begin(GL.LINES);
            MeshMaterial.SetPass(0);
            GL.Color(new Color(0f, 0f, 0f, 1f));

            _drawQueue = drawStart;
            while (_drawQueue < drawStop)
            {
                GL.Vertex3(renderQueue[_drawQueue].x, renderQueue[_drawQueue].y, renderQueue[_drawQueue].z);
                GL.Vertex3(renderQueue[_drawQueue + 1].x, renderQueue[_drawQueue + 1].y, renderQueue[_drawQueue + 1].z);

                _drawQueue += 1;
            }

            GL.End();

            //begin drawing second set of lines

            GL.Begin(GL.LINES);
            WireMaterial.SetPass(0);
            GL.Color(new Color(0f, 0f, 0f, 1f));

            _drawQueue = drawStart2;
            while (_drawQueue < drawStop2 - 1)
            {
                GL.Vertex3(renderQueue[_drawQueue].x, renderQueue[_drawQueue].y, renderQueue[_drawQueue].z);
                GL.Vertex3(renderQueue[_drawQueue + 1].x, renderQueue[_drawQueue + 1].y, renderQueue[_drawQueue + 1].z);

                _drawQueue += 1;
            }
            GL.End();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public static class accessories
{
    public static Vector3 GridCentrer()

    {
      
        float q = 2f * gridLength;
        return new Vector3(q * (0.5f + (Mathf.Round(p.x / q))), 0, q * (0.5f + Mathf.Round(p.z / q)));
    }
}
*/

public class Grid : MonoBehaviour {

	Renderer renderer;
	Transform ship;
	float offset, gridLength;

	void Start () {

		ship = GameObject.FindWithTag ("Player").transform;
		offset = transform.position.z - ship.position.z;
		gridLength = GetComponent<MeshFilter>().mesh.bounds.size.z;

		MeshFilter mf = GetComponent<MeshFilter>();
		Debug.Log ("vert: " + mf.mesh.vertexCount);
		Texture2D texture = new Texture2D(16, 16);
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Repeat;

		for (int y = 0; y < texture.height; y++) {
			for (int x = 0; x < texture.width; x++) {
				Color color = (((x % 16) == 0) || ((y % 16) == 0)) ? new Color (1, 1, 1, 1f) : new Color (0, 0, 0, 0);
				texture.SetPixel(x, y, color);
			}
		}
		texture.Apply();

		renderer = GetComponent<Renderer>();
		renderer.material.mainTexture = texture;
		renderer.material.mainTextureScale = new Vector2 (0.5f*transform.localScale.x + (1f/16f), 0.5f*transform.localScale.z + (1f/16f)); 
		renderer.enabled = true;
		/*
		float width = 1.0f;
		float height = 1.0f;

		  Mesh mesh = new Mesh();
		mf.mesh = mesh;

		Vector3[] vertices = new Vector3[4];

		// 2 3
		// 0 1

		vertices[0] = new Vector3(-0.5f*width, 0, -0.5f*height);
		vertices[1] = new Vector3( 0.5f*width, 0, -0.5f*height);
		vertices[2] = new Vector3(-0.5f*width, 0,  0.5f*height);
		vertices[3] = new Vector3( 0.5f*width, 0,  0.5f*height);

		mesh.vertices = vertices;

		int[] tri = new int[6];

		tri[0] = 0;
		tri[1] = 2;
		tri[2] = 1;

		tri[3] = 2;
		tri[4] = 3;
		tri[5] = 1;

		mesh.triangles = tri;

		Vector3[] normals  = new Vector3[4];

		normals[0] = Vector3.up;
		normals[1] = Vector3.up;
		normals[2] = Vector3.up;
		normals[3] = Vector3.up;

		mesh.normals = normals;

		Vector2[] uv = new Vector2[4];

		uv[0] = new Vector2(0, 0);
		uv[1] = new Vector2(1, 0);
		uv[2] = new Vector2(0, 1);
		uv[3] = new Vector2(1, 1);

		mesh.uv = uv;
*/
	}

	void Update () {
		transform.position = new Vector3 (0, -10, ship.position.z + offset);
		renderer.material.mainTextureOffset  = new Vector2 (0, -ship.position.z/(2f*gridLength)); 
	}

	public void Warp(Vector3 position) {
	}

	public Vector3 GetGridPosition(Vector3 p) {
		float q = 2f*gridLength;
		return new Vector3(q*(0.5f + (Mathf.Round(p.x/q))), 0, q*(0.5f + Mathf.Round(p.z/q)));
	}

}

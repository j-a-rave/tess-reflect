using UnityEngine;
using System.Collections;

public class CubemapRender : MonoBehaviour {

	public int cubemapSize = 128;
	public float nearClip = 0.01f;
	public float farClip = 500;
	public bool oneFacePerFrame = false;
	public LayerMask layerMask;
	Camera cam;
	RenderTexture rTex;

	void Start () {
		UpdateCubemap (63);
	}

	void UpdateCubemap(int faceMask)
	{
		if (!cam)
		{
			GameObject cmc = new GameObject("CubemapCamera", typeof(Camera));
			cmc.hideFlags = HideFlags.HideAndDontSave;
			cmc.transform.position = transform.position;
			cmc.transform.rotation = Quaternion.identity;

			cam = cmc.GetComponent<Camera>();
			cam.cullingMask = layerMask;
			cam.nearClipPlane = nearClip;
			cam.farClipPlane= farClip;
			cam.fieldOfView = 12.0f;
			cam.enabled = false;
		}

		if (!rTex)
		{
			rTex = new RenderTexture(cubemapSize, cubemapSize, 16);
			rTex.isPowerOfTwo = true;
			rTex.isCubemap = true;
			rTex.hideFlags = HideFlags.HideAndDontSave;

			foreach(Renderer r in GetComponentsInChildren<Renderer>())
			{
				foreach (Material m in r.sharedMaterials)
				{
					if (m.HasProperty("_Cube"))
					{
						m.SetTexture("_Cube",rTex);
					}
				}
			}
		}
		cam.transform.position = transform.position;
		cam.RenderToCubemap (rTex, faceMask);
	}

	void LateUpdate()
	{
		if (oneFacePerFrame) {
			int faceToRender = Time.frameCount % 6;
			int faceMask = 1 << faceToRender; //cubemap faces are a bitfield.
			UpdateCubemap (faceMask);
		} else
		{
			UpdateCubemap (63);
		}
	}

	void OnDisable()
	{
		DestroyImmediate (cam);
		DestroyImmediate (rTex);
	}
}

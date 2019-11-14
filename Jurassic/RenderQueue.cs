using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RenderQueue : MonoBehaviour {
	public int renderQueue = 4000;//在Transparent前面

	public Material material;
	public Material material1;

	void Start()
	{
		material.renderQueue = renderQueue;
		material1.renderQueue = renderQueue;
	}



}

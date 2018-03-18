using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour {
	public static List<GameObject> SelecGameObjects = new List<GameObject>();

	/// <summary>
	/// If no shader was given to <see cref="SetShader"/> via argument, this shader is used.
	/// </summary>
	[SerializeField] private Shader selectShader;

	/// <summary>
	/// A stack from all shaders which where overriden by SetShader. The <see cref="RevertShader"/> pops the shaders back from the stack
	/// </summary>
	private Stack<Shader> oldShaders = new Stack<Shader>();
	
	void Awake() {
		SelecGameObjects.Add(this.gameObject);
	}

	/// <summary>
	/// Sets the shader of this object and puts it on to the stack
	/// </summary>
	/// <param name="shader">the shader</param>
	public void SetShader(Shader shader = null)
	{
		if (shader == null)
			shader = selectShader;
		Renderer renderer = GetComponent<Renderer>();
		if (renderer != null)
		{
			oldShaders.Push(renderer.material.shader);
			renderer.material.shader = shader;
		}
		else
		{
			oldShaders.Push(null);
		}
	}

	/// <summary>
	/// Reverts to the last shader
	/// </summary>
	public void RevertShader()
	{
		if (oldShaders.Count == 0)
			return;
		Renderer renderer = GetComponent<Renderer>();
		if (renderer != null)
			renderer.material.shader = oldShaders.Pop();
	}
}

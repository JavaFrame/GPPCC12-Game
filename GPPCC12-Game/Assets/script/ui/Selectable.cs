using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour {
	public static List<GameObject> SelectGameObjects = new List<GameObject>();

	/// <summary>
	/// If no shader was given to <see cref="SetShader"/> via argument, this shader is used.
	/// </summary>
	[SerializeField] private Shader selectShader;
	[SerializeField] private Shader secundarySelectShader;

	/// <summary>
	/// A stack from all shaders which where overriden by SetShader. The <see cref="RevertShader"/> pops the shaders back from the stack
	/// </summary>
	private Stack<Shader> oldShaders = new Stack<Shader>();

	
	void Awake() {
		SelectGameObjects.Add(this.gameObject);
	}

	void OnDestroy()
	{
		SelectGameObjects.Remove(this.gameObject);
	}

	/// <summary>
	/// Sets the shader of this object and puts it on to the stack
	/// </summary>
	/// <param name="shader">the shader</param>
	/// <param name="secundary">if the secundary selected shader  or the primary selected shader should be used if no shader is provider</param>
	public void SetShader(Shader shader = null, bool secundary = false)
	{
		if (shader == null)
			shader = secundary ? secundarySelectShader : selectShader;
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

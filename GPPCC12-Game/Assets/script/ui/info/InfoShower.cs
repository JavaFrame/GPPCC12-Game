using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoShower : MonoBehaviour
{
	public static InfoShower Instance
	{
		get;
		private set;
	}
	/// <summary>
	/// This delegate is used for changes in the selection of the InfoShower
	/// </summary>
	/// <param name="added">The gameobject which where added to the selection</param>
	/// <param name="removed">The gameobject which where remobed from the selection</param>
	/// <param name="selected">the gameobject which are at the moment selected</param>
	public delegate void ChangeSelection(ReadOnlyCollection<GameObject> added, ReadOnlyCollection<GameObject> removed, ReadOnlyCollection<GameObject> selected);

	public delegate void ChangePosition(Vector3 pos);

	public event ChangePosition ChangePositionEvent;

	public event ChangeSelection ChangeSelectionEvent;

	public event ChangeSelection ChangeSecundarySelectionEvent;

	public delegate void SelectionContains(GameObject go, ReadOnlyCollection<GameObject> selected);

	private Dictionary<GameObject, List<SelectionContains>> _selectionContainsListeners =
		new Dictionary<GameObject, List<SelectionContains>>();

	private Dictionary<GameObject, List<SelectionContains>> _secundarySelectionContainsListeners =
		new Dictionary<GameObject, List<SelectionContains>>();

	/// <summary>
	/// The transform under which the ui elements are spawned
	/// </summary>
	[SerializeField] private Transform uiSpawnParent;

	/// <summary>
	/// A list of all selected objects
	/// </summary>
	private List<GameObject> selectedObjects = new List<GameObject>();

	/// <summary>
	/// A list of all object which where selected by using the right mouse button. 
	/// </summary>
	private List<GameObject> secundarySelectedObjects = new List<GameObject>();

	/// <summary>
	/// If the user doesn't hit an object, this positon will be selected
	/// </summary>
	private Vector3 selectedPosition;

	/// <summary>
	/// The starting point (mouse coordinates/screen coordinates) of the multi selection
	/// </summary>
	private Vector2 selectionStartMousePos;

	/// <summary>
	/// This boolean is true if multi selection is active. Multi selecting is acivated when the user moves the mouse cursor when the LeftMouse Button is pressed down
	/// </summary>
	private bool multipleSelectionActive = false;

	/// <summary>
	/// The selection mode.
	/// </summary>
	private SelectionModeEnum _selectionMode = SelectionModeEnum.Single;

	/// <summary>
	/// The texture for the selection box. It is auto created
	/// </summary>
	private Texture2D selectionBoxTexture;

	/// <summary>
	/// The texture for the selection box for the secundary selection. It is auto created
	/// </summary>
	private Texture2D secundarySelectionBoxTexture;

	/// <summary>
	/// The default cursor texture.
	/// </summary>
	[SerializeField] private Texture2D cursorSelectionTexture;

	/// <summary>
	/// The cursor texture which is used when the user pressed control and changes the selection mode to MultipleAdd
	/// </summary>
	[SerializeField] private Texture2D cursorAddSelectionTexture;

	/// <summary>
	/// The camera of the player. It is used for converting the screen coordinates to world coordinates
	/// </summary>
	[SerializeField] private Camera camera;

	/// <summary>
	/// The last gui which was opend. This is used in the <see cref="HideObjectUi"/>.
	/// If no gui has been opened yet, it is null.
	/// </summary>
	private GameObject lastUi = null;

	/// <summary>
	/// The eventsystem used for deciding if the mouse is over a ui or not. If it is, the ui isn't closed <br/><br/>
	/// 
	/// It is fetched from the GameObject named "EventSystem".
	/// </summary>
	private EventSystem eventSystem;

	[SerializeField] private UiParent defaultObjectUi;

	protected Dictionary<GameObject, List<SelectionContains>> SelectionContainsListeners
	{
		get { return _selectionContainsListeners; }
	}

	protected Dictionary<GameObject, List<SelectionContains>> SecundarySelectionContainsListeners
	{
		get { return _secundarySelectionContainsListeners; }
	}

	/// <summary>
	/// A list of all selected objects
	/// </summary>
	public ReadOnlyCollection<GameObject> SelectedObjects
	{
		get { return selectedObjects.AsReadOnly(); }
	}

	/// <summary>
	/// A list of all object which where selected by using the right mouse button. 
	/// </summary>
	public ReadOnlyCollection<GameObject> SecundarySelectedObjects
	{
		get { return secundarySelectedObjects.AsReadOnly(); }
	}

	public Vector3 SelectedPosition
	{
		get { return selectedPosition; }
	}

	/// <summary>
	/// The selection mode.
	/// </summary>
	public SelectionModeEnum SelectionMode
	{
		get { return _selectionMode; }
		set { _selectionMode = value; }
	}

	void Awake()
	{
		if(Instance != null)
			throw new Exception("There are multiple InfoShowers in the scene active! InfoShwower.Instance was already set by \"" + Instance.gameObject.name + "\"!");
		Instance = this;
	}

	void Start()
	{
		selectionBoxTexture = new Texture2D(1, 1);
		selectionBoxTexture.SetPixel(0, 0, new Color(1, 1, 1, 0.5f));
		selectionBoxTexture.Apply();

		secundarySelectionBoxTexture = new Texture2D(1, 1);
		secundarySelectionBoxTexture.SetPixel(0, 0, new Color(1, 0, 0, 0.5f));
		secundarySelectionBoxTexture.Apply();

		GameObject eventsystemGo = GameObject.Find("EventSystem");
		if (eventsystemGo != null)
		{
			eventSystem = eventsystemGo.GetComponent<EventSystem>();
			if (eventSystem == null)
				Debug.LogWarning("GameObject \"EventSystem\" hasn't a EventSystem Component on it");
		}
		else
			Debug.LogWarning("Couldn't find \"EventSystem\" GameObject!");
	}

	void Update()
	{
		bool mousePressed = Input.GetButton("LeftMouse") || Input.GetButton("RightMouse");
		bool mouseDown = Input.GetButtonDown("LeftMouse") || Input.GetButtonDown("RightMouse");
		bool mouseUp = Input.GetButtonUp("LeftMouse") || Input.GetButtonUp("RightMouse");
		bool leftMouse = Input.GetButton("LeftMouse") || Input.GetButtonDown("LeftMouse") || Input.GetButtonUp("LeftMouse");

		//switches between the modes depending on the controll key
		if (SelectionMode != SelectionModeEnum.MultipleAdd && Input.GetButton("Control"))
			SelectionMode = SelectionModeEnum.MultipleAdd;
		if (SelectionMode == SelectionModeEnum.MultipleAdd && !Input.GetButton("Control"))
			SelectionMode = SelectionModeEnum.MultipleClear;

		//Clears the selection if the selection mode isn't addative
		if (mouseDown && SelectionMode != SelectionModeEnum.MultipleAdd && leftMouse)
		{
			ClearSelection();
		} else if (mouseDown && SelectionMode != SelectionModeEnum.MultipleAdd && !leftMouse)
		{
			ClearSecundarySelection();
		}

		//saves the start position of the mouse for the selection box
		if (mouseDown && !multipleSelectionActive)
			selectionStartMousePos = Input.mousePosition;

		//Checks if the mouse position has changed since the user clicked the LeftMouse Button. If the distance is greater then 0.1 (a threadshold) then multiselecting is activated
		if (mousePressed &&
		    Mathf.Abs(Vector2.Distance(selectionStartMousePos, Input.mousePosition)) > 0.1 && !multipleSelectionActive)
		{
			multipleSelectionActive = true;
			SelectionMode = SelectionModeEnum.MultipleClear; //the default selection mode
		}

		//Updates the cursor texture
		if (SelectionMode == SelectionModeEnum.MultipleAdd)
			Cursor.SetCursor(cursorAddSelectionTexture, Vector2.zero, CursorMode.Auto);
		else
			Cursor.SetCursor(cursorSelectionTexture, Vector2.zero, CursorMode.Auto);


		//In here is the main multi selection logic
		//here is the selecting and the setting of the shader logic 
		if (multipleSelectionActive && mouseUp)
		{
			//precalculates the bounds
			Bounds viewportBounds = GetViewportBounds(camera, selectionStartMousePos, Input.mousePosition);
			var addedGos = new List<GameObject>();
			var removedGos = new List<GameObject>();
			//goes through every object which can be selected (
			foreach (var go in Selectable.SelecGameObjects)
			{
				if (go == null) continue;
				if (viewportBounds.Contains(camera.WorldToViewportPoint(go.transform.position)))
				{

					if (selectedObjects.Contains(go))
					{
						if(leftMouse)
							RemoveObjectFromSelection(go, false);
						else
							RemoveObjectFromSecundarySelection(go, false);
						removedGos.Add(go);
					}
					else
					{
						//If yes the old shader gets saved and the select shader applied
						if(leftMouse)
							AddObjectToSelection(go, false);
						else 
							AddObjectToSecundarySelection(go, false);
						addedGos.Add(go);
					}
				}
			}

			if (ChangeSelectionEvent != null)
			{
				if(leftMouse)
					ChangeSelectionEvent.Invoke(addedGos.AsReadOnly(), removedGos.AsReadOnly(), SelectedObjects);
				else
					ChangeSecundarySelectionEvent.Invoke(addedGos.AsReadOnly(), removedGos.AsReadOnly(), SelectedObjects);
			}
		}


		//hides the last ui canvas
		if (mouseDown && !eventSystem.IsPointerOverGameObject() && leftMouse)
			HideObjectUi();

		//Here is the single selection in
		if (mouseUp && !multipleSelectionActive)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			Debug.DrawRay(ray.origin, ray.direction);
			if (Physics.Raycast(ray, out hit, 100.0f) && !EventSystem.current.IsPointerOverGameObject())
			{
				GameObject hittedGo = hit.collider.gameObject;
				if (hittedGo.GetComponent<Selectable>() != null)
				{
					if (selectedObjects.Contains(hittedGo))
					{
						if(leftMouse)
							RemoveObjectFromSelection(hittedGo);
						else
							RemoveObjectFromSecundarySelection(hittedGo);
					}
					else
					{
						if (leftMouse)
							AddObjectToSelection(hittedGo);
						else
							AddObjectToSecundarySelection(hittedGo);
					}
				}
				else
				{
					if (!leftMouse)
					{
						selectedPosition = hit.point;
						if(ChangePositionEvent != null)
							ChangePositionEvent.Invoke(selectedPosition);
					}
				}
			}
		}

		//disables the multiple selection mode
		if (mouseUp && multipleSelectionActive)
		{
			multipleSelectionActive = false;
			SelectionMode = SelectionModeEnum.Single;
		}
	}

	void OnGUI()
	{
		bool leftMouse = Input.GetButton("LeftMouse") || Input.GetButtonDown("LeftMouse") || Input.GetButtonUp("LeftMouse");
		if (multipleSelectionActive)
		{
			Vector2 start = Vector2.Min(selectionStartMousePos, Input.mousePosition);
			Vector2 stop = Vector2.Max(selectionStartMousePos, Input.mousePosition);
			Vector2 size = new Vector2(start.x - stop.x, start.y - stop.y);
			GUI.DrawTexture(
				new Rect(start.x, Screen.height - start.y, stop.x - start.x,
					-1 * ((Screen.height - start.y) - (Screen.height - stop.y))), leftMouse?selectionBoxTexture:secundarySelectionBoxTexture);
		}
	}
	
	/// <summary>
	/// Adds a listner for an gameobject. If this gameobject is selected, the listner will be invoked
	/// </summary>
	/// <param name="go">the gameobject on which the listener should be registered</param>
	/// <param name="listener">the listener it self</param>
	/// <returns>the SelectionContains delgate which was given to this function</returns>
	public SelectionContains AddSelectionContainsListener(GameObject go, SelectionContains listener)
	{
		if(!SelectionContainsListeners.ContainsKey(go))
			SelectionContainsListeners.Add(go, new List<SelectionContains>());
		SelectionContainsListeners[go].Add(listener);
		return listener;
	}

	public bool RemoveSelectionContainsListner(GameObject go, SelectionContains listener)
	{
		if (!SelectionContainsListeners.ContainsKey(go)) return false;
		return SelectionContainsListeners[go].Remove(listener);
	}

	/// <summary>
	/// Clears the selection and resets the shaders 
	/// </summary>
	public void ClearSelection()
	{
		selectedObjects.ForEach(o =>
		{
			o.GetComponent<Selectable>().RevertShader();
		});
		if (ChangeSelectionEvent != null)
			ChangeSelectionEvent.Invoke(Array.AsReadOnly(new GameObject[] { }), SelectedObjects, new List<GameObject>().AsReadOnly());
		selectedObjects.Clear();
	}

	/// <summary>
	/// Adds an object to the selection, changes the shader and shows and hides the appropriate canvas
	/// </summary>
	/// <param name="go">The Gameobject which is added</param>
	protected void AddObjectToSelection(GameObject go, bool invokeSelectionChangeEvent = true)
	{
		if (selectedObjects.Contains(go)) return;

		Selectable selectable = go.GetComponent<Selectable>();
		if (selectable == null)
			throw new Exception("You tried to add an GameObject which hans't a Selectable component on it: (Gameobject: " + go + ")");
		selectable.SetShader();

		selectedObjects.Add(go);

		if (selectedObjects.Count == 1)
			ShowObjectUi(selectedObjects.First());
		else if (!eventSystem.IsPointerOverGameObject())
			HideObjectUi();
		if (SelectionContainsListeners.ContainsKey(go))
		{
			foreach(var l in SelectionContainsListeners[go])
				l.Invoke(go, selectedObjects.AsReadOnly());
		}
		if(invokeSelectionChangeEvent && ChangeSelectionEvent != null)
			ChangeSelectionEvent.Invoke(Array.AsReadOnly(new GameObject[]{go}), Array.AsReadOnly(new GameObject[] {}), SelectedObjects);
	}

	/// <summary>
	/// Remove an object from the selection and reverts the shader
	/// </summary>
	/// <param name="go"></param>
	/// <param name="invokeSelectionChangeEvent"></param>
	protected void RemoveObjectFromSelection(GameObject go, bool invokeSelectionChangeEvent = true)
	{
		if (selectedObjects.Remove(go))
		{
			go.GetComponent<Selectable>().RevertShader();
			if (invokeSelectionChangeEvent && ChangeSelectionEvent != null)
				ChangeSelectionEvent.Invoke(Array.AsReadOnly(new GameObject[] { }), Array.AsReadOnly(new GameObject[] { go }), SelectedObjects);
		}
	}


	/// <summary>
	/// Adds a listner for an gameobject. If this gameobject is selected, the listner will be invoked
	/// </summary>
	/// <param name="go">the gameobject on which the listener should be registered</param>
	/// <param name="listener">the listener it self</param>
	/// <returns>the SelectionContains delgate which was given to this function</returns>
	public SelectionContains AddSecundarySelectionContainsListener(GameObject go, SelectionContains listener)
	{
		if (!SecundarySelectionContainsListeners.ContainsKey(go))
			SecundarySelectionContainsListeners.Add(go, new List<SelectionContains>());
		SecundarySelectionContainsListeners[go].Add(listener);
		return listener;
	}

	public bool RemoveSecundarySelectionContainsListner(GameObject go, SelectionContains listener)
	{
		if (!SecundarySelectionContainsListeners.ContainsKey(go)) return false;
		return SecundarySelectionContainsListeners[go].Remove(listener);
	}

	/// <summary>
	/// Clears the selection and resets the shaders 
	/// </summary>
	public void ClearSecundarySelection()
	{
		secundarySelectedObjects.ForEach(o =>
		{
			o.GetComponent<Selectable>().RevertShader();
		});
		if (ChangeSecundarySelectionEvent!= null)
			ChangeSecundarySelectionEvent.Invoke(Array.AsReadOnly(new GameObject[] { }), SecundarySelectedObjects, new List<GameObject>().AsReadOnly());
		secundarySelectedObjects.Clear();
	}

	/// <summary>
	/// Adds an object to the selection, changes the shader and shows and hides the appropriate canvas
	/// </summary>
	/// <param name="go">The Gameobject which is added</param>
	protected void AddObjectToSecundarySelection(GameObject go, bool invokeSelectionChangeEvent = true)
	{
		if (secundarySelectedObjects.Contains(go)) return;

		Selectable selectable = go.GetComponent<Selectable>();
		if (selectable == null)
			throw new Exception("You tried to add an GameObject which hans't a Selectable component on it: (Gameobject: " + go + ")");
		selectable.SetShader(secundary: true);

		secundarySelectedObjects.Add(go);

		if (SecundarySelectionContainsListeners.ContainsKey(go))
		{
			foreach (var l in SecundarySelectionContainsListeners[go])
				l.Invoke(go, secundarySelectedObjects.AsReadOnly());
		}
		if (invokeSelectionChangeEvent && ChangeSecundarySelectionEvent != null)
			ChangeSecundarySelectionEvent.Invoke(Array.AsReadOnly(new GameObject[] { go }), Array.AsReadOnly(new GameObject[] { }), SecundarySelectedObjects);
	}

	/// <summary>
	/// Remove an object from the selection and reverts the shader
	/// </summary>
	/// <param name="go"></param>
	/// <param name="invokeSelectionChangeEvent"></param>
	protected void RemoveObjectFromSecundarySelection(GameObject go, bool invokeSelectionChangeEvent = true)
	{
		if (secundarySelectedObjects.Remove(go))
		{
			go.GetComponent<Selectable>().RevertShader();
			if (invokeSelectionChangeEvent && ChangeSecundarySelectionEvent != null)
				ChangeSecundarySelectionEvent.Invoke(Array.AsReadOnly(new GameObject[] { }), Array.AsReadOnly(new GameObject[] { go }), SecundarySelectedObjects);
		}
	}

	/// <summary>
	/// Shows the ui of the given game object
	/// </summary>
	/// <param name="go">the gameobject</param>
	private void ShowObjectUi(GameObject go)
	{
		if (go == null) return;
		UiReferrer referrer = go.GetComponent<UiReferrer>();
		if (referrer != null)
		{
			if (referrer.canvasInstance == null)
			{
				var prefab = referrer.canvasPrefab ?? defaultObjectUi;
				referrer.canvasInstance = Instantiate(prefab.gameObject, uiSpawnParent);
				referrer.canvasInstance.GetComponent<UiParent>().Parent =
					referrer.gameObject; //sets the Parent of the ParentUi object to the object to which it belongs
			}

			referrer.canvasInstance.SetActive(true);
			lastUi = referrer.canvasInstance;
		}
	}

	/// <summary>
	/// Hides the last showed ui
	/// </summary>
	private void HideObjectUi()
	{
		if (lastUi != null)
			lastUi.SetActive(false);
	}

	/// <summary>
	/// Returns the bounds 
	/// </summary>
	/// <param name="camera">the camera</param>
	/// <param name="start">the start point</param>
	/// <param name="stop">the stop point</param>
	/// <returns>the bound which resulted from these parameters</returns>
	private static Bounds GetViewportBounds(Camera camera, Vector3 start, Vector3 stop)
	{
		var v1 = Camera.main.ScreenToViewportPoint(start);
		var v2 = Camera.main.ScreenToViewportPoint(stop);
		var min = Vector3.Min(v1, v2);
		var max = Vector3.Max(v1, v2);
		min.z = camera.nearClipPlane;
		max.z = camera.farClipPlane;

		var bounds = new Bounds();
		bounds.SetMinMax(min, max);
		return bounds;
	}

	/// <summary>
	/// The selection modes
	/// </summary>
	public enum SelectionModeEnum
	{
		Single,
		MultipleClear,
		MultipleAdd
	}
}
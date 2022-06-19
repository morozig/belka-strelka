using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    private UIController uiController;

    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();

        uiController = new UIController(uiDocument.rootVisualElement);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

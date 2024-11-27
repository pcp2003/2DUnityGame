using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class UIHandler : MonoBehaviour
{
    

    private VisualElement m_Healthbar;

    public static UIHandler instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }




    // Start is called before the first frame update
    void Start()
    {
       
    UIDocument uiDocument = GetComponent<UIDocument>();
    if (uiDocument != null)
    {
        Debug.LogError("UI document is empty");
    }

    m_Healthbar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");
    
    if (m_Healthbar != null)
    {
        Debug.LogError("m_Healthbar is empty");
    }
        SetHealthValue(1.0f);
    }


    public void SetHealthValue(float percentage) {
        m_Healthbar.style.width = Length.Percent(percentage * 100.0f);
    }

}

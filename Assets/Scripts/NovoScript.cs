using UnityEngine;
using UnityEngine.UIElements;

public class IncrementKeys : MonoBehaviour
{ 
    // Referência ao Label do UI Builder
    private Label swordpower;
    private Label arrow;
    private Label goldenkey;
    private Label bluekey;
    private Label redKey;
    private int keyCount = 0; // Contador de chaves

    void OnEnable()
    {
        // Obter a referência ao UIDocument
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Procurar o elemento pelo ID
        swordpower = root.Q<Label>("swordpower");
        arrow = root.Q<Label>("arrow");
        goldenkey = root.Q<Label>("goldenkey");
        bluekey = root.Q<Label>("bluekey");
        redKey = root.Q<Label>("redkey");
        
        //teste de codigo
        updateArrow(1);
        updateBlueKey(1);
        updateRedKey(1);
        updateGoldenKey(1);
        updateSwordPower(1);
        
        
    }

    // Métodos para incrementar os obj visuais max até 9
    public void updateSwordPower(int amount)
    {
        if (swordpower != null)
        {
            swordpower.text = $"{amount}";
        }
    }

    public void updateArrow(int amount)
    {
        if (arrow != null)
        {
            arrow.text = $"{amount}";
        }
    }

    public void updateGoldenKey(int amount)
    {
        if (goldenkey != null)
        {
            goldenkey.text = $"{amount}";
        }
    }

    public void updateBlueKey(int amount)
    {
        if (bluekey != null)
        {
            bluekey.text = $"{amount}";
        }
    }

    public void updateRedKey(int amount)
    {
        if (redKey != null)
        {
            redKey.text = $"{amount}";
        }
    }
    
    
    

}
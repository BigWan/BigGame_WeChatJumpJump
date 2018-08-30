using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainUI : UIBase {

    public Button btnStartGame;

    

    private void Awake() {
        cg = GetComponent<CanvasGroup>();
    }

}

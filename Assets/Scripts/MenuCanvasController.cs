using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvasController : MonoBehaviour
{
    public bool lightUpDone = false;

    public void LightUpDone() {
        lightUpDone = !lightUpDone;
    }

    public bool GetLightUpDone() {
        return lightUpDone;
    }
}

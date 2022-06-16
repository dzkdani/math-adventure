using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtendedButtonOnClickPassingVariable : MonoBehaviour
{
    public List<int> _integer;
    public List<string> _string;
    public Button.ButtonClickedEvent _onClick;
}

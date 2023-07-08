using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public interface IDialogTrigger
{
    string DialogName { get; }
    bool Enabled { get; set; }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleController : BaseController {
    public void ChangeSelect()
    {
        //ChangeScene(SceneName.Select);
        ChangeScene(SceneName.Action);
    }
}

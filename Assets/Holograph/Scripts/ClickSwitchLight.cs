﻿// /********************************************************
// *                                                       *
// *   Copyright (C) Microsoft. All rights reserved.       *
// *                                                       *
// ********************************************************/

namespace Holograph
{
    using System;

    using HoloToolkit.Unity.InputModule;

    using UnityEngine;

    public class ClickSwitchLight : MonoBehaviour, IInputHandler
    {
        public GameObject lightSource;

        void IInputHandler.OnInputDown(InputEventData eventData)
        {
            throw new NotImplementedException();
        }

        void IInputHandler.OnInputUp(InputEventData eventData)
        {
            lightSource.SetActive(!lightSource.activeSelf);
        }

    }
}
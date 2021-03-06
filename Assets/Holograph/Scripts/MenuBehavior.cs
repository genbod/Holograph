﻿// /********************************************************
// *                                                       *
// *   Copyright (C) Microsoft. All rights reserved.       *
// *                                                       *
// ********************************************************/

namespace Holograph
{
    using System;
    using System.IO;

    using HoloToolkit.Sharing;

    using UnityEngine;

    /// <summary>
    ///     The menu behavior.
    /// </summary>
    public class MenuBehavior : MonoBehaviour
    {
        /// <summary>
        ///     The enrich panel.
        /// </summary>
        public GameObject EnrichPanel;

        /// <summary>
        ///     The globe.
        /// </summary>
        public GameObject Globe;

        /// <summary>
        ///     The info panel.
        /// </summary>
        public GameObject InfoPanel;

        /// <summary>
        ///     The JSON file asset.
        /// </summary>
        public TextAsset JsonFileAsset;

        /// <summary>
        ///     The mitigate panel.
        /// </summary>
        public GameObject MitigatePanel;

        /// <summary>
        ///     The report panel.
        /// </summary>
        public GameObject ReportPanel;

        /// <summary>
        ///     The stage story manager.
        /// </summary>
        public StoryManager StageStoryManager;

        /// <summary>
        ///     Goes back -- closes panel
        /// </summary>
        public void Back()
        {
            Debug.Log("Back button pushed");
        }

        /// <summary>
        ///     Closes all open Panels
        /// </summary>
        public void CloseAllPanels()
        {
            this.InfoPanel.SetActive(false);
            this.EnrichPanel.SetActive(false);
            this.MitigatePanel.SetActive(false);
        }

        /// <summary>
        ///     Toggles the hexagonal menu
        /// </summary>
        public void TogglesMenu(bool on)
        {
            this.CloseAllPanels();
            gameObject.SetActive(on);
        }

        public void TogglesMenu()
        {
            TogglesMenu(!this.gameObject.activeSelf);
        }

        /// <summary>
        ///     Opens up the Enrich Panel on the hexagonal Menu
        /// </summary>
        public void Enrich()
        {
            this.EnrichPanel.SetActive(!this.EnrichPanel.activeSelf);
            this.StageStoryManager.TriggerStoryWithNetworking(StoryManager.StoryAction.ListInfo, GetComponentInParent<NodeBehavior>().Index);
        }

        /// <summary>
        ///     Expands the graph when icon is hit
        /// </summary>
        public void Expand()
        {
            this.TogglesMenu(false);
            this.StageStoryManager.TriggerStoryWithNetworking(StoryManager.StoryAction.Expand, GetComponentInParent<NodeBehavior>().Index);
        }

        /// <summary>
        ///     Handles the network message from a button click
        /// </summary>
        /// <param name="message">
        ///     The network message.
        /// </param>
        public void HandleMenuButtonClickNetworkMessage(NetworkInMessage message)
        {
            message.ReadInt64();
            int l = message.ReadInt32();
            var methodNameChars = new char[l];
            for (var i = 0; i < l; ++i)
            {
                methodNameChars[i] = Convert.ToChar(message.ReadByte());
            }

            var methodName = new string(methodNameChars);
            this.Invoke(methodName, 0);
        }

        /// <summary>
        ///     Opens up the Info Panel on the hexagonal Menu
        /// </summary>
        public void ListInfo()
        {
            this.InfoPanel.SetActive(!this.InfoPanel.activeSelf);
            this.InfoPanel.GetComponent<InfoPanelBehavior>().UpdateInfo(GetComponentInParent<NodeBehavior>().NodeInfo);
        }

        /// <summary>
        ///     Opens up the Mitigate Panel on the hexagonal Menu
        /// </summary>
        public void Mitigate()
        {
            this.MitigatePanel.SetActive(!this.MitigatePanel.activeSelf);
        }

        /// <summary>
        ///     Resets the application back to the globe stage
        /// </summary>
        public void ResetStory()
        {
            this.StageStoryManager.TriggerStoryWithNetworking(StoryManager.StoryAction.ResetStory);
        }

        /// <summary>
        ///     Called when instantiated but not active
        /// </summary>
        /// <exception cref="FileNotFoundException">
        ///     JSON file was not found
        /// </exception>
        private void Awake()
        {
            if (this.JsonFileAsset == null)
            {
                throw new FileNotFoundException();
            }

            string json = this.JsonFileAsset.text;
            var nodeMenuItems = JsonUtility.FromJson<JNodeMenu>(json).NodeMenuItems;
            for (var i = 0; i < nodeMenuItems.Length; ++i)
            {
                transform.GetChild(i).GetComponent<ButtonBehavior>().initLayout(nodeMenuItems[i]);
            }

            NetworkMessages.Instance.MessageHandlers[NetworkMessages.MessageID.RadialMenuClickIcon] = this.HandleMenuButtonClickNetworkMessage;
        }

        /// <summary>
        ///     The deserialized struct for the JSON File
        /// </summary>
        [Serializable]
        public struct JNodeMenu
        {
            /// <summary>
            ///     The node menu items.
            /// </summary>
            public NodeMenuItem[] NodeMenuItems;

            /// <summary>
            ///     The node menu item.
            /// </summary>
            [Serializable]
            public struct NodeMenuItem
            {
                /// <summary>
                ///     Menu button name
                /// </summary>
                public string Name;

                /// <summary>
                ///     Menu button method
                /// </summary>
                public string MethodName;
            }

        }

    }

}
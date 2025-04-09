using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tabs
{
    public class Tabs : MonoBehaviour
    {
        public readonly HashSet<Tab> TabsList = new();
        
        private Tab _currentTab;
        public event Action<Tab> OnTabChanged;
        public Tab CurrentTab
        {
            get => _currentTab;
            set
            {
                if (_currentTab == value) return;
                _currentTab = value;
                DeselectTabs();
                OnTabChanged?.Invoke(_currentTab);
            }
        }

        private void Reset()
        {
            TabsList.Clear();
            foreach (var tab in GetComponentsInChildren<Tab>()) TabsList.Add(tab);
            Debug.Log(TabsList.Count);
        }

        /// Deselects all but current tab
        private void DeselectTabs()
        {
            foreach (var tab in TabsList) tab.Deselect();
        }
    }
}
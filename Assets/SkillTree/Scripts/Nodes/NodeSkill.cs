﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Nodes {
    public class NodeSkill : INode {
        public List<INode> Children { get; } = new List<INode>();
        public bool IsPurchased { get; private set; }
        public bool IsEnabled { get; private set; }
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public Sprite Graphic { get; set; }
        public string Description { get; set; }
        
        public UnityEvent OnPurchase { get; } = new UnityEvent();
        public UnityEvent OnParentPurchase { get; } = new UnityEvent();
        public UnityEvent OnRefund { get; } = new UnityEvent();
        public UnityEvent OnParentRefund { get; } = new UnityEvent();

        public void AddChild (INode node) {
            Children.Add(node);
        }

        public void Purchase () {
            IsPurchased = true;
            
            foreach (var child in Children) {
                child.ParentPurchased();
            }
            
            OnPurchase.Invoke();
        }
        
        public void ParentPurchased () {
            IsEnabled = true;
            OnParentPurchase.Invoke();
        }

        public void Refund () {
            IsPurchased = false;
            
            foreach (var child in Children) {
                child.ParentRefund();
            }
            
            OnRefund.Invoke();
        }
        
        public void ParentRefund () {
            IsEnabled = false;
            IsPurchased = false;
            
            foreach (var child in Children) {
                child.ParentRefund();
            }
            
            OnParentRefund.Invoke();
        }
    }
}

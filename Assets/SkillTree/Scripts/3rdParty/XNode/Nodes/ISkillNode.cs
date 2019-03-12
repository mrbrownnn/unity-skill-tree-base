using System.Collections.Generic;
using CleverCrow.DungeonsAndHumans.SkillTrees.Nodes;
using UnityEngine;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.ThirdParties.XNodes {
    public interface ISkillNode {
        string Id { get; }
        string DisplayName { get; }
        Sprite Graphic { get; }
        bool Hide { get; }
        bool IsPurchased { get; }
        
        List<ISkillNode> Children { get; }
        string Description { get; }
    }
}

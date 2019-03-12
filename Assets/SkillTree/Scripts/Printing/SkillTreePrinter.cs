using CleverCrow.DungeonsAndHumans.SkillTrees.Nodes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CleverCrow.DungeonsAndHumans.SkillTrees {
    public class SkillTreePrinter : MonoBehaviour {
        [Tooltip("Where nodes will be output in a Canvas")]
        [SerializeField]
        private RectTransform _nodeOutput;

        [SerializeField]
        private SkillPrint _skillPrefab;

        [SerializeField]
        private SkillPrint _abilityPrefab;
        
        [SerializeField] 
        private ContextPrint _context;

        [SerializeField] 
        private Text _pointOutput;

        public void Build (SkillTreeInstance tree) {
            foreach (Transform child in _nodeOutput) {
                Destroy(child.gameObject);
            }

            RecursivePrint(tree.Root, _nodeOutput);
        }

        private void RecursivePrint (INode node, RectTransform parent) {
            foreach (var child in node.Children) {
                var nodePrefab = _skillPrefab;
                if (child.SkillType == SkillType.Ability) nodePrefab = _abilityPrefab;
                
                var skill = Instantiate(nodePrefab, parent);
                skill.Setup(child, node);
                skill.button.onClick.AddListener(() => _context.Open(child));
                RecursivePrint(child, skill.childOutput);
            }
        }

        public void SetPoints (int points) {
            _pointOutput.text = $"Skill Points: {points}";
        }
    }
}

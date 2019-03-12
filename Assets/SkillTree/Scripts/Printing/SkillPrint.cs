using CleverCrow.DungeonsAndHumans.SkillTrees.Nodes;
using UnityEngine;
using UnityEngine.UI;

namespace CleverCrow.DungeonsAndHumans.SkillTrees {
    public class SkillPrint : MonoBehaviour {
        private Color _normalColor;
        private INode _node;
        
        public Text title;
        public Image graphic;
        public Image purchaseGraphic;
        public Button button;
        public LayoutGroup alignment;

        public RectTransform childOutput;
        public RectTransform connectorLeft;
        public RectTransform connectorRight;
        public RectTransform childDivider;

        private void Awake() {
            _normalColor = button.colors.normalColor;
        }

        public void Setup (INode child, INode parent, SkillPrint printParent) {
            _node = child;
            
            title.text = child.DisplayName;
            graphic.sprite = child.Graphic;
            
            connectorLeft.gameObject.SetActive(printParent != null);
            connectorRight.gameObject.SetActive(child.Children.Count > 0);
            childDivider.gameObject.SetActive(child.Children.Count > 1);

            if (!(parent is NodeRoot) && parent.Children.Count > 1) {
                AdjustAlignment(child, parent);
            }
            
            // If single and parent aligned, align self
            if (parent.Children.Count == 1 && printParent != null) {
                InheritAlignment(printParent.alignment.childAlignment);
            }

            RefreshState();

            child.OnPurchase.AddListener(RefreshState);
            child.OnParentPurchase.AddListener(RefreshState);
            child.OnRefund.AddListener(RefreshState);
            child.OnParentRefund.AddListener(RefreshState);
            child.OnDisable.AddListener(RefreshState);
        }

        private void InheritAlignment (TextAnchor newAlignment) {
            alignment.childAlignment = newAlignment;
        }

        private void RefreshState () {
            purchaseGraphic.gameObject.SetActive(_node.IsPurchased);
            
            if (_node.IsEnabled) {
                ButtonEnable();
            } else {
                ButtonDisable();
            }
        }

        // Appease the parent refund hook
        private void RefreshState (bool state) {
            RefreshState();
        }

        private void AdjustAlignment (INode child, INode parent) {
            if (parent.Children[0] == child) {
                alignment.childAlignment = TextAnchor.UpperLeft;
            } else if (parent.Children[parent.Children.Count - 1] == child) {
                alignment.childAlignment = TextAnchor.LowerLeft;
            }
        }

        private void ButtonDisable () {
            var colors = button.colors;
            colors.normalColor = colors.disabledColor;

            button.colors = colors;
        }

        private void ButtonEnable () {
            var colors = button.colors;
            colors.normalColor = _normalColor;

            button.colors = colors;
        }

        private void OnDestroy() {
            _node?.OnPurchase.RemoveListener(RefreshState);
            _node?.OnParentPurchase.RemoveListener(RefreshState);
            _node?.OnRefund.RemoveListener(RefreshState);
            _node?.OnParentRefund.RemoveListener(RefreshState);
            _node?.OnDisable.RemoveListener(RefreshState);
        }
    }
}

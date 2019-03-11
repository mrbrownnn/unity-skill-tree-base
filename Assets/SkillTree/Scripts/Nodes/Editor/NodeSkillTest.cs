using NSubstitute;
using NUnit.Framework;

namespace CleverCrow.DungeonsAndHumans.SkillTrees.Nodes.Editors {
    public class NodeSkillTest {
        private NodeSkill _node;
        
        [SetUp]
        public void BeforeEach () {
            _node = new NodeSkill();
        }
        
        public class AddChildMethod : NodeSkillTest {
            [Test]
            public void It_should_add_a_child_to_Children () {
                var child = Substitute.For<INode>();

                _node.AddChild(child);
                
                Assert.IsTrue(_node.Children.Contains(child));
            }
        }

        public class IsPurchasedProperty : NodeSkillTest {
            [Test]
            public void It_should_return_false_by_default () {
                Assert.IsFalse(_node.IsPurchased);
            }
        }

        public class PurchaseMethod : NodeSkillTest {
            [Test]
            public void It_should_mark_the_node_as_IsPurchased_to_true () {
                _node.Purchase();
                
                Assert.IsTrue(_node.IsPurchased);
            }

            [Test]
            public void It_should_call_ParentPurchased_on_children () {
                var child = Substitute.For<INode>();

                _node.AddChild(child);
                _node.Purchase();

                child.Received(1).ParentPurchased();
            }

            [Test]
            public void It_should_trigger_an_OnPurchase_event () {
                var result = false;
                _node.OnPurchase.AddListener(() => result = true);
                
                _node.Purchase();
                
                Assert.IsTrue(result);
            }
        }

        public class ParentPurchasedMethod : NodeSkillTest {
            [Test]
            public void It_should_mark_the_node_as_IsEnabled () {
                _node.ParentPurchased();
                
                Assert.IsTrue(_node.IsEnabled);
            }

            [Test]
            public void It_should_trigger_OnParentPurchase () {
                var result = false;
                _node.OnParentPurchase.AddListener(() => result = true);
                
                _node.ParentPurchased();
                
                Assert.IsTrue(result);
            }
        }
        
        public class RefundMethod : NodeSkillTest {
            [Test]
            public void It_should_mark_the_node_as_IsPurchased_to_false () {
                _node.Purchase();
                _node.Refund();
                
                Assert.IsFalse(_node.IsPurchased);
            }

            [Test]
            public void It_should_trigger_a_refund_event () {
                var result = false;
                _node.OnRefund.AddListener(() => result = true);

                _node.Purchase();
                _node.Refund();
                
                Assert.IsTrue(result);
            }

            [Test]
            public void It_should_trigger_ParentRefund_on_children () {
                var child = Substitute.For<INode>();

                _node.AddChild(child);
                _node.Purchase();
                _node.Refund();

                child.Received(1).ParentRefund();
            }
        }

        public class ParentRefundMethod : NodeSkillTest {
            [Test]
            public void It_should_clear_IsPurchased () {
                _node.Purchase();
                _node.ParentRefund();
                
                Assert.IsFalse(_node.IsPurchased);
            }
            
            [Test]
            public void It_should_clear_IsEnabled () {
                _node.ParentPurchased();
                _node.ParentRefund();
                
                Assert.IsFalse(_node.IsEnabled);
            }

            [Test]
            public void It_should_trigger_ParentRefund_on_children () {
                var child = Substitute.For<INode>();
                _node.AddChild(child);

                _node.ParentRefund();
                
                child.Received(1).ParentRefund();
            }

            [Test]
            public void It_should_trigger_OnParentRefund () {
                var result = false;
                _node.OnParentRefund.AddListener(() => result = true);
                
                _node.ParentRefund();
                
                Assert.IsTrue(result);
            }
        }
    }
}

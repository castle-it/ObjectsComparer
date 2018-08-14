using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;

namespace ObjectsComparer.Tests.CustomComparers
{
    [TestFixture]
    public class CustomComparerTests
    {
        public class TestCompareClass
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<TestCompareChildClass> Children { get; set; }
        }

        public class TestCompareChildClass
        {
            public int Id { get; set; }
            public int RelatedId { get; set; }
            public int Order { get; set; }
            public int Quantity { get; set; }
            public string Name { get; set; }
        }
        public class CustomCompareLimitedFields : AbstractComparer, IComparerWithCondition
        {
            public CustomCompareLimitedFields(ComparisonSettings settings = null, IComparersFactory factory = null) : base(settings, null, factory)
            {
                AddFieldDifferenceConfiguarions();
            }
            public bool SkipMember(Type type, MemberInfo member)
            {
                return false;
            }

            public new void AddFieldDifferenceConfiguarions()
            {
                AddFieldDifferenceConfiguarion("Name", "The names are different");
            }

            public new ComparisonSettings Settings => new ComparisonSettings()
            {
                ContinueCompareAfterCustomCompare = true
            };

           
           

            /*public IEnumerable<Difference> CalculateDifferences(Type type, object obj1, object obj2)
            {
                var differences = new List<Difference>();

                var testCompare1 = (TestCompareClass)obj1;
                var testCompare2 = (TestCompareClass)obj2;

                if (testCompare1.Name != testCompare2.Name)
                {
                    differences.Add(new Difference("Name", testCompare1.Name, testCompare2.Name, "The name has been modified", DifferenceTypes.ValueMismatch, DifferenceSeverity.Informational));
                }

                return differences;
            }

            public IEnumerable<Difference> CalculateDifferences<T>(T obj1, T obj2)
            {
                var type = typeof(T);
                return CalculateDifferences(type, obj1, obj2);
            }*/

            public bool IsMatch(Type type, object obj1, object obj2)
            {
                return type.Name == "TestCompareClass";
            }

            public bool IsStopComparison(Type type, object obj1, object obj2)
            {
                return obj1 == null || obj2 == null;
            }


            public CustomCompareLimitedFields(ComparisonSettings settings, BaseComparer parentComparer, IComparersFactory factory) : base(settings, parentComparer, factory)
            {
            }

            public override IEnumerable<Difference> CalculateDifferences(Type type, object obj1, object obj2)
            {
                return new List<Difference>();
            }
        }

        public class CustomCompare : IComparerWithCondition
        {
            public IValueComparer DefaultValueComparer => throw new NotImplementedException();
            public Dictionary<string, List<FieldComparisonConfig>> FieldCompareConfiguarations { get; set; }

            public ComparisonSettings Settings => new ComparisonSettings()
            {
                ContinueCompareAfterCustomCompare = true
            };

            public void AddComparerOverride<TProp>(Expression<Func<TProp>> memberLambda, IValueComparer valueComparer)
            {
                throw new NotImplementedException();
            }

            public void AddComparerOverride(MemberInfo memberInfo, IValueComparer valueComparer)
            {
                throw new NotImplementedException();
            }

            public void AddComparerOverride(Type type, IValueComparer valueComparer, Func<MemberInfo, bool> filter = null)
            {
                throw new NotImplementedException();
            }

            public void AddComparerOverride<TType>(IValueComparer valueComparer, Func<MemberInfo, bool> filter = null)
            {
                throw new NotImplementedException();
            }

            public void AddComparerOverride<TProp>(Expression<Func<TProp>> memberLambda, Func<TProp, TProp, ComparisonSettings, bool> compareFunction, Func<TProp, string> toStringFunction)
            {
                throw new NotImplementedException();
            }

            public void AddComparerOverride<TProp>(Expression<Func<TProp>> memberLambda, Func<TProp, TProp, ComparisonSettings, bool> compareFunction)
            {
                throw new NotImplementedException();
            }

            public void AddComparerOverride(string memberName, IValueComparer valueComparer, Func<MemberInfo, bool> filter = null)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<Difference> CalculateDifferences(Type type, object obj1, object obj2)
            {
                var differences = new List<Difference>();

                var testCompare1 = (TestCompareClass) obj1;
                var testCompare2 = (TestCompareClass) obj2;

                if (testCompare1.Name != testCompare2.Name)
                {
                    differences.Add(new Difference("Name", testCompare1.Name, testCompare2.Name, "The name has been modified", DifferenceTypes.ValueMismatch, DifferenceSeverity.Informational));
                }
                
                return differences;
            }

            public IEnumerable<Difference> CalculateDifferences<T>(T obj1, T obj2)
            {
                var type = typeof(T);
                return CalculateDifferences(type, obj1, obj2);
            }

            public bool Compare(Type type, object obj1, object obj2, out IEnumerable<Difference> differences)
            {
                throw new NotImplementedException();
            }

            public bool Compare<T>(T obj1, T obj2, out IEnumerable<Difference> differences)
            {
                throw new NotImplementedException();
            }

            public bool Compare(Type type, object obj1, object obj2)
            {
                throw new NotImplementedException();
            }

            public bool Compare<T>(T obj1, T obj2)
            {
                throw new NotImplementedException();
            }

            public bool IsMatch(Type type, object obj1, object obj2)
            {
                return obj1.GetType() == obj2.GetType() && obj1.GetType() == type;
            }

            public bool IsStopComparison(Type type, object obj1, object obj2)
            {
                return Settings.EmptyAndNullEnumerablesEqual && (obj1 == null || obj2 == null);
            }

            public void SetDefaultComparer(IValueComparer valueComparer)
            {
                throw new NotImplementedException();
            }

            public bool SkipMember(Type type, MemberInfo member)
            {
                if (member.Name == "Name")
                {
                    return true;
                }
                return false;//should skip?
            }
        }

        public class CustomCompareEnumeration : BaseComparer, IComparerWithCondition
        {
            public CustomCompareEnumeration() : base(null, null, null)
            {

            }

            public CustomCompareEnumeration(ComparisonSettings settings, BaseComparer comparer, IComparersFactory factory) : base(settings, comparer, factory)
            {

            }
            

            public IEnumerable<Difference> CalculateDifferences(Type type, object obj1, object obj2)
            {
                var differences = new List<Difference>();

                var testCompare1 = (TestCompareClass)obj1;
                var testCompare2 = (TestCompareClass)obj2;

                if (testCompare1.Name != testCompare2.Name)
                {
                    differences.Add(new Difference("Name", testCompare1.Name, testCompare2.Name, "The name has been modified", DifferenceTypes.ValueMismatch, DifferenceSeverity.Informational));
                }

                return differences;
            }

            public IEnumerable<Difference> CalculateDifferences<T>(T obj1, T obj2)
            {
                var type = typeof(T);
                return CalculateDifferences(type, obj1, obj2);
            }

            public bool Compare(Type type, object obj1, object obj2, out IEnumerable<Difference> differences)
            {
                throw new NotImplementedException();
            }

            public bool Compare<T>(T obj1, T obj2, out IEnumerable<Difference> differences)
            {
                throw new NotImplementedException();
            }

            public bool Compare(Type type, object obj1, object obj2)
            {
                throw new NotImplementedException();
            }

            public bool Compare<T>(T obj1, T obj2)
            {
                throw new NotImplementedException();
            }

            public bool IsMatch(Type type, object obj1, object obj2)
            {
                return type == typeof(TestCompareClass) && obj1.GetType() == obj2.GetType() && obj1.GetType() == type;
            }

            public bool IsStopComparison(Type type, object obj1, object obj2)
            {
                return Settings.EmptyAndNullEnumerablesEqual && (obj1 == null || obj2 == null);
            }

            public void SetDefaultComparer(IValueComparer valueComparer)
            {
                throw new NotImplementedException();
            }

            public bool SkipMember(Type type, MemberInfo member)
            {
                if (member.Name == "Name")
                {
                    return true;
                }
                return false;//should skip?
            }
        }

        /*[SetUp]
        public void StartTesting()
        {
            System.Diagnostics.Debugger.Launch();
        }*/

        [Test]
        public void CustomObjectComparer()
        {

            var a1 = new TestCompareClass {Id = 1, Name = "Bob", Children = new List<TestCompareChildClass>()
            {
                new TestCompareChildClass(){Id = 1, Quantity = 5}
            }};
            var a2 = new TestCompareClass { Id = 2, Name = "Steve",
                Children = new List<TestCompareChildClass>()
                {
                    new TestCompareChildClass(){Id = 1, Quantity = 10}
                }
            };

            var comparer = new Comparer<TestCompareClass>();
            comparer.AddComparer(new CustomCompare());

            var differences = comparer.CalculateDifferences(a1, a2).ToList();
            
            Assert.AreEqual(3, differences.Count);

            Assert.AreEqual(DifferenceTypes.ValueMismatch, differences[0].DifferenceType);
            Assert.AreEqual(DifferenceSeverity.Informational, differences[0].DifferenceSeverity);
            Assert.AreEqual(a1.Name, differences[0].Value1);
            Assert.AreEqual(a2.Name, differences[0].Value2);
            Assert.AreEqual("The name has been modified", differences[0].DifferenceDescription);
            Assert.AreEqual("Name", differences[0].MemberPath);

            Assert.AreEqual(DifferenceTypes.ValueMismatch, differences[1].DifferenceType);
            Assert.AreEqual(DifferenceSeverity.Informational, differences[1].DifferenceSeverity);
            Assert.AreEqual(a1.Id.ToString(), differences[1].Value1);
            Assert.AreEqual(a2.Id.ToString(), differences[1].Value2);
            Assert.AreEqual("Id", differences[1].MemberPath);

            Assert.AreEqual(DifferenceTypes.ValueMismatch, differences[2].DifferenceType);
            Assert.AreEqual(DifferenceSeverity.Informational, differences[2].DifferenceSeverity);
            Assert.AreEqual(a1.Children[0].Quantity.ToString(), differences[2].Value1);
            Assert.AreEqual(a2.Children[0].Quantity.ToString(), differences[2].Value2);
            Assert.AreEqual("Children[0].Quantity", differences[2].MemberPath);
        }

        public class ChildObjectKeyComparison : IObjectKeyComparison
        {
            public Type Type => typeof(TestCompareChildClass);

            public object FindMatchingItemInList(object item,
                object itemList)
            {
                var childObject = (TestCompareChildClass) item;
                var childObjectList = (List<TestCompareChildClass>) itemList;

                return childObjectList.FirstOrDefault(co => co.Id == childObject.Id);
            }

            public object FindIndexInList(object item,
                object itemList)
            {
                var childObject = (TestCompareChildClass)item;
                return childObject.Order;
            }
        }

        [Test]
        public void ReorderChildrenNoSetting()
        {

            var a1 = new TestCompareClass
            {
                Id = 1,
                Name = "Bob",
                Children = new List<TestCompareChildClass>()
            {
                new TestCompareChildClass(){Id = 1, Quantity = 5},
                new TestCompareChildClass(){Id = 2, Quantity = 5}
            }
            };
            var a2 = new TestCompareClass
            {
                Id = 1,
                Name = "Bob",
                Children = new List<TestCompareChildClass>()
                {
                    new TestCompareChildClass(){Id = 2, Quantity = 5},
                    new TestCompareChildClass(){Id = 1, Quantity = 5}
                }
            };

            var comparisonSettings = new ComparisonSettings()
            {
                EnumerationsWithOrderComparison = true,
                ObjectKeyComparisons = new List<IObjectKeyComparison>()
                {
                    new ChildObjectKeyComparison()
                }
            };

            var comparer = new Comparer<TestCompareClass>(comparisonSettings);
            comparer.AddComparer(new CustomCompare());

            var differences = comparer.CalculateDifferences(a1, a2).ToList();

            Assert.AreEqual(0, differences.Count);

        }

        [Test]
        public void ReorderChildren()
        {

            var a1 = new TestCompareClass
            {
                Id = 1,
                Name = "Bob",
                Children = new List<TestCompareChildClass>()
            {
                new TestCompareChildClass(){Id = 1, Quantity = 5},
                new TestCompareChildClass(){Id = 2, Quantity = 5}
            }
            };
            var a2 = new TestCompareClass
            {
                Id = 1,
                Name = "Bob",
                Children = new List<TestCompareChildClass>()
                {
                    new TestCompareChildClass(){Id = 2, Quantity = 5},
                    new TestCompareChildClass(){Id = 1, Quantity = 5}
                }
            };

            var comparisonSettings = new ComparisonSettings()
            {
                EnumerationsWithOrderComparison = true,
                EnumerationsWithOrderComparisonIncludeIndexChanges = true,
                ObjectKeyComparisons = new List<IObjectKeyComparison>()
                {
                    new ChildObjectKeyComparison()
                }
            };

            var comparer = new Comparer<TestCompareClass>(comparisonSettings);
            comparer.AddComparer(new CustomCompare());

            var differences = comparer.CalculateDifferences(a1, a2).ToList();

            Assert.AreEqual(1, differences.Count);

            Assert.AreEqual(DifferenceTypes.ValueMismatch, differences[0].DifferenceType);
            Assert.AreEqual(DifferenceSeverity.Informational, differences[0].DifferenceSeverity);
            Assert.AreEqual(2, differences[0].Differences.Count);
            Assert.AreEqual("Children", differences[0].MemberPath);

            var childDifferences = differences[0].Differences;
            Assert.AreEqual(DifferenceTypes.ItemMovedInList, childDifferences[0].DifferenceType);
            Assert.AreEqual(DifferenceSeverity.Informational, childDifferences[0].DifferenceSeverity);
            Assert.AreEqual(0, childDifferences[0].Index1);
            Assert.AreEqual(1, childDifferences[0].Index2);
            Assert.AreEqual("0", childDifferences[0].MemberPath);

            Assert.AreEqual(DifferenceTypes.ItemMovedInList, childDifferences[1].DifferenceType);
            Assert.AreEqual(DifferenceSeverity.Informational, childDifferences[1].DifferenceSeverity);
            Assert.AreEqual(1, childDifferences[1].Index1);
            Assert.AreEqual(0, childDifferences[1].Index2);
            Assert.AreEqual("1", childDifferences[1].MemberPath);

        }

        [Test]
        public void ReorderChildren_WithChanges()
        {

            var a1 = new TestCompareClass
            {
                Id = 1,
                Name = "Bob",
                Children = new List<TestCompareChildClass>()
            {
                new TestCompareChildClass(){Id = 1, Quantity = 5},
                new TestCompareChildClass(){Id = 2, Quantity = 5}
            }
            };
            var a2 = new TestCompareClass
            {
                Id = 1,
                Name = "Bob",
                Children = new List<TestCompareChildClass>()
                {
                    new TestCompareChildClass(){Id = 2, Quantity = 5},
                    new TestCompareChildClass(){Id = 1, Quantity = 3}
                }
            };

            var comparisonSettings = new ComparisonSettings()
            {
                EnumerationsWithOrderComparison = true,
                EnumerationsWithOrderComparisonIncludeIndexChanges = true,
                ObjectKeyComparisons = new List<IObjectKeyComparison>()
                {
                    new ChildObjectKeyComparison()
                }
            };

            var comparer = new Comparer<TestCompareClass>(comparisonSettings);
            comparer.AddComparer(new CustomCompare());

            var differences = comparer.CalculateDifferences(a1, a2).ToList();

            Assert.AreEqual(1, differences.Count);

            Assert.AreEqual(DifferenceTypes.ValueMismatch, differences[0].DifferenceType);
            Assert.AreEqual(DifferenceSeverity.Informational, differences[0].DifferenceSeverity);
            Assert.AreEqual("Children", differences[0].MemberPath);

            var childDifferences = differences[0].Differences;
            Assert.AreEqual(2, childDifferences.Count);

            Assert.AreEqual(DifferenceTypes.ItemMovedInList, childDifferences[0].DifferenceType);
            Assert.AreEqual(DifferenceSeverity.Informational, childDifferences[0].DifferenceSeverity);
            Assert.AreEqual(0, childDifferences[0].Index1);
            Assert.AreEqual(1, childDifferences[0].Index2);
            Assert.AreEqual("0", childDifferences[0].MemberPath);

            //child differences
            Assert.AreEqual(1, childDifferences[0].Differences.Count);
            Assert.AreEqual(DifferenceTypes.ValueMismatch, childDifferences[0].Differences[0].DifferenceType);
            Assert.AreEqual(DifferenceSeverity.Informational, childDifferences[0].Differences[0].DifferenceSeverity);
            Assert.AreEqual(5.ToString(), childDifferences[0].Differences[0].Value1);
            Assert.AreEqual(3.ToString(), childDifferences[0].Differences[0].Value2);
            Assert.AreEqual("Quantity", childDifferences[0].Differences[0].MemberPath);

            Assert.AreEqual(0, childDifferences[1].Differences.Count);
            Assert.AreEqual(DifferenceTypes.ItemMovedInList, childDifferences[1].DifferenceType);
            Assert.AreEqual(DifferenceSeverity.Informational, childDifferences[1].DifferenceSeverity);
            Assert.AreEqual(1, childDifferences[1].Index1);
            Assert.AreEqual(0, childDifferences[1].Index2);
            Assert.AreEqual("1", childDifferences[1].MemberPath);

        }

        [Test]
        public void AddChildren()
        {

            var a1 = new TestCompareClass
            {
                Id = 1,
                Name = "Bob",
                Children = new List<TestCompareChildClass>()
            {
                new TestCompareChildClass(){Id = 1, Quantity = 5}
            }
            };
            var a2 = new TestCompareClass
            {
                Id = 1,
                Name = "Bob",
                Children = new List<TestCompareChildClass>()
                {
                    new TestCompareChildClass(){Id = 1, Quantity = 5},
                    new TestCompareChildClass(){Id = 2, Quantity = 5},
                }
            };

            var comparisonSettings = new ComparisonSettings()
            {
                EnumerationsWithOrderComparison = true,
                ObjectKeyComparisons = new List<IObjectKeyComparison>()
                {
                    new ChildObjectKeyComparison()
                }
            };

            var comparer = new Comparer<TestCompareClass>(comparisonSettings);
            comparer.AddComparer(new CustomCompare());

            var differences = comparer.CalculateDifferences(a1, a2).ToList();

            Assert.AreEqual(1, differences.Count);

            Assert.AreEqual(DifferenceTypes.ValueMismatch, differences[0].DifferenceType);
            Assert.AreEqual(DifferenceSeverity.Informational, differences[0].DifferenceSeverity);
            Assert.AreEqual("Children", differences[0].MemberPath);

            var childrenDifferences = differences[0].Differences;

            Assert.AreEqual(1, childrenDifferences.Count);
            Assert.AreEqual(DifferenceTypes.ItemAddedToList, childrenDifferences[0].DifferenceType);
            Assert.AreEqual(DifferenceSeverity.Informational, childrenDifferences[0].DifferenceSeverity);
            Assert.AreEqual(null, childrenDifferences[0].Value1);
            Assert.AreEqual(JsonConvert.SerializeObject(a2.Children[1]), childrenDifferences[0].Value2);
            Assert.AreEqual("1", childrenDifferences[0].MemberPath);

        }

        [Test]
        public void RemoveChildren()
        {

            var a1 = new TestCompareClass
            {
                Id = 1,
                Name = "Bob",
                Children = new List<TestCompareChildClass>()
            {
                new TestCompareChildClass(){Id = 1, Quantity = 5},
                new TestCompareChildClass(){Id = 2, Quantity = 5}
            }
            };
            var a2 = new TestCompareClass
            {
                Id = 1,
                Name = "Bob",
                Children = new List<TestCompareChildClass>()
                {
                    new TestCompareChildClass(){Id = 1, Quantity = 5}
                }
            };

            var comparisonSettings = new ComparisonSettings()
            {
                EnumerationsWithOrderComparison = true,
                ObjectKeyComparisons = new List<IObjectKeyComparison>()
                {
                    new ChildObjectKeyComparison()
                }
            };

            var comparer = new Comparer<TestCompareClass>(comparisonSettings);
            comparer.AddComparer(new CustomCompare());

            var differences = comparer.CalculateDifferences(a1, a2).ToList();

            Assert.AreEqual(1, differences.Count);

            Assert.AreEqual(DifferenceTypes.ValueMismatch, differences[0].DifferenceType);
            Assert.AreEqual(DifferenceSeverity.Informational, differences[0].DifferenceSeverity);
            Assert.AreEqual("Children", differences[0].MemberPath);

            var childrenDifferences = differences[0].Differences;

            Assert.AreEqual(1, childrenDifferences.Count);
            Assert.AreEqual(DifferenceTypes.ItemRemovedFromList, childrenDifferences[0].DifferenceType);
            Assert.AreEqual(DifferenceSeverity.Informational, childrenDifferences[0].DifferenceSeverity);
            Assert.AreEqual(JsonConvert.SerializeObject(a1.Children[1]), childrenDifferences[0].Value1);
            Assert.AreEqual(null, childrenDifferences[0].Value2);
            Assert.AreEqual("1", childrenDifferences[0].MemberPath);

        }

        [Test]
        public void ChildrenUpdatesWithNoOrderChanges()
        {

            var a1 = new TestCompareClass
            {
                Id = 1,
                Name = "Bob",
                Children = new List<TestCompareChildClass>()
            {
                new TestCompareChildClass(){Id = 1, Quantity = 5},
                new TestCompareChildClass(){Id = 2, Quantity = 5}
            }
            };
            var a2 = new TestCompareClass
            {
                Id = 1,
                Name = "Bob",
                Children = new List<TestCompareChildClass>()
                {
                    new TestCompareChildClass(){Id = 1, Quantity = 5},
                    new TestCompareChildClass(){Id = 2, Quantity = 3}
                }
            };

            var comparisonSettings = new ComparisonSettings()
            {
                EnumerationsWithOrderComparison = true,
                ObjectKeyComparisons = new List<IObjectKeyComparison>()
                {
                    new ChildObjectKeyComparison()
                }
            };

            var comparer = new Comparer<TestCompareClass>(comparisonSettings);
            comparer.AddComparer(new CustomCompare());

            var differences = comparer.CalculateDifferences(a1, a2).ToList();

            Assert.AreEqual(1, differences.Count);

            Assert.AreEqual(DifferenceTypes.ValueMismatch, differences[0].DifferenceType);
            Assert.AreEqual(DifferenceSeverity.Informational, differences[0].DifferenceSeverity);
            Assert.AreEqual("Children", differences[0].MemberPath);

            var childrenDifferences = differences[0].Differences;

            Assert.AreEqual(1, childrenDifferences.Count);
            Assert.AreEqual(DifferenceTypes.ValueMismatch, childrenDifferences[0].DifferenceType);
            Assert.AreEqual(DifferenceSeverity.Informational, childrenDifferences[0].DifferenceSeverity);
            Assert.AreEqual("1", childrenDifferences[0].MemberPath);

            //child differences
            Assert.AreEqual(1, childrenDifferences[0].Differences.Count);
            Assert.AreEqual(DifferenceTypes.ValueMismatch, childrenDifferences[0].Differences[0].DifferenceType);
            Assert.AreEqual(DifferenceSeverity.Informational, childrenDifferences[0].Differences[0].DifferenceSeverity);
            Assert.AreEqual(5.ToString(), childrenDifferences[0].Differences[0].Value1);
            Assert.AreEqual(3.ToString(), childrenDifferences[0].Differences[0].Value2);
            Assert.AreEqual("Quantity", childrenDifferences[0].Differences[0].MemberPath);



        }

        [Test]
        public void LimitedFieldCompares()
        {

            var a1 = new TestCompareClass
            {
                Id = 1,
                Name = "Bob",
                Children = new List<TestCompareChildClass>()
            {
                new TestCompareChildClass(){Id = 1, Quantity = 5},
                new TestCompareChildClass(){Id = 2, Quantity = 5}
            }
            };
            var a2 = new TestCompareClass
            {
                Id = 2,
                Name = "Bobs Burger",
                Children = new List<TestCompareChildClass>()
                {
                    new TestCompareChildClass(){Id = 2, Quantity = 5},
                    new TestCompareChildClass(){Id = 1, Quantity = 3}
                }
            };

            var comparisonSettings = new ComparisonSettings()
            {
            };

            var comparer = new Comparer<TestCompareClass>(comparisonSettings);
            comparer.AddComparer(new CustomCompareLimitedFields(comparisonSettings));

            var differences = comparer.CalculateDifferences(a1, a2).ToList();

            Assert.AreEqual(1, differences.Count);

            Assert.AreEqual(DifferenceTypes.ValueMismatch, differences[0].DifferenceType);
            Assert.AreEqual(DifferenceSeverity.Informational, differences[0].DifferenceSeverity);
            Assert.AreEqual("Name", differences[0].MemberPath);
            Assert.AreEqual(a1.Name, differences[0].Value1);
            Assert.AreEqual(a2.Name, differences[0].Value2);
            Assert.AreEqual("The names are different", differences[0].DifferenceDescription);
           
        }

        /*[Test]
        public void ReorderWithCustomIndex()
        {

            var a1 = new TestCompareClass
            {
                Id = 1,
                Name = "Bob",
                Children = new List<TestCompareChildClass>()
            {
                new TestCompareChildClass(){Id = 1, Order = 2, Quantity = 5},
                new TestCompareChildClass(){Id = 2, Order = 1, Quantity = 5}
            }
            };
            var a2 = new TestCompareClass
            {
                Id = 1,
                Name = "Bob",
                Children = new List<TestCompareChildClass>()
                {
                    new TestCompareChildClass(){Id = 1, Order = 1, Quantity = 5},
                    new TestCompareChildClass(){Id = 2, Order = 2, Quantity = 5}
                }
            };

            var comparisonSettings = new ComparisonSettings()
            {
                EnumerationsWithOrderComparison = true,
                ObjectKeyComparisons = new List<IObjectKeyComparison>()
                {
                    new ChildObjectKeyComparison()
                }
            };

            var comparer = new Comparer<TestCompareClass>(comparisonSettings);
            comparer.AddComparer(new CustomCompare());

            var differences = comparer.CalculateDifferences(a1, a2).ToList();

            Assert.AreEqual(1, differences.Count);

            Assert.AreEqual(DifferenceTypes.ValueMismatch, differences[0].DifferenceType);
            Assert.AreEqual(DifferenceSeverity.Informational, differences[0].DifferenceSeverity);
            Assert.AreEqual("Children", differences[0].MemberPath);

            var childrenDifferences = differences[0].Differences;

            Assert.AreEqual(1, childrenDifferences.Count);
            Assert.AreEqual(DifferenceTypes.ValueMismatch, childrenDifferences[0].DifferenceType);
            Assert.AreEqual(DifferenceSeverity.Informational, childrenDifferences[0].DifferenceSeverity);
            Assert.AreEqual("1", childrenDifferences[0].MemberPath);

            //child differences
            Assert.AreEqual(1, childrenDifferences[0].Differences.Count);
            Assert.AreEqual(DifferenceTypes.ValueMismatch, childrenDifferences[0].Differences[0].DifferenceType);
            Assert.AreEqual(DifferenceSeverity.Informational, childrenDifferences[0].Differences[0].DifferenceSeverity);
            Assert.AreEqual(5.ToString(), childrenDifferences[0].Differences[0].Value1);
            Assert.AreEqual(3.ToString(), childrenDifferences[0].Differences[0].Value2);
            Assert.AreEqual("Quantity", childrenDifferences[0].Differences[0].MemberPath);



        }*/

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using ObjectsComparer.Utils;

namespace ObjectsComparer
{
    internal class EnumerablesComparer<T> : AbstractComparer
    {
        private readonly Comparer<T> _comparer;

        public EnumerablesComparer(ComparisonSettings settings, BaseComparer parentComparer, IComparersFactory factory)
            :base(settings, parentComparer, factory)
        {
            _comparer = new Comparer<T>(Settings, this);
        }

        public override IEnumerable<Difference> CalculateDifferences(Type type, object obj1, object obj2)
        {
            if (!type.InheritsFrom(typeof(IEnumerable<>)))
            {
                throw new ArgumentException("Invalid type");
            }

            if (!Settings.EmptyAndNullEnumerablesEqual &&
                (obj1 == null || obj2 == null) && obj1 != obj2)
            {
                yield break;
            }

            obj1 = obj1 ?? Enumerable.Empty<T>();
            obj2 = obj2 ?? Enumerable.Empty<T>();

            if (!obj1.GetType().InheritsFrom(typeof(IEnumerable<T>)))
            {
                throw new ArgumentException(nameof(obj1));
            }

            if (!obj2.GetType().InheritsFrom(typeof(IEnumerable<T>)))
            {
                throw new ArgumentException(nameof(obj2));
            }

            var list1 = ((IEnumerable<T>)obj1).ToList();
            var list2 = ((IEnumerable<T>)obj2).ToList();
            var keyComparison = Settings.ObjectKeyComparisons.FirstOrDefault(okc => okc.Type == type.GetGenericArguments().Single());
            Difference listDifference = null;
            

                if (list1.Count != list2.Count)
            {
                if (!type.InheritsFrom(typeof(ICollection<>)) && !type.GetTypeInfo().IsArray)
                {
                    if (!Settings.EnumerationsWithOrderComparison || keyComparison == null)
                    {
                        yield return new Difference("", list1.Count.ToString(), list2.Count.ToString(),
                            DifferenceTypes.NumberOfElementsMismatch);
                        yield break;
                    }
                }

                
            }

            listDifference = new Difference("", list1.Count.ToString(), list2.Count.ToString(),
                    DifferenceTypes.ValueMismatch);
            

            if (Settings.EnumerationsWithOrderComparison && keyComparison != null)
            {
                //foreach of the first list, check for removed / differences
                for (int i = 0; i < list1.Count; i++)
                {
                    var itemInOriginalList = list1[i];
                    //find if removed
                    var matchedItem = keyComparison.FindMatchingItemInList(itemInOriginalList, list2);
                    if (matchedItem == null)
                    {
                        var difference = new Difference(i.ToString(), JsonConvert.SerializeObject(itemInOriginalList), null,
                            DifferenceTypes.ItemRemovedFromList);
                        listDifference.Differences.Add(difference);
                        continue;//move on to the next item this is marked for being removed
                        //yield return difference;
                    }

                    Difference parentDifference = null;
                    //check if index moved
                    var indexFound = list2.IndexOf((T)matchedItem);

                    if (i == indexFound || !Settings.EnumerationsWithOrderComparisonIncludeIndexChanges)
                    {
                        parentDifference = new Difference(i.ToString(), null, null);
                    }
                    else
                    {
                        parentDifference = new Difference(i.ToString(), null, null,
                            DifferenceTypes.ItemMovedInList);
                        parentDifference.Index1 = i;
                        parentDifference.Index2 = indexFound;
                    }

                    //we found the item, so we want to get the differences
                    
                    parentDifference.Differences = new List<Difference>();
                    foreach (var difference in _comparer.CalculateDifferences(itemInOriginalList, (T)matchedItem))
                    {
                         parentDifference.Differences.Add(difference);
                    }

                    if (parentDifference.Differences.Count > 0 || parentDifference.DifferenceType == DifferenceTypes.ItemMovedInList)
                    {
                        //yield return parentDifference;//return the overral difference if one exists
                        listDifference.Differences.Add(parentDifference);
                    }
                    
                }

                //foreach second list check for just added
                for (int i = 0; i < list2.Count; i++)
                {
                    var potentialyNewItem = list2[i];
                    //find if removed
                    var matchedItem = keyComparison.FindMatchingItemInList(potentialyNewItem, list1);
                    if (matchedItem == null)
                    {
                        var difference = new Difference(i.ToString(), null, JsonConvert.SerializeObject(potentialyNewItem),
                            DifferenceTypes.ItemAddedToList);
                        listDifference.Differences.Add(difference);
                       
                    }
                }

                if (listDifference.Differences.Count > 0)
                {
                    yield return listDifference;
                }
                yield break;
            }
            
            //traditional index based differences
            for (int i = 0; i < list2.Count; i++)
            {
                foreach (var difference in _comparer.CalculateDifferences(list1[i], list2[i]))
                {
                    yield return difference.InsertPath($"[{i}]");
                }
            }
                
            
            
        }
    }
}

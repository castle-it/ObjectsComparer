using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using ObjectsComparer.Utils;

namespace ObjectsComparer
{
    /// <summary>
    /// Provides base implementation to configure comparer.
    /// </summary>
    public abstract class BaseComparer: IBaseComparer
    {
        /// <summary>
        /// Comparison Settings.
        /// </summary>
        public ComparisonSettings Settings { get; }
        public List<IComparerWithCondition> _conditionalComparers;

        /// <summary>
        /// Default <see cref="IValueComparer"/>
        /// </summary>
        public IValueComparer DefaultValueComparer { get; private set; }

        public Dictionary<string, List<FieldComparisonConfig>> FieldCompareConfiguarations { get; set; }

        protected IComparersFactory Factory { get; }

        internal ComparerOverridesCollection OverridesCollection { get;  } = new ComparerOverridesCollection();
        
        protected BaseComparer(ComparisonSettings settings, BaseComparer parentComparer, IComparersFactory factory)
        {
            Factory = factory ?? new ComparersFactory();
            Settings = settings ?? new ComparisonSettings();
            DefaultValueComparer = new DefaultValueComparer();
            FieldCompareConfiguarations = new Dictionary<string, List<FieldComparisonConfig>>();

            if (parentComparer != null)
            {
                DefaultValueComparer = parentComparer.DefaultValueComparer;
                OverridesCollection.Merge(parentComparer.OverridesCollection);
                _conditionalComparers = parentComparer._conditionalComparers;
            }

            AddFieldDifferenceConfiguarions();
        }

        /// <summary>
        /// Adds Comparer Override by Type.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="valueComparer">Value Comparer.</param>
        /// <param name="filter">Value Comparer will be used only if filter(memberInfo) == true. Null by default.</param>
        public void AddComparerOverride(Type type, IValueComparer valueComparer, Func<MemberInfo, bool> filter = null)
        {
            OverridesCollection.AddComparer(type, valueComparer, filter);
        }

        public virtual void AddFieldDifferenceConfiguarions()
        {
            //override this to implement
        }

        public void AddFieldDifferenceConfiguarion(string fieldName, string message = null, DifferenceTypes differenceType = DifferenceTypes.ValueMismatch, DifferenceSeverity severity = DifferenceSeverity.Informational)
        {
            if (!FieldCompareConfiguarations.ContainsKey(fieldName.ToLower()))
            {
                FieldCompareConfiguarations.Add(fieldName.ToLower(), new List<FieldComparisonConfig>());
            }

            FieldCompareConfiguarations[fieldName.ToLower()].Add(new FieldComparisonConfig()
            {
                FieldPath = fieldName,
                DifferenceSeverity = severity,
                DifferenceType = differenceType,
                Message = message
            });
        }

        /// <summary>
        /// Adds Comparer Override by Type.
        /// </summary>
        /// <typeparam name="TType">Type.</typeparam>
        /// <param name="valueComparer">Value Comparer.</param>
        /// <param name="filter">Value Comparer will be used only if filter(memberInfo) == true. Null by default.</param>
        public void AddComparerOverride<TType>(IValueComparer valueComparer, Func<MemberInfo, bool> filter = null)
        {
            AddComparerOverride(typeof(TType), valueComparer, filter);
        }

        /// <summary>
        /// Adds Comparer Override by Member.
        /// </summary>
        /// <typeparam name="TProp">Type of the member.</typeparam>
        /// <param name="memberLambda">Lambda to get member.</param>
        /// <param name="valueComparer">Value Comparer.</param>
        public void AddComparerOverride<TProp>(Expression<Func<TProp>> memberLambda, IValueComparer valueComparer)
        {
            OverridesCollection.AddComparer(PropertyHelper.GetMemberInfo(memberLambda), valueComparer);
        }

        /// <summary>
        /// Adds Comparer Override by Member.
        /// </summary>
        /// <typeparam name="TProp">Type of the member.</typeparam>
        /// <param name="memberLambda">Lambda to get member.</param>
        /// <param name="compareFunction">Function to compare objects.</param>
        /// <param name="toStringFunction">Function to convert objects to string.</param>
        public void AddComparerOverride<TProp>(
            Expression<Func<TProp>> memberLambda, 
            Func<TProp, TProp, ComparisonSettings, bool> compareFunction, 
            Func<TProp, string> toStringFunction)
        {
            OverridesCollection.AddComparer(
                PropertyHelper.GetMemberInfo(memberLambda),
                new DynamicValueComparer<TProp>(
                    compareFunction,
                    toStringFunction));
        }

        /// <summary>
        /// Adds Comparer Override by Member.
        /// </summary>
        /// <typeparam name="TProp">Type of the member.</typeparam>
        /// <param name="memberLambda">Lambda to get member.</param>
        /// <param name="compareFunction">Function to compare objects.</param>
        public void AddComparerOverride<TProp>(
            Expression<Func<TProp>> memberLambda,
            Func<TProp, TProp, ComparisonSettings, bool> compareFunction)
        {
            OverridesCollection.AddComparer(
                PropertyHelper.GetMemberInfo(memberLambda),
                new DynamicValueComparer<TProp>(
                    compareFunction,
                    obj => obj?.ToString()));
        }

        /// <summary>
        /// Adds Comparer Override by Member.
        /// </summary>
        /// <param name="memberInfo">Member Info.</param>
        /// <param name="valueComparer">Value Comparer.</param>
        public void AddComparerOverride(MemberInfo memberInfo, IValueComparer valueComparer)
        {
            OverridesCollection.AddComparer(memberInfo, valueComparer);
        }

        /// <summary>
        /// Adds Comparer Override by Member name.
        /// </summary>
        /// <param name="memberName">Member Name.</param>
        /// <param name="valueComparer">Value Comparer.</param>
        /// <param name="filter">Value Comparer will be used only if filter(memberInfo) == true. Null by default.</param>
        public void AddComparerOverride(string memberName, IValueComparer valueComparer, Func<MemberInfo, bool> filter = null)
        {
            OverridesCollection.AddComparer(memberName, valueComparer, filter);
        }

        /// <summary>
        /// Sets <see cref="IBaseComparer.DefaultValueComparer"/>.
        /// </summary>
        /// <param name="valueComparer">Value Comparer.</param>
        public void SetDefaultComparer(IValueComparer valueComparer)
        {
            if (valueComparer == null)
            {
                throw new ArgumentNullException(nameof(valueComparer));
            }

            DefaultValueComparer = valueComparer;
        }
    }
}
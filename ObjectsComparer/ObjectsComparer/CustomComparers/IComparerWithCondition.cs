using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ObjectsComparer
{
    public interface IComparerWithCondition: IComparer
    {
        bool IsMatch(Type type, object obj1, object obj2);

        bool IsStopComparison(Type type, object obj1, object obj2);

        bool SkipMember(Type type, MemberInfo member);
        
    }

    /*public class BaseComparerWithCondition : IComparerWithCondition
    {
        public IValueComparer DefaultValueComparer { get; }
        public ComparisonSettings Settings { get; }
        public void SetDefaultComparer(IValueComparer valueComparer)
        {
            throw new NotImplementedException();
        }

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

        public IEnumerable<Difference> CalculateDifferences(Type type, object obj1, object obj2)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Difference> CalculateDifferences<T>(T obj1, T obj2)
        {
            throw new NotImplementedException();
        }

        public bool IsMatch(Type type, object obj1, object obj2)
        {
            throw new NotImplementedException();
        }

        public bool IsStopComparison(Type type, object obj1, object obj2)
        {
            return Settings.EmptyAndNullEnumerablesEqual && (obj1 == null || obj2 == null);
        }

        public bool SkipMember(Type type, MemberInfo member)
        {
            return false;
        }

        public List<string> OnlyMembersToCompare { get; set; }
    }*/
}

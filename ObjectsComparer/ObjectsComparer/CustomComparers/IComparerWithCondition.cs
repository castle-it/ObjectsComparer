using System;
using System.Reflection;

namespace ObjectsComparer
{
    public interface IComparerWithCondition: IComparer
    {
        bool IsMatch(Type type, object obj1, object obj2);

        bool IsStopComparison(Type type, object obj1, object obj2);

        bool SkipMember(Type type, MemberInfo member);
    }
}

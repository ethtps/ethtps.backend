using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ETHTPS.Configuration.Database;

public partial class EmptyDBSet<T> : DbSet<T> where T : class
{
    public override IEntityType EntityType => new EmptyEntityType();
}

public sealed class EmptyEntityType : IEntityType
{
    private readonly object _nothing = new();
    public object? this[string name] => _nothing;

    public IEntityType? BaseType => this;

    public InstantiationBinding? ConstructorBinding => null;

    public InstantiationBinding? ServiceOnlyConstructorBinding => null;

    public IModel Model => (IModel)_nothing;

    public string Name => string.Empty;

    public Type ClrType => typeof(object);

    public bool HasSharedClrType => false;

    public bool IsPropertyBag => false;
    IReadOnlyEntityType? IReadOnlyEntityType.BaseType => null;

    IReadOnlyModel IReadOnlyTypeBase.Model => new EmptyIReadOnlyModel();

    public IAnnotation AddRuntimeAnnotation(string name, object? value)
    {
        return new EmptyAnnotation();
    }

    public IAnnotation? FindAnnotation(string annotation)
    {
        return null;
    }

    public IEnumerable<IForeignKey> FindDeclaredForeignKeys(IReadOnlyList<IReadOnlyProperty> properties)
    {
        return Enumerable.Empty<IForeignKey>();
    }

    public INavigation? FindDeclaredNavigation(string annotation)
    {
        return null;
    }
    public IProperty? FindDeclaredProperty(string name)
    {
        return null;
    }

    public ITrigger? FindDeclaredTrigger(string annotation)
    {
        return null;
    }

    public IForeignKey? FindForeignKey(IReadOnlyList<IReadOnlyProperty> properties, IReadOnlyKey principalKey, IReadOnlyEntityType entityType)
    {
        return null;
    }

    public IEnumerable<IForeignKey> FindForeignKeys(IReadOnlyList<IReadOnlyProperty> properties)
    {
        return FindForeignKeys(properties);
    }

    public IIndex? FindIndex(IReadOnlyList<IReadOnlyProperty> list)
    {
        return null;
    }

    public IIndex? FindIndex(string name)
    {
        return null;
    }

    public PropertyInfo? FindIndexerPropertyInfo()
    {
        return null;
    }

    public IKey? FindKey(IReadOnlyList<IReadOnlyProperty> properties)
    {
        return null;
    }

    public IKey? FindPrimaryKey()
    {
        return null;
    }

    public IReadOnlyList<IReadOnlyProperty>? FindProperties(IReadOnlyList<string> propertyNames)
    {
        return null;
    }

    public IProperty? FindProperty(string name)
    {
        return null;
    }

    public IAnnotation? FindRuntimeAnnotation(string name)
    {
        return null;
    }

    public IServiceProperty? FindServiceProperty(string name)
    {
        return null;
    }

    public ISkipNavigation? FindSkipNavigation(string name)
    {
        return null;
    }

    public IEnumerable<IAnnotation> GetAnnotations()
    {
        return Enumerable.Empty<EmptyAnnotation>();
    }

    public ChangeTrackingStrategy GetChangeTrackingStrategy()
    {
        return ChangeTrackingStrategy.ChangedNotifications;
    }

    public IEnumerable<IForeignKey> GetDeclaredForeignKeys()
    {
        return Enumerable.Empty<IForeignKey>();
    }

    public IEnumerable<IIndex> GetDeclaredIndexes()
    {
        return Enumerable.Empty<IIndex>();
    }

    public IEnumerable<IKey> GetDeclaredKeys()
    {
        return Enumerable.Empty<IKey>();
    }

    public IEnumerable<INavigation> GetDeclaredNavigations()
    {
        return Enumerable.Empty<INavigation>();
    }

    public IEnumerable<IProperty> GetDeclaredProperties()
    {
        return Enumerable.Empty<IProperty>();
    }

    public IEnumerable<IForeignKey> GetDeclaredReferencingForeignKeys()
    {
        return Enumerable.Empty<IForeignKey>();
    }

    public IEnumerable<IServiceProperty> GetDeclaredServiceProperties()
    {
        return Enumerable.Empty<IServiceProperty>();
    }

    public IEnumerable<IReadOnlySkipNavigation> GetDeclaredSkipNavigations()
    {
        return Enumerable.Empty<IReadOnlySkipNavigation>();
    }

    public IEnumerable<ITrigger> GetDeclaredTriggers()
    {
        return Enumerable.Empty<ITrigger>();
    }

    public IEnumerable<IForeignKey> GetDerivedForeignKeys()
    {
        return Enumerable.Empty<IForeignKey>();
    }

    public IEnumerable<IIndex> GetDerivedIndexes()
    {
        return Enumerable.Empty<IIndex>();
    }

    public IEnumerable<IReadOnlyNavigation> GetDerivedNavigations()
    {
        return Enumerable.Empty<IReadOnlyNavigation>();
    }

    public IEnumerable<IReadOnlyProperty> GetDerivedProperties()
    {
        return Enumerable.Empty<IReadOnlyProperty>();
    }

    public IEnumerable<IReadOnlyServiceProperty> GetDerivedServiceProperties()
    {
        return Enumerable.Empty<IReadOnlyServiceProperty>();
    }

    public IEnumerable<IReadOnlySkipNavigation> GetDerivedSkipNavigations()
    {
        return Enumerable.Empty<IReadOnlySkipNavigation>();
    }

    public IEnumerable<IReadOnlyEntityType> GetDerivedTypes()
    {
        return Enumerable.Empty<IReadOnlyEntityType>();
    }

    public IEnumerable<IEntityType> GetDirectlyDerivedTypes()
    {
        return Enumerable.Empty<IEntityType>();
    }

    public string? GetDiscriminatorPropertyName()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IProperty> GetForeignKeyProperties()
    {
        return Enumerable.Empty<IProperty>();
    }

    public IEnumerable<IForeignKey> GetForeignKeys()
    {
        return Enumerable.Empty<IForeignKey>();
    }

    public IEnumerable<IIndex> GetIndexes()
    {
        return Enumerable.Empty<IIndex>();
    }

    public IEnumerable<IKey> GetKeys()
    {
        return Enumerable.Empty<IKey>();
    }

    public PropertyAccessMode GetNavigationAccessMode()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<INavigation> GetNavigations()
    {
        return Enumerable.Empty<INavigation>();
    }

    public TValue GetOrAddRuntimeAnnotationValue<TValue, TArg>(string name, Func<TArg?, TValue> valueFactory, TArg? arg)
    {
        return (TValue)(new object());
    }

    public IEnumerable<IProperty> GetProperties()
    {
        return Enumerable.Empty<IProperty>();
    }

    public PropertyAccessMode GetPropertyAccessMode()
    {
        throw new NotImplementedException();
    }

    public LambdaExpression? GetQueryFilter()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IForeignKey> GetReferencingForeignKeys()
    {
        return Enumerable.Empty<IForeignKey>();
    }

    public IEnumerable<IAnnotation> GetRuntimeAnnotations()
    {
        return Enumerable.Empty<IAnnotation>();
    }

    public IEnumerable<IDictionary<string, object?>> GetSeedData(bool providerValues = false)
    {
        return Enumerable.Empty<IDictionary<string, object?>>();
    }

    public IEnumerable<IServiceProperty> GetServiceProperties()
    {
        return Enumerable.Empty<IServiceProperty>();
    }

    public IEnumerable<ISkipNavigation> GetSkipNavigations()
    {
        return Enumerable.Empty<ISkipNavigation>();
    }

    public IEnumerable<IProperty> GetValueGeneratingProperties()
    {
        return Enumerable.Empty<IProperty>();
    }

    public IAnnotation? RemoveRuntimeAnnotation(string name)
    {
        return null;
    }

    public IAnnotation SetRuntimeAnnotation(string name, object? value)
    {
        return new EmptyAnnotation();
    }

    IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.FindDeclaredForeignKeys(IReadOnlyList<IReadOnlyProperty> properties)
    {
        return Enumerable.Empty<EmptyReadOnlyForeignKey>();
    }

    IReadOnlyNavigation? IReadOnlyEntityType.FindDeclaredNavigation(string name)
    {
        return null;
    }

    IReadOnlyProperty? IReadOnlyEntityType.FindDeclaredProperty(string name)
    {
        return null;
    }

    IReadOnlyTrigger? IReadOnlyEntityType.FindDeclaredTrigger(string name)
    {
        return null;
    }

    IReadOnlyForeignKey? IReadOnlyEntityType.FindForeignKey(IReadOnlyList<IReadOnlyProperty> properties, IReadOnlyKey principalKey, IReadOnlyEntityType principalEntityType)
    {
        return null;
    }

    IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.FindForeignKeys(IReadOnlyList<IReadOnlyProperty> properties)
    {
        return Enumerable.Empty<IReadOnlyForeignKey>();
    }

    IReadOnlyIndex? IReadOnlyEntityType.FindIndex(IReadOnlyList<IReadOnlyProperty> properties)
    {
        return null;
    }

    IReadOnlyIndex? IReadOnlyEntityType.FindIndex(string name)
    {
        return null;
    }

    IReadOnlyKey? IReadOnlyEntityType.FindKey(IReadOnlyList<IReadOnlyProperty> properties)
    {
        return null;
    }

    IReadOnlyKey? IReadOnlyEntityType.FindPrimaryKey()
    {
        return null;
    }

    IReadOnlyProperty? IReadOnlyEntityType.FindProperty(string name)
    {
        return null;
    }

    IReadOnlyServiceProperty? IReadOnlyEntityType.FindServiceProperty(string name)
    {
        return null;
    }

    IReadOnlySkipNavigation? IReadOnlyEntityType.FindSkipNavigation(string name)
    {
        return null;
    }

    IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.GetDeclaredForeignKeys()
    {
        return Enumerable.Empty<IReadOnlyForeignKey>();
    }

    IEnumerable<IReadOnlyIndex> IReadOnlyEntityType.GetDeclaredIndexes()
    {
        return Enumerable.Empty<IReadOnlyIndex>();
    }

    IEnumerable<IReadOnlyKey> IReadOnlyEntityType.GetDeclaredKeys()
    {
        return Enumerable.Empty<IReadOnlyKey>();
    }

    IEnumerable<IReadOnlyNavigation> IReadOnlyEntityType.GetDeclaredNavigations()
    {
        return Enumerable.Empty<IReadOnlyNavigation>();
    }

    IEnumerable<IReadOnlyProperty> IReadOnlyEntityType.GetDeclaredProperties()
    {
        return Enumerable.Empty<IReadOnlyProperty>();
    }

    IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.GetDeclaredReferencingForeignKeys()
    {
        return Enumerable.Empty<IReadOnlyForeignKey>();
    }

    IEnumerable<IReadOnlyServiceProperty> IReadOnlyEntityType.GetDeclaredServiceProperties()
    {
        return Enumerable.Empty<IReadOnlyServiceProperty>();
    }

    IEnumerable<IReadOnlyTrigger> IReadOnlyEntityType.GetDeclaredTriggers()
    {
        return Enumerable.Empty<IReadOnlyTrigger>();
    }

    IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.GetDerivedForeignKeys()
    {
        return Enumerable.Empty<IReadOnlyForeignKey>();
    }

    IEnumerable<IReadOnlyIndex> IReadOnlyEntityType.GetDerivedIndexes()
    {
        return Enumerable.Empty<IReadOnlyIndex>();
    }

    IEnumerable<IReadOnlyEntityType> IReadOnlyEntityType.GetDirectlyDerivedTypes()
    {
        return Enumerable.Empty<IReadOnlyEntityType>();
    }

    IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.GetForeignKeys()
    {
        return Enumerable.Empty<IReadOnlyForeignKey>();
    }

    IEnumerable<IReadOnlyIndex> IReadOnlyEntityType.GetIndexes()
    {
        return Enumerable.Empty<IReadOnlyIndex>();
    }

    IEnumerable<IReadOnlyKey> IReadOnlyEntityType.GetKeys()
    {
        return Enumerable.Empty<IReadOnlyKey>();
    }

    IEnumerable<IReadOnlyNavigation> IReadOnlyEntityType.GetNavigations()
    {
        return Enumerable.Empty<IReadOnlyNavigation>();
    }

    IEnumerable<IReadOnlyProperty> IReadOnlyEntityType.GetProperties()
    {
        return Enumerable.Empty<IReadOnlyProperty>();
    }

    IEnumerable<IReadOnlyForeignKey> IReadOnlyEntityType.GetReferencingForeignKeys()
    {
        return Enumerable.Empty<IReadOnlyForeignKey>();
    }

    IEnumerable<IReadOnlyServiceProperty> IReadOnlyEntityType.GetServiceProperties()
    {
        return Enumerable.Empty<IReadOnlyServiceProperty>();
    }

    IEnumerable<IReadOnlySkipNavigation> IReadOnlyEntityType.GetSkipNavigations()
    {
        return Enumerable.Empty<IReadOnlySkipNavigation>();
    }
}

public class EmptyAnnotation : IAnnotation
{
    public string Name => string.Empty;

    public object? Value => null;
}

public class EmptyIReadOnlyModel : IReadOnlyModel
{
    public object? this[string name] => throw new NotImplementedException();

    public IAnnotation? FindAnnotation(string name)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyEntityType? FindEntityType(string name)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyEntityType? FindEntityType(string name, string definingNavigationName, IReadOnlyEntityType definingEntityType)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyEntityType? FindEntityType(Type type)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyEntityType? FindEntityType(Type type, string definingNavigationName, IReadOnlyEntityType definingEntityType)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IReadOnlyEntityType> FindEntityTypes(Type type)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IAnnotation> GetAnnotations()
    {
        throw new NotImplementedException();
    }

    public ChangeTrackingStrategy GetChangeTrackingStrategy()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IReadOnlyEntityType> GetEntityTypes()
    {
        throw new NotImplementedException();
    }

    public PropertyAccessMode GetPropertyAccessMode()
    {
        throw new NotImplementedException();
    }

    public bool IsShared([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] Type type)
    {
        throw new NotImplementedException();
    }
}

public class EmptyReadOnlyForeignKey : IReadOnlyForeignKey
{
    public object? this[string name] => throw new NotImplementedException();

    public IReadOnlyEntityType DeclaringEntityType => throw new NotImplementedException();

    public IReadOnlyList<IReadOnlyProperty> Properties => throw new NotImplementedException();

    public IReadOnlyEntityType PrincipalEntityType => throw new NotImplementedException();

    public IReadOnlyKey PrincipalKey => throw new NotImplementedException();

    public IReadOnlyNavigation? DependentToPrincipal => throw new NotImplementedException();

    public IReadOnlyNavigation? PrincipalToDependent => throw new NotImplementedException();

    public bool IsUnique => throw new NotImplementedException();

    public bool IsRequired => throw new NotImplementedException();

    public bool IsRequiredDependent => throw new NotImplementedException();

    public bool IsOwnership => throw new NotImplementedException();

    public DeleteBehavior DeleteBehavior => throw new NotImplementedException();

    public IAnnotation? FindAnnotation(string name)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IAnnotation> GetAnnotations()
    {
        throw new NotImplementedException();
    }
}
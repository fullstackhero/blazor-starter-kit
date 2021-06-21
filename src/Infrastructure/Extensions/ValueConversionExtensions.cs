using BlazorHero.CleanArchitecture.Application.Interfaces.Serialization.Serializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BlazorHero.CleanArchitecture.Infrastructure.Extensions
{
    public static class ValueConversionExtensions
    {
        public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder, IJsonSerializer serializer) where T : class//, new()
        {
            var converter = new ValueConverter<T, string>
            (
                v => serializer.Serialize(v),
                v => serializer.Deserialize<T>(v) // ?? new T()
            );

            var comparer = new ValueComparer<T>
            (
                (l, r) => serializer.Serialize(l) == serializer.Serialize(r),
                v => v == null ? 0 : serializer.Serialize(v).GetHashCode(),
                v => serializer.Deserialize<T>(serializer.Serialize(v))
            );

            propertyBuilder.HasConversion(converter);
            propertyBuilder.Metadata.SetValueConverter(converter);
            propertyBuilder.Metadata.SetValueComparer(comparer);
            propertyBuilder.HasColumnType("nvarchar(max)");

            return propertyBuilder;
        }
    }
}
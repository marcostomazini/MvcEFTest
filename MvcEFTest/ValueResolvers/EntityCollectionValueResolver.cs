using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using MvcEFTest.Entities;
using MvcEFTest.Models;

namespace MvcEFTest.ValueResolvers
{
    public class EntityCollectionValueResolver<TSourceParent, TSource, TDest> : IValueResolver where TSource : ViewModelBase where TDest : EntityBase
    {
        private readonly Expression<Func<TSourceParent, ICollection<TSource>>> _sourceMember;

        public EntityCollectionValueResolver(
          Expression<Func<TSourceParent, ICollection<TSource>>> sourceMember)
        {
            _sourceMember = sourceMember;
        }

        public ResolutionResult Resolve(ResolutionResult source)
        {
            // Get source collection
            var sourceCollection = ((TSourceParent)source.Value).GetPropertyValue(_sourceMember);

            // If we are mapping to existing collection of entities...
            if (source.Context.DestinationValue != null)
            {
                // Get entities collection parent
                var destinationCollection = (ICollection<TDest>)source.Context.DestinationValue

                    // Get entities collection by member name defined in mapping profile
                    .GetPropertyValue(source.Context.MemberName);

                // Delete entities that are not in source collection
                var sourceIds = sourceCollection.Select(i => i.Id).ToList();
                foreach (var item in destinationCollection.Where(item => !sourceIds.Contains(item.Id)))
                {
                    destinationCollection.Remove(item);
                }

                // Map entities that are in source collection
                foreach (var sourceItem in sourceCollection)
                {
                    // If item is in destination collection...
                    var originalItem = destinationCollection.SingleOrDefault(o => o.Id == sourceItem.Id);
                    if (originalItem != null)
                    {
                        // ...map to existing item
                        Mapper.Map(sourceItem, originalItem);
                    }
                    else
                    {
                        // ...or create new entity in collection
                        var destItem = Mapper.Map<TSource, TDest>(sourceItem);
                        destinationCollection.Add(destItem);
                    }
                }

                return source.New(destinationCollection, source.Context.DestinationType);
            }

            // We are mapping to new collection of entities...
            // ...then just create new collection
            var value = new HashSet<TDest>();

            // ...and map every item from source collection
            foreach (var destItem in sourceCollection.Select(Mapper.Map<TSource, TDest>))
            {
                value.Add(destItem);
            }

            // Create new result mapping context
            source = source.New(value, source.Context.DestinationType);
            return source;
        }
    }
}
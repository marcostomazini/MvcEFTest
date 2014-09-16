using System;
using System.ComponentModel.DataAnnotations;

namespace MvcEFTest.Entities
{
    public class EntityBase<T> : IEquatable<EntityBase<T>> where T : EntityBase<T>
    {
        [Key]
        public int Id { get; set; }

        private int? OldHashCode { get; set; }

        public static bool operator ==(EntityBase<T> left, EntityBase<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EntityBase<T> left, EntityBase<T> right)
        {
            return !Equals(left, right);
        }

        public override int GetHashCode()
        {
            if (OldHashCode.HasValue)
            {
                return OldHashCode.Value;
            }

            bool thisIsTransient = Id == default(int);
            if (!thisIsTransient)
            {
                return Id;
            }

            OldHashCode = base.GetHashCode();
            return OldHashCode.Value;
        }

        public bool Equals(EntityBase<T> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            bool otherIsTransient = other.Id == default(int);
            bool thisIsTransient = Id == default(int);
            if (otherIsTransient && thisIsTransient)
            {
                return ReferenceEquals(other, this);
            }

            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as T);
        }
    }
}
using System;

namespace MayoSolutions.Media.MediaData.Aggregators
{
    /// <summary>
    /// Decoration identifying a class as an available aggregator source.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class AggregatorAttribute : Attribute
    {
        /// <summary>Name of the aggregator to present to the user.</summary>
        public string Name { get; }

        /// <summary>
        /// Identifies a class as an available aggregator source.
        /// </summary>
        /// <param name="name">Name of the aggregator to present to the user.</param>
        public AggregatorAttribute(string name)
        {
            Name = name;
        }
    }
}
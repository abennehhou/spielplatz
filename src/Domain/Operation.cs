using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Playground.Domain
{
    /// <summary>
    /// Information about an operation performed on an entity.
    /// </summary>
    public class Operation
    {
        /// <summary>
        /// Identifier of the operation.
        /// </summary>
        [BsonId]
        public ObjectId Id { get; set; }

        /// <summary>
        /// Identifier of the entity.
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// Entity type.
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// Date when the operation is performed.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Operation Type.
        /// </summary>
        public string OperationType { get; set; }

        /// <summary>
        /// List of differences between old and new version. Filled when the operation is an update.
        /// </summary>
        public List<Difference> Differences { get; set; }

    }
}

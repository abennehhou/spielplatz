using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Playground.Domain;

namespace Playground.Repositories
{
    /// <summary>
    /// Extension for Bson documents.
    /// </summary>
    public static class BsonDocumentExtensions
    {
        private const string PropertyNameDocument = "Document";

        /// <summary>
        /// Compares two Bson documents.
        /// </summary>
        /// <param name="document1">Document to compare.</param>
        /// <param name="document2">Other document to compare.</param>
        /// <returns></returns>
        public static List<Difference> GetDifferences(this BsonDocument document1, BsonDocument document2)
        {
            var differences = new List<Difference>();
            if (document1 == null && document2 == null)
                return differences;

            if (document1 == null || document2 == null)
            {
                differences.Add(new Difference { PropertyName = PropertyNameDocument, Value1 = document1, Value2 = document2 });
                return differences;
            }
            var objectsToCompare = new Stack<Difference>();
            objectsToCompare.Push(new Difference { PropertyName = PropertyNameDocument, Value1 = document1, Value2 = document2 });
            while (objectsToCompare.Count > 0)
            {
                var objectToCompare = objectsToCompare.Pop();
                var name = objectToCompare.PropertyName;
                var object1 = objectToCompare.Value1;
                var object2 = objectToCompare.Value2;
                var diff = new Difference { PropertyName = objectToCompare.PropertyName, Value1 = object1, Value2 = object2 };
                if (object1 == null && object2 == null)
                    continue;

                if (object1 == null || object2 == null)
                {
                    differences.Add(diff);
                    continue;
                }

                // Checks for BsonDocument
                if (object1.IsBsonDocument)
                {
                    if (!object2.IsBsonDocument)
                    {
                        differences.Add(diff);
                        continue;
                    }

                    var elementsInObject1 = object1.AsBsonDocument.Elements.ToList();
                    var elementsInObject2 = object2.AsBsonDocument.Elements.ToList();
                    foreach (var element in elementsInObject1)
                    {
                        var matchingElementValue = elementsInObject2.Where(x => x.Name == element.Name).Select(x => x.Value).FirstOrDefault();
                        objectsToCompare.Push(new Difference { PropertyName = $"{name}.{element.Name}", Value1 = element.Value, Value2 = matchingElementValue });
                    }
                    foreach (var element in elementsInObject2)
                    {
                        var matchingElementValue = elementsInObject1.Where(x => x.Name == element.Name).Select(x => x.Value).FirstOrDefault();
                        if (matchingElementValue == null)
                            objectsToCompare.Push(new Difference { PropertyName = $"{name}.{element.Name}", Value1 = null, Value2 = element.Value });
                    }
                }
                else if (object1.IsBsonArray)
                {
                    // Checks for list
                    if (!object2.IsBsonArray)
                    {
                        differences.Add(diff);
                        continue;
                    }
                    var array1 = object1.AsBsonArray.OrderBy(x => x).ToArray();
                    var array2 = object2.AsBsonArray.OrderBy(x => x).ToArray();
                    for (int i = 0; i < array1.Length; i++)
                    {
                        objectsToCompare.Push(new Difference { PropertyName = $"{name}[{i}]", Value1 = array1[i], Value2 = array2.ElementAtOrDefault(i) });
                    }
                    if (array2.Length > array1.Length)
                    {
                        for (int i = array1.Length; i < array2.Length; i++)
                        {
                            objectsToCompare.Push(new Difference { PropertyName = $"{name}[{i}]", Value1 = null, Value2 = array2[i] });
                        }
                    }
                }
                else
                {
                    // Checks for simple element
                    if (!object1.Equals(object2))
                    {
                        differences.Add(new Difference { PropertyName = name, Value1 = object1, Value2 = object2 });
                    }
                }
            }

            return differences;
        }
    }
}

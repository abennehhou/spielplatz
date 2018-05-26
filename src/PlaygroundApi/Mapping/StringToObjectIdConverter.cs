using AutoMapper;
using MongoDB.Bson;

namespace PlaygroundApi.Mapping
{
    public class StringToObjectIdConverter : ITypeConverter<string, ObjectId>
    {
        public ObjectId Convert(string source, ObjectId destination, ResolutionContext context)
        {
            var result = ObjectId.Empty;
            ObjectId.TryParse(source, out result);
            return result;
        }
    }
}

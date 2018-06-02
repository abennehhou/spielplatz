namespace Playground.Domain
{
    public class ItemSearchParameter : SearchBase
    {
        public string Name { get; set; }
        public string Owner { get; set; }
        public string Tag { get; set; }
    }
}

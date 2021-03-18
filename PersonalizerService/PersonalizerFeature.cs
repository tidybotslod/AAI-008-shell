using System.Text;

namespace AAI
{
    public class PersonalizationFeature
    {
        public PersonalizationFeature() {}
        public PersonalizationFeature(PersonalizationFeature other)
        {
            Name = other.Name;
            Prompt = other.Prompt;
            Values = other.Values;
        }

        public string Name { get; set; }
        public string Prompt { get; set; }
        public string[] Values { get; set; }
    }
}

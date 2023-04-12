using System.IO;
using System.Xml.Serialization;

namespace Main.XML
{
    public class XmlContext
    {
        private readonly string _dataFolder;

        public XmlContext(string dataFolder)
        {
            _dataFolder = dataFolder;
        }

        public DbSet<T> Set<T>() where T : class
        {
            return new DbSet<T>(GetFilePath<T>(), this);
        }

        internal string GetFilePath<T>()
        {
            return Path.Combine(_dataFolder, typeof(T).Name.ToLower() + ".xml");
        }

        internal T[] LoadData<T>()
        {
            var filePath = GetFilePath<T>();
            if (!File.Exists(filePath))
            {
                return new T[0];
            }

            var xml = File.ReadAllText(filePath);
            var serializer = new XmlSerializer(typeof(T[]));
            var entities = (T[])serializer.Deserialize(new StringReader(xml));
            return entities;
        }

        internal void SaveData<T>(T[] entities)
        {
            var filePath = GetFilePath<T>();
            var serializer = new XmlSerializer(typeof(T[]));
            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, entities);
            }
        }
    }
}
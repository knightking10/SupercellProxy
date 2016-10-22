using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace SupercellProxy
{
    partial class PacketDefinition
    {
        // Dictionary mapping packet IDs to there definitions.
        private static Dictionary<int, PacketDefinition> s_definitions = new Dictionary<int, PacketDefinition>();

        private int _id;
        private string _name;
        // Path pointing to the definition file.
        private string _path;
        private DateTime _lastWrite;

        private readonly List<Field> _fields;

        public int ID => _id;
        public string Name => _name;
        public ReadOnlyCollection<Field> Fields => _fields.AsReadOnly();

        public PacketDefinition()
        {
            _lastWrite = DateTime.Now;
            _fields = new List<Field>();
        }

        public void Reload()
        {
            Load(_path);
        }

        public void Load(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            // Read the definition file.
            var jsonFile = File.ReadAllText(path);
            var definitionToken = JObject.Parse(File.ReadAllText(path));

            // Gets the last time the file was written to.
            // This to figure out whether or not to reload the definition file
            // give some fancy 'real time' editing.
            _lastWrite = File.GetLastWriteTime(path);

            _id = definitionToken.TryToObject<int>("id");
            _name = definitionToken.TryToObject<string>("name");

            // Read the fieldsToken and deserialize the packet definition fields.
            var fieldsToken = definitionToken["fields"];

            if (fieldsToken == null)
            {
                // TODO: Handle.
            }

            foreach (var token in fieldsToken.AsJEnumerable())
            {
                var field = ParseField(token);
                _fields.Add(field);
            }

            _path = path;
            s_definitions.Add(_id, this);
        }

        public static void LoadAll(Game game)
        {
            var dir = string.Empty;
            switch (game)
            {
                case Game.CLASH_OF_CLANS:
                    dir = "packet-def-coc";
                    break;
                case Game.CLASH_ROYALE:
                    dir = "packet-def-cr";
                    break;
                case Game.BOOM_BEACH:
                    dir = "packet-def-bb";
                    break;
            }

            if (!Directory.Exists(dir))
            {
                // TODO: Handle.
            }

            var componentDir = Path.Combine(dir, "component");
            var clientDir = Path.Combine(dir, "client");
            var serverDir = Path.Combine(dir, "server");

            LoadDefinitionsAt(componentDir);
            LoadDefinitionsAt(clientDir);
            LoadDefinitionsAt(serverDir);
        }

        public static PacketDefinition GetDefinition(int id)
        {
            var def = (PacketDefinition)null;
            if (!s_definitions.TryGetValue(id, out def))
                return null;

            // Check if file was modified.
            var lastWrite = File.GetLastWriteTime(def._path);
            // If yes, we reload the definition.
            if (lastWrite > def._lastWrite)
                def.Reload();

            return def;
        }


        private static void LoadDefinitionsAt(string rootDir)
        {
            if (!Directory.Exists(rootDir))
            {
                // TODO: Handle.
            }

            var files = Directory.GetFiles(rootDir);
            for (int i = 0; i < files.Length; i++)
            {
                var definition = new PacketDefinition();
                definition.Load(files[i]);
            }

            Logger.Log($"Loaded {files.Length} packet definitions in {rootDir}.");
        }
    }
}

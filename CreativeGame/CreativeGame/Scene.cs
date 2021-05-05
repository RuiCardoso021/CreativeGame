using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CreativeGame
{
    public class Scene
    {
        private List<Sprite> _sprites;

        public Scene(Game game, string name)
        {
            string filename = $"Content/scenes/{name}.dt";
            _sprites = new List<Sprite>();
            using (StreamReader reader = File.OpenText(filename))
            {
                JObject sceneJson = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
                JArray spriteJson = (JArray)sceneJson["composite"]["sImages"];
                foreach (JObject image in spriteJson)
                {
                    float x = (float)(image["x"] ?? 0);
                    float y = (float)(image["y"] ?? 0);
                    string imageName = (string)image["imageName"];
                    string imageFilename = $"assets/orig/images/{imageName}";
                    _sprites.Add(new Sprite(game, imageFilename, new Vector2(x, y)));
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (Sprite sprite in _sprites)
            {
                sprite.Draw(spriteBatch, gameTime);
            }
        }
    }
}
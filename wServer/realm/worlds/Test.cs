using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using terrain;

namespace wServer.realm.worlds
{
    public class Test : World
    {
        public Test()
        {
            Id = TEST_ID;
            Name = "Pony's castle";
            Background = 2;
        }

        public void LoadJson(string json)
        {
            FromWorldMap(new MemoryStream(Json2Wmap.Convert(json)));
        }                                                   
    }
}

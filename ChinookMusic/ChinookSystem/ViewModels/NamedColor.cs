using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinookSystem.ViewModels
{
    //a record is a readonly. To change it,you have to recreate it an once an instance is created,
    //it cannot be updated. See the aboutservices, new instances are created eachtime.
    public record NamedColor(string RgbCode, string HexCode, string Name, int ColorType, bool Available)
    {
       
    }
}

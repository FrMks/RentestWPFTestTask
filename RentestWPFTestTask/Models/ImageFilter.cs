using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentestWPFTestTask.Models
{
    internal class ImageFilter
    {
        public string Name { get; }
        public string Id { get; }

        public ImageFilter(string name, string id)
        {
            Name = name;
            Id = id;
        }
    }
}

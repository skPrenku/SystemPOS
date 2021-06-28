using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPOS
{
    class Products
    {
        int id;
        string name;
        string barcode;
        double price;

        DateTime created;

        public int Id { get { return id; } set { id = value; } }
        public string Name { get { return name; } set { name = value; } }

        public string Barcode { get { return barcode; } set { barcode = value; } }

        public double Price { get { return price; } set { price = value; } }

        public DateTime Created { get { return created; } set { created = value; } }

    }
}

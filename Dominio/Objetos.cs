using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    internal class Objetos
    {
        public string Nombre { get; set; }
        public int Id { get; set; }
        [DisplayName("Número de cuenta")]
        public int Saldo { get; set; }
        //public int MyProperty { get; set; }

    }
}

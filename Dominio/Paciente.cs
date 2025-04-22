using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Paciente
    {
        public int Id { get; set; }    
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Dni {  get; set; }
        public int Celular { get; set; }
        public string Descripcion { get; set; }
    }
}

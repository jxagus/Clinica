using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Dominio;

namespace Acesso_a_Datos
{
    class NegocioPaciente
    {

        public class PokemonNegocio
        {

            public List<Paciente> listar()
            {
                List<Paciente> lista = new List<Paciente>();
                SqlConnection conexion = new SqlConnection();
                SqlCommand comando = new SqlCommand();
                SqlDataReader lector;

                try
                {
                    conexion.ConnectionString = "server=.\\SQLEXPRESS; database=PACIENTES_DB; integrated security=true";
                    comando.CommandType = System.Data.CommandType.Text;
                    comando.CommandText = "SELECT Id, Nombre, Apellido, FechaNacimiento, Dni, Descripcion FROM PACIENTES WHERE Activo = 1";
                    comando.Connection = conexion;

                    conexion.Open();
                    lector = comando.ExecuteReader();

                    while (lector.Read())
                    {
                        NegocioPaciente aux = new NegocioPaciente();
                        aux.Id = (int)lector["Id"];
                        aux.Nombre = (string)lector["Nombre"];
                        aux.Apellido = (string)lector["Nombre"];
                        aux.Descripcion = (string)lector["Descripcion"];
                        aux.Dni = (int)lector["Dni"];

                        lista.Add(aux);
                    }

                    conexion.Close();
                    return lista;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            public void agregar(Paciente nuevo)
            {
                AccesoDatos datos = new AccesoDatos();

                try
                {
                    datos.setearConsulta("Insert into POKEMONS (Numero, Nombre, Descripcion, Activo, IdTipo, IdDebilidad, UrlImagen)values(" + nuevo.Numero + ", '" + nuevo.Nombre + "', '" + nuevo.Descripcion + "', 1, @idTipo, @idDebilidad, @urlImagen)");
                    //datos.setearParametro("@idTipo", nuevo.Tipo.Id);
                    //datos.setearParametro("@idDebilidad", nuevo.Debilidad.Id);
                    //datos.setearParametro("@urlImagen", nuevo.UrlImagen);
                    datos.ejecutarAccion();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    datos.cerrarConexion();
                }
            }

            public void modificar(Paciente paciente)
            {
                AccesoDatos datos = new AccesoDatos();
                try
                {
                    datos.setearConsulta("update POKEMONS set Numero = @numero, Nombre = @nombre, Descripcion = @desc, UrlImagen = @img, IdTipo = @idTipo, IdDebilidad = @idDebilidad Where Id = @id");
                    datos.setearParametro("@id", paciente.Id);
                    datos.setearParametro("@nombre", paciente.Nombre);
                    datos.setearParametro("@nombre", paciente.Apellido);
                    datos.setearParametro("@desc", paciente.Descripcion);
                    datos.setearParametro("@dni", paciente.Dni);

                    datos.ejecutarAccion();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    datos.cerrarConexion();
                }
            }

            public List<Paciente> filtrar(string campo, string criterio, string filtro)
            {
                List<Paciente> lista = new List<Paciente>();
                AccesoDatos datos = new AccesoDatos();
                try
                {
                    string consulta = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id From POKEMONS P, ELEMENTOS E, ELEMENTOS D Where E.Id = P.IdTipo And D.Id = P.IdDebilidad And P.Activo = 1 And ";
                    if (campo == "Número")
                    {
                        switch (criterio)
                        {
                            case "Mayor a":
                                consulta += "Numero > " + filtro;
                                break;
                            case "Menor a":
                                consulta += "Numero < " + filtro;
                                break;
                            default:
                                consulta += "Numero = " + filtro;
                                break;
                        }
                    }
                    else if (campo == "Nombre")
                    {
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "Nombre like '" + filtro + "%' ";
                                break;
                            case "Termina con":
                                consulta += "Nombre like '%" + filtro + "'";
                                break;
                            default:
                                consulta += "Nombre like '%" + filtro + "%'";
                                break;
                        }
                    }
                    else
                    {
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "P.Descripcion like '" + filtro + "%' ";
                                break;
                            case "Termina con":
                                consulta += "P.Descripcion like '%" + filtro + "'";
                                break;
                            default:
                                consulta += "P.Descripcion like '%" + filtro + "%'";
                                break;
                        }
                    }

                    datos.setearConsulta(consulta);
                    datos.ejecutarLectura();
                    while (datos.Lector.Read())
                    {
                        Paciente aux = new Paciente();
                        aux.Id = (int)datos.Lector["Id"];
                        aux.Dni = datos.Lector.GetInt32(0);
                        aux.Nombre = (string)datos.Lector["Nombre"];
                        aux.Apellido = (string)datos.Lector["Apellido"];
                        aux.Descripcion = (string)datos.Lector["Descripcion"];                       
                        //aux.Tipo = new Elemento();
                        //aux.Tipo.Id = (int)datos.Lector["IdTipo"];
                        //aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];
                        //aux.Debilidad = new Elemento();
                        //aux.Debilidad.Id = (int)datos.Lector["IdDebilidad"];
                        //aux.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];

                        lista.Add(aux);
                    }

                    return lista;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public void eliminar(int id)
            {
                try
                {
                    AccesoDatos datos = new AccesoDatos();
                    datos.setearConsulta("delete from pokemons where id = @id");
                    datos.setearParametro("@id", id);
                    datos.ejecutarAccion();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public void eliminarLogico(int id)
            {
                try
                {
                    AccesoDatos datos = new AccesoDatos();
                    datos.setearConsulta("update POKEMONS set Activo = 0 Where id = @id");
                    datos.setearParametro("@id", id);
                    datos.ejecutarAccion();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }


        }
    }
}

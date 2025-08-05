using Datos;
using Dominio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class ArticuloNegocio
    {

        public List<Articulo> Listar()
        {
            List<Articulo> Lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("select A.Id, A.Codigo, A.Nombre, A.Descripcion, M.Descripcion MarcaDescripcion, C.Descripcion CategoriaDescripcion, A.ImagenUrl, A.Precio, A.IdCategoria, A.IdMarca from ARTICULOS A, MARCAS M, CATEGORIAS C where A.IdMarca  = M.Id and A.IdCategoria = C.Id");
                datos.ejecutarLectura();

                while(datos.Lector.Read())
                {
                    Articulo Articulo = new Articulo();
                    Articulo.Id = (int)datos.Lector["Id"];
                    Articulo.codigoArticulo = (string)datos.Lector["Codigo"];
                    Articulo.nombre = (string)datos.Lector["Nombre"];
                    Articulo.descripcion = (string)datos.Lector["Descripcion"];

                    Articulo.marca = new Marca();
                    Articulo.marca.Id = (int)datos.Lector["IdMarca"];
                    Articulo.marca.Descripcion = (string)datos.Lector["MarcaDescripcion"];

                    Articulo.categoria = new Categoria();
                    Articulo.categoria.Id = (int)datos.Lector["IdCategoria"];
                    Articulo.categoria.Descripcion = (string)datos.Lector["CategoriaDescripcion"];

                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        Articulo.urlImagen = (string)datos.Lector["ImagenUrl"];

                    if (!(datos.Lector["Precio"] is DBNull))
                        Articulo.precio = Convert.ToDecimal(datos.Lector["Precio"]);

                    Lista.Add(Articulo);
                }

                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void agregarArticulo(Articulo articulo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("insert into ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio) values (@Codigo, @Nombre, @Descripcion, @IdMarca, @IdCategoria, @ImagenUrl, @Precio)");
                datos.setearParametros("@Codigo", articulo.codigoArticulo);
                datos.setearParametros("@Nombre", articulo.nombre);
                datos.setearParametros("@Descripcion", articulo.descripcion);
                datos.setearParametros("@IdMarca", articulo.marca.Id);
                datos.setearParametros("@IdCategoria", articulo.categoria.Id);
                datos.setearParametros("@ImagenUrl", articulo.urlImagen);
                datos.setearParametros("@Precio", articulo.precio);
                datos.ejecutarAccion();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificar(Articulo articulo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update ARTICULOS set Codigo=@codigo, Nombre=@nombre, Descripcion=@descripcion, IdMarca=@idMarca, IdCategoria=@idCategoria, ImagenUrl=@imagenUrl, Precio=@precio where Id = @id");
                datos.setearParametros("@codigo", articulo.codigoArticulo);
                datos.setearParametros("@nombre", articulo.nombre);
                datos.setearParametros("@descripcion", articulo.descripcion);
                datos.setearParametros("@idMarca", articulo.marca.Id);
                datos.setearParametros("@idCategoria", articulo.categoria.Id);
                datos.setearParametros("@imagenUrl", articulo.urlImagen);
                datos.setearParametros("@precio", articulo.precio);
                datos.setearParametros("@id", articulo.Id);
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

        public void eliminarFisico(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("delete from ARTICULOS where Id=@Id");
                datos.setearParametros("@Id", id);
                datos.ejecutarAccion();

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public List<Articulo> Filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "select A.Id, A.Codigo, A.Nombre, A.Descripcion, M.Descripcion MarcaDescripcion, C.Descripcion CategoriaDescripcion, A.ImagenUrl, A.Precio, A.IdCategoria, A.IdMarca from ARTICULOS A, MARCAS M, CATEGORIAS C where A.IdMarca  = M.Id and A.IdCategoria = C.Id and ";

                switch (campo)
                {
                    case "Código":
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "A.Codigo like '" + filtro + "%' ";
                                break;

                            case "Termina con":
                                consulta += "A.Codigo like '%" + filtro + "' ";
                                break;

                            default:
                                consulta += "A.Codigo like '%" + filtro + "%' ";
                                break;
                        }
                        break;

                    case "Nombre":
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "A.Nombre like '" + filtro + "%' ";
                                break;

                            case "Termina con":
                                consulta += "A.Nombre like '%" + filtro + "' ";
                                break;

                            default:
                                consulta += "A.Nombre like '%" + filtro + "%' ";
                                break;
                        }
                        break;

                    default:
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "C.Descripcion like '" + filtro + "%' ";
                                break;

                            case "Termina con":
                                consulta += "C.Descripcion like '%" + filtro + "' ";
                                break;

                            default:
                                consulta += "C.Descripcion like '%" + filtro + "%' ";
                                break;
                        }
                        break;
                }

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo Articulo = new Articulo();
                    Articulo.Id = (int)datos.Lector["Id"];
                    Articulo.codigoArticulo = (string)datos.Lector["Codigo"];
                    Articulo.nombre = (string)datos.Lector["Nombre"];
                    Articulo.descripcion = (string)datos.Lector["Descripcion"];

                    Articulo.marca = new Marca();
                    Articulo.marca.Id = (int)datos.Lector["IdMarca"];
                    Articulo.marca.Descripcion = (string)datos.Lector["MarcaDescripcion"];

                    Articulo.categoria = new Categoria();
                    Articulo.categoria.Id = (int)datos.Lector["IdCategoria"];
                    Articulo.categoria.Descripcion = (string)datos.Lector["CategoriaDescripcion"];

                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        Articulo.urlImagen = (string)datos.Lector["ImagenUrl"];

                    if (!(datos.Lector["Precio"] is DBNull))
                        Articulo.precio = Convert.ToDecimal(datos.Lector["Precio"]);

                    lista.Add(Articulo);
                }

                return lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

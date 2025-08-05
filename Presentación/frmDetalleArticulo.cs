using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace Presentación
{
    public partial class frmDetalleArticulo : Form
    {
        private Articulo articulo = null;

        public frmDetalleArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
        }

        /// <>
        /// CARGA DE VENTANA
        /// <>
        private void frmDetalleArticulo_Load(object sender, EventArgs e)
        {
            try
            {
                if (articulo != null)
                {
                    txtCodigo.Text = articulo.codigoArticulo.ToString();
                    txtNombre.Text = articulo.nombre.ToString();
                    txtDescripcion.Text = articulo.descripcion.ToString();
                    txtMarca.Text = articulo.marca.Descripcion.ToString();
                    txtCategoria.Text = articulo.categoria.Descripcion.ToString();
                    txtImagen.Text = articulo.urlImagen.ToString();
                    cargarImagen(articulo.urlImagen);
                    txtPrecio.Text = articulo.precio.ToString();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <>
        /// BOTON ACEPTAR
        /// <>
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Close();
        }


        /// <>
        /// CARGAR IMAGEN
        /// <>
        private void cargarImagen(string imagen)
        {
            try
            {
                pbxImagenArticulo.Load(imagen);
            }
            catch (Exception)
            {

                pbxImagenArticulo.Load("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQeJQeJyzgAzTEVqXiGe90RGBFhfp_4RcJJMQ&s");
            }
        }
    }
}

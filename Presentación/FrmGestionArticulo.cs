using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentación
{
    public partial class FrmGestionArticulo : Form
    {
        private Articulo articulo = null;

        public FrmGestionArticulo()
        {
            InitializeComponent();
        }
        public FrmGestionArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Articulo";
        }

        /// <>
        /// CARGA DE VENTANA
        /// <>
        private void FrmGestionArticulo_Load(object sender, EventArgs e)
        {
            CategoriaNegocio negocioCategoria = new CategoriaNegocio();
            MarcaNegocio negocioMarca = new MarcaNegocio();               
            try
            {
                cbxCategoria.DataSource = negocioCategoria.Listar();
                cbxCategoria.ValueMember = "Id";
                cbxCategoria.DisplayMember = "Descripcion";

                cbxMarca.DataSource = negocioMarca.Listar();
                cbxMarca.ValueMember = "Id";
                cbxMarca.DisplayMember = "Descripcion";

                if(articulo != null)
                {
                    txtCodigo.Text = articulo.codigoArticulo.ToString();
                    txtNombre.Text = articulo.nombre.ToString();
                    txtDescripcion.Text = articulo.descripcion.ToString();
                    cbxMarca.SelectedValue = (int)articulo.marca.Id;
                    cbxCategoria.SelectedValue = (int)articulo.categoria.Id;
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
        /// BOTON ACEPTAR/CANCELAR
        /// <>
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (articulo == null)
                    articulo = new Articulo();

                articulo.codigoArticulo = txtCodigo.Text;
                articulo.nombre = txtNombre.Text;
                articulo.descripcion = txtDescripcion.Text;
                articulo.marca = (Marca)cbxMarca.SelectedItem;
                articulo.categoria = (Categoria)cbxCategoria.SelectedItem;
                articulo.urlImagen = txtImagen.Text;
                articulo.precio = decimal.Parse(txtPrecio.Text);

                if (articulo.Id != 0)
                {
                    negocio.modificar(articulo);
                    MessageBox.Show("Modificación Exitosa");
                    Close();
                }
                else
                {
                    negocio.agregarArticulo(articulo);
                    MessageBox.Show("Carga Exitosa");
                    Close();
                }
            }
            catch(FormatException)
            {
                MessageBox.Show("Campos incorrectos.", "Error de formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
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

        private void txtImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImagen.Text);
        }
    }
}

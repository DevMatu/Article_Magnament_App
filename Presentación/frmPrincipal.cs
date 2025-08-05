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
    public partial class frmPrincipal : Form
    {
        private List<Articulo> listArticulos;
        public frmPrincipal()
        {
            InitializeComponent();
        }

        /// <>
        /// CARGA DE VENTANA PRINCIPAL
        /// <>
        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                listArticulos = negocio.Listar();
                dgvArticulos.DataSource = listArticulos;
                ocultarColumnas();
                if (listArticulos.Count > 0)
                    pbxImagen.Load(listArticulos[0].urlImagen);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.ToString());              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pbxImagen.Load(imagen);
            }
            catch (Exception)
            {

                pbxImagen.Load("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTxFKyGh2UQkUphVdmPy3W0Xn_CtT2Rc1-_xQ&s");
            }
        }
        private void ocultarColumnas()
        {
            dgvArticulos.Columns["urlImagen"].Visible = false;
            dgvArticulos.Columns["Id"].Visible = false;
        }
        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            cargar();
            cbxCampo.Items.Add("Código");
            cbxCampo.Items.Add("Nombre");
            cbxCampo.Items.Add("Categoría");
        }
        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.urlImagen);
            }
        }


        /// <>
        /// BOTON AGREGAR/MODIFICAR/DETALLE
        /// <>
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            FrmGestionArticulo gestionArticulo = new FrmGestionArticulo();
            gestionArticulo.ShowDialog();
            cargar();
        }
        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo articulo;
            try
            {
                if(validarRowSeleccionada())
                    return;

                articulo = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                FrmGestionArticulo gestionArticulo = new FrmGestionArticulo(articulo);
                gestionArticulo.ShowDialog();
                cargar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }
        private void btnDetalle_Click(object sender, EventArgs e)
        {
            Articulo articulo;
            try
            {
                if (validarRowSeleccionada())
                    return;

                articulo = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                frmDetalleArticulo detalleArticulo = new frmDetalleArticulo(articulo);
                detalleArticulo.ShowDialog();
                cargar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        /// <>
        /// BOTON ELIMINAR
        /// <>
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (validarRowSeleccionada())
                return;
            Eliminar();
        }
        private void Eliminar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Eliminar articulo seleccionado? Esta acción es permanente.", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if(respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    negocio.eliminarFisico(seleccionado.Id);
                    cargar();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }


        /// <>
        /// FILTRO/BOTON BUSCAR
        /// <>
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (validarFiltro())
                    return;

                string campo = cbxCampo.Text;
                string criterio = cbxCriterio.Text;
                string filtro = txtFiltro.Text;

                dgvArticulos.DataSource = negocio.Filtrar(campo, criterio, filtro);

                if (lblCampo.ForeColor == Color.Red || lblCriterio.ForeColor == Color.Red)
                {
                    lblCampo.ForeColor = Color.Black;
                    lblCriterio.ForeColor = Color.Black;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void cbxCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cbxCampo.SelectedItem.ToString();
            if (opcion == "Código" || opcion == "Nombre" || opcion == "Categoría")
            {
                cbxCriterio.Items.Clear();
                cbxCriterio.Items.Add("Comienza con");
                cbxCriterio.Items.Add("Termina con");
                cbxCriterio.Items.Add("Contiene");
            }
        }


        /// <>
        /// VALIDACIONES
        /// <>
        private bool validarRowSeleccionada()
        {
            if(dgvArticulos.CurrentRow == null)
            {
                MessageBox.Show("Por favor, Seleccione un artículo");
                return true;
            }
            return false;
        }
        private bool validarFiltro()
        {
            if (cbxCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Faltan campos obligatorios.", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblCampo.ForeColor = Color.Red;
                return true;
            }
            if (cbxCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Faltan campos obligatorios.", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblCriterio.ForeColor = Color.Red;
                return true;
            }
            if (cbxCampo.SelectedItem.ToString() == "Número")
            {
                if (esNumero(txtFiltro.Text))
                {
                    MessageBox.Show("Faltan campos obligatorios.", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            }
            return false;
        }
        private bool esNumero(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (char.IsNumber(caracter))
                {
                    return false;
                }
            }
            return true;
        }
        
    }
}

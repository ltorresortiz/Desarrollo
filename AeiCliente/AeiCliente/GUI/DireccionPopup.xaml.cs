﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AeiCliente.ServicioAEI;
using Windows.UI.Popups;

namespace AeiCliente
{
    public sealed partial class DireccionPopup : UserControl
    {
        private Popup popup = null;
        private ServicioAEIClient servicioDireccion = new ServicioAEIClient();
        private List<Direccion> listaEstados = null;
        private  List<Direccion> listaCiudad = null;
        PerfilPage pagina = new PerfilPage();

        public DireccionPopup(Popup padre, PerfilPage pagina)
        {
            if (padre == null) throw new ArgumentNullException("Debe asignar un Popup al controlador");
            this.popup = padre;
            this.InitializeComponent();
            cargarEstados();
            this.pagina = pagina;
        }

        private async void cargarEstados()
        {
            listaEstados = await servicioDireccion.consultarEstadosAsync();
            comboBoxEstado.Items.Add("Selecione");
            comboBoxCiudad.Items.Add("Selecione");

            for (int i = 0; i < listaEstados.Count(); i++)
            {
                comboBoxEstado.Items.Add(listaEstados[i].Estado);
            }
            comboBoxEstado.SelectedIndex = 0;
            comboBoxCiudad.SelectedIndex = 0;

        }

       
        private async void buttonAgregar_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog mensajeError = new MessageDialog("Los campos con * son OBLIGATORIOS");
            int error = -1;

            if (comboBoxCiudad.SelectedIndex != 0 && comboBoxEstado.SelectedIndex != 0 && textboxCodigoPostal.Text.Length != 0)
            {
                int idCiudad = listaCiudad[comboBoxCiudad.SelectedIndex-1].Id;
                error = await servicioDireccion.agregarDireccionUsuarioAsync(BufferUsuario.Usuario.Id, idCiudad, textBoxDetalle.Text, int.Parse(textboxCodigoPostal.Text));
                if (error == 1)
                {
                    BufferUsuario.Usuario.Direcciones = await servicioDireccion.buscarDireccionUsuarioAsync(BufferUsuario.Usuario.Id);
                    var rootFrame = new Frame();
                    pagina.cargarDireciones();
                    popup.IsOpen = false;
                }
                
            }
            else
            {
                mensajeError.ShowAsync();
            }

            if (error == 0)
            {
                mensajeError.Content = "No se pudo agregar la nueva dirección";
                mensajeError.ShowAsync();

            }

        }

        private async void comboBoxEstado_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

            comboBoxCiudad.Items.Clear();
            comboBoxCiudad.Items.Add("Seleccion");
            comboBoxCiudad.SelectedIndex=0;
            int idEstado = listaEstados[comboBoxEstado.SelectedIndex].Id;
            listaCiudad = await servicioDireccion.consultarCiudadAsync(idEstado);
            for (int i = 0; i < listaCiudad.Count(); i++)
            {
                comboBoxCiudad.Items.Add(listaCiudad[i].Ciudad);
            }
        }

        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = false;
        }

       

    }
}

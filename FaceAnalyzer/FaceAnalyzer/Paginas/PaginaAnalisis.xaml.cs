using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media.Abstractions;
using FaceAnalyzer.Servicios;
using FaceAnalyzer.Helpers;

namespace FaceAnalyzer.Paginas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PaginaAnalisis : ContentPage
	{
		public PaginaAnalisis ()
		{
			InitializeComponent ();
		}

        MediaFile foto;

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        async void btnTomarFoto_Clicked(object sender, EventArgs e)
        {
            try
            {
                Loading(true);

                foto = await ServicioImagen.TomarFoto();
                if (foto != null)
                    imagen.Source = ImageSource.FromStream(foto.GetStream);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Excepción: " + ex.Message, "OK");
            }
            finally
            {
                Loading(false);
            }
        }

        async void btnAnalizar_Clicked(object sender, EventArgs e)
        {
            if (foto != null)
            {
                try
                {
                    Loading(true);

                    // Fase 1 - Face
                    var rostro = await ServicioFace.DetectarRostro(foto);

                    var frente = ImageAnalyzer.AnalizarPostura(rostro);
                    txtFrente.Text = frente.ToString("N2");

                    if (frente > Constantes.LookingAwayAngleThreshold)
                    {
                        txtFrente.TextColor = Color.Red;
                        txtAnalisisFrente.TextColor = Color.Red;
                        txtAnalisisFrente.Text = "No estás mirando al frente";
                    }
                    else
                    {
                        txtFrente.TextColor = Color.Green;
                        txtAnalisisFrente.TextColor = Color.Green;
                        txtAnalisisFrente.Text = "OK";
                    }

                    var boca = ImageAnalyzer.AnalizarBoca(rostro);
                    txtBoca.Text = boca.ToString("N2");

                    if (boca > Constantes.YawningApertureThreshold)
                    {
                        txtBoca.TextColor = Color.Red;
                        txtAnalisisBoca.TextColor = Color.Red;
                        txtAnalisisBoca.Text = "Posiblemente está bostezando";
                    }
                    else
                    {
                        txtBoca.TextColor = Color.Green;
                        txtAnalisisBoca.TextColor = Color.Green;
                        txtAnalisisBoca.Text = "OK";
                    }

                    var ojos = ImageAnalyzer.AnalizarOjos(rostro);
                    txtOjos.Text = ojos.ToString("N2");

                    if (ojos < Constantes.SleepingApertureThreshold)
                    {
                        txtOjos.TextColor = Color.Red;
                        txtAnalisisOjos.TextColor = Color.Red;
                        txtAnalisisOjos.Text = "¡Está dormido!";
                    }
                    else
                    {
                        txtOjos.TextColor = Color.Green;
                        txtAnalisisOjos.TextColor = Color.Green;
                        txtAnalisisOjos.Text = "OK";
                    }

                    // Fase 2 - Vision
                    var descripcion = await ServicioVision.DescribirImagen(foto);
                    var analisis = await ServicioVision.AnalizarImagen(foto);

                    if (descripcion.Description.Captions.Length > 0)
                    {
                        var distraccion = descripcion.Description.Captions[0].Text;
                        if (distraccion.Contains("phone"))
                        {
                            txtCelular.Text = "SI";
                            txtCelular.TextColor = Color.Red;
                            txtAnalisisCelular.TextColor = Color.Red;
                            txtAnalisisCelular.Text = "¡Está usando el teléfono móvil!";
                        }
                        else
                        {
                            txtCelular.Text = "NO";
                            txtCelular.TextColor = Color.Green;
                            txtAnalisisCelular.TextColor = Color.Green;
                            txtAnalisisCelular.Text = "OK";
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "Excepción: " + ex.Message, "OK");
                }
                finally
                {
                    Loading(false);
                }
            }
            else
                await DisplayAlert("Error", "Debes tomar la fotografía", "OK");
        }
    }
}
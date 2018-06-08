using System;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using FaceAnalyzer.Helpers;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;

namespace FaceAnalyzer.Servicios
{
    public static class ServicioVision
    {
        public async static Task<AnalysisResult> DescribirImagen(MediaFile foto)
        {
            AnalysisResult analisis = null;

            try
            {
                if (foto != null)
                {
                    using (var stream = foto.GetStream())
                    {
                        var clienteVision = new VisionServiceClient(Constantes.VisionApiKey, Constantes.VisionApiURL);
                        analisis = await clienteVision.DescribeAsync(foto.GetStream());
                    }
                }
            }
            catch(Exception ex)
            {

            }

            return analisis;
        }

        public async static Task<AnalysisResult> AnalizarImagen(MediaFile foto)
        {
            AnalysisResult analisis = null;

            try
            {
                if (foto != null)
                {
                    using (var stream = foto.GetStream())
                    {
                        var clienteVision = new VisionServiceClient(Constantes.VisionApiKey, Constantes.VisionApiURL);
                        var caracteristicas = new VisualFeature[] { VisualFeature.Tags, VisualFeature.Faces, VisualFeature.Categories, VisualFeature.Description, VisualFeature.Color };
                        analisis = await clienteVision.AnalyzeImageAsync(foto.GetStream(), caracteristicas);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return analisis;
        }
    }
}

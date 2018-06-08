using System;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Media.Abstractions;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using FaceAnalyzer.Helpers;

namespace FaceAnalyzer.Servicios
{
    public static class ServicioFace
    {
        public static async Task<Face> DetectarRostro(MediaFile foto)
        {
            Face rostro = null;

            try
            {
                if (foto != null)
                {
                    var clienteFace = new FaceServiceClient(Constantes.FaceApiKey, Constantes.FaceApiURL);
                    var atributosFace = new FaceAttributeType[] { FaceAttributeType.Age, FaceAttributeType.Gender, FaceAttributeType.HeadPose, FaceAttributeType.Emotion };

                    using (var stream = foto.GetStream())
                    {
                        Face[] rostros = await clienteFace.DetectAsync(stream, false, true, atributosFace);
                        rostro = rostros.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return rostro;
        }
    }
}

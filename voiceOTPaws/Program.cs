// AGREGAR ESTE JSON A AIM
//{
//    "Version": "2012-10-17",
//    "Statement": [
//        {
//        "Sid": "FullAccess",
//            "Effect": "Allow",
//            "Action": [
//                "sms-voice:*"
//            ],
//            "Resource": "*"
//        }
//    ]
//}

// DOC. https://docs.aws.amazon.com/pinpoint/latest/developerguide/pinpoint-dg.pdf


using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.PinpointSMSVoice;
using Amazon.PinpointSMSVoice.Model;
using Amazon.Runtime;
namespace PinpointVoiceMessageClient
{
    class Program
    {
        private static readonly string awsKeyId = "AKIA3K4I4W3CUMYXKAGD"; // Agregar tu api key de AIM.
        private static readonly string awsKeySecret = "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"; // Agregar el key secret de AIM
        private static readonly string region = "us-east-1";
        private static readonly string originationNumber = "+50249782248"; // Agregar aqui el número de telefono comprado
        private static readonly string destinationNumber = "+50249782248"; // Agregar el numero de telefono para pruebas previamente verificado
        private static readonly string voiceName = "Mia"; // Voces latinas solo es Mia. También hay voces Castellanas pero sonaria raro, no se.
        private static string languageCode = "es-MX";

        // finalmente llegamos al contenido que se va sintenizar a voz:
        // se usan etiquetas cono xml para diferentes ajustes : <speak/>, <amazon:effect phonation='soft'/>, <break strength='weak'/>

        private static readonly string ssmlMessage = "<speak>Tu código de seguridad es: "
        + "<emphasis>7 8 4 5 9 5 </emphasis> </speak>";
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enviando OTP...");
                Task.Run(() => SendVoiceMessage()).Wait();
                Console.WriteLine("OTP enviado");
            }
            catch (Exception ex)
            {
                Console.WriteLine("OTP no enviado. Error: " + ex.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }
        private static async Task SendVoiceMessage()
        {
            var awsCredentials = new BasicAWSCredentials(awsKeyId, awsKeySecret);
            using (AmazonPinpointSMSVoiceClient client = new AmazonPinpointSMSVoiceClient(awsCredentials, RegionEndpoint.GetBySystemName(region)))
            {
                SendVoiceMessageRequest sendVoiceMessageRequest = new SendVoiceMessageRequest
                {
                    DestinationPhoneNumber = destinationNumber,
                    OriginationPhoneNumber = originationNumber,
                    Content = new VoiceMessageContent
                    {
                        SSMLMessage = new SSMLMessageType
                        {
                            LanguageCode = languageCode,
                            VoiceId = voiceName,
                            Text = ssmlMessage
                        }
                    }
                };
                SendVoiceMessageResponse response = await client.SendVoiceMessageAsync(sendVoiceMessageRequest);
            }
        }
    }
}

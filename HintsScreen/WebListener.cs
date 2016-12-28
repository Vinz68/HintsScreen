
using Restup.Webserver.Attributes;
using Restup.Webserver.Http;
using Restup.Webserver.Models.Schemas;
using Restup.Webserver.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibeSoft.WinIoTSolution.WebEventDistributor;

namespace HintsScreen
{
    class WebListener
    {
        private HttpServer _httpServer;

        public async Task Run(int port)
        {
            var restRouteHandler = new RestRouteHandler();
            restRouteHandler.RegisterController<TextHintController>();

            var configuration = new HttpServerConfiguration()
                .ListenOnPort(port)
                .RegisterRoute("api", restRouteHandler)
                .EnableCors(); // allow cors requests on all origins 
                               //.EnableCors(x => x.AddAllowedOrigin("http://specificserver:<listen-port>")); 


            var httpServer = new HttpServer(configuration);
            _httpServer = httpServer;
            await httpServer.StartServerAsync();

            // Don't release deferral, otherwise app will stop 

        }
    }



    [RestController(InstanceCreationType.Singleton)]
    public class TextHintController
    {
        public class DataReceived
        {
            public int TimeInSec { get; set; }

            public string HintText { get; set; }
        }

        // http://192.168.178.76:8888/api/hint/30/Hint: Probeer met je ogen open de de puzzle op te lossen
        [UriFormat("/hint/{timeInSec}/{hintText}")]
        public GetResponse GetWithSimpleParameters(int timeInSec, string hintText)
        {
            DataReceived data = new DataReceived() { TimeInSec = timeInSec, HintText = hintText };

            WebEventDistributor.Instance.DistributeHint( data.TimeInSec, data.HintText );

            return new GetResponse(GetResponse.ResponseStatus.OK, data);
        }
    }

}

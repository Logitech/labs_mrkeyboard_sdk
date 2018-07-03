
    var portNumber = 58960;    

    function KeyboardAPICall(url,params,onMessageFunction)
    {
        if ("WebSocket" in window)
        {
            // Let us open a web socket
            var ws = new WebSocket("ws://localhost:"+portNumber+url);

            ws.onopen = function()
            {
                // Web Socket is connected, send data using send()
                ws.send(params);
            };

            ws.onmessage = function (evt)
            {
                var received_msg = evt.data;
                onMessageFunction(received_msg);                
                ws.close();
            };

            ws.onclose = function()
            {
                // websocket is closed.
                console.log("Connection is closed...");
            };
        }
        else
        {
            // The browser doesn't support WebSocket
            alert("WebSocket NOT supported by your Browser!");
        }
    }

     

    function SetPort()
    {
        portNumber = document.getElementById('PORT_NUMBER_BOX').value;
    }

 

    function SetGenericType(name, value){
        var onMessageFunc = function(received_msg){
            console.log("Call result: "+received_msg);
        };
        KeyboardAPICall("/bridgeAPI", '{\"name\":\"' + name + '\",\"value\":\"' + value + '\"}', onMessageFunc);
    }

    function RequestData(apiCall, onMessageFunc){        
        KeyboardAPICall("/bridgeAPI", '{\"name\":\"' + apiCall + '\",\"value\":\"' + 0 + '\"}', onMessageFunc);
    }

    var header = document.querySelector('header');
    var section = document.querySelector('section');
    var requestURL;

        function onButtonClick() {
            tracetabledata.innerHTML = "";
    var pageSize = document.getElementById("pageSize").value;
        var pageNumber = document.getElementById("pageNumber").value -1;

        requestURL = 'http://localhost:62150/trace'; //Fix this hard coding
        var postString = '?page=' + pageNumber + "&size=" + pageSize;
        requestURL = requestURL + postString;
        requestTraces();
        }

        function requestTraces() {
            var request = new XMLHttpRequest();
            request.open('GET', requestURL);
            request.responseType = 'json';
            request.send();
            request.onload = function () {
                var traces = request.response;
                console.log(traces);

                showTracesTable(traces);
            }
        }

    function showTraces(jsonObj) {
      

      for(var i = 0; i < jsonObj.length; i++) {
        var _traces = document.createElement('table');

        var _id = document.createElement('p');
        var _type = document.createElement('p');
        var _requestContent = document.createElement('p');
        var _requestUri = document.createElement('p');
        var _requestMethod = document.createElement('p');
        var _requestTimestamp = document.createElement('p');
        var _responseContent = document.createElement('p');
        var _responseStatusCode = document.createElement('p');
        var _responseTimestamp = document.createElement('p');
        var _responseTimeMs = document.createElement('p');

        _id.textContent = 'Id: ' + jsonObj[i].Id;
        _type.textContent = 'Type: ' + jsonObj[i].Type;
        _requestContent.textContent = 'Request Content: ' + jsonObj[i].RequestContent;
        _requestUri.textContent = 'Request Uri: ' + jsonObj[i].RequestUri;
        _requestMethod.textContent = 'Request Method: ' + jsonObj[i].RequestMethod;
        _requestTimestamp.textContent = 'Request Timestamp: ' + jsonObj[i].RequestTimestamp;
        _responseContent.textContent = 'Response Content: ' + jsonObj[i].ResponseContent;
        _responseStatusCode.textContent = 'Response Status Code: ' + jsonObj[i].ResponseStatusCode;
        _responseTimestamp.textContent = 'Response Timestamp: ' + jsonObj[i].ResponseTimestamp;
        _responseTimeMs.textContent = 'Response TimeMs: ' + jsonObj[i].ResponseTimestamp;

        _traces.appendChild(_id);
        _traces.appendChild(_type);
        _traces.appendChild(_requestContent);
        _traces.appendChild(_requestUri);
        _traces.appendChild(_requestMethod);
        _traces.appendChild(_requestTimestamp);
        _traces.appendChild(_responseContent);
        _traces.appendChild(_responseStatusCode);
        _traces.appendChild(_responseTimestamp);
        _traces.appendChild(_responseTimeMs);

        section.appendChild(_traces);


        
        

      }
    }

function showTracesTable(jsonObj) {
    var table = document.getElementById("tracetabledata")

    for (var i = 0; i < jsonObj.length; i++) {

        //Create Row
        var row = table.insertRow(i);
        //Populate Columns
        var _id = row.insertCell(0);
        var _type = row.insertCell(1);
        var _requestContent = row.insertCell(2);
        var _requestUri = row.insertCell(3);
        var _requestMethod = row.insertCell(4);
        var _requestTimestamp = row.insertCell(5);
        var _responseContent = row.insertCell(6);
        var _responseStatusCode = row.insertCell(7);
        var _responseTimestamp = row.insertCell(8);
        var _responseTimeMs = row.insertCell(9);
        //Insert Inner HTML
        _id.innerHTML =                 jsonObj[i].Id;
        _type.innerHTML =               jsonObj[i].Type;
        _requestContent.innerHTML =     jsonObj[i].RequestContent;
        _requestUri.innerHTML =         jsonObj[i].RequestUri;
        _requestMethod.innerHTML =      jsonObj[i].RequestMethod;
        _requestTimestamp.innerHTML =   jsonObj[i].RequestTimestamp;
        _responseContent.innerHTML =    jsonObj[i].ResponseContent;
        _responseStatusCode.innerHTML = jsonObj[i].ResponseStatusCode;
        _responseTimestamp.innerHTML =  jsonObj[i].ResponseTimestamp;
        _responseTimeMs.innerHTML =     jsonObj[i].ResponseTimestamp;

    }


}



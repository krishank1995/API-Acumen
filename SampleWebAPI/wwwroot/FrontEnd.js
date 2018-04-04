var header = document.querySelector('header');
var section = document.querySelector('section');
var requestURL;

function onButtonClick() {
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

function showTracesTable(jsonObj) {
    var table = document.getElementById("tracetable");
    $("#tracetable").find("tr:not(:first)").remove(); //jQ
    for (var i = 0; i < jsonObj.length; i++) {
        //Create Row
        var row = table.insertRow(i+1);
        //Populate Columns
        var _id = row.insertCell(0).innerHTML = jsonObj[i].Id;
        var _type = row.insertCell(1).innerHTML = jsonObj[i].Type;
        var _requestContent = row.insertCell(2).innerHTML = jsonObj[i].RequestContent;
        var _requestUri = row.insertCell(3).innerHTML = jsonObj[i].RequestUri;
        var _requestMethod = row.insertCell(4).innerHTML = jsonObj[i].RequestMethod;
        var _requestTimestamp = row.insertCell(5).innerHTML = jsonObj[i].RequestTimestamp;
        var _responseContent = row.insertCell(6).innerHTML = jsonObj[i].ResponseContent;
        var _responseStatusCode = row.insertCell(7).innerHTML = jsonObj[i].ResponseStatusCode;
        var _responseTimestamp = row.insertCell(8).innerHTML = jsonObj[i].ResponseTimestamp;
        var _responseTimeMs = row.insertCell(9).innerHTML = jsonObj[i].ResponseTimestamp;
    }

}



					Call Tracer Library  (API Acumen) 

The library captures the calls to SampleWebAPI (api/Products). It can be configured to store the information either In-Memory,MongoDB Database,
MySQL Database. It is compatible with MySQL service over PCF and can connect to any MongoDB database. 

Library two Front-End endpoints:
1. /ui --> UI for traces.
2. /analysis --> UI for Quick overview.

Library has two Back-End endpoints which serve jSON data to Front-End:
1. /trace --> Return trace data.
2. /traceanalysis --> Return quick overview data.

It can be pushed to PCF using the PowerShell script "PCFPush.ps1" provided.



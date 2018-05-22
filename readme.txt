						API Acumen 

API Acumen is a genric middleware for tracing and analysing Web APIs.

To demo the same it is cofigured with a Sample Products API for referene. It can be configured to store the information either In-Memory,MongoDB Database or a MySQL Database. It is compatible with MySQL service over PCF. 

Library two Front-End endpoints:
1. /ui --> UI for traces.
2. /analysis --> UI for Quick overview.

Library has two Back-End endpoints which serve jSON data to Front-End:
1. /trace --> Return trace data.
2. /traceanalysis --> Return quick overview data.

It can be pushed to PCF using the PowerShell script "PCFPush.ps1" provided.



# Endpoints

Endpoints provide a way to send or recieve data to somewhere.

## Provided Endpoints

### Read/Write File Endpoint

This simply takes the data object from previous steps and writes it from a file, or reads a file and passes the data object to the next step to process it. Currently it is all synchronous but depending upon appetite there may be a threaded version added to support non-blocking IO.

### Read/Write Player Prefs Endpoint

This basically shoves your data into a player pref string based upon a key provided, or will retrieve that data from player prefs. This offers a unity specific way to just store your data.

## Creating Endpoints

To create an endpoint just implement `ISendDataEndpoint` or `IReceiveDataEndpoint` for your scenario, such as a `HttpPutRequestEndpoint` and `HttpDeleteRequestEndpoint` etc. 

There is only a single method per interface which handles the persistence or retrieval of data. For sending data you may want to consume a response object too, so in the case of HTTP you may want to consume the returned Id from inserting a user via an API endpoint, and the `onSuccess` argument allows an object to be passed back which can be used for these scenarios.
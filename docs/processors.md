# Processors

So processors offer a way to do *stuff* on the data before it goes to it destination, be it outbound to some endpoint, or inbound to some model.

This can range from things like encrypting the data, gzipping it, url encoding it etc, and you can run as many of them as you want in the pipeline in whatever order you want.

## Provided Processors

### Encrypt/Decrypt Processors

There are processors provided for encryption and decryption of data, this by default provides an AES encryptor which provides a simple way to keep all your data more secure than it would be by default.

### Url Encode/Decode Processors

This allows you to url encode the data for web requests. Until you have a http endpoint it is not really that useful but adding functionality which would require external libs is out of the scope of the first release.

## Creating Processors

To create a processor just implement `IProcessor` for your scenario, such as a `GzipDeflateProcessor` and `GzipInflateProcessor` etc. There is only a single method which takes the `DataObject` representing the underlying data, and then returns back a new `DataObject` containing the processed data.
# ZeroFriction

## Design Details
* Includes a simple API key authentication. Using a Web API middleware.
* Includes a common exception handler that decorates the response status and error messages(if any) based on the exceptions.
* Invoice service with unit tests.
* Cosmos DB service with the logical partitioning strategy based on the document id. (Assumption behind that is, this solution is a very simple usecase with just creating, reading or updating of a single document. If there are other documents with different usecases, then the partition strategy should be changed to support equally distribution of the data load)
* Invoice data model. Invoice Lines are under the Invoice document as subdocuments. (Since, an invoice line doesn't consume much storage space. Therefore can be included in the same invoice document.)
* For concurrency handling "eTag" is being used and if there is any concurrency violation API throws 409 conflict status.

## API Documentation
* Postman collection is included with the name "InvoiceAPI.postman_collection.json".

## Notes for postman collection
* Collection includes few collection variables where the values should be added by user like BaseUrl, AuthKey. (Already prefilled with local development values)
* There are some other collection variables which are set by the request responces like invoiceId, invoiceLastRetrievedETag. These are there for usage easiness. 
    * Example :- When created an invoice, the id of the record is added to the "invoiceId" variable which is referred in the GET invoice call.
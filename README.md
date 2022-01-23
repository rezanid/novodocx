# Novo Docx 
Novo Docx exists because I looked everywhere and I couldn't find a viable and simple library that can help me populate Word templates. The templates I'm talking about are the ones that have placeholders that people can fill like a form. Some people call them Word forms. You might be thinking what about generating the whole document from the scratch, since there are some libraries for that purpose and you would be right, but generating the whole document from the scratch by code is not an easy task unless your document is extremely simple and nothing like a real world form or report.

Imagine having a standard Word document in whatever formatting, complexity or length and only adding placeholders that your app can detect and fill with real data at runtime. Even the business users can do it, in fact they can do it better than you. They might have even shared it with you already and that might be why you are here. 😆 If that is the case you won't be disappointed, I promise. 

> 📯 NOTE! 
>
> If you are not familiar with the templates or how to build them in Microsoft Word, please take a look at the following article. 
>
> [Create forms that users complete or print in Word](https://support.microsoft.com/en-us/office/create-forms-that-users-complete-or-print-in-word-040c5cc1-e309-445b-94ac-542f732c8c8b)

 

## Other alternatives

To be honest, at the time of this writing I only know two other alternatives that worth exploring and they might even fit better in your workflow.

* Word templates feature in Microsoft Dynamics (Power Platform) - this one is an old and mature functionality of Microsoft Dynamics CRM that has inherited by the Power Platform and this here is one of the downsides, you can only use it in a Power Platform solution and nothing else. Another downside is that it is very limited, a template can only work with an assigned table and its direct related tables.
* Word online connector for Power Platform - again this one works fine, but one downside is that it only works with Power Platform and another big downside is that when the source of your document is dynamic, for example you have a flow that is part of a solution that deploys to different environments (think DevOps and ALM) and the URLs change, you will have to rely on random numbers instead of placeholder names!
* There are several paid alternatives as well. One of them is good by the way, but even that one is not as simple as you might imagine to work with and there is no other open-source alternative that I am aware of. 

If the above options sound gibberish to you, they are for Power Platform developers (basically a low-code application development platform from Microsoft), but even they some times are not easy or capable enough to work with. Now enter Novo Docx.

 ## What is Novo Docx

Novo Docx is a .NET core library and an Azure Functions App that hosts it. You can send a Word template along with a JSON object to populate the template and return the result to you in the same call, lightning fast.

> 📯 NOTE! 
>
> I will be making a Lambda Function to make it easy for those who are into AWS to host it there too. 

## How to use

There are several options to use Novo Docx, it depends on whether you want to use it as a library in your app, or you prefer to host it somewhere as a severless service an simply call it over HTTP(S).

### Use serverless as an Azure Functions App

If you are going to host the Azure Function App, basically you just need one REST call from the comfort of your app! 😉

Request:

```
POST /api/Word HTTP/1.1
Host: something.azurewebsites.net
Content-Type: application/json
Content-Length: 1690176
{
  "parameters":
 {
    "totalcost":"125000",
    "address":"Somewhere street 1, somecity"
    "items": [
      {
        "name": "Item 1",
        "price": 1000,
        "quantity": 20
      }
    ],
    ...
  }
  "file":"base64-encoded docx file to be populated"
}
```

Response:

```
{
  "result": {
    "success": true,
    "message": "Document has been populated successfully."
  }
  "file":"base64-encoded docx file to be populated"
}
```

There will be other options soon to send back the file in binary without base-64 encoding it.

> 📯 NOTE! 
>
> As a bonus, the Functions App also produces an OpenAPI definition that you can access visually from `/api/word/swagger/ui`. I will keep it up-to-date with the implementation. 

### Use the library directly

Coming soon.

## Roadmap

The next milestones for 2022 are listed below with no order of priority.

* Publish the alpha version of the library as a NuGet.
* Report warnings in the result as well, like a good compiler would. This should be possible to turn on/off.
* Support AWS's Lambda.
* Provide ARM template and pipeline for Azure.
* Add file conversion (e.g. PDF) functionality.
* Power Platform Connector.

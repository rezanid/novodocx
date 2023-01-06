# Novo Docx 
Novo Docx exists because I looked everywhere and I couldn't find a viable and simple-to-use library that can help me populate Word templates. The templates I'm talking about are standard template of Microsoft Word. In other words, Word documents that have placeholders. These are use by actual people like a form that they can fill. Some people call them Word forms.

![Word template example screenshot](.media/word-template-example)

You might be thinking what about generating the whole document from the scratch, since there are some libraries for that purpose and you would be right, but generating the whole document from the scratch by code is not an easy task unless your document is extremely simple and nothing like a real world form or a real world report.

Imagine you have received a standard Word document in whatever formatting, complexity or length. You are tasked with making parts of the content, dynamically change based on some external data. Some of these content parts might be even repeatable. You can add content placeholders (using Developer tab in Microsoft Word) where needed (e.g. even rows of tables can become place holders). NovoDocx can detect and fill them with real data at runtime. Even the business users can add place holders. In fact they might do it even better than you. NovoDocx expects a JSON object as the input. This JSON object should contain all the required data to fill the document template. As long as your place holders match the names of properties in the JSON object, NovoDocx will find them. You can also use arrays for repeatable sections of the document (i.e. a table row).

> 📯 NOTE! 
>
> If you are not familiar with the templates or how to build them in Microsoft Word, please take a look at the following article. 

To know more about Word forms I suggest you read the following article from official support documents.
> [Create forms that users complete or print in Word](https://support.microsoft.com/en-us/office/create-forms-that-users-complete-or-print-in-word-040c5cc1-e309-445b-94ac-542f732c8c8b)

## Other alternatives

To be honest, at the time of this writing I only know two other alternatives that worth exploring and they might even fit better in your workflow.

* Word templates feature in Microsoft Dynamics (Power Platform) - this one is an old and mature functionality of Microsoft Dynamics CRM that has inherited by the Power Platform and this here is one of the downsides, you can only use it in a Power Platform solution and nothing else. Another downside is that it is very limited, a template can only work with an assigned table and its direct related tables.
* Word online connector for Power Platform - again this one works fine, but one downside is that it only works with Power Platform, it's a premium connector (i.e. paid) and another downside is that when the location of your document is dynamic, for example you have a flow that is part of a solution that deploys to different environments (think DevOps and ALM) and the URLs change, you will have to rely on random numbers instead of placeholder names!
* There are several paid alternatives as well. One of them is good by the way, but even that one is not as simple as you might imagine to work with and there is no other open-source alternative that I am aware of. 

If the first two alternatives sound gibberish to you, they are for Power Platform developers (basically a low-code application development platform from Microsoft), but even they some times are not easy or good enough to work with and both require licensing.

 ## What is Novo Docx

Novo Docx is a .NET core library and an Azure Functions App that hosts it. You can send a Word template in your JSON object to populate the template and it replies with a filled document, lightning fast. In case of any issues or potencial issues, it will explain well in the reply.

> 📯 NOTE! 
>
> I will make a Lambda Function to make it easy for those who are into AWS. For now, you can read the Azure Function source code an build your Lambda function based on that.

## How to use

There are several ways to use Novo Docx, it depends on whether you want to use it as a library in your app, as a CLI from your terminal of choice, or you prefer to host it somewhere as a severless service and simply call it over HTTP(S). Right now you have the following options out-of-the-box.

* [Use from Terminal / Console / PowerShell: docx CLI](#use-from-terminal--console--powershell-ndocx-cli)
* [Use as Serverless: Docx Functions App](#use-as-serverless-docx-functions-app)
* [Docx .NET Library](#docx-net-library)

## Use from Terminal / Console / PowerShell: ndocx CLI

Using `docx` in your terminal might be the easiest way to populate word templates. It can be as easy as:

```bash
> ndocx populate yourworddocument.docx
```

You might be asking what about the parameter. Well, by default, the `populate` command assumes that there is a file called "params.json" right beside your word document. If you have your parameters in a different file or location, you can use the following syntax.

```bash
> ndocx populate yourworddocument.docx -params myparams.json
```

If you don't want to directly fill the word document and instead fill a copy of the file, you may use the following syntax.

```bash
> ndocx populate yourworddocument.docx -params myparams.json -output filleddocument.docx
```

The full syntax is like this:

```
ndocx populate [<TEMPLATE>...] [options]

Arguments:
  <TEMPLATE>  The template file to operate on. If a file is not specified, the command will search the current
              directory for a .docx file.

Options:
  -p, --params <PARAMS>  The params file to use as input parameter. If none is given, the command will look for
                         params.json file in the current path.
  -o, --output <OUTPUT>  The output file to generate. If none is given, the operation will be performed in-place on the
                         input file.
  -?, -h, --help         Show command line help.
```

If you look carefully at the above description, you'll see that you can even fill multiple files.

## Use as Serverless: Docx Functions App

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
  "file":"Populated base64-encoded file"
}
```

There will be other options soon to send back the file in binary without base-64 encoding it.

> 📯 NOTE! 
>
> As a bonus, the Functions App also produces an OpenAPI definition that you can access visually from `/api/word/swagger/ui`. I will keep it up-to-date with the implementation. 

## Docx .NET Library

Coming soon.

## Roadmap

The next milestones for 2023 are listed below with no order of priority.

* Publish the alpha version of the library as a NuGet.
* Report warnings in the result as well, like a good compiler would. This should be possible to turn on/off.
* Support AWS's Lambda.
* Provide ARM template and pipeline for Azure.
* Add file conversion (e.g. PDF) functionality.
* Power Platform Connector.

# Introduction
This project provides a Generator for building CSV objects from files and vice versa. The CSV object structure is very simple and string orientated, reflecting the nature of the CSV file itself, with Rows and Cells modelled as their own classes. There is no strong typing involved.

The CSV Definition
-----------------
The **CsvDefinition** class is a pure representation of CSV content. It has two properties providing lists of the Headers and a list of Rows. A Row is a **CsvRow** object that consists of one or more **CsvCell** objects. The class has several methods to enable you to manually add individual or multiple headers and rows to the definition. The syntax is fluent so you can build a definition in a single block. For example...

    var myCsv = new CsvDefinition()
       .AddHeaders("Name", "Age", "Grade")
       .AddRow("Bobby", 12, "C+")
       .AddRow("Sam", 12, "A")
       .AddRow("Lily", 11, "B");

The CSV Generator
-----------------

The CSV Generator class is the heart of the library. It is a static class that manages translating CSV content into an object and vice versa.

- **From Bytes(byte[] source)** - Takes a byte array (say, from a file upload) and converts it into a **CsvDefinition** object.
- **FromString(string source)** - Takes a string representing a whole CSV file content and converts it into a **CsvDefinition** object.
- **GenerateString(CsvDefinition definition)** - Takes a **CsvDefinition** object and converts it to string content. Can be used as an extension method to the **CsvDefinition** object.
- **GenerateFile(CsvDefinition definition, string path)** - Takes a **CsvDefinition** object and writes a file to the specified path. Can be used as an extension method to the **CsvDefinition** object.
- **GenerateBytes(CsvDefinition definition)** - Takes a **CsvDefinition** object and converts it into a byte array. Can be used as an extension method to the **CsvDefinition** object.

These can be used to create a CSV file from code like this...

    var myCsv = new CsvDefinition()
       .AddHeaders("Name", "Age", "Grade")
       .AddRow("Bobby", 12, "C+")
       .AddRow("Sam", 12, "A")
       .AddRow("Lily", 11, "B")
       .GenerateFile("C:\mycsv.csv");


or in reverse...

    var myCsv = CsvGenerator.FromBytes(File.ReadAllBytes("C:\mycsv.csv"));
    

# The Ministry of Technology Open Source Products
Welcome to The Ministry of Technology open source products. All open source Ministry of Technology products are distributed under the MIT License for maximum re-usability. Details on more of our products and services can be found on our website at http://www.ministryotech.co.uk

Our other open source repositories can be found here...

* [https://bitbucket.org/ministryotech](https://bitbucket.org/ministryotech)
* [https://github.com/ministryotech](https://github.com/ministryotech)
* [https://github.com/tiefling](https://github.com/tiefling)

Newer content prefers Github. Bitbucket is no longer actively used.

### Where can I get it?
You can download the package for this project from any of the following package managers...

- **NUGET** - [https://www.nuget.org/packages/Ministry.Csv](https://www.nuget.org/packages/Ministry.Csv)

### Contribution guidelines
If you would like to contribute to the project, please contact me.

### Who do I talk to?
* Keith Jackson - keith@ministryotech.co.uk

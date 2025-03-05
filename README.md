# Smile.Local.Ollama

A RAG solution to local Ollama AI setup.

Uses SQL Server to store documents and embeddings with SP to find relevant documents.

Now hooked into Ollama also tried and tested with Symantic Kernel Onnx Runtime solution (.Net c#)

**Please Note:**

The system I am using for this project is a Windows 11/24Gb memory/Core i7 with a Tesla T4 GPU with 16Gb memory
and the models I am using are "mxbai-embed-large" for embeddings and "phi4" for chat.

**Update**

Ollama is now working with Documents, but only implemented with the CLI project. (see Smile.Local.Ollama.CLI)

Have just added Image analysis.

**CLI Commands**

Rename Smile.Local.Ollama.CLI.exe to smile.exe and
(found in C:\Users\\<user>\source\repos\Smile.Local.Ollama\Smile.Local.Ollama.CLI\bin\Debug\net8.0\)

Add path to smile.exe where yours is:

eg. C:\Users\\<user>\source\repos\Smile.Local.Ollama\Smile.Local.Ollama.CLI\bin\Debug\net8.0\smile.exe

Commands:

smile upload "Path to file to upload"

smile ask "Your question"

smile ask-documents "Your question"

smile analyse "path to image file" "prompt"


**Remember**

You need the Smile.Local.Ollama web project running to use smile.exe

**Contact Me**

If you have found this useful please let me know at: **benjaminemanuel13@gmail.com**

** Coming Soon**

Soon I hope to use not just documents as data source but also CSV and Json files as well
as Stored Procedures (SQL Server) as data sources.

This would mean having a command such as:

smile upload-data "Path to file to upload" "This would be a sentence or two to describe the data for vector search"
&
smile ask-data "Your question"

# Smile.Local.Ollama

A RAG solution to local Ollama AI setup.

Uses SQL Server to store documents and embeddings with SP to find relevant documents.

Not hooked into Ollama yet but tried and tested with Symantic Kernel Onnx Runtime solution (.Net c#)

**Please Note:**

The system I am using for this project is a Windows 11/24Gb memory/Core i7 with a Tesla T4 GPU with 16Gb memory
and the models I am using are "all-minilm" for embeddings and "phi4" for chat.

**Update**

Ollama now working but not with Documents yet, only streaming chat supported just now.
(see Smile.Local.Ollama.Console)

**Another Update**

Have added document upload function and Ollama chat AskDocuments is also written, not tested yet.

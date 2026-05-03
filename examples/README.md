# ParadeDB for .NET: Examples & Cookbook

Welcome to the **ParadeDB for .NET** examples! This directory contains a collection of self-contained console applications 
designed to teach you how to integrate powerful search and analytics features into your .NET application using ParadeDB.

Think of this as a **cookbook**: whether you need simple keyword search, an e-commerce filtering system, or a cutting-edge 
RAG (Retrieval-Augmented Generation) pipeline, you'll find a recipe here.

## 🚀 Getting Started

### 1. Install Dependencies

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)

### 2. Start ParadeDB

You need a running ParadeDB instance. We provide a helper script to start one via Docker:

```bash
./scripts/run_paradedb.sh
```

> **Note:** If you already have a PostgreSQL instance with ParadeDB installed, update the connection string in the 
> example's `appsettings.json` before running.

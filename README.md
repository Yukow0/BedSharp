# BedSharp

[![Build](https://img.shields.io/badge/build-unknown-lightgrey)](https://github.com/Yukow0/BedSharp/actions)
[![License](https://img.shields.io/badge/license-MIT-blue)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-blueviolet)](https://dotnet.microsoft.com/download/dotnet/10.0)

**BedSharp** is an experimental Minecraft Bedrock Edition server written entirely in C#.

The project is focused on providing a native, community-driven Bedrock server implementation targeting modern Bedrock versions.

---

## Why the project was restarted

BedSharp was restarted from scratch for several reasons.

### 1. Moving to .NET 10

The previous version targeted .NET 9.0, which is a Standard Term Support (STS) release.

The new version targets **.NET 10**, an LTS release, providing a longer-lived foundation for the project.

---

### 2. A cleaner project structure

BedSharp is focused exclusively on the **server implementation**.

The protocol implementations are maintained as separate repositories:

* **BedSharp** — Minecraft Bedrock server
* **NetherNet** — NetherNet transport implementation
* **MCPE Protocol** — Minecraft Bedrock packet protocol implementation

This keeps the server independent from the implementation details of the networking and packet layers.

The two protocol projects are intended to be reusable by other developers and projects as well.

---

### 3. The shift from RakNet to NetherNet

Traditional Minecraft Bedrock networking was based on **RakNet**.

Recent Bedrock versions have moved to **NetherNet**. For the versions targeted by BedSharp, using RakNet as the transport is no longer a viable solution. Selecting RakNet on the affected server versions can cause the server to crash.

BedSharp therefore targets NetherNet directly rather than attempting to maintain a RakNet-based transport.

The NetherNet implementation is developed independently from BedSharp.

---

## Project Structure

BedSharp itself contains the server implementation.

The overall project ecosystem is divided into three repositories:

```text
┌─────────────────────┐
│      BedSharp       │
│   Server / Gameplay │
└──────────┬──────────┘
           │
           ├──────────────────────┐
           │                      │
           ▼                      ▼
┌─────────────────────┐  ┌─────────────────────┐
│     MCPE Protocol   │  │      NetherNet      │
│   Packet Protocol   │  │   Network Transport │
└─────────────────────┘  └─────────────────────┘
```

### BedSharp

Responsible for the actual Minecraft server:

* Server lifecycle
* Player management
* World management
* Chunks
* Entities
* Gameplay
* Commands
* Configuration
* Persistence
* Future server extensions

### MCPE Protocol

Separate repository responsible for the Minecraft Bedrock packet protocol.

### NetherNet

Separate repository responsible for the NetherNet transport.

---

# NetherNet

NetherNet is the newer networking stack used by recent versions of Minecraft Bedrock Edition.

It replaces the traditional RakNet transport for the versions targeted by BedSharp.

NetherNet uses **WebRTC** for the gameplay connection and involves additional signalling/authentication mechanisms during connection establishment.

The available specification is limited, so parts of the implementation require protocol analysis and reverse engineering.

See the [NetherNet specification](https://github.com/df-mc/nethernet-spec) for the currently available documentation.

The NetherNet implementation itself is **not part of this repository**.

---

# Status

**Project state:** Fresh start — server scaffold only.

### Implemented

* .NET 10 project scaffold
* Basic BedSharp server project structure
* Initial `Program.cs`

### Not implemented

* NetherNet integration
* MCPE protocol integration
* Client connection
* Authentication
* Login/join handshake
* Player/session management
* World storage
* Chunk streaming
* Entities
* Physics
* Gameplay
* Commands
* Persistence
* Almost everything else

The current project is **not yet a usable Minecraft server**.

---

# Requirements

* [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
* Minecraft Bedrock Edition for testing
* A matching Bedrock protocol version

For protocol development and debugging, additional tools such as Wireshark and Bedrock Dedicated Server may be required.

---

# Quickstart

## Clone

```sh
git clone https://github.com/Yukow0/BedSharp.git
cd BedSharp
```

## Build

```sh
dotnet build
```

## Run

```sh
dotnet run
```

> **Note:** BedSharp is currently only a scaffold and does not accept Minecraft connections yet.

---

# Repository Layout

```text
/
├── Program.cs
├── BedSharp.csproj
└── ...
```

The protocol implementations are intentionally **not included in this repository**.

They are maintained separately as:

* NetherNet
* MCPE Protocol

---

# Development Principles

### Explicit protocol handling

Protocol-related code should remain explicit and easy to debug.

### Defensive parsing

Network data should always be treated as untrusted input.

### Small, focused tests

Packet serialization, deserialization and server behaviour should be covered by focused tests whenever possible.

### Minimal abstractions

Avoid introducing abstractions before their purpose is clear.

### Version awareness

Minecraft Bedrock changes frequently. Version-specific behaviour should be explicit rather than hidden behind assumptions that different protocol versions are compatible.

---

# Roadmap

The short-term roadmap is:

1. Integrate the NetherNet library.
2. Integrate the MCPE protocol library.
3. Establish a working client connection.
4. Implement the Bedrock login/join handshake.
5. Implement a minimal player/session manager.
6. Send the first valid play packets to a connected client.
7. Begin chunk streaming.
8. Add basic entity support.
9. Start implementing actual gameplay systems.

---

# How You Can Help

The project is still in an early stage.

Useful contributions include:

* Testing BedSharp with different Bedrock versions
* Reporting connection and protocol issues
* Server implementation
* Packet handling
* Serialization/deserialization tests
* World/chunk implementation
* Entity implementation
* Gameplay systems

For protocol-related issues, please include:

```text
Minecraft version:
BDS version:
BedSharp commit:
Operating system:
Logs:
Packet/protocol area:
Steps to reproduce:
Wireshark capture:
```

---

# Security

BedSharp is experimental software.

**Do not expose an unfinished BedSharp server to untrusted networks.**

The server and its networking dependencies are still under active development and may contain vulnerabilities or insufficient malformed-packet handling.

For security issues, please report them privately when possible.

---

# Contributing

1. Fork the repository.
2. Create a branch:

```sh
git checkout -b feature/short-description
```

3. Make your changes.
4. Add tests where appropriate.
5. Verify that the project builds.
6. Open a pull request with a clear description of the changes.

---

# License

MIT — see [LICENSE](LICENSE).

---

# Thanks

Thanks to the projects and communities that made researching Minecraft Bedrock possible:

* [PocketMine](https://github.com/pmmp/PocketMine-MP) (Rest in peace)
* [bedrock.dev](https://bedrock.dev/)
* [CloudBurst](https://github.com/CloudburstMC)

---

# Contact

Maintainer: **Yukow0**

[GitHub](https://github.com/Yukow0)

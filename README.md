# BedSharp

[![Build](https://img.shields.io/badge/build-unknown-lightgrey)](https://github.com/Yukow0/BedSharp/actions)
[![License](https://img.shields.io/badge/license-MIT-blue)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-10.0-blueviolet)](https://dotnet.microsoft.com/download/dotnet/10.0)

*BedSharp* is an experimental Minecraft Bedrock Edition server implementation written entirely in C#.  
It's a from-scratch rewrite that's still in its very early stages, so don't expect a playable server yet.

## Why I restarted the project

I restarted BedSharp from scratch for a few reasons.

### .NET 9 was STS (Standard Term Support)

The previous version was built on .NET 9.0, which is a standard term support release with a pretty short support window.  
I wanted the project to stay supported even if I decide to stop working on it someday, so BedSharp now targets **.NET 10**, an LTS (Long Term Support) release.

### A cleaner structure

I have a lot of respect for the work done by **MiNET**, but the project got abandoned and was later taken over by a maintainer I'm not really trusting.  
The C# community deserves a Bedrock server implementation it can build on, and I want BedSharp to be that.  
To keep things sustainable and reusable, the project is split into separate pieces: **BedSharp** (the server itself, this repository), **NetherNet** (the transport library) and **MCPE Protocol** (the packet protocol library).  
That way the protocol libraries stay useful to anyone who wants to build their own engine.

### RakNet is gone

Mojang switched Bedrock networking away from RakNet on recent versions, so the old transport isn't viable anymore.  
BedSharp had to move to **NetherNet** instead, more on that below.  
It was another good reason to start over.

### You

If you want to build your own engine, I'm all for it.  
I'll do what I can to help you, and shipping NetherNet and the MCPE protocol as standalone libraries is the first step.

## The plan

The project is split into three independent pieces:  
**BedSharp** depends on the two libraries and turns them into an actual server.  
**NetherNet** is a standalone library for the NetherNet transport.  
**MCPE Protocol** is a standalone library for the Bedrock packet protocol.  

Right now only **BedSharp** exists, and it's a fresh scaffold.  
The two libraries will be extracted as the protocol work comes back.

## NetherNet

Mojang switched the protocol from RakNet to NetherNet.  
NetherNet uses WebRTC to establish the connection between client and server, plus a web socket for the Xbox authentication.

It's a bit hard to explain briefly, so I recommend reading the [NetherNet documentation](https://github.com/df-mc/nethernet-spec).  
It's not that detailed, but it's a good starting point.

## Status

*Project state:* fresh start, scaffold only. No protocol work yet.

**Implemented:**

* .NET 10 project scaffold (`Program.cs` + project file)

**Not implemented:**

* NetherNet transport (separate library, planned)
* MCPE/Bedrock protocol library (planned)
* Client connection and login handshake
* World storage, chunk streaming, entities, physics, gameplay
* Just about everything else

The project is **not** a usable Minecraft server yet.

## Requirements

* .NET **10.0** SDK
* A Minecraft Bedrock client for testing (matching your target protocol version)

## Quickstart

### Clone

```sh
git clone https://github.com/Yukow0/BedSharp.git
cd BedSharp
```

### Build

```sh
dotnet build
```

### Run

```sh
dotnet run --project BedSharp.Core
```

> **Note:** right now the server only prints "Hello, World!". It doesn't accept connections yet.

## Repository layout

```
/
  Program.cs
  BedSharp.Core.csproj
  LICENSE
  README.md
```

The protocol implementations live outside this repository when they come back.

## Development notes

* Protocol code should stay explicit and easy to test.
* Prefer small, focused tests for parsing and serialization.
* Keep public APIs minimal until the protocols stabilize.
* Treat all network data as untrusted input.

## Roadmap (short-term)

1. Restore the NetherNet transport behind a clean interface.
2. Restore the MCPE protocol as a separate library.
3. Reconnect the client login handshake.
4. Add a minimal player/session manager.
5. Start chunk streaming and basic entity stubs.

## How you can help

* Test different Bedrock versions and report issues (logs + client version help a lot).
* Add parsing/serialization tests.
* Implement missing packet handlers.
* Help design a minimal test suite for a full client login.

## Security

* **Do not expose an unfinished server to untrusted networks.** The networking layers are under active development and malformed-packet handling isn't hardened yet.
* For security issues, report privately or open an issue marked **security**.

## Contributing

1. Fork the repository.
2. Create a branch: `git checkout -b feature/short-description`
3. Make your changes and add tests when possible.
4. Verify the project builds.
5. Open a pull request with a clear description.

## License

MIT, see [LICENSE](LICENSE).

## Thanks

Thanks to the projects and communities that made researching Minecraft Bedrock possible:

* [PocketMine](https://github.com/pmmp/PocketMine-MP) (Rest in peace)
* [bedrock.dev](https://bedrock.dev/)
* [CloudBurst](https://github.com/CloudburstMC)

## Contact

Maintainer: **Yukow0** — [GitHub](https://github.com/Yukow0)
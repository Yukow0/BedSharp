# BedSharp

[![Build](https://img.shields.io/badge/build-unknown-lightgrey)](https://github.com/Yukow0/BedSharp/actions)
[![License](https://img.shields.io/badge/license-MIT-blue)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-9.0-blueviolet)](https://dotnet.microsoft.com/download/dotnet/9.0)
[![Qodana](https://github.com/Yukow0/BedSharp/actions/workflows/qodana_code_quality.yml/badge.svg?branch=dev)](https://github.com/Yukow0/BedSharp/actions/workflows/qodana_code_quality.yml)

**BedSharp** is an experimental, native C# Minecraft Bedrock Edition server prototype. It currently focuses on low-level networking: early RakNet implementation and initial Bedrock protocol handling. This is *not* production-ready and serves primarily as a learning and experimentation project.

## Status

* **Project state:** Very early prototype / proof-of-concept
* **Implemented:**

    * Basic RakNet connection handling (initial framing and parsing)
    * Early Bedrock protocol packet structures + partial handlers
    * Client join/login handshake (client can begin the join process)
* **Not implemented:**

    * World storage, chunk streaming, entities, physics, gameplay logic
    * Full RakNet coverage (ordering, reliability not fully implemented)
    * Full Bedrock packet coverage
    * Player auth persistence, player manager, plugin system
    * Hardened networking or production security

## Why this project exists

The goal is to build a clean, C#-first Bedrock server core. The current focus is correctness and clarity in packet handling so higher-level features can be added later.

## Requirements

* .NET **9.0** SDK
* Minecraft Bedrock client (recommended: matching your protocol version)

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
dotnet run --project BedSharp
```

**Notes:**

* This runs the prototype server. Expect minimal behaviour.
* The join process may start but disconnect due to missing features.
* Debug RakNet/Bedrock logs (when enabled) will show packet activity.

## Repository layout (high-level)

```
/src
  /BedSharp         → Server entry point / test host
  /Protocols
      /RakNet       → Framing, parsing, reliability base
      /Bedrock      → Packet definitions + partial handlers
/tests              → Protocol tests (if present)
/docs               → Notes, experiments
```

## Development notes

* Protocol code should be explicit and well-tested.
* Prefer small, focused unit tests for parsing/serialization.
* Keep public APIs minimal until protocols stabilize.
* Use defensive parsing and logging to help debug version mismatches.

## How you can help

* Try the prototype and file issues (with logs + client version).
* Add parsing/serialization tests.
* Implement missing packet handlers.
* Improve RakNet reliability/ordering.
* Help design a minimal test-suite for full client login.

## Roadmap (short-term)

1. Finish RakNet reliability/ordering for join packets.
2. Add remaining Bedrock packets required for complete join.
3. Implement minimal in-memory player/session manager.
4. Add simple configuration and logging.
5. Begin chunk streaming + basic entity stubs.

## Security & Support

* **Do not expose to untrusted networks.**
* This is experimental and missing protections.
* For security issues: open a private report or GitHub issue marked **security**.

## Contributing

1. Fork the repository.
2. Create a branch: `git checkout -b feature/short-description`
3. Add protocol tests when possible.
4. Open a pull request with a clear description + logs or tests.

## License

MIT — see `LICENSE`.

## Contact

Maintainer: **Yukow0** — [https://github.com/Yukow0](https://github.com/Yukow0)
